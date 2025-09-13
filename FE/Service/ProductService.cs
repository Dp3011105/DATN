using FE.Models;
using FE.Service.IService;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
            return await _httpClient.GetFromJsonAsync<List<SanPham>>($"{BaseUrl}SanPham") ?? new List<SanPham>();
        }

        public async Task<SanPham> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SanPham>($"{BaseUrl}SanPham/{id}");
        }

        public async Task<List<Size>> GetSizesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Size>>($"{BaseUrl}Size") ?? new List<Size>();
        }

        public async Task<List<Topping>> GetToppingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Topping>>($"{BaseUrl}Topping") ?? new List<Topping>();
        }

        public async Task<List<LuongDa>> GetLuongDasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<LuongDa>>($"{BaseUrl}LuongDa") ?? new List<LuongDa>();
        }

        public async Task<List<DoNgot>> GetDoNgotsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DoNgot>>($"{BaseUrl}DoNgot") ?? new List<DoNgot>();
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadStream());
            content.Add(fileContent, "image", image.FileName);

            var response = await _httpClient.PostAsync($"{BaseUrl}SanPham/upload-image", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> AddProductAsync(AddProductViewModel model)
        {
            var product = new
            {
                iD_San_Pham = 0,
                ten_San_Pham = model.TenSanPham,
                gia = model.Gia,
                so_Luong = model.SoLuong,
                hinh_Anh = model.Image != null ? await UploadImageAsync(model.Image) : null,
                mo_Ta = model.MoTa,
                trang_Thai = true,
                sizes = model.SelectedSizes ?? new List<int>(),
                luongDas = model.SelectedLuongDas ?? new List<int>(),
                doNgots = model.SelectedDoNgots ?? new List<int>(),
                toppings = model.SelectedToppings ?? new List<int>()
            };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}SanPham", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductViewModel model)
        {
            try
            {
                // Nếu có hình ảnh mới, xóa hình ảnh cũ và tải lên hình ảnh mới
                string newImagePath = model.CurrentImagePath; // Mặc định giữ nguyên đường dẫn cũ
                if (model.Image != null && model.Image.Length > 0)
                {
                    // Gọi API xóa hình ảnh cũ
                    if (!string.IsNullOrEmpty(model.CurrentImagePath))
                    {
                        var deleteResponse = await _httpClient.DeleteAsync($"{BaseUrl}SanPham/delete-image?imagePath={model.CurrentImagePath}");
                        deleteResponse.EnsureSuccessStatusCode();
                    }

                    // Tải lên hình ảnh mới
                    newImagePath = await UploadImageAsync(model.Image);
                }

                // Tạo đối tượng JSON khớp với cấu trúc API
                var product = new
                {
                    iD_San_Pham = model.ID_San_Pham,
                    ten_San_Pham = model.TenSanPham,
                    gia = model.Gia,
                    so_Luong = model.SoLuong,
                    hinh_Anh = newImagePath, // Sử dụng đường dẫn cũ hoặc mới
                    mo_Ta = model.MoTa,
                    trang_Thai = model.TrangThai, // Sử dụng trạng thái từ model
                    sizes = model.SelectedSizes ?? new List<int>(),
                    luongDas = model.SelectedLuongDas ?? new List<int>(),
                    doNgots = model.SelectedDoNgots ?? new List<int>(),
                    toppings = model.SelectedToppings ?? new List<int>()
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}SanPham/{model.ID_San_Pham}", product);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }




        // Method mới: Gọi API most-purchased, trả về 10 sản phẩm
        public async Task<List<SanPham>> GetMostPurchasedProductsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SanPham>>($"{BaseUrl}SanPham/most-purchased") ?? new List<SanPham>();
            }
            catch (HttpRequestException ex)
            {
                // Log error nếu cần
                Console.WriteLine($"Lỗi khi gọi API most-purchased: {ex.Message}");
                return new List<SanPham>(); // Graceful fallback
            }
        }



    }



}