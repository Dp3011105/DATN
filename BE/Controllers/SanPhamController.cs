using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepository;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Images_SanPham");

        public SanPhamController(ISanPhamRepository sanPhamRepository)
        {
            _sanPhamRepository = sanPhamRepository;
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        // ---------- GET ALL ----------
        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        //public async Task<ActionResult> GetAllSanPham()
        //{
        //    var sanPhams = await _sanPhamRepository.GetAllWithDetailsAsync();
        //    var result = sanPhams.Select(sp => new
        //    {
        //        iD_San_Pham = sp.ID_San_Pham,
        //        ten_San_Pham = sp.Ten_San_Pham,
        //        gia = sp.Gia,
        //        so_Luong = sp.So_Luong,
        //        hinh_Anh = sp.Hinh_Anh,
        //        mo_Ta = sp.Mo_Ta,
        //        trang_Thai = sp.Trang_Thai,
        //        sizes = sp.SanPhamSizes.Select(sps => new
        //        {
        //            iD_Size = sps.Size.ID_Size,
        //            sizeName = sps.Size.SizeName,
        //            trang_Thai = sps.Size.Trang_Thai
        //        }).ToList(),
        //        luongDas = sp.SanPhamLuongDas.Select(spld => new
        //        {
        //            iD_LuongDa = spld.LuongDa.ID_LuongDa,
        //            ten_LuongDa = spld.LuongDa.Ten_LuongDa,
        //            trang_Thai = spld.LuongDa.Trang_Thai
        //        }).ToList(),
        //        doNgots = sp.SanPhamDoNgots.Select(spdn => new
        //        {
        //            iD_DoNgot = spdn.DoNgot.ID_DoNgot,
        //            muc_Do = spdn.DoNgot.Muc_Do,
        //            trang_Thai = spdn.DoNgot.Trang_Thai
        //        }).ToList(),
        //        toppings = sp.SanPhamToppings.Select(spt => new
        //        {
        //            iD_Topping = spt.Topping.ID_Topping,
        //            ten = spt.Topping.Ten,
        //            so_Luong = spt.Topping.So_Luong,
        //            gia = spt.Topping.Gia,
        //            trang_Thai = spt.Topping.Trang_Thai
        //        }).ToList(),
        //        khuyenMais = sp.SanPhamKhuyenMais.Select(spkm => new
        //        {
        //            gia_Giam = spkm.Gia_Giam,
        //            ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
        //            ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
        //            ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
        //        }).ToList()
        //    }).ToList();

        //    return Ok(result);
        //}


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllSanPham()
        {
            var sanPhams = await _sanPhamRepository.GetAllWithDetailsAsync();
            var result = sanPhams.Select(sp => new
            {
                iD_San_Pham = sp.ID_San_Pham,
                ten_San_Pham = sp.Ten_San_Pham,
                gia = sp.Gia,
                so_Luong = sp.So_Luong,
                hinh_Anh = sp.Hinh_Anh,
                mo_Ta = sp.Mo_Ta,
                trang_Thai = sp.Trang_Thai,
                sizes = sp.SanPhamSizes.Select(sps => new
                {
                    iD_Size = sps.Size.ID_Size,
                    sizeName = sps.Size.SizeName,
                    trang_Thai = sps.Size.Trang_Thai
                }).ToList(),
                luongDas = sp.SanPhamLuongDas.Select(spld => new
                {
                    iD_LuongDa = spld.LuongDa.ID_LuongDa,
                    ten_LuongDa = spld.LuongDa.Ten_LuongDa,
                    trang_Thai = spld.LuongDa.Trang_Thai
                }).ToList(),
                doNgots = sp.SanPhamDoNgots.Select(spdn => new
                {
                    iD_DoNgot = spdn.DoNgot.ID_DoNgot,
                    muc_Do = spdn.DoNgot.Muc_Do,
                    trang_Thai = spdn.DoNgot.Trang_Thai
                }).ToList(),
                toppings = sp.SanPhamToppings.Select(spt => new
                {
                    iD_Topping = spt.Topping.ID_Topping,
                    ten = spt.Topping.Ten,
                    so_Luong = spt.Topping.So_Luong,
                    gia = spt.Topping.Gia,
                    trang_Thai = spt.Topping.Trang_Thai
                }).ToList(),
                khuyenMais = sp.SanPhamKhuyenMais.Select(spkm => new
                {
                    iD_Khuyen_Mai = spkm.ID_Khuyen_Mai, // Thêm ID khuyến mãi
                    gia_Giam = spkm.Gia_Giam,
                    ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                    ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                    ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // ---------- GET BY ID ----------
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSanPhamById(int id)
        {
            var sanPham = await _sanPhamRepository.GetByIdWithDetailsAsync(id);
            if (sanPham == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            var result = new
            {
                iD_San_Pham = sanPham.ID_San_Pham,
                ten_San_Pham = sanPham.Ten_San_Pham,
                gia = sanPham.Gia,
                so_Luong = sanPham.So_Luong,
                hinh_Anh = sanPham.Hinh_Anh,
                mo_Ta = sanPham.Mo_Ta,
                trang_Thai = sanPham.Trang_Thai,
                sizes = sanPham.SanPhamSizes.Select(sps => new
                {
                    iD_Size = sps.Size.ID_Size,
                    sizeName = sps.Size.SizeName,
                    trang_Thai = sps.Size.Trang_Thai
                }).ToList(),
                luongDas = sanPham.SanPhamLuongDas.Select(spld => new
                {
                    iD_LuongDa = spld.LuongDa.ID_LuongDa,
                    ten_LuongDa = spld.LuongDa.Ten_LuongDa,
                    trang_Thai = spld.LuongDa.Trang_Thai
                }).ToList(),
                doNgots = sanPham.SanPhamDoNgots.Select(spdn => new
                {
                    iD_DoNgot = spdn.DoNgot.ID_DoNgot,
                    muc_Do = spdn.DoNgot.Muc_Do,
                    trang_Thai = spdn.DoNgot.Trang_Thai
                }).ToList(),
                toppings = sanPham.SanPhamToppings.Select(spt => new
                {
                    iD_Topping = spt.Topping.ID_Topping,
                    ten = spt.Topping.Ten,
                    so_Luong = spt.Topping.So_Luong,
                    gia = spt.Topping.Gia,
                    trang_Thai = spt.Topping.Trang_Thai
                }).ToList(),
                khuyenMais = sanPham.SanPhamKhuyenMais.Select(spkm => new
                {
                    gia_Giam = spkm.Gia_Giam,
                    ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                    ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                    ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
                }).ToList()
            };

            return Ok(result);
        }

        // ---------- UPLOAD IMAGE ----------
        [HttpPost("upload-image")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { message = "Hình ảnh là bắt buộc" });

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await image.CopyToAsync(stream);

            var imagePath = $"/uploads/Images_SanPham/{fileName}";
            return Ok(imagePath);
        }

        // ---------- DELETE IMAGE ----------
        [HttpDelete("delete-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteImage([FromQuery] string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return BadRequest(new { message = "Đường dẫn hình ảnh không được để trống." });

            imagePath = imagePath.TrimStart('/');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound(new { message = "Không tìm thấy hình ảnh." });

            try
            {
                System.IO.File.Delete(fullPath);
                return Ok(new { message = "Đã xóa hình ảnh thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa hình ảnh.", error = ex.Message });
            }
        }

        // ---------- CREATE ----------
        [HttpPost]
        [ProducesResponseType(typeof(SanPhamDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SanPhamDTO>> CreateSanPham([FromBody] SanPhamDTO sanPhamDTO)
        {
            if (string.IsNullOrEmpty(sanPhamDTO.Hinh_Anh))
                return BadRequest(new { message = "Đường dẫn hình ảnh là bắt buộc khi thêm sản phẩm" });

            var sanPham = await _sanPhamRepository.CreateSanPhamAsync(sanPhamDTO, sanPhamDTO.Hinh_Anh);
            return CreatedAtAction(nameof(GetSanPhamById), new { id = sanPham.ID_San_Pham }, sanPham);
        }

        // ---------- UPDATE ----------
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SanPhamDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SanPhamDTO>> UpdateSanPham(int id, [FromBody] SanPhamDTO sanPhamDTO)
        {
            if (sanPhamDTO.ID_San_Pham != null && sanPhamDTO.ID_San_Pham != id)
                return BadRequest(new { message = "ID sản phẩm không khớp" });

            var sanPham = await _sanPhamRepository.UpdateSanPhamAsync(id, sanPhamDTO, sanPhamDTO.Hinh_Anh);
            if (sanPham == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            return Ok(sanPham);
        }

        // =====================================================================
        // ===============  EXISTENCE / STOCK  =================================
        // =====================================================================

        public class AdjustStockItem
        {
            public int ID_San_Pham { get; set; }
            public int SoLuongTru { get; set; }
        }

        /// <summary>
        /// Kiểm tra danh sách đặt hàng có vượt tồn không.
        /// </summary>
        [HttpPost("kiem-tra-ton")]
        public async Task<IActionResult> KiemTraTon([FromBody] List<AdjustStockItem> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { success = false, message = "Danh sách trống." });

            var errors = new List<string>();

            foreach (var it in items)
            {
                if (it.SoLuongTru <= 0)
                {
                    errors.Add($"SP#{it.ID_San_Pham}: số lượng không hợp lệ.");
                    continue;
                }

                var sp = await _sanPhamRepository.GetByIdWithDetailsAsync(it.ID_San_Pham);
                if (sp == null)
                {
                    errors.Add($"SP#{it.ID_San_Pham} không tồn tại.");
                    continue;
                }

                if (sp.So_Luong < it.SoLuongTru)
                    errors.Add($"\"{sp.Ten_San_Pham}\": tồn {sp.So_Luong} < đặt {it.SoLuongTru}");
            }

            if (errors.Any())
                return Ok(new { success = false, message = string.Join("\n", errors) });

            return Ok(new { success = true });
        }

        /// <summary>
        /// Trừ tồn kho theo danh sách sản phẩm.
        /// FE nên gọi cái này ngay sau khi tạo hóa đơn thành công.
        /// </summary>
        [HttpPost("tru-ton")]
        public async Task<IActionResult> TruTon([FromBody] List<AdjustStockItem> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { success = false, message = "Danh sách trống." });

            var errors = new List<string>();
            var updated = new List<object>();

            // kiểm tra trước
            foreach (var it in items)
            {
                if (it.SoLuongTru <= 0)
                {
                    errors.Add($"SP#{it.ID_San_Pham}: số lượng trừ không hợp lệ.");
                    continue;
                }

                var sp = await _sanPhamRepository.GetByIdWithDetailsAsync(it.ID_San_Pham);
                if (sp == null)
                {
                    errors.Add($"SP#{it.ID_San_Pham} không tồn tại.");
                    continue;
                }

                if (sp.So_Luong < it.SoLuongTru)
                {
                    errors.Add($"\"{sp.Ten_San_Pham}\": tồn {sp.So_Luong} < trừ {it.SoLuongTru}");
                    continue;
                }
            }

            if (errors.Any())
                return BadRequest(new { success = false, message = string.Join("\n", errors) });

            // cập nhật tồn
            foreach (var it in items)
            {
                var sp = await _sanPhamRepository.GetByIdWithDetailsAsync(it.ID_San_Pham);
                if (sp == null) continue;

                var newQty = sp.So_Luong - it.SoLuongTru;

                // Dùng UpdateSanPhamAsync để chỉ đổi So_Luong, giữ nguyên field khác
                var dto = new SanPhamDTO
                {
                    ID_San_Pham = sp.ID_San_Pham,
                    Ten_San_Pham = sp.Ten_San_Pham,
                    Gia = sp.Gia,
                    So_Luong = newQty,
                    Hinh_Anh = sp.Hinh_Anh,
                    Mo_Ta = sp.Mo_Ta,
                    Trang_Thai = sp.Trang_Thai
                };

                var after = await _sanPhamRepository.UpdateSanPhamAsync(sp.ID_San_Pham, dto, sp.Hinh_Anh);
                updated.Add(new { id = sp.ID_San_Pham, old = sp.So_Luong, @new = after?.So_Luong ?? newQty });
            }

            return Ok(new { success = true, message = "Đã trừ tồn.", updated });
        }
    }
}
