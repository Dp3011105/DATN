using BE.Data;
using BE.DTOs.Requests;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GanVoucherController : ControllerBase
    {
        private readonly IGanVoucherRepository _repo;
        private readonly MyDbContext _context;
        public GanVoucherController(IGanVoucherRepository repo , MyDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("khachhang-all")]
        public async Task<IActionResult> GetAllKhachHang()
        {
            try
            {
                var result = await _repo.GetAllKhachHangAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi lấy danh sách khách hàng: {ex.Message}" });
            }
        }

        [HttpGet("top10-vip")]
        public async Task<IActionResult> GetTop10Vip()
        {
            try
            {
                var result = await _repo.GetTop10KhachHangVipAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi lấy top 10 khách hàng VIP: {ex.Message}" });
            }
        }

        [HttpGet("vouchers")]
        public async Task<IActionResult> GetAllVouchers()
        {
            try
            {
                var result = await _repo.GetAllVouchersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi lấy danh sách voucher: {ex.Message}" });
            }
        }

        [HttpPost("gan-voucher")]
        public async Task<IActionResult> GanVoucher([FromBody] GanVoucherRequest req)
        {
            try
            {
                // Validation đầu vào với thông báo rõ ràng hơn
                if (req == null)
                {
                    return BadRequest(new
                    {
                        message = "❌ Dữ liệu yêu cầu không hợp lệ.",
                        success = false
                    });
                }

                if (req.ID_Khach_Hang == null || req.ID_Khach_Hang.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "❌ Vui lòng chọn ít nhất một khách hàng để gán voucher.",
                        success = false
                    });
                }

                if (req.ID_Voucher == null || req.ID_Voucher.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "❌ Vui lòng chọn ít nhất một voucher để gán.",
                        success = false
                    });
                }

                if (req.SoLuong <= 0)
                {
                    return BadRequest(new
                    {
                        message = "❌ Số lượng voucher phải lớn hơn 0.",
                        success = false
                    });
                }

                if (req.SoLuong > 100)
                {
                    return BadRequest(new
                    {
                        message = "❌ Số lượng voucher không được vượt quá 100 voucher/người để đảm bảo hiệu suất.",
                        success = false
                    });
                }

                if (string.IsNullOrWhiteSpace(req.GhiChu))
                {
                    return BadRequest(new
                    {
                        message = "❌ Vui lòng nhập ghi chú cho việc gán voucher.",
                        success = false
                    });
                }

                if (req.GhiChu.Length < 5)
                {
                    return BadRequest(new
                    {
                        message = "❌ Ghi chú phải có ít nhất 5 ký tự.",
                        success = false
                    });
                }

                // Kiểm tra giới hạn số lượng để tránh quá tải
                var totalOperations = req.ID_Khach_Hang.Count * req.ID_Voucher.Count;
                if (totalOperations > 500)
                {
                    return BadRequest(new
                    {
                        message = $"❌ Tổng số lượt gán ({totalOperations}) vượt quá giới hạn 500. Vui lòng giảm số lượng khách hàng hoặc voucher.",
                        success = false
                    });
                }

                // Thực hiện gán voucher với thông tin về số lượng
                var startTime = DateTime.Now;
                var message = await _repo.GanVoucherAsync(req);
                var processingTime = (DateTime.Now - startTime).TotalSeconds;

                // Thêm thông tin thời gian xử lý vào message
                if (processingTime > 1)
                {
                    message += $"\n⏱️ Thời gian xử lý: {processingTime:F1} giây";
                }

                // Phân tích kết quả dựa trên message
                var isSuccess = message.Contains("🎉 HOÀN THÀNH");
                var hasWarnings = message.Contains("⚠️") || message.Contains("bỏ qua");
                var isFailure = message.Contains("❌ THẤT BẠI") || message.Contains("❌ Lỗi");

                if (isFailure)
                {
                    return BadRequest(new
                    {
                        message = message,
                        success = false,
                        hasWarnings = false
                    });
                }
                else if (isSuccess)
                {
                    return Ok(new
                    {
                        message = message,
                        success = true,
                        hasWarnings = hasWarnings
                    });
                }
                else
                {
                    // Trường hợp không có thay đổi
                    return Ok(new
                    {
                        message = message,
                        success = true,
                        hasWarnings = true
                    });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết cho debugging
                var errorDetails = new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace?.Substring(0, Math.Min(500, ex.StackTrace?.Length ?? 0))
                };

                return BadRequest(new
                {
                    message = $"❌ Lỗi hệ thống: {ex.Message}",
                    success = false,
                    details = ex.InnerException?.Message,
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
        }

        [HttpGet("vouchers-by-customer/{khachHangId}")]
        public async Task<IActionResult> GetVouchersByKhachHang(int khachHangId)
        {
            try
            {
                if (khachHangId <= 0)
                {
                    return BadRequest(new { message = "ID khách hàng không hợp lệ." });
                }

                var result = await _repo.GetVouchersByKhachHangAsync(khachHangId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi lấy voucher của khách hàng: {ex.Message}" });
            }
        }

        [HttpGet("check-voucher/{khachHangId}/{voucherId}")]
        public async Task<IActionResult> CheckVoucherAssignment(int khachHangId, int voucherId)
        {
            try
            {
                if (khachHangId <= 0 || voucherId <= 0)
                {
                    return BadRequest(new { message = "ID khách hàng hoặc voucher không hợp lệ." });
                }

                var result = await _repo.IsVoucherAssignedToCustomerAsync(khachHangId, voucherId);
                return Ok(new { isAssigned = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi kiểm tra voucher: {ex.Message}" });
            }
        }

        [HttpGet("customer-voucher-assignments")]
        public async Task<IActionResult> GetAllCustomerVoucherAssignments()
        {
            try
            {
                var assignments = await _repo.GetAllCustomerVoucherAssignmentsAsync();
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = $"Lỗi khi lấy danh sách voucher assignments: {ex.Message}",
                    success = false
                });
            }
        }



        [HttpPost("filter-khachhang")]
        public IActionResult FilterKhachHang([FromBody] List<int> idVouchers)
        {
            try
            {
                // Lấy danh sách ID_Khach_Hang có trong bảng KhachHangVoucher với ID_Voucher trong danh sách
                var khachHangWithVouchers = _context.KhachHang_Voucher
                    .Where(khv => idVouchers.Contains(khv.ID_Voucher))
                    .Select(khv => khv.ID_Khach_Hang)
                    .Distinct()
                    .ToList();

                // Lấy danh sách khách hàng không nằm trong danh sách ID_Khach_Hang trên
                var result = _context.Khach_Hang
                    .Where(kh => !khachHangWithVouchers.Contains(kh.ID_Khach_Hang))
                    .Select(kh => new
                    {
                        kh.ID_Khach_Hang,
                        kh.Ho_Ten,
                        kh.Email
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }











    }
}