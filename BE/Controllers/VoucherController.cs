using BE.DTOs;
using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        // GET: api/Voucher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherDTO>>> GetVouchers()
        {
            try
            {
                var vouchers = await _voucherRepository.GetAllAsync();
                return Ok(vouchers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách voucher.", error = ex.Message });
            }
        }

        // GET: api/Voucher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherDTO>> GetVoucher(int id)
        {
            try
            {
                var voucher = await _voucherRepository.GetByIdAsync(id);
                return Ok(voucher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy chi tiết voucher.", error = ex.Message });
            }
        }

        // POST: api/Voucher
        [HttpPost]
        public async Task<ActionResult<VoucherDTO>> CreateVoucher([FromBody] VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdVoucher = await _voucherRepository.AddAsync(voucherDTO);
                return CreatedAtAction(nameof(GetVoucher), new { id = createdVoucher.ID_Voucher }, createdVoucher);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm voucher.", error = ex.Message });
            }
        }

        // PUT: api/Voucher/5
        [HttpPut("{id}")]
        public async Task<ActionResult<VoucherDTO>> UpdateVoucher(int id, [FromBody] VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != voucherDTO.ID_Voucher)
            {
                return BadRequest(new { message = "ID voucher không khớp." });
            }

            try
            {
                var updatedVoucher = await _voucherRepository.UpdateAsync(id, voucherDTO);
                return Ok(updatedVoucher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật voucher.", error = ex.Message });
            }
        }
    }
}