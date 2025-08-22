using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Gio_HangController : ControllerBase
    {

        private readonly IGio_HangRepository _cartRepository;

        public Gio_HangController(IGio_HangRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{idKhachHang}")]
        public async Task<IActionResult> GetCartByCustomerId(int idKhachHang)
        {
           

            // Lấy giỏ hàng của khách hàng
            var gioHang = await _cartRepository.GetCartByCustomerIdAsync(idKhachHang);
            if (gioHang == null || gioHang.GioHang_ChiTiets == null || !gioHang.GioHang_ChiTiets.Any())
            {
                return Ok(new { message = "Không có dữ liệu giỏ hàng." });
            }

            // Chuẩn bị danh sách chi tiết giỏ hàng với giá sản phẩm
            var cartDetails = new List<object>();
            var currentDate = DateTime.Now;

            foreach (var chiTiet in gioHang.GioHang_ChiTiets)
            {
                // Kiểm tra khuyến mãi của sản phẩm
                var khuyenMai = await _cartRepository.GetActivePromotionForProductAsync(chiTiet.ID_San_Pham, currentDate);

                // Luôn lấy giá gốc từ SanPham
                decimal giaGoc = chiTiet.San_Pham.Gia ?? 0;

                // Xác định giá hiển thị (giá giảm nếu có, иначе giá gốc)
                decimal giaHienThi = khuyenMai != null && khuyenMai.Gia_Giam.HasValue
                    ? khuyenMai.Gia_Giam.Value
                    : giaGoc;

                // Tạo object chứa thông tin chi tiết giỏ hàng
                var cartDetail = new
                {
                    chiTiet.ID_GioHang_ChiTiet,
                    chiTiet.ID_Gio_Hang,
                    chiTiet.ID_San_Pham,
                    Ten_San_Pham = chiTiet.San_Pham.Ten_San_Pham,
                    chiTiet.ID_Size,
                    Ten_Size = chiTiet.Size?.SizeName,
                    chiTiet.ID_SanPham_DoNgot,
                    Ten_DoNgot = chiTiet.DoNgot?.Muc_Do,
                    chiTiet.ID_LuongDa,
                    Ten_LuongDa = chiTiet.LuongDa?.Ten_LuongDa,
                    Toppings = chiTiet.GioHangChiTiet_Toppings.Select(t => new
                    {
                        t.ID_Topping,
                        Ten_Topping = t.Topping.Ten,
                        Gia_Topping = t.Topping.Gia, // Thêm giá của topping
                    }).ToList(),
                    chiTiet.So_Luong,
                    Gia_Goc = giaGoc,
                    Gia_Hien_Thi = giaHienThi,
                    chiTiet.Ghi_Chu,
                    chiTiet.Ngay_Tao,
                    Khuyen_Mai = khuyenMai != null ? new
                    {
                        khuyenMai.ID_Khuyen_Mai,
                        khuyenMai.BangKhuyenMai.Ten_Khuyen_Mai,
                        khuyenMai.Phan_Tram_Giam,
                        khuyenMai.Gia_Giam
                    } : null
                };

                cartDetails.Add(cartDetail);
            }

            // Trả về thông tin giỏ hàng
            var response = new
            {
                ID_Gio_Hang = gioHang.ID_Gio_Hang,
                ID_Khach_Hang = gioHang.ID_Khach_Hang,
                Ngay_Tao = gioHang.Ngay_Tao,
                Ngay_Cap_Nhat = gioHang.Ngay_Cap_Nhat,
                Trang_Thai = gioHang.Trang_Thai,
                Chi_Tiet_Gio_Hang = cartDetails
            };

            return Ok(response);
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDto request)
        {
            // Kiểm tra dữ liệu đầu vào cơ bản
            if (request == null || request.ID_San_Pham <= 0 || request.ID_Khach_Hang <= 0 || request.So_Luong <= 0)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ." });
            }

            // Tìm hoặc tạo giỏ hàng
            var gioHang = await _cartRepository.GetCartByCustomerIdAsync(request.ID_Khach_Hang);
            if (gioHang == null)
            {
                gioHang = new Gio_Hang
                {
                    ID_Khach_Hang = request.ID_Khach_Hang,
                    Ngay_Tao = DateTime.Now,
                    Ngay_Cap_Nhat = DateTime.Now,
                    Trang_Thai = true,
                    GioHang_ChiTiets = new List<GioHang_ChiTiet>()
                };
                await _cartRepository.AddCartAsync(gioHang);
            }
            else
            {
                gioHang.Ngay_Cap_Nhat = DateTime.Now;
                await _cartRepository.UpdateCartAsync(gioHang);
            }

            // Tạo chi tiết giỏ hàng
            var gioHangChiTiet = new GioHang_ChiTiet
            {
                ID_Gio_Hang = gioHang.ID_Gio_Hang,
                ID_San_Pham = request.ID_San_Pham,
                ID_Size = request.ID_Size,
                ID_SanPham_DoNgot = request.ID_SanPham_DoNgot,
                ID_LuongDa = request.ID_LuongDa,
                Ma_GioHang_ChiTiet = $"GHCT_{Guid.NewGuid().ToString().Substring(0, 8)}",
                So_Luong = request.So_Luong,
                Ghi_Chu = request.Ghi_Chu,
                Ngay_Tao = DateTime.Now,
                GioHangChiTiet_Toppings = (request.ID_Toppings ?? new List<int>()).Select(t => new GioHangChiTiet_Topping
                {
                    ID_Topping = t
                }).ToList()
            };

            await _cartRepository.AddCartDetailAsync(gioHangChiTiet);

            // Tạo DTO để trả về
            var response = new CartDetailResponseDto
            {
                ID_GioHang_ChiTiet = gioHangChiTiet.ID_GioHang_ChiTiet,
                ID_Gio_Hang = gioHangChiTiet.ID_Gio_Hang,
                ID_San_Pham = gioHangChiTiet.ID_San_Pham,
                ID_Size = gioHangChiTiet.ID_Size,
                ID_SanPham_DoNgot = gioHangChiTiet.ID_SanPham_DoNgot,
                ID_LuongDa = gioHangChiTiet.ID_LuongDa,
                Toppings = gioHangChiTiet.GioHangChiTiet_Toppings.Select(t => new ToppingDTO
                {
                    ID_Topping = t.ID_Topping
                }).ToList(),
                So_Luong = gioHangChiTiet.So_Luong,
                Ghi_Chu = gioHangChiTiet.Ghi_Chu,
                Ngay_Tao = gioHangChiTiet.Ngay_Tao
            };

            return Ok(new
            {
                message = "Thêm vào giỏ hàng thành công.",
                GioHang_ChiTiet = response
            });
        }



        [HttpDelete("delete/{idGioHangChiTiet}")]
        public async Task<IActionResult> DeleteCartDetail(int idGioHangChiTiet)
        {
            // Kiểm tra dữ liệu đầu vào cơ bản
            if (idGioHangChiTiet <= 0)
            {
                return BadRequest(new { message = "ID_GioHang_ChiTiet không hợp lệ." });
            }

            // Xóa chi tiết giỏ hàng
            var result = await _cartRepository.DeleteCartDetailAsync(idGioHangChiTiet);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy chi tiết giỏ hàng để xóa." });
            }

            return Ok(new { message = "Xóa chi tiết giỏ hàng thành công." });
        }



    }
}
