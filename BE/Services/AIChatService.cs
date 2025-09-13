using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE.Services
{
    public class AIChatService
    {
        private readonly IAIRepository _aiRepository;
        private readonly List<string> _conversationHistory;

        public AIChatService(IAIRepository aiRepository)
        {
            _aiRepository = aiRepository;
            _conversationHistory = new List<string>();
        }

        public async Task<(List<string> TextResponses, List<string> JsonResponses)> GenerateResponsesAsync(string userQuery)
        {
            // Lưu câu hỏi vào lịch sử
            _conversationHistory.Add(userQuery);

            // Chuẩn hóa câu hỏi
            var normalizedQuery = NormalizeQuery(userQuery?.ToLower().Trim() ?? "");

            // Phân loại ý định
            var intent = ClassifyIntent(normalizedQuery);

            // Tìm kiếm sản phẩm từ repository
            var products = await _aiRepository.SearchProductsAsync(normalizedQuery);

            // Danh sách câu trả lời
            var textResponses = new List<string>();

            // Xử lý dựa trên ý định
            switch (intent)
            {
                case "availability":
                    textResponses.AddRange(GenerateAvailabilityResponses(products, normalizedQuery));
                    break;
                case "size":
                    textResponses.AddRange(GenerateSizeResponses(products, normalizedQuery));
                    break;
                case "topping":
                    textResponses.AddRange(GenerateToppingResponses(products, normalizedQuery));
                    break;
                case "promotion":
                    textResponses.AddRange(GeneratePromotionResponses(products, normalizedQuery));
                    break;
                default:
                    textResponses.AddRange(GenerateGeneralResponses(products, normalizedQuery));
                    break;
            }

            // Nếu không tìm thấy sản phẩm, gợi ý sản phẩm tương tự
            if (!products.Any())
            {
                textResponses.Add("Xin lỗi, tôi không tìm thấy sản phẩm nào phù hợp. Bạn có muốn thử với từ khóa khác không?");
                textResponses.AddRange(await GenerateSimilarProductSuggestions(normalizedQuery));
            }

            // Thêm câu hỏi gợi ý dựa trên lịch sử
            if (_conversationHistory.Count > 1)
            {
                textResponses.Add("Bạn có muốn tôi tiếp tục tìm kiếm dựa trên câu hỏi trước đó không?");
            }

            // Giới hạn số lượng câu trả lời tối đa
            textResponses = textResponses.Take(5).ToList();

            // Lấy dữ liệu JSON từ GetAllWithDetailsAsync
            var allProducts = await _aiRepository.GetAllWithDetailsAsync();

            // Lọc JSON theo từ khóa nếu cần
            var jsonResponses = allProducts
                ?.Where(sp => !string.IsNullOrEmpty(normalizedQuery) &&
                              normalizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Any(k => sp?.ToLower().Contains(k) == true))
                .Take(5)
                .ToList() ?? new List<string>();

            return (textResponses, jsonResponses);
        }

        private string NormalizeQuery(string query)
        {
            // Xử lý bỏ dấu tiếng Việt
            var replacements = new Dictionary<string, string>
            {
                { "[áàảãạăắằẳẵặ]", "a" },
                { "[éèẻẽẹêếềểễệ]", "e" },
                { "[íìỉĩị]", "i" },
                { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                { "[úùủũụưứừửũụ]", "u" },
                { "[ýỳỷỹỵ]", "y" },
                { "[đ]", "d" }
            };

            foreach (var pair in replacements)
            {
                query = Regex.Replace(query, pair.Key, pair.Value);
            }

            return query;
        }

        private string ClassifyIntent(string query)
        {
            if (string.IsNullOrEmpty(query)) return "general";
            if (query.Contains("có sẵn") || query.Contains("số lượng") || query.Contains("còn hàng"))
                return "availability";
            if (query.Contains("kích thước") || query.Contains("size"))
                return "size";
            if (query.Contains("topping") || query.Contains("thêm"))
                return "topping";
            if (query.Contains("khuyến mãi") || query.Contains("giảm giá"))
                return "promotion";
            return "general";
        }

        private List<string> GenerateAvailabilityResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null)
                {
                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' hiện có {product.So_Luong} đơn vị trong kho.");
                }
            }
            return responses;
        }

        private List<string> GenerateSizeResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Sizes?.Any() == true)
                {
                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' có các kích thước: {string.Join(", ", product.Sizes)}.");
                }
            }
            return responses;
        }

        private List<string> GenerateToppingResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Toppings?.Any() == true)
                {
                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' có thể thêm các topping: {string.Join(", ", product.Toppings)}.");
                }
            }
            return responses;
        }

        private List<string> GeneratePromotionResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Co_Khuyen_Mai)
                {
                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' hiện đang có chương trình khuyến mãi!");
                }
            }
            return responses;
        }

        private List<string> GenerateGeneralResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null)
                {
                    string mainResponse = $"Sản phẩm '{product.Ten_San_Pham}' hiện có sẵn với số lượng {product.So_Luong}.";
                    if (product.Sizes?.Any() == true)
                    {
                        mainResponse += $" Có các kích thước: {string.Join(", ", product.Sizes)}.";
                    }
                    if (product.Toppings?.Any() == true)
                    {
                        mainResponse += $" Bạn có thể chọn thêm các topping: {string.Join(", ", product.Toppings)}.";
                    }
                    if (product.Co_Khuyen_Mai)
                    {
                        mainResponse += " Sản phẩm này hiện đang có chương trình khuyến mãi!";
                    }
                    responses.Add(mainResponse);
                    responses.Add($"Bạn đang tìm '{product.Ten_San_Pham}' đúng không? Sản phẩm này có {product.So_Luong} đơn vị trong kho.");
                }
            }
            return responses;
        }

        private async Task<List<string>> GenerateSimilarProductSuggestions(string query)
        {
            var suggestions = new List<string>();
            var firstKeyword = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            var similarProducts = await _aiRepository.SearchProductsAsync(firstKeyword);
            foreach (var product in similarProducts.Take(2))
            {
                if (product != null)
                {
                    suggestions.Add($"Có thể bạn quan tâm đến '{product.Ten_San_Pham}' với số lượng {product.So_Luong} trong kho.");
                }
            }
            return suggestions;
        }
    }
}



