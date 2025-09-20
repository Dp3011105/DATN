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
        //            iD_Khuyen_Mai = spkm.ID_Khuyen_Mai,
        //            gia_Giam = spkm.Gia_Giam,
        //            ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
        //            ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
        //            ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
        //        }).ToList()
        //    }).ToList();

        //    return Ok(result);
        //}



        // ---------- GET ALL ----------
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
                khuyenMais = sp.SanPhamKhuyenMais//Lọc Khuyến Mãi ở đây nhé ae , ko có ở repository đâu
                    .Where(spkm => spkm.BangKhuyenMai.Trang_Thai)
                    .Select(spkm => new
                    {
                        iD_Khuyen_Mai = spkm.ID_Khuyen_Mai,
                        gia_Giam = spkm.Gia_Giam,
                        ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                        ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                        ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
                    }).ToList()
            }).ToList();

            return Ok(result);
        }

        // ---------- GET BY ID ----------
        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> GetSanPhamById(int id)
        //{
        //    var sanPham = await _sanPhamRepository.GetByIdWithDetailsAsync(id);
        //    if (sanPham == null)
        //        return NotFound(new { message = "Sản phẩm không tồn tại" });

        //    var result = new
        //    {
        //        iD_San_Pham = sanPham.ID_San_Pham,
        //        ten_San_Pham = sanPham.Ten_San_Pham,
        //        gia = sanPham.Gia,
        //        so_Luong = sanPham.So_Luong,
        //        hinh_Anh = sanPham.Hinh_Anh,
        //        mo_Ta = sanPham.Mo_Ta,
        //        trang_Thai = sanPham.Trang_Thai,
        //        sizes = sanPham.SanPhamSizes.Select(sps => new
        //        {
        //            iD_Size = sps.Size.ID_Size,
        //            sizeName = sps.Size.SizeName,
        //            trang_Thai = sps.Size.Trang_Thai
        //        }).ToList(),
        //        luongDas = sanPham.SanPhamLuongDas.Select(spld => new
        //        {
        //            iD_LuongDa = spld.LuongDa.ID_LuongDa,
        //            ten_LuongDa = spld.LuongDa.Ten_LuongDa,
        //            trang_Thai = spld.LuongDa.Trang_Thai
        //        }).ToList(),
        //        doNgots = sanPham.SanPhamDoNgots.Select(spdn => new
        //        {
        //            iD_DoNgot = spdn.DoNgot.ID_DoNgot,
        //            muc_Do = spdn.DoNgot.Muc_Do,
        //            trang_Thai = spdn.DoNgot.Trang_Thai
        //        }).ToList(),
        //        toppings = sanPham.SanPhamToppings.Select(spt => new
        //        {
        //            iD_Topping = spt.Topping.ID_Topping,
        //            ten = spt.Topping.Ten,
        //            so_Luong = spt.Topping.So_Luong,
        //            gia = spt.Topping.Gia,
        //            trang_Thai = spt.Topping.Trang_Thai
        //        }).ToList(),
        //        khuyenMais = sanPham.SanPhamKhuyenMais.Select(spkm => new
        //        {
        //            gia_Giam = spkm.Gia_Giam,
        //            ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
        //            ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
        //            ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
        //        }).ToList()
        //    };

        //    return Ok(result);
        //}


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
                khuyenMais = sanPham.SanPhamKhuyenMais
                    .Where(spkm => spkm.BangKhuyenMai.Trang_Thai
                                && spkm.BangKhuyenMai.Ngay_Bat_Dau <= DateTime.Now
                                && spkm.BangKhuyenMai.Ngay_Ket_Thuc >= DateTime.Now)
                    .Select(spkm => new
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

            // Gộp theo ID_San_Pham để check đúng tổng số lượng đặt
            var grouped = items
                .Where(x => x != null)
                .GroupBy(x => x.ID_San_Pham)
                .Select(g => new AdjustStockItem
                {
                    ID_San_Pham = g.Key,
                    SoLuongTru = g.Sum(x => Math.Max(0, x.SoLuongTru))
                })
                .ToList();

            foreach (var it in grouped)
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
        /// Trừ tồn kho theo danh sách sản phẩm (AN TOÀN – chỉ cập nhật cột So_Luong).
        /// FE nên gọi cái này ngay sau khi tạo hóa đơn thành công.
        /// </summary>
        [HttpPost("tru-ton")]
        public async Task<IActionResult> TruTon([FromBody] List<AdjustStockItem> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { success = false, message = "Danh sách trống." });

            // Gộp items theo sản phẩm để tránh trừ lặp
            var grouped = items
                .Where(x => x != null)
                .GroupBy(x => x.ID_San_Pham)
                .Select(g => new AdjustStockItem
                {
                    ID_San_Pham = g.Key,
                    SoLuongTru = g.Sum(x => Math.Max(0, x.SoLuongTru))
                })
                .ToList();

            var errors = new List<string>();
            var updated = new List<object>();

            // 1) Kiểm tra đủ tồn trước
            foreach (var it in grouped)
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
                    errors.Add($"\"{sp.Ten_San_Pham}\": tồn {sp.So_Luong} < trừ {it.SoLuongTru}");
            }

            if (errors.Any())
                return BadRequest(new { success = false, message = string.Join("\n", errors) });

            // 2) Cập nhật tồn – CHỈ đổi cột So_Luong (không đụng navigation)
            foreach (var it in grouped)
            {
                var sp = await _sanPhamRepository.GetByIdWithDetailsAsync(it.ID_San_Pham);
                if (sp == null) continue;

                var newQty = sp.So_Luong - it.SoLuongTru;
                if (newQty < 0) newQty = 0;

                // === QUAN TRỌNG: dùng method chỉ update tồn ===
                // YÊU CẦU trong ISanPhamRepository:
                // Task<bool> UpdateStockOnlyAsync(int sanPhamId, int newQty);
                var ok = await _sanPhamRepository.UpdateStockOnlyAsync(sp.ID_San_Pham, newQty);

                if (!ok)
                {
                    errors.Add($"Cập nhật tồn thất bại cho SP#{sp.ID_San_Pham}.");
                }
                else
                {
                    updated.Add(new { id = sp.ID_San_Pham, old = sp.So_Luong, @new = newQty });
                }
            }

            if (errors.Any())
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = string.Join("\n", errors) });

            return Ok(new { success = true, message = "Đã trừ tồn.", updated });
        }



        //Lấy sản phầm nhiều lượt mua nhất 

        //[HttpGet("most-purchased")]
        //[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> GetTop10MostPurchasedProducts()
        //{
        //    var sanPhams = await _sanPhamRepository.GetTop10MostPurchasedProductsAsync();
        //    if (sanPhams == null || !sanPhams.Any())
        //    {
        //        return NotFound("No products found in invoices.");
        //    }

        //    var result = sanPhams.Select(sanPham => new
        //    {
        //        iD_San_Pham = sanPham.ID_San_Pham,
        //        ten_San_Pham = sanPham.Ten_San_Pham,
        //        gia = sanPham.Gia,
        //        so_Luong = sanPham.So_Luong,
        //        hinh_Anh = sanPham.Hinh_Anh,
        //        mo_Ta = sanPham.Mo_Ta,
        //        trang_Thai = sanPham.Trang_Thai,
        //        sizes = sanPham.SanPhamSizes.Select(sps => new
        //        {
        //            iD_Size = sps.Size.ID_Size,
        //            sizeName = sps.Size.SizeName,
        //            trang_Thai = sps.Size.Trang_Thai
        //        }).ToList(),
        //        luongDas = sanPham.SanPhamLuongDas.Select(spld => new
        //        {
        //            iD_LuongDa = spld.LuongDa.ID_LuongDa,
        //            ten_LuongDa = spld.LuongDa.Ten_LuongDa,
        //            trang_Thai = spld.LuongDa.Trang_Thai
        //        }).ToList(),
        //        doNgots = sanPham.SanPhamDoNgots.Select(spdn => new
        //        {
        //            iD_DoNgot = spdn.DoNgot.ID_DoNgot,
        //            muc_Do = spdn.DoNgot.Muc_Do,
        //            trang_Thai = spdn.DoNgot.Trang_Thai
        //        }).ToList(),
        //        toppings = sanPham.SanPhamToppings.Select(spt => new
        //        {
        //            iD_Topping = spt.Topping.ID_Topping,
        //            ten = spt.Topping.Ten,
        //            so_Luong = spt.Topping.So_Luong,
        //            gia = spt.Topping.Gia,
        //            trang_Thai = spt.Topping.Trang_Thai
        //        }).ToList(),
        //        khuyenMais = sanPham.SanPhamKhuyenMais.Select(spkm => new
        //        {
        //            iD_Khuyen_Mai = spkm.ID_Khuyen_Mai,
        //            gia_Giam = spkm.Gia_Giam,
        //            ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
        //            ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
        //            ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
        //        }).ToList()
        //    }).ToList();

        //    return Ok(result);
        //}



        [HttpGet("most-purchased")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTop10MostPurchasedProducts()
        {
            var sanPhams = await _sanPhamRepository.GetTop10MostPurchasedProductsAsync();
            if (sanPhams == null || !sanPhams.Any())
            {
                return NotFound("No products found in invoices.");
            }

            var result = sanPhams.Select(sanPham => new
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
                khuyenMais = sanPham.SanPhamKhuyenMais //Lọc Khuyến Mãi ở đây nhé ae , ko có ở repository đâu
                    .Where(spkm => spkm.BangKhuyenMai.Trang_Thai
                                && spkm.BangKhuyenMai.Ngay_Bat_Dau <= DateTime.Now
                                && spkm.BangKhuyenMai.Ngay_Ket_Thuc >= DateTime.Now)
                    .Select(spkm => new
                    {
                        iD_Khuyen_Mai = spkm.ID_Khuyen_Mai,
                        gia_Giam = spkm.Gia_Giam,
                        ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                        ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                        ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc
                    }).ToList()
            }).ToList();

            return Ok(result);
        }


    }
}
