using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BE.models;
using Service.IService;

namespace Service.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _http;

        public HoaDonService(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7169/"); // chỉnh theo API BE của bạn
        }

        // ====== GIỮ NGUYÊN (trả entity) ======
        public async Task<IEnumerable<HoaDon>> GetAllAsync()
            => await _http.GetFromJsonAsync<IEnumerable<HoaDon>>("api/HoaDon/entities");

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

        // ====== Màn list (projection/DTO) ======
        public async Task<IEnumerable<object>> GetAllListAsync()
        {
            var data = await _http.GetFromJsonAsync<IEnumerable<object>>("api/HoaDon");
            return data ?? Enumerable.Empty<object>();
        }

        // ====== Cập nhật trạng thái + lý do ======
        public async Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy)
        {
            var payload = new { TrangThai = trangThaiDb, LyDoHuy = lyDoHuy };
            var resp = await _http.PatchAsJsonAsync($"api/HoaDon/{hoaDonId}/TrangThai", payload);
            return resp.IsSuccessStatusCode;
        }

        // ====== NEW: Hủy + restock ======
        public async Task<bool> CancelWithRestockAsync(int hoaDonId, string lyDo, List<(int chiTietId, int soLuong)> selections)
        {
            var payload = new
            {
                lyDo,
                items = selections.Select(s => new
                {
                    hoaDonChiTietId = s.chiTietId,
                    quantity = s.soLuong
                })
            };

            var resp = await _http.PostAsJsonAsync($"api/HoaDon/{hoaDonId}/cancel-with-restock", payload);
            if (!resp.IsSuccessStatusCode) return false;

            // response: { ok: true }
            var obj = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            return obj != null && obj.TryGetValue("ok", out var ok) && ok?.ToString() == "True";
        }
    }
}
