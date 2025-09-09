using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers()
        {
            var vouchers = await _voucherRepository.GetAllAsync();
            return Ok(vouchers);
        }

        // THÊM: API lấy vouchers hoạt động
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetActiveVouchers()
        {
            var vouchers = await _voucherRepository.GetActiveVouchersAsync();
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);

            if (voucher == null)
            {
                return NotFound();
            }

            return Ok(voucher);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Voucher>> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);

            if (voucher == null)
            {
                return NotFound();
            }

            return Ok(voucher);
        }

        // THÊM: API validate voucher
        [HttpPost("validate")]
        public async Task<ActionResult<object>> ValidateVoucher([FromBody] VoucherValidationRequest request)
        {
            var canUse = await _voucherRepository.CanUseVoucherAsync(request.Code, request.OrderAmount);

            if (!canUse)
            {
                return BadRequest(new { message = "Voucher không thể sử dụng với đơn hàng này" });
            }

            var voucher = await _voucherRepository.GetByCodeAsync(request.Code);
            var discountAmount = (voucher!.Gia_Tri_Giam / 100) * request.OrderAmount;

            return Ok(new
            {
                canUse = true,
                discountPercent = voucher.Gia_Tri_Giam,
                discountAmount = discountAmount,
                finalAmount = request.OrderAmount - discountAmount
            });
        }

        [HttpPost]
        public async Task<ActionResult<Voucher>> CreateVoucher(Voucher voucher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // SỬA: Validate phần trăm giảm (1-100%)
            if (voucher.Gia_Tri_Giam.HasValue &&
                (voucher.Gia_Tri_Giam.Value < 1 || voucher.Gia_Tri_Giam.Value > 100))
            {
                return BadRequest("Phần trăm giảm phải từ 1% đến 100%");
            }

            // THÊM: Validate ngày
            if (voucher.Ngay_Bat_Dau.HasValue && voucher.Ngay_Ket_Thuc.HasValue &&
                voucher.Ngay_Bat_Dau.Value >= voucher.Ngay_Ket_Thuc.Value)
            {
                return BadRequest("Ngày kết thúc phải sau ngày bắt đầu");
            }

            // Kiểm tra mã voucher đã tồn tại
            if (await _voucherRepository.CodeExistsAsync(voucher.Ma_Voucher))
            {
                return BadRequest("Mã voucher đã tồn tại");
            }

            voucher.Trang_Thai = true; // Mặc định là hoạt động

            var createdVoucher = await _voucherRepository.CreateAsync(voucher);
            return CreatedAtAction(nameof(GetVoucher), new { id = createdVoucher.ID_Voucher }, createdVoucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(int id, Voucher voucher)
        {
            if (id != voucher.ID_Voucher)
            {
                return BadRequest("ID không khớp");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // SỬA: Validate phần trăm giảm (1-100%)
            if (voucher.Gia_Tri_Giam.HasValue &&
                (voucher.Gia_Tri_Giam.Value < 1 || voucher.Gia_Tri_Giam.Value > 100))
            {
                return BadRequest("Phần trăm giảm phải từ 1% đến 100%");
            }

            // THÊM: Validate ngày
            if (voucher.Ngay_Bat_Dau.HasValue && voucher.Ngay_Ket_Thuc.HasValue &&
                voucher.Ngay_Bat_Dau.Value >= voucher.Ngay_Ket_Thuc.Value)
            {
                return BadRequest("Ngày kết thúc phải sau ngày bắt đầu");
            }

            if (!await _voucherRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            // Kiểm tra mã voucher đã tồn tại (trừ voucher hiện tại)
            if (await _voucherRepository.CodeExistsAsync(voucher.Ma_Voucher, id))
            {
                return BadRequest("Mã voucher đã tồn tại");
            }

            await _voucherRepository.UpdateAsync(voucher);
            return Ok(voucher);
        }

        // THÊM: API kích hoạt voucher
        [HttpPost("activate/{id}")]
        public async Task<IActionResult> ActivateVoucher(int id)
        {
            var result = await _voucherRepository.ActivateAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Voucher đã được kích hoạt" });
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateVoucher(int id)
        {
            var result = await _voucherRepository.DeactivateAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(new { message = "Voucher đã được ngừng hoạt động" });
        }

        [HttpGet("check-code/{code}")]
        public async Task<ActionResult<bool>> CheckCodeExists(string code, int? excludeId = null)
        {
            var exists = await _voucherRepository.CodeExistsAsync(code, excludeId);
            return Ok(exists);
        }
    }

    // THÊM: Request model cho validate voucher
    public class VoucherValidationRequest
    {
        public string Code { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
    }
}