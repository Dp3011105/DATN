using BE.DTOs;
using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Gio_HangController : ControllerBase
    // Cái này dùng cho chức năng lấy danh sách dữ liệu giỏ hàng của khách hàng (Phước) 
    // controller này là toàn bộ về giỏ hàng của khách hàng như thêm sản phầm vào giỏ hàng , lấy dữ liệu giỏ hàng của khách hàng , cập nhật giỏ hàng của khách hàng, xóa sản phẩm trong giỏ hàng của khách hàng

    {
        private readonly IGio_HangRepository _gioHangRepository;

        public Gio_HangController(IGio_HangRepository gioHangRepository)
        {
            _gioHangRepository = gioHangRepository;
        }



        [HttpGet("{idKhachHang}")]
        public async Task<IActionResult> GetGioHangByKhachHang(int idKhachHang)
        {
            var gioHang = await _gioHangRepository.GetGioHangByKhachHangIdAsync(idKhachHang);

            if (gioHang == null)
            {
                gioHang = new Gio_Hang
                {
                    ID_Khach_Hang = idKhachHang,
                    Ngay_Tao = DateTime.Now,
                    Ngay_Cap_Nhat = DateTime.Now,
                    Trang_Thai = true
                };
                await _gioHangRepository.CreateGioHangAsync(gioHang);
            }

            var gioHangDto = new GioHangDTO
            {
                ID_Gio_Hang = gioHang.ID_Gio_Hang,
                ID_Khach_Hang = gioHang.ID_Khach_Hang,
                Ngay_Tao = gioHang.Ngay_Tao,
                Ngay_Cap_Nhat = gioHang.Ngay_Cap_Nhat,
                Trang_Thai = gioHang.Trang_Thai,
                GioHang_ChiTiets = gioHang.GioHang_ChiTiets?.Select(ct => new GioHangChiTietDTO
                {
                    ID_GioHang_ChiTiet = ct.ID_GioHang_ChiTiet,
                    ID_Gio_Hang = ct.ID_Gio_Hang,
                    ID_San_Pham = ct.ID_San_Pham,
                    ID_Size = ct.ID_Size,
                    ID_SanPham_DoNgot = ct.ID_SanPham_DoNgot,
                    ID_LuongDa = ct.ID_LuongDa,
                    Ma_GioHang_ChiTiet = ct.Ma_GioHang_ChiTiet,
                    So_Luong = ct.So_Luong,
                    Ghi_Chu = ct.Ghi_Chu,
                    Ngay_Tao = ct.Ngay_Tao,
                    San_Pham = ct.San_Pham != null ? new SanPhamDTO
                    {
                        ID_San_Pham = ct.San_Pham.ID_San_Pham,
                        Ten_San_Pham = ct.San_Pham.Ten_San_Pham,
                        Gia = ct.San_Pham.Gia,
                        So_Luong = ct.San_Pham.So_Luong,
                        Hinh_Anh = ct.San_Pham.Hinh_Anh,
                        Mo_Ta = ct.San_Pham.Mo_Ta,
                        Trang_Thai = ct.San_Pham.Trang_Thai,
                        Sizes = ct.San_Pham.SanPhamSizes?.Select(s => s.ID_Size).ToList() ?? new List<int>(),
                        LuongDas = ct.San_Pham.SanPhamLuongDas?.Select(ld => ld.ID_LuongDa).ToList() ?? new List<int>(),
                        DoNgots = ct.San_Pham.SanPhamDoNgots?.Select(dn => dn.ID_DoNgot).ToList() ?? new List<int>(),
                        Toppings = ct.San_Pham.SanPhamToppings?.Select(t => t.ID_Topping).ToList() ?? new List<int>()
                    } : null,
                    Size = ct.Size != null ? new SizeDTO
                    {
                        ID_Size = ct.Size.ID_Size,
                        SizeName = ct.Size.SizeName,
                        Gia = ct.Size.Gia,
                        Trang_Thai = ct.Size.Trang_Thai
                    } : null,
                    DoNgot = ct.DoNgot != null ? new DoNgotDTO
                    {
                        ID_DoNgot = ct.DoNgot.ID_DoNgot,
                        Muc_Do = ct.DoNgot.Muc_Do,
                        Trang_Thai = ct.DoNgot.Trang_Thai
                    } : null,
                    LuongDa = ct.LuongDa != null ? new LuongDaDTO
                    {
                        ID_LuongDa = ct.LuongDa.ID_LuongDa,
                        Ten_LuongDa = ct.LuongDa.Ten_LuongDa,
                        Trang_Thai = ct.LuongDa.Trang_Thai
                    } : null,
                    GioHangChiTiet_Toppings = ct.GioHangChiTiet_Toppings?.Select(t => new GioHangChiTietToppingDTO
                    {
                        ID_GioHangChiTiet_Topping = t.ID_GioHangChiTiet_Topping,
                        ID_GioHang_ChiTiet = t.ID_GioHang_ChiTiet,
                        ID_Topping = t.ID_Topping,
                        Topping = t.Topping != null ? new ToppingDTO
                        {
                            ID_Topping = t.Topping.ID_Topping,
                            Ten = t.Topping.Ten,
                            Gia = t.Topping.Gia,
                            So_Luong = t.Topping.So_Luong,
                            Hinh_Anh = t.Topping.Hinh_Anh,
                            Ghi_Chu = t.Topping.Ghi_Chu,
                            Trang_Thai = t.Topping.Trang_Thai
                        } : null
                    }).ToList() ?? new List<GioHangChiTietToppingDTO>()
                }).ToList() ?? new List<GioHangChiTietDTO>()
            };

            return Ok(gioHangDto);
        }




        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gioHang = await _gioHangRepository.GetGioHangByKhachHangIdAsync(addToCartDto.ID_Khach_Hang);

            if (gioHang == null)
            {
                gioHang = new Gio_Hang
                {
                    ID_Khach_Hang = addToCartDto.ID_Khach_Hang,
                    Ngay_Tao = DateTime.Now,
                    Ngay_Cap_Nhat = DateTime.Now,
                    Trang_Thai = true
                };
                await _gioHangRepository.CreateGioHangAsync(gioHang);
            }

            // Cập nhật ngày cập nhật giỏ hàng
            gioHang.Ngay_Cap_Nhat = DateTime.Now;

            // Tạo chi tiết giỏ hàng mới
            var gioHangChiTiet = new GioHang_ChiTiet
            {
                ID_Gio_Hang = gioHang.ID_Gio_Hang,
                ID_San_Pham = addToCartDto.ID_San_Pham,
                ID_Size = addToCartDto.ID_Size,
                ID_SanPham_DoNgot = addToCartDto.ID_DoNgot,
                ID_LuongDa = addToCartDto.ID_LuongDa,
                Ma_GioHang_ChiTiet = Guid.NewGuid().ToString().Substring(0, 10), // Generate mã ngẫu nhiên
                So_Luong = addToCartDto.So_Luong,
                Ghi_Chu = addToCartDto.Ghi_Chu,
                Ngay_Tao = DateTime.Now
            };

            // Thêm toppings nếu có
            gioHangChiTiet.GioHangChiTiet_Toppings = addToCartDto.Toppings?.Select(toppingId => new GioHangChiTiet_Topping
            {
                ID_Topping = toppingId
            }).ToList() ?? new List<GioHangChiTiet_Topping>();

            await _gioHangRepository.AddGioHangChiTietAsync(gioHangChiTiet);

            return Ok(new { Message = "Sản phẩm đã được thêm vào giỏ hàng", ID_GioHang_ChiTiet = gioHangChiTiet.ID_GioHang_ChiTiet });
        }






        [HttpDelete("DeleteFromCart/{idKhachHang}/{idGioHangChiTiet}")]
        public async Task<IActionResult> DeleteFromCart(int idKhachHang, int idGioHangChiTiet)
        {
            var result = await _gioHangRepository.DeleteGioHangChiTietAsync(idGioHangChiTiet, idKhachHang);

            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy sản phẩm trong giỏ hàng hoặc sản phẩm không thuộc khách hàng này." });
            }

            return Ok(new { Message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
        }



    }
}