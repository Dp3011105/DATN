//using BE.Data;
//using BE.DTOs;
//using BE.models;
//using BE.Repository.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace BE.Services
//{
//    public class AIChatService
//    {
//        private readonly IAIRepository _aiRepository;
//        private readonly List<string> _conversationHistory;

//        public AIChatService(IAIRepository aiRepository)
//        {
//            _aiRepository = aiRepository;
//            _conversationHistory = new List<string>();
//        }

//        public async Task<(List<string> TextResponses, List<string> JsonResponses)> GenerateResponsesAsync(string userQuery)
//        {
//            // Lưu câu hỏi vào lịch sử
//            _conversationHistory.Add(userQuery);

//            // Chuẩn hóa câu hỏi
//            var normalizedQuery = NormalizeQuery(userQuery?.ToLower().Trim() ?? "");

//            // Phân loại ý định
//            var intent = ClassifyIntent(normalizedQuery);

//            // Tìm kiếm sản phẩm từ repository
//            var products = await _aiRepository.SearchProductsAsync(normalizedQuery);

//            // Danh sách câu trả lời
//            var textResponses = new List<string>();

//            // Xử lý dựa trên ý định
//            switch (intent)
//            {
//                case "availability":
//                    textResponses.AddRange(GenerateAvailabilityResponses(products, normalizedQuery));
//                    break;
//                case "size":
//                    textResponses.AddRange(GenerateSizeResponses(products, normalizedQuery));
//                    break;
//                case "topping":
//                    textResponses.AddRange(GenerateToppingResponses(products, normalizedQuery));
//                    break;
//                case "promotion":
//                    textResponses.AddRange(GeneratePromotionResponses(products, normalizedQuery));
//                    break;
//                default:
//                    textResponses.AddRange(GenerateGeneralResponses(products, normalizedQuery));
//                    break;
//            }

//            // Nếu không tìm thấy sản phẩm, gợi ý sản phẩm tương tự
//            if (!products.Any())
//            {
//                textResponses.Add("Xin lỗi, tôi không tìm thấy sản phẩm nào phù hợp. Bạn có muốn thử với từ khóa khác không?");
//                textResponses.AddRange(await GenerateSimilarProductSuggestions(normalizedQuery));
//            }

//            // Thêm câu hỏi gợi ý dựa trên lịch sử
//            if (_conversationHistory.Count > 1)
//            {
//                textResponses.Add("Bạn có muốn tôi tiếp tục tìm kiếm dựa trên câu hỏi trước đó không?");
//            }

//            // Giới hạn số lượng câu trả lời tối đa
//            textResponses = textResponses.Take(5).ToList();

//            // Lấy dữ liệu JSON từ GetAllWithDetailsAsync
//            var allProducts = await _aiRepository.GetAllWithDetailsAsync();

//            // Lọc JSON theo từ khóa nếu cần
//            var jsonResponses = allProducts
//                ?.Where(sp => !string.IsNullOrEmpty(normalizedQuery) &&
//                              normalizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries)
//                                  .Any(k => sp?.ToLower().Contains(k) == true))
//                .Take(5)
//                .ToList() ?? new List<string>();

//            return (textResponses, jsonResponses);
//        }

//        private string NormalizeQuery(string query)
//        {
//            // Xử lý bỏ dấu tiếng Việt
//            var replacements = new Dictionary<string, string>
//            {
//                { "[áàảãạăắằẳẵặ]", "a" },
//                { "[éèẻẽẹêếềểễệ]", "e" },
//                { "[íìỉĩị]", "i" },
//                { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
//                { "[úùủũụưứừửũụ]", "u" },
//                { "[ýỳỷỹỵ]", "y" },
//                { "[đ]", "d" }
//            };

//            foreach (var pair in replacements)
//            {
//                query = Regex.Replace(query, pair.Key, pair.Value);
//            }

//            return query;
//        }

//        private string ClassifyIntent(string query)
//        {
//            if (string.IsNullOrEmpty(query)) return "general";
//            if (query.Contains("có sẵn") || query.Contains("số lượng") || query.Contains("còn hàng"))
//                return "availability";
//            if (query.Contains("kích thước") || query.Contains("size"))
//                return "size";
//            if (query.Contains("topping") || query.Contains("thêm"))
//                return "topping";
//            if (query.Contains("khuyến mãi") || query.Contains("giảm giá"))
//                return "promotion";
//            return "general";
//        }

//        private List<string> GenerateAvailabilityResponses(List<ProductSearchResult> products, string query)
//        {
//            var responses = new List<string>();
//            foreach (var product in products)
//            {
//                if (product != null)
//                {
//                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' hiện có {product.So_Luong} đơn vị trong kho.");
//                }
//            }
//            return responses;
//        }

