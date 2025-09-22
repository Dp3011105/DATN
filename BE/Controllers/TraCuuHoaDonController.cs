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
                    hd.ID_Hoa_Don,
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
                hoaDon.ID_Hoa_Don,
                hoaDon.Dia_Chi_Tu_Nhap,
                hoaDon.Ngay_Tao,
                hoaDon.Tong_Tien,
                hoaDon.Trang_Thai,
                hoaDon.Ghi_Chu
            });
        }




        [HttpPost("hoan-tra/{idHoaDon}")]
        public async Task<IActionResult> HoanTraSanPhamVaTopping(int idHoaDon, [FromBody] HoanTraRequest request)
        {
            if (string.IsNullOrEmpty(request?.LyDoHuyDon))
            {
                return BadRequest(new { message = "Lý do hủy đơn hàng là bắt buộc" });
            }

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Truy vấn thông tin hóa đơn cùng với chi tiết sản phẩm và topping
                var invoice = await _context.Hoa_Don
                    .Include(h => h.HoaDonChiTiets)
                        .ThenInclude(hdct => hdct.SanPham)
                    .Include(h => h.HoaDonChiTiets)
                        .ThenInclude(hdct => hdct.HoaDonChiTietToppings)
                        .ThenInclude(hdctt => hdctt.Topping)
                    .FirstOrDefaultAsync(h => h.ID_Hoa_Don == idHoaDon);

                if (invoice == null)
                {
                    return NotFound(new { message = $"Không tìm thấy hóa đơn với ID {idHoaDon}" });
                }

                // Đổi trạng thái hóa đơn thành Huy_Don và lưu lý do
                invoice.Trang_Thai = "Huy_Don";
                invoice.LyDoHuyDon = request.LyDoHuyDon;
                _context.Hoa_Don.Update(invoice);

                // Hoàn trả số lượng sản phẩm
                foreach (var hdct in invoice.HoaDonChiTiets)
                {
                    if (hdct.SanPham != null)
                    {
                        hdct.SanPham.So_Luong += hdct.So_Luong;
                        _context.San_Pham.Update(hdct.SanPham);
                    }

                    // Hoàn trả số lượng topping
                    foreach (var hdctt in hdct.HoaDonChiTietToppings)
                    {
                        if (hdctt.Topping != null)
                        {
                            int soLuongTopping = hdctt.So_Luong ?? 0;
                            hdctt.Topping.So_Luong += soLuongTopping;
                            _context.Topping.Update(hdctt.Topping);
                        }
                    }
                }

                // Lưu tất cả thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { message = $"Hủy hóa đơn {idHoaDon} và hoàn trả số lượng sản phẩm, topping thành công" });
            }
            catch (Exception ex)
            {
                // Rollback giao dịch nếu có lỗi
                await tx.RollbackAsync();
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu", error = ex.Message });
            }
        }


    }

    public class CheckHoaDonRequest
    {
        public string MaHoaDon { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class HoanTraRequest
    {
        public string LyDoHuyDon { get; set; }
    }
}
