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

        public BanHangTKController(IBanHangTKRepository repository)
        {
            _repository = repository;
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


    }
}
