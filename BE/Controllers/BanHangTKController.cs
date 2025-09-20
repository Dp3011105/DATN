using BE.DTOs;
using BE.Repository;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanHangTKController : ControllerBase
    {
        private readonly IBanHangTKRepository _repository;
        private readonly ILogger<BanHangTKRepository> _logger; // Khai báo ILogger

        public BanHangTKController(IBanHangTKRepository repository, ILogger<BanHangTKRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // API 1: GET all active addresses for a customer
        [HttpGet("khachhang/{idKhachHang}/diachi")]
        public async Task<ActionResult<IEnumerable<DiaChiBanHangDTO>>> GetDiaChiByKhachHang(int idKhachHang)
        {
            var diaChiList = await _repository.GetActiveDiaChiByKhachHangId(idKhachHang);
            return Ok(diaChiList);
        }

        // API 2: PUT update address data by ID_Dia_Chi for a specific customer
        [HttpPut("khachhang/{idKhachHang}/diachi/{idDiaChi}")]
        public async Task<IActionResult> UpdateDiaChi(int idKhachHang, int idDiaChi, [FromBody] UpdateDiaChiBanHangDTO dto)
        {
            var success = await _repository.UpdateDiaChi(idKhachHang, idDiaChi, dto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // API 3: POST add new address for a customer
        [HttpPost("khachhang/{idKhachHang}/diachi")]
        public async Task<ActionResult<int>> AddDiaChi(int idKhachHang, [FromBody] CreateDiaChiBanHangDTO dto)
        {
            var newId = await _repository.AddDiaChiForKhachHang(idKhachHang, dto);
            return CreatedAtAction(nameof(GetDiaChiByKhachHang), new { idKhachHang }, newId);
        }




        [HttpGet("vouchers/{idKhachHang}")]
        public async Task<ActionResult<IEnumerable<VoucherBanHangDTO>>> GetVouchersByKhachHang(int idKhachHang)
        {
            var vouchers = await _repository.GetVouchersByKhachHang(idKhachHang);
            return Ok(vouchers);
        }


        // Lấy tất cả hình thức thanh toán, chỉ hiển thị các bản ghi có Trang_Thai = true
        [HttpGet("hinhthucthanhtoan")]
        public async Task<ActionResult<IEnumerable<HinhThucThanhToanDTO>>> GetAllHinhThucThanhToan()
        {
            var hinhThucThanhToans = await _repository.GetAllHinhThucThanhToan();
            return Ok(hinhThucThanhToans);
        }



        [HttpPost("checkout")]
        public async Task<ActionResult<HoaDonBanHangTKDTO>> CheckOutTk([FromBody] HoaDonBanHangTKDTO hoaDonDto)
        {
            try
            {
                var hoaDon = await _repository.CheckOutTk(hoaDonDto);
                return Ok(hoaDon);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }







        // API 1: Check có số điện thoại chưa
        [HttpGet("kiem-tra-so-dien-thoai/{idKhachHang}")]
        public async Task<IActionResult> KiemTraSoDienThoai(int idKhachHang)
        {
            var coSoDienThoai = await _repository.KiemTraCoSoDienThoaiAsync(idKhachHang);
            return Ok(coSoDienThoai);
        }




        // API 2: Lấy số điện thoại của khách hàng dựa trên idKhachHang
        [HttpGet("lay-so-dien-thoai/{idKhachHang}")]
        public async Task<IActionResult> LaySoDienThoai(int idKhachHang)
        {
            try
            {
                // Gọi phương thức từ repository để lấy số điện thoại
                var soDienThoai = await _repository.LaySoDienThoaiAsync(idKhachHang);

                // Kiểm tra nếu không tìm thấy khách hàng hoặc số điện thoại
                if (string.IsNullOrEmpty(soDienThoai))
                {
                    return NotFound(new { Message = "Không tìm thấy khách hàng hoặc số điện thoại." });
                }

                // Trả về số điện thoại
                return Ok(new { IdKhachHang = idKhachHang, SoDienThoai = soDienThoai });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi lấy số điện thoại.", Error = ex.Message });
            }
        }




        // API 3: Thêm số điện thoại
        [HttpPost("them-so-dien-thoai")]
        public async Task<IActionResult> ThemSoDienThoai([FromBody] SoDienThoaiBanHangTKDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.So_Dien_Thoai))
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            await _repository.ThemSoDienThoaiAsync(dto.ID_Khach_Hang, dto.So_Dien_Thoai);
            return Ok("Thêm số điện thoại thành công");
        }

        // API 4: Update số điện thoại
        [HttpPut("cap-nhat-so-dien-thoai")]
        public async Task<IActionResult> CapNhatSoDienThoai([FromBody] SoDienThoaiBanHangTKDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.So_Dien_Thoai))
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            await _repository.CapNhatSoDienThoaiAsync(dto.ID_Khach_Hang, dto.So_Dien_Thoai);
            return Ok("Cập nhật số điện thoại thành công");
        }




        [HttpGet("khachhang/{id}")]
        public async Task<ActionResult<KhachHangCheckoutDTO>> GetKhachHang(int id)
        {
            var khachHang = await _repository.GetKhachHangByIdAsync(id);
            if (khachHang == null)
            {
                return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}" });
            }
            return Ok(khachHang);
        }

        //[HttpPut("khachhang/{id}")]
        //public async Task<ActionResult> UpdateKhachHang(int id, [FromBody] KhachHangCheckoutDTO khachHangDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != khachHangDto.ID_Khach_Hang)
        //    {
        //        return BadRequest(new { message = "ID khách hàng không khớp" });
        //    }

        //    var result = await _repository.UpdateKhachHangAsync(id, khachHangDto);
        //    if (!result)
        //    {
        //        return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}" });
        //    }

        //    return NoContent();
        //}


        [HttpPut("khachhang/{id}")]
        public async Task<ActionResult> UpdateKhachHang(int id, [FromBody] KhachHangCheckoutDTO khachHangDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != khachHangDto.ID_Khach_Hang)
            {
                return BadRequest(new { message = "ID khách hàng không khớp" });
            }

            try
            {
                var result = await _repository.UpdateKhachHangAsync(id, khachHangDto);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy khách hàng với ID {id}" });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Email đã được sử dụng"))
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi không xác định khi cập nhật khách hàng ID {id}: {ex.Message}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật khách hàng.", error = ex.Message });
            }
        }



    }
}