//        private List<string> GenerateSizeResponses(List<ProductSearchResult> products, string query)
//        {
//            var responses = new List<string>();
//            foreach (var product in products)
//            {
//                if (product != null && product.Sizes?.Any() == true)
//                {
//                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' có các kích thước: {string.Join(", ", product.Sizes)}.");
//                }
//            }
//            return responses;
//        }

//        private List<string> GenerateToppingResponses(List<ProductSearchResult> products, string query)
//        {
//            var responses = new List<string>();
//            foreach (var product in products)
//            {
//                if (product != null && product.Toppings?.Any() == true)
//                {
//                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' có thể thêm các topping: {string.Join(", ", product.Toppings)}.");
//                }
//            }
//            return responses;
//        }

//        private List<string> GeneratePromotionResponses(List<ProductSearchResult> products, string query)
//        {
//            var responses = new List<string>();
//            foreach (var product in products)
//            {
//                if (product != null && product.Co_Khuyen_Mai)
//                {
//                    responses.Add($"Sản phẩm '{product.Ten_San_Pham}' hiện đang có chương trình khuyến mãi!");
//                }
//            }
//            return responses;
//        }

//        private List<string> GenerateGeneralResponses(List<ProductSearchResult> products, string query)
//        {
//            var responses = new List<string>();
//            foreach (var product in products)
//            {
//                if (product != null)
//                {
//                    string mainResponse = $"Sản phẩm '{product.Ten_San_Pham}' hiện có sẵn với số lượng {product.So_Luong}.";
//                    if (product.Sizes?.Any() == true)
//                    {
//                        mainResponse += $" Có các kích thước: {string.Join(", ", product.Sizes)}.";
//                    }
//                    if (product.Toppings?.Any() == true)
//                    {
//                        mainResponse += $" Bạn có thể chọn thêm các topping: {string.Join(", ", product.Toppings)}.";
//                    }
//                    if (product.Co_Khuyen_Mai)
//                    {
//                        mainResponse += " Sản phẩm này hiện đang có chương trình khuyến mãi!";
//                    }
//                    responses.Add(mainResponse);
//                    responses.Add($"Bạn đang tìm '{product.Ten_San_Pham}' đúng không? Sản phẩm này có {product.So_Luong} đơn vị trong kho.");
//                }
//            }
//            return responses;
//        }

