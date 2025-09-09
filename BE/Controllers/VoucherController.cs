using BE.DTOs;
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

        // =================== LOOKUP BY CODE (cho FE) ===================

        // Hỗ trợ: GET api/Voucher/by-code/Faker
        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<VoucherDTO>> GetByCode([FromRoute] string code)
        {
            try
            {
                var key = (code ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(key))
                    return BadRequest(new { message = "Thiếu mã voucher." });

                // Nếu repo chưa có GetByCodeAsync -> fallback lấy tất cả rồi lọc
                var all = await _voucherRepository.GetAllAsync();
                var found = all.FirstOrDefault(v =>
                    string.Equals((v.Ma_Voucher ?? string.Empty).Trim(),
                                  key, StringComparison.OrdinalIgnoreCase));

                if (found == null)
                    return NotFound(new { message = "Không tìm thấy voucher theo mã." });

                return Ok(found);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi tra cứu voucher theo mã.", error = ex.Message });
            }
        }

        // Hỗ trợ: GET api/Voucher?code=Faker  (để FE có thể gọi dạng query)
        [HttpGet]
        public async Task<ActionResult> GetVouchers([FromQuery] string? code)
        {
            try
            {
                // Nếu có query ?code=... -> trả về 1 object (hoặc 404)
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var key = code.Trim();
                    var all = await _voucherRepository.GetAllAsync();
                    var found = all.FirstOrDefault(v =>
                        string.Equals((v.Ma_Voucher ?? string.Empty).Trim(),
                                      key, StringComparison.OrdinalIgnoreCase));

                    if (found == null)
                        return NotFound(new { message = "Không tìm thấy voucher theo mã." });

                    return Ok(found);
                }

                // Không có query -> trả về danh sách
                var vouchers = await _voucherRepository.GetAllAsync();
                return Ok(vouchers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách/tra cứu voucher.", error = ex.Message });
            }
        }

        // =================== GET BY ID ===================
        // GET: api/Voucher/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VoucherDTO>> GetVoucher([FromRoute] int id)
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

        // =================== CREATE ===================
        // POST: api/Voucher
        [HttpPost]
        public async Task<ActionResult<VoucherDTO>> CreateVoucher([FromBody] VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        // =================== UPDATE ===================
        // PUT: api/Voucher/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VoucherDTO>> UpdateVoucher([FromRoute] int id, [FromBody] VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != voucherDTO.ID_Voucher)
                return BadRequest(new { message = "ID voucher không khớp." });

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
