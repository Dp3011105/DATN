using BE.DTOs;
using ClosedXML.Excel;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.IO;
using System.Linq.Expressions;

namespace FE.Controllers
{
    [RoleAuthorize(2)] // Phương thức này được để trong thư mục Filters nhé ae
    public class KhachHangController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchTerm = "", bool? statusFilter = null)
        {
            // Giới hạn pageSize từ 5 đến 30
            if (pageSize < 5 || pageSize > 30)
            {
                pageSize = 10; // Giá trị mặc định nếu nằm ngoài phạm vi
            }

            var customers = await _khachHangService.GetAllKhachHang();
            var totalItems = customers.Count();

            // Áp dụng filter
            var filteredCustomers = customers.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredCustomers = filteredCustomers.Where(kh => kh.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                                 kh.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                                 kh.So_Dien_Thoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            if (statusFilter.HasValue)
            {
                filteredCustomers = filteredCustomers.Where(kh => kh.Trang_Thai == statusFilter.Value);
            }

            var paginatedCustomers = filteredCustomers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)filteredCustomers.Count() / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize; // Đảm bảo pageSize được truyền vào ViewBag
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.TotalFilteredItems = filteredCustomers.Count();

            return View(paginatedCustomers);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _khachHangService.GetKhachHangById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return PartialView("Details", customer); // Đổi thành PartialView cho modal
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new KhachHangDTO { Trang_Thai = true }; // Đặt trạng thái mặc định là true (Đang hoạt động)
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KhachHangDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Đảm bảo Trang_Thai được đặt mặc định là true nếu không được gửi từ form
                    if (!entity.Trang_Thai)
                    {
                        entity.Trang_Thai = true;
                    }
                    await _khachHangService.AddKhachHang(entity);
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("Index", "KhachHang");
                }
                return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating customer: {ex.Message}");
                return View(entity);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _khachHangService.GetKhachHangById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHangDTO entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _khachHangService.UpdateKhachHang(id, entity);
                    return RedirectToAction("Index", "KhachHang");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _khachHangService.DeleteKhachHang(id);
                return RedirectToAction("Index", "KhachHang");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string searchTerm = "", bool? statusFilter = null)
        {
            var result = await _khachHangService.GetAllKhachHang();
            var filteredResult = result.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(kh => kh.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            if (statusFilter.HasValue)
            {
                filteredResult = filteredResult.Where(kh => kh.Trang_Thai == statusFilter.Value);
            }
            var data = filteredResult.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("KhachHang");
                worksheet.Cell(1, 1).Value = "ID_Khach_Hang";
                worksheet.Cell(1, 2).Value = "Ho_Ten";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "So_Dien_Thoai";
                worksheet.Cell(1, 5).Value = "Trang_Thai";
                worksheet.Cell(1, 6).Value = "GhiChu";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].ID_Khach_Hang;
                    worksheet.Cell(i + 2, 2).Value = data[i].Ho_Ten;
                    worksheet.Cell(i + 2, 3).Value = data[i].Email;
                    worksheet.Cell(i + 2, 4).Value = data[i].So_Dien_Thoai;
                    worksheet.Cell(i + 2, 5).Value = data[i].Trang_Thai ? "Đang hoạt động" : "Không hoạt động";
                    worksheet.Cell(i + 2, 6).Value = data[i].Ghi_Chu;
                }

                var range = worksheet.RangeUsed();
                var table = range.CreateTable();
                table.Theme = XLTableTheme.TableStyleMedium9;
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuanLyKhachHang.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult Import()
        {
            return PartialView("_ImportExcel");
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn file Excel.");
                return RedirectToAction("Index");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RowsUsed().Skip(1);
                    foreach (var row in rows)
                    {
                        var entity = new KhachHangDTO
                        {
                            Ho_Ten = row.Cell(2).GetString(),
                            Email = row.Cell(3).GetString(),
                            So_Dien_Thoai = row.Cell(4).GetString(),
                            Trang_Thai = row.Cell(5).GetString() == "Đang hoạt động" ? true : false,
                            Ghi_Chu = row.Cell(6).GetString()
                        };

                        // validate cơ bản
                        if (string.IsNullOrEmpty(entity.Ho_Ten)) continue;
                        await _khachHangService.AddKhachHang(entity);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}