//        private async Task<List<string>> GenerateSimilarProductSuggestions(string query)
//        {
//            var suggestions = new List<string>();
//            var firstKeyword = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
//            var similarProducts = await _aiRepository.SearchProductsAsync(firstKeyword);
//            foreach (var product in similarProducts.Take(2))
//            {
//                if (product != null)
//                {
//                    suggestions.Add($"Có thể bạn quan tâm đến '{product.Ten_San_Pham}' với số lượng {product.So_Luong} trong kho.");
//                }
//            }
//            return suggestions;
//        }
//    }
//}





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
        private readonly Random _random = new Random(); // Để đa dạng hóa responses

        // Thêm 10 câu hoạt ngôn để chèn ngẫu nhiên vào responses
        private readonly string[] _activePhrases = new string[]
        {
            "Ồ, nghe hay quá nhỉ!",
            "Mình nghĩ bạn sẽ mê mẩn luôn đấy!",
            "Woa, lựa chọn tuyệt vời!",
            "Hay ho lắm, để mình kể thêm nhé!",
            "Bạn có gu thật đấy!",
            "Mình hào hứng quá, tiếp tục nào!",
            "Siêu thú vị, mình thích kiểu này!",
            "Ừm, nghe hấp dẫn quá đi!",
            "Mình đoán bạn sẽ thích lắm đây!",
            "Chà, ý tưởng hay ho!"
        };

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

            // Phân loại ý định, thêm intent "preference" để xử lý sở thích
            var intent = ClassifyIntent(normalizedQuery);

            // Tìm kiếm sản phẩm từ repository, hỗ trợ tìm sâu hơn (partial match)
            var products = await _aiRepository.SearchProductsAsync(normalizedQuery);
            products = FilterPartialMatchProducts(products, normalizedQuery); // Lọc partial match để thông minh hơn

            // Nếu intent là preference, mở rộng tìm kiếm các biến thể liên quan
            if (intent == "preference")
            {
                var extendedProducts = await GetExtendedPreferenceProducts(normalizedQuery);
                products.AddRange(extendedProducts);
                products = products.Distinct().Take(3).ToList(); // Giới hạn để tránh quá nhiều
            }

            // Giới hạn số lượng sản phẩm tối đa là 3 để gợi ý ít hơn
            products = products.Take(3).ToList();

            // Danh sách câu trả lời
            var textResponses = new List<string>();

            // Xử lý dựa trên ý định, với responses hoạt ngôn hơn
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
                case "preference":
                    textResponses.AddRange(GeneratePreferenceResponses(products, normalizedQuery));
                    break;
                default:
                    textResponses.AddRange(GenerateGeneralResponses(products, normalizedQuery));
                    break;
            }

            // Nếu không tìm thấy sản phẩm, gợi ý sản phẩm tương tự (chỉ 1-2 cái, hoạt ngôn hơn)
            if (!products.Any())
            {
                textResponses.Add("Ồ, mình không tìm thấy sản phẩm chính xác nào phù hợp với từ khóa của bạn. Để mình gợi ý vài cái tương tự nhé?");
                textResponses.AddRange(await GenerateSimilarProductSuggestions(normalizedQuery));
            }

            // Thêm câu hỏi gợi ý dựa trên lịch sử, để tăng tính tương tác
            if (_conversationHistory.Count > 1)
            {
                textResponses.Add(GetRandomFollowUpQuestion());
            }

            // Giới hạn số lượng câu trả lời tối đa
            textResponses = textResponses.Distinct().Take(5).ToList(); // Tránh lặp và giới hạn

            // Lấy dữ liệu JSON từ GetAllWithDetailsAsync
            var allProducts = await _aiRepository.GetAllWithDetailsAsync();

            // Lọc JSON theo từ khóa chính xác hơn và giới hạn 3
            var jsonResponses = allProducts
                ?.Where(sp => !string.IsNullOrEmpty(normalizedQuery) &&
                              normalizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                  .Any(k => sp?.ToLower().Contains(k) == true)) // Partial match cho JSON để thông minh hơn
                .Take(3)
                .ToList() ?? new List<string>();

            return (textResponses, jsonResponses);
        }

        private List<ProductSearchResult> FilterPartialMatchProducts(List<ProductSearchResult> products, string query)
        {
            var keywords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return products
                .Where(p => keywords.Any(k => p.Ten_San_Pham?.ToLower().Contains(k) == true)) // Partial match để tìm sâu hơn
                .OrderByDescending(p => keywords.Count(k => p.Ten_San_Pham?.ToLower().Contains(k) == true)) // Xếp hạng theo số match
                .ToList();
        }

        private async Task<List<ProductSearchResult>> GetExtendedPreferenceProducts(string query)
        {
            // Mở rộng tìm kiếm cho preference, ví dụ: nếu "hong tra", tìm "tra sua * hong tra"
            var keywords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var extendedQuery = string.Join(" ", keywords.Select(k => $"{k} tra sua")); // Thêm biến thể như "tra sua" + keyword
            return await _aiRepository.SearchProductsAsync(extendedQuery);
        }

        private string NormalizeQuery(string query)
        {
            // Xử lý bỏ dấu tiếng Việt (giữ nguyên)
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
            if (query.Contains("thích") || query.Contains("muốn") || query.Contains("ưa thích")) // Thêm intent preference
                return "preference";
            return "general";
        }

        private List<string> GenerateAvailabilityResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null)
                {
                    responses.Add(GetRandomAvailabilityPhrase(product.Ten_San_Pham, product.So_Luong));
                }
            }
            return responses;
        }

        private string GetRandomAvailabilityPhrase(string name, int quantity)
        {
            var phrases = new[]
            {
                $"Ồ, sản phẩm '{name}' còn {quantity} đơn vị trong kho đấy! Bạn cần bao nhiêu?",
                $"Mình kiểm tra rồi, '{name}' hiện có sẵn {quantity} cái. Sẵn sàng order chưa ạ?",
                $"Yay, '{name}' còn {quantity} đơn vị. Có gì mình giúp thêm không?"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private List<string> GenerateSizeResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Sizes?.Any() == true)
                {
                    responses.Add(GetRandomSizePhrase(product.Ten_San_Pham, product.Sizes));
                }
            }
            return responses;
        }

        private string GetRandomSizePhrase(string name, List<string> sizes)
        {
            var phrases = new[]
            {
                $"Sản phẩm '{name}' có các size: {string.Join(", ", sizes)}. Bạn thích size nào nhất?",
                $"Ừm, '{name}' sẵn các kích thước {string.Join(", ", sizes)}. Phù hợp với bạn chứ?",
                $"Mình thấy '{name}' có size {string.Join(", ", sizes)}. Thử chọn một cái xem sao!"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private List<string> GenerateToppingResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Toppings?.Any() == true)
                {
                    responses.Add(GetRandomToppingPhrase(product.Ten_San_Pham, product.Toppings));
                }
            }
            return responses;
        }

        private string GetRandomToppingPhrase(string name, List<string> toppings)
        {
            var phrases = new[]
            {
                $"Bạn có thể thêm topping cho '{name}' như: {string.Join(", ", toppings)}. Ngon miệng lắm đấy!",
                $"Ồ, '{name}' hỗ trợ topping {string.Join(", ", toppings)}. Bạn muốn thêm gì không ạ?",
                $"Thử topping cho '{name}': {string.Join(", ", toppings)}. Mình recommend hết luôn!"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private List<string> GeneratePromotionResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null && product.Co_Khuyen_Mai)
                {
                    responses.Add(GetRandomPromotionPhrase(product.Ten_San_Pham));
                }
            }
            return responses;
        }

        private string GetRandomPromotionPhrase(string name)
        {
            var phrases = new[]
            {
                $"Tin vui! '{name}' đang có khuyến mãi hot. Đừng bỏ lỡ nhé!",
                $"Woa, '{name}' giảm giá rồi. Bạn mua ngay đi ạ!",
                $"Mình thấy '{name}' có chương trình khuyến mãi. Siêu hời luôn đấy!"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private List<string> GenerateGeneralResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            foreach (var product in products)
            {
                if (product != null)
                {
                    string mainResponse = GetRandomGeneralPhrase(product);
                    responses.Add(mainResponse);
                }
            }
            return responses;
        }

        private string GetRandomGeneralPhrase(ProductSearchResult product)
        {
            var baseInfo = $"Sản phẩm '{product.Ten_San_Pham}' có sẵn {product.So_Luong} đơn vị.";
            if (product.Sizes?.Any() == true)
            {
                baseInfo += $" Size: {string.Join(", ", product.Sizes)}.";
            }
            if (product.Toppings?.Any() == true)
            {
                baseInfo += $" Topping: {string.Join(", ", product.Toppings)}.";
            }
            if (product.Co_Khuyen_Mai)
            {
                baseInfo += " Đang khuyến mãi!";
            }

            var phrases = new[]
            {
                $"Bạn đang tìm '{product.Ten_San_Pham}' hả? {baseInfo} Thích không ạ?",
                $"Ồ, mình có '{product.Ten_San_Pham}' đây: {baseInfo} Bạn cần chi tiết hơn không?",
                $"Hay quá, '{product.Ten_San_Pham}': {baseInfo} Mình giúp bạn order luôn nhé!"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private List<string> GeneratePreferenceResponses(List<ProductSearchResult> products, string query)
        {
            var responses = new List<string>();
            if (products.Any())
            {
                responses.Add("Dựa trên sở thích của bạn, mình tìm được vài món liên quan đây:");
            }
            foreach (var product in products)
            {
                if (product != null)
                {
                    responses.Add(GetRandomPreferencePhrase(product.Ten_San_Pham, product.So_Luong));
                }
            }
            return responses;
        }

        private string GetRandomPreferencePhrase(string name, int quantity)
        {
            var phrases = new[]
            {
                $"Nếu bạn thích thì thử '{name}' xem, còn {quantity} cái đấy!",
                $"Mình recommend '{name}', phù hợp với gu của bạn lắm. Còn {quantity} đơn vị.",
                $"'{name}' nghe hợp đấy, hiện có {quantity} trong kho. Bạn nghĩ sao?"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private async Task<List<string>> GenerateSimilarProductSuggestions(string query)
        {
            var suggestions = new List<string>();
            var firstKeyword = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
            var similarProducts = (await _aiRepository.SearchProductsAsync(firstKeyword)).Take(2).ToList(); // Giới hạn 2
            foreach (var product in similarProducts)
            {
                if (product != null)
                {
                    suggestions.Add(GetRandomSimilarPhrase(product.Ten_San_Pham, product.So_Luong));
                }
            }
            return suggestions;
        }

        private string GetRandomSimilarPhrase(string name, int quantity)
        {
            var phrases = new[]
            {
                $"Có lẽ bạn thích '{name}' không? Còn {quantity} cái trong kho đấy!",
                $"Mình gợi ý '{name}', hiện có {quantity} đơn vị. Thử xem sao nhé?",
                $"'{name}' tương tự lắm, còn {quantity} cái. Bạn thấy thế nào?"
            };
            return $"{_activePhrases[_random.Next(_activePhrases.Length)]} {phrases[_random.Next(phrases.Length)]}";
        }

        private string GetRandomFollowUpQuestion()
        {
            var questions = new[]
            {
                "Bạn muốn mình tìm thêm dựa trên câu hỏi trước không ạ?",
                "Có gì cần hỏi thêm về sản phẩm không? Mình sẵn sàng giúp!",
                "Tiếp tục nhé? Bạn đang nghĩ gì vậy?"
            };
            return questions[_random.Next(questions.Length)];
        }
    }
}

