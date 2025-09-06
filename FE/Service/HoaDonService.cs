using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BE.models;
using BE.DTOs;
using Service.IService;

namespace Service.Services // namespace của bạn
{
    public class HoaDonService : IHoaDonService
    {
        private readonly HttpClient _http;

        public HoaDonService(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7169/"); // chỉnh theo API của bạn
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

        // ====== THÊM MỚI (trả DTO cho danh sách) ======
        public async Task<IEnumerable<HoaDonDTO>> GetAllListAsync()
        {
            var data = await _http.GetFromJsonAsync<IEnumerable<HoaDonDTO>>("api/HoaDon");
            return data ?? Enumerable.Empty<HoaDonDTO>();
        }

        // ====== THÊM MỚI (cập nhật trạng thái + lý do hủy) ======
        public async Task<bool> UpdateTrangThaiAsync(int hoaDonId, string trangThaiDb, string? lyDoHuy)
        {
            var payload = new { TrangThai = trangThaiDb, LyDoHuy = lyDoHuy };
            var resp = await _http.PatchAsJsonAsync($"api/HoaDon/{hoaDonId}/TrangThai", payload);
            return resp.IsSuccessStatusCode;
        }
    }
}
