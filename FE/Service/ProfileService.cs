using FE.Models;
using FE.Service.IService;
using System.Text.Json;

namespace FE.Service
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;

        public ProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public async Task<ProfileModel> GetProfileAsync(int khachHangId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Profile/{khachHangId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProfileModel>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateProfileAsync(int khachHangId, ProfileUpdateModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Profile/{khachHangId}", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddAddressAsync(int khachHangId, AddressCreateModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/Profile/{khachHangId}/address", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAddressAsync(int khachHangId, int diaChiId, AddressModel model)
        {
            try
            {
                var updateModel = new
                {
                    ID_Dia_Chi = model.ID_Dia_Chi,
                    Dia_Chi = model.Dia_Chi,
                    Tinh_Thanh = model.Tinh_Thanh,
                    Ghi_Chu = model.Ghi_Chu,
                    Trang_Thai = model.Trang_Thai
                };

                var response = await _httpClient.PutAsJsonAsync($"api/Profile/{khachHangId}/address/{diaChiId}", updateModel);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAddressAsync(int khachHangId, int diaChiId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Profile/{khachHangId}/address/{diaChiId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}