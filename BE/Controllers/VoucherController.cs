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

        // =================== READ ===================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers()
        {
            var vouchers = await _voucherRepository.GetAllAsync();
            return Ok(vouchers);
        }

        // Lấy vouchers hoạt động (chưa hết hạn, đang bật)
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetActiveVouchers()
        {
            var vouchers = await _voucherRepository.GetActiveVouchersAsync();
            return Ok(vouchers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Voucher>> GetVoucher(int id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Voucher>> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        // =================== VALIDATE ===================

        [HttpPost("validate")]
        public async Task<ActionResult<object>> ValidateVoucher([FromBody] VoucherValidationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Code))
                return BadRequest(new { message = "Thiếu mã voucher" });

            var canUse = await _voucherRepository.CanUseVoucherAsync(request.Code, request.OrderAmount);
            if (!canUse)
                return BadRequest(new { message = "Voucher không thể sử dụng với đơn hàng này" });

            var voucher = await _voucherRepository.GetByCodeAsync(request.Code)!;
            var percent = voucher!.Gia_Tri_Giam.GetValueOrDefault(0m);
            var discountAmount = (percent / 100m) * request.OrderAmount;
            return Ok(new
            {
                canUse = true,
                discountPercent = percent,
                discountAmount,
                finalAmount = request.OrderAmount - discountAmount
            });
        }

        // =================== CREATE/UPDATE ===================

        [HttpPost]
        public async Task<ActionResult<Voucher>> CreateVoucher([FromBody] Voucher voucher)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validate phần trăm
            if (voucher.Gia_Tri_Giam.HasValue &&
                (voucher.Gia_Tri_Giam.Value < 1 || voucher.Gia_Tri_Giam.Value > 100))
                return BadRequest("Phần trăm giảm phải từ 1% đến 100%");

            // Validate ngày
            if (voucher.Ngay_Bat_Dau.HasValue && voucher.Ngay_Ket_Thuc.HasValue &&
                voucher.Ngay_Bat_Dau.Value >= voucher.Ngay_Ket_Thuc.Value)
                return BadRequest("Ngày kết thúc phải sau ngày bắt đầu");

            // Trùng mã
            if (await _voucherRepository.CodeExistsAsync(voucher.Ma_Voucher))
                return BadRequest("Mã voucher đã tồn tại");

            voucher.Trang_Thai = true; // mặc định bật
            var created = await _voucherRepository.CreateAsync(voucher);
            return CreatedAtAction(nameof(GetVoucher), new { id = created.ID_Voucher }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVoucher(int id, [FromBody] Voucher voucher)
        {
            if (id != voucher.ID_Voucher) return BadRequest("ID không khớp");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (voucher.Gia_Tri_Giam.HasValue &&
                (voucher.Gia_Tri_Giam.Value < 1 || voucher.Gia_Tri_Giam.Value > 100))
                return BadRequest("Phần trăm giảm phải từ 1% đến 100%");

            if (voucher.Ngay_Bat_Dau.HasValue && voucher.Ngay_Ket_Thuc.HasValue &&
                voucher.Ngay_Bat_Dau.Value >= voucher.Ngay_Ket_Thuc.Value)
                return BadRequest("Ngày kết thúc phải sau ngày bắt đầu");

            if (!await _voucherRepository.ExistsAsync(id)) return NotFound();

            if (await _voucherRepository.CodeExistsAsync(voucher.Ma_Voucher, id))
                return BadRequest("Mã voucher đã tồn tại");

            await _voucherRepository.UpdateAsync(voucher);
            return Ok(voucher);
        }

        // =================== ACTIVATE/DEACTIVATE ===================

        [HttpPost("activate/{id:int}")]
        public async Task<IActionResult> ActivateVoucher(int id)
        {
            var ok = await _voucherRepository.ActivateAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Voucher đã được kích hoạt" });
        }

        [HttpPost("deactivate/{id:int}")]
        public async Task<IActionResult> DeactivateVoucher(int id)
        {
            var ok = await _voucherRepository.DeactivateAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Voucher đã được ngừng hoạt động" });
        }

        // =================== CODE CHECK ===================

        [HttpGet("check-code/{code}")]
        public async Task<ActionResult<bool>> CheckCodeExists(string code, int? excludeId = null)
        {
            var exists = await _voucherRepository.CodeExistsAsync(code, excludeId);
            return Ok(exists);
        }

        // =================== CONSUME (GIẢM SỐ LƯỢNG) ===================

        // Giảm theo ID (atomic, chỉ trừ khi đủ So_Luong)
        [HttpPost("consume/{id:int}")]
        public async Task<IActionResult> Consume(int id, [FromQuery] int qty = 1)
        {
            if (qty <= 0) qty = 1;
            var ok = await _voucherRepository.DecrementAsync(id, qty);
            return ok
                ? Ok(new { id, consumed = qty })
                : BadRequest(new { message = "Voucher không tồn tại hoặc So_Luong không đủ", id, qty });
        }

        // Giảm theo Code (atomic)
        [HttpPost("consume-by-code/{code}")]
        public async Task<IActionResult> ConsumeByCode(string code, [FromQuery] int qty = 1)
        {
            if (qty <= 0) qty = 1;
            var ok = await _voucherRepository.DecrementByCodeAsync(code, qty);
            return ok
                ? Ok(new { code, consumed = qty })
                : BadRequest(new { message = "Voucher không tồn tại hoặc So_Luong không đủ", code, qty });
        }
    }

    // ===== DTO cho validate =====
    public class VoucherValidationRequest
    {
        public string Code { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
    }
}
