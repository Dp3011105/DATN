using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanHangCKController : ControllerBase
    {
        private readonly IBanHangCKRepository _repository;

        public BanHangCKController(IBanHangCKRepository repository)
        {
            _repository = repository;
        }

       


        // Lấy tất cả hình thức thanh toán, chỉ hiển thị các bản ghi có Trang_Thai = true
        [HttpGet("hinhthucthanhtoan")]
        public async Task<ActionResult<IEnumerable<HinhThucThanhToanDTO>>> GetAllHinhThucThanhToan()
        {
            var hinhThucThanhToans = await _repository.GetAllHinhThucThanhToan();
            return Ok(hinhThucThanhToans);
        }



        [HttpPost("checkout")]
        public async Task<ActionResult<HoaDonBanHangCKDTO>> CheckOutTk([FromBody] HoaDonBanHangCKDTO hoaDonDto)
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



        // API 1: Lấy toàn bộ dữ liệu Voucher với DTO
        [HttpGet("vouchers")]
        public async Task<ActionResult<List<VoucherBanHangCKDto>>> GetAllVouchers()
        {
            var vouchers = await _repository.GetAllVouchersAsync();
            return Ok(vouchers);
        }

        // API 2: Truy vấn Voucher theo ID và giảm số lượng đi 1
        [HttpPut("vouchers/{id}/decrease-quantity")]
        public async Task<ActionResult> DecreaseVoucherQuantity(int id)
        {
            var voucher = await _repository.GetVoucherByIdAsync(id);
            if (voucher == null)
            {
                return NotFound(new { Message = "Voucher không tồn tại." });
            }

            if (voucher.So_Luong == null || voucher.So_Luong <= 0)
            {
                return BadRequest(new { Message = "Số lượng voucher không hợp lệ hoặc đã hết." });
            }

            voucher.So_Luong -= 1;
            var updated = await _repository.UpdateVoucherAsync(voucher);
            if (!updated)
            {
                return StatusCode(500, new { Message = "Cập nhật voucher thất bại." });
            }

            return Ok(new { Message = "Số lượng voucher đã được giảm thành công.", Voucher = voucher });
        }



    }
}
