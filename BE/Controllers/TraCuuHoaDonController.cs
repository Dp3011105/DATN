using BE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraCuuHoaDonController : ControllerBase
    {
        private readonly MyDbContext _context; // Replace with your actual DbContext

        public TraCuuHoaDonController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("check-hoa-don")]
        public async Task<IActionResult> CheckHoaDon([FromBody] CheckHoaDonRequest request)
        {
            if (string.IsNullOrEmpty(request.MaHoaDon) || string.IsNullOrEmpty(request.SoDienThoai))
            {
                return BadRequest(new { message = "Mã hóa đơn và số điện thoại không được để trống" });
            }

            // Tìm hóa đơn theo Ma_Hoa_Don
            var hoaDon = await _context.Hoa_Don
                .Where(hd => hd.Ma_Hoa_Don == request.MaHoaDon)
                .Select(hd => new
                {
                    hd.Ma_Hoa_Don,
                    hd.Ghi_Chu,
                    hd.Dia_Chi_Tu_Nhap,
                    hd.Ngay_Tao,
                    hd.Tong_Tien,
                    hd.Trang_Thai
                })
                .FirstOrDefaultAsync();

            if (hoaDon == null)
            {
                return NotFound(new { message = "Không tìm thấy hóa đơn với mã này" });
            }

            // Kiểm tra Ghi_Chu có chứa số điện thoại hay không
            if (string.IsNullOrEmpty(hoaDon.Ghi_Chu))
            {
                return BadRequest(new { message = "Hóa đơn không có ghi chú" });
            }

            // Sử dụng regex để tìm số điện thoại trong Ghi_Chu
            var phoneMatch = Regex.Match(hoaDon.Ghi_Chu, @"Số Điện Thoại: (\d{10})");
            if (!phoneMatch.Success)
            {
                return BadRequest(new { message = "Ghi chú không chứa số điện thoại hợp lệ" });
            }

            // So sánh số điện thoại
            if (phoneMatch.Groups[1].Value != request.SoDienThoai)
            {
                return BadRequest(new { message = "Số điện thoại không khớp" });
            }

            // Trả về chi tiết hóa đơn
            return Ok(new
            {
                hoaDon.Ma_Hoa_Don,
                hoaDon.Dia_Chi_Tu_Nhap,
                hoaDon.Ngay_Tao,
                hoaDon.Tong_Tien,
                hoaDon.Trang_Thai,
                hoaDon.Ghi_Chu
            });
        }
    }

    public class CheckHoaDonRequest
    {
        public string MaHoaDon { get; set; }
        public string SoDienThoai { get; set; }
    }
}
