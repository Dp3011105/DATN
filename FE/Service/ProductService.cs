using FE.Models;
using FE.Service.IService;
using System.Diagnostics;
using System.Text.Json;

namespace FE.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7169/api/";


        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SanPham>> GetAllProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SanPham>>("https://localhost:7169/api/SanPham") ?? new List<SanPham>();
        }

        public async Task<SanPham> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SanPham>($"https://localhost:7169/api/SanPham/{id}");
        }

        public async Task<List<Size>> GetSizesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Size>>("https://localhost:7169/api/Size") ?? new List<Size>();
        }

        public async Task<List<Topping>> GetToppingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Topping>>("https://localhost:7169/api/Topping") ?? new List<Topping>();
        }

        public async Task<List<LuongDa>> GetLuongDasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<LuongDa>>("https://localhost:7169/api/LuongDa") ?? new List<LuongDa>();
        }

        public async Task<List<DoNgot>> GetDoNgotsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DoNgot>>("https://localhost:7169/api/DoNgot") ?? new List<DoNgot>();
        }
        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadStream());
            content.Add(fileContent, "image", image.FileName);

            var response = await _httpClient.PostAsync("https://localhost:7169/api/SanPham/upload-image", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> AddProductAsync(AddProductViewModel model)
        {
            var product = new
            {
                ID_San_Pham = 0,
                Ten_San_Pham = model.TenSanPham,
                Gia = model.Gia,
                So_Luong = model.SoLuong,
                Hinh_Anh = await UploadImageAsync(model.Image),
                Mo_Ta = model.MoTa,
                Trang_Thai = true,
                Sizes = model.SelectedSizes ?? new List<int>(),
                LuongDas = model.SelectedLuongDas ?? new List<int>(),
                DoNgots = model.SelectedDoNgots ?? new List<int>(),
                Toppings = model.SelectedToppings ?? new List<int>()
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7169/api/SanPham", product);
            return response.IsSuccessStatusCode;
        }

      


        public async Task<bool> UpdateProductAsync(UpdateProductViewModel model)
        {
            // Nếu có hình ảnh mới, xóa hình ảnh cũ trước
            string newImagePath = model.CurrentImagePath; // Mặc định giữ nguyên đường dẫn cũ
            if (model.Image != null)
            {
                // Gọi API xóa hình ảnh cũ
                if (!string.IsNullOrEmpty(model.CurrentImagePath))
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"https://localhost:7169/api/SanPham/delete-image?imagePath={model.CurrentImagePath}");
                    deleteResponse.EnsureSuccessStatusCode();
                }

                // Tải lên hình ảnh mới
                newImagePath = await UploadImageAsync(model.Image);
            }

            var product = new
            {
                ID_San_Pham = model.ID_San_Pham,
                Ten_San_Pham = model.TenSanPham,
                Gia = model.Gia,
                So_Luong = model.SoLuong,
                Hinh_Anh = newImagePath, // Sử dụng đường dẫn mới hoặc cũ
                Mo_Ta = model.MoTa,
                Trang_Thai = true,
                Sizes = model.SelectedSizes ?? new List<int>(),
                LuongDas = model.SelectedLuongDas ?? new List<int>(),
                DoNgots = model.SelectedDoNgots ?? new List<int>(),
                Toppings = model.SelectedToppings ?? new List<int>()
            };

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7169/api/SanPham/{model.ID_San_Pham}", product);
            return response.IsSuccessStatusCode;
        }
    }
}
