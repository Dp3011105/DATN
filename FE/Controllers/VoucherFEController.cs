// FE/Controllers/VoucherFEController.cs
using FE.Models;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class VoucherFEController : Controller
    {
        private readonly IVoucherService _voucherService;

        public VoucherFEController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index()
        {
            var vouchers = await _voucherService.GetAllAsync();
            return View(vouchers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var voucher = await _voucherService.GetByIdAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherViewModel voucher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _voucherService.CreateAsync(voucher);
                    TempData["SuccessMessage"] = "Tạo voucher thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(voucher);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var voucher = await _voucherService.GetByIdAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VoucherViewModel voucher)
        {
            if (id != voucher.ID_Voucher)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _voucherService.UpdateAsync(voucher);
                    TempData["SuccessMessage"] = "Cập nhật voucher thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(voucher);
        }

        [HttpPost]
        [Route("Voucher/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var result = await _voucherService.DeactivateAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Ngừng hoạt động voucher thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể ngừng hoạt động voucher!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> CheckCodeExists(string code, int? excludeId = null)
        {
            var exists = await _voucherService.CheckCodeExistsAsync(code, excludeId);
            return Json(new { exists });
        }

        // THÊM: Endpoint cho bulk update status
        [HttpPost]
        [Route("Voucher/BulkUpdateStatus")]
        public async Task<JsonResult> BulkUpdateStatus([FromBody] BulkUpdateRequest request)
        {
            try
            {
                if (request == null || request.VoucherIds == null || !request.VoucherIds.Any())
                {
                    return Json(new { success = false, message = "Không có dữ liệu để xử lý" });
                }

                bool result = false;
                string message = "";

                switch (request.Action.ToLower())
                {
                    case "activate":
                        result = await _voucherService.BulkActivateAsync(request.VoucherIds);
                        message = result ? $"Đã kích hoạt {request.VoucherIds.Count} voucher thành công"
                                        : "Có lỗi xảy ra khi kích hoạt voucher";
                        break;

                    case "deactivate":
                        result = await _voucherService.BulkDeactivateAsync(request.VoucherIds);
                        message = result ? $"Đã ngừng hoạt động {request.VoucherIds.Count} voucher thành công"
                                        : "Có lỗi xảy ra khi ngừng hoạt động voucher";
                        break;

                    default:
                        return Json(new { success = false, message = "Hành động không hợp lệ" });
                }

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // THÊM: Endpoint riêng cho bulk activate (để support trực tiếp từ JS nếu cần)
        [HttpPost]
        public async Task<JsonResult> BulkActivate([FromBody] List<int> voucherIds)
        {
            try
            {
                if (voucherIds == null || !voucherIds.Any())
                {
                    return Json(new { success = false, message = "Không có voucher nào được chọn" });
                }

                var result = await _voucherService.BulkActivateAsync(voucherIds);
                var message = result ? $"Đã kích hoạt {voucherIds.Count} voucher thành công"
                                    : "Có lỗi xảy ra khi kích hoạt voucher";

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // THÊM: Endpoint riêng cho bulk deactivate (để support trực tiếp từ JS nếu cần)
        [HttpPost]
        public async Task<JsonResult> BulkDeactivate([FromBody] List<int> voucherIds)
        {
            try
            {
                if (voucherIds == null || !voucherIds.Any())
                {
                    return Json(new { success = false, message = "Không có voucher nào được chọn" });
                }

                var result = await _voucherService.BulkDeactivateAsync(voucherIds);
                var message = result ? $"Đã ngừng hoạt động {voucherIds.Count} voucher thành công"
                                    : "Có lỗi xảy ra khi ngừng hoạt động voucher";

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Class để nhận dữ liệu từ AJAX request
        public class BulkUpdateRequest
        {
            public string Action { get; set; } = string.Empty;
            public List<int> VoucherIds { get; set; } = new List<int>();
        }
    }
}