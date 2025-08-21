using BE.models;
using Service.IService;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Service.Services // hoặc namespace của bạn cho layer Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _http;

        public HoaDonService(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7169/");
        }
        public async Task<IEnumerable<HoaDon>> GetAllAsync()
            => await _http.GetFromJsonAsync<IEnumerable<HoaDon>>("api/HoaDon");

        public async Task<HoaDon> GetByIdAsync(int id)
            => await _http.GetFromJsonAsync<HoaDon>($"api/HoaDon/{id}");

        public async Task AddAsync(HoaDon entity)
        {
            var res = await _http.PostAsJsonAsync("api/HoaDon", entity);
            res.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, HoaDon entity)
        {
            var res = await _http.PutAsJsonAsync($"api/HoaDon/{id}", entity);
            res.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/HoaDon/{id}");
            res.EnsureSuccessStatusCode();
        }
        public async Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy)
        {
            var payload = new
            {
                HoaDonId = hoaDonId,
                TrangThai = trangThaiDb,
                LyDoHuy = lyDoHuy
            };

            // ví dụ BE có endpoint: PATCH api/HoaDon/{id}/TrangThai
            var resp = await _http.PatchAsJsonAsync($"api/HoaDon/{hoaDonId}/TrangThai", payload);
            if (resp.IsSuccessStatusCode) return true;

            // fallback: nếu chưa có PATCH, dùng POST tạm 1 endpoint bạn tự tạo
            // var resp = await _http.PostAsJsonAsync("api/HoaDon/CapNhatTrangThai", payload);

            return false;
        }
    }
}
