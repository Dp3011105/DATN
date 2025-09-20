//using BE.models;
//using ClosedXML.Excel;
//using FE.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Service.IService;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FE.Controllers
//{
//    [RoleAuthorize(2)]
//    public class NhanVienController : Controller
//    {
//        private readonly INhanVienService _service;

//        public NhanVienController(INhanVienService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
//        {
//            var result = await _service.GetAllAsync();
//            var filteredResult = result.AsQueryable();

//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filteredResult = filteredResult.Where(nv =>
//                    nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.So_Dien_Thoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
//            }

//            if (statusFilter.HasValue)
//            {
//                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
//            }

//            ViewBag.TotalEmployees = result.Count();
//            ViewBag.WorkingEmployees = result.Count(nv => nv.Trang_Thai == true);
//            ViewBag.LeavingEmployees = result.Count(nv => nv.Trang_Thai == false);
//            ViewBag.NewEmployees = result.Count(nv => (DateTime.Now - nv.NamSinh).TotalDays / 365 < 1);

//            var totalItems = filteredResult.Count();
//            var paginatedResult = filteredResult
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
//            ViewBag.CurrentPage = page;
//            ViewBag.PageSize = pageSize;
//            ViewBag.SearchTerm = searchTerm;
//            ViewBag.StatusFilter = statusFilter;

//            return View(paginatedResult);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Details(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null)
//            {
//                return NotFound();
//            }
//            return PartialView("Details", result);
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(NhanVien entity, IFormFile anhNhanVien)
//        {
//            if (anhNhanVien != null && anhNhanVien.Length > 0)
//            {
//                // Validate file type
//                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
//                var extension = Path.GetExtension(anhNhanVien.FileName).ToLowerInvariant();
//                if (!allowedExtensions.Contains(extension))
//                {
//                    ModelState.AddModelError("anhNhanVien", "Chỉ hỗ trợ định dạng .jpg, .jpeg, .png");
//                    return View(entity);
//                }

//                // Validate file size (e.g., max 5MB)
//                if (anhNhanVien.Length > 5 * 1024 * 1024)
//                {
//                    ModelState.AddModelError("anhNhanVien", "Kích thước file không được vượt quá 5MB");
//                    return View(entity);
//                }

//                var fileName = Path.GetFileName(anhNhanVien.FileName);
//                var filePath = Path.Combine("wwwroot/uploads", fileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await anhNhanVien.CopyToAsync(stream);
//                }

//                entity.AnhNhanVien = "/uploads/" + fileName;
//            }

//            // Set default Trang_Thai to true (Đang làm)
//            entity.Trang_Thai = true;

//            await _service.AddAsync(entity);
//            return RedirectToAction("Index", "NhanVien");
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null)
//            {
//                return NotFound();
//            }
//            ViewBag.IsAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
//            return View(result);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, NhanVien entity, IFormFile anhNhanVien)
//        {
//            var currentEmployee = await _service.GetByIdAsync(id);
//            if (currentEmployee == null)
//            {
//                return NotFound();
//            }

//            if (anhNhanVien != null && anhNhanVien.Length > 0)
//            {
//                var fileName = Path.GetFileName(anhNhanVien.FileName);
//                var filePath = Path.Combine("wwwroot/uploads", fileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await anhNhanVien.CopyToAsync(stream);
//                }

//                entity.AnhNhanVien = "/uploads/" + fileName;
//            }
//            else
//            {
//                entity.AnhNhanVien = currentEmployee.AnhNhanVien;
//            }

//            entity.ID_Nhan_Vien = id;
//            await _service.UpdateAsync(id, entity);
//            return RedirectToAction("Index", "NhanVien");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                await _service.DeleteAsync(id);
//                return RedirectToAction(nameof(Index));
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteSelected([FromBody] int[] ids)
//        {
//            try
//            {
//                foreach (var id in ids)
//                {
//                    await _service.DeleteAsync(id);
//                }
//                return Json(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> UpdateStatus([FromBody] dynamic data)
//        {
//            try
//            {
//                var ids = data.ids.ToObject<int[]>();
//                var status = data.status.ToObject<bool?>();
//                foreach (var id in ids)
//                {
//                    var employee = await _service.GetByIdAsync(id);
//                    if (employee != null)
//                    {
//                        employee.Trang_Thai = status;
//                        await _service.UpdateAsync(id, employee);
//                    }
//                }
//                return Json(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToExcel(string searchTerm = "", bool? statusFilter = null)
//        {
//            var result = await _service.GetAllAsync();
//            var filteredResult = result.AsQueryable();

//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filteredResult = filteredResult.Where(nv =>
//                    nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.So_Dien_Thoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
//            }

//            if (statusFilter.HasValue)
//            {
//                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
//            }

//            var data = filteredResult.ToList();

//            using (var workbook = new XLWorkbook())
//            {
//                var worksheet = workbook.Worksheets.Add("NhanVien");

//                worksheet.Cell(1, 1).Value = "ID_Nhan_Vien";
//                worksheet.Cell(1, 2).Value = "Ho_Ten";
//                worksheet.Cell(1, 3).Value = "Email";
//                worksheet.Cell(1, 4).Value = "GioiTinh";
//                worksheet.Cell(1, 5).Value = "So_Dien_Thoai";
//                worksheet.Cell(1, 6).Value = "Dia_Chi";
//                worksheet.Cell(1, 7).Value = "NamSinh";
//                worksheet.Cell(1, 8).Value = "CCCD";
//                worksheet.Cell(1, 9).Value = "Trang_Thai";
//                worksheet.Cell(1, 10).Value = "Ghi_Chu";
//                worksheet.Cell(1, 11).Value = "AnhNhanVien";

//                for (int i = 0; i < data.Count; i++)
//                {
//                    worksheet.Cell(i + 2, 1).Value = data[i].ID_Nhan_Vien;
//                    worksheet.Cell(i + 2, 2).Value = data[i].Ho_Ten;
//                    worksheet.Cell(i + 2, 3).Value = data[i].Email;
//                    worksheet.Cell(i + 2, 4).Value = data[i].GioiTinh.HasValue ? (data[i].GioiTinh.Value ? "Nam" : "Nữ") : "Khác";
//                    worksheet.Cell(i + 2, 5).Value = data[i].So_Dien_Thoai;
//                    worksheet.Cell(i + 2, 6).Value = data[i].Dia_Chi;
//                    worksheet.Cell(i + 2, 7).Value = data[i].NamSinh.ToString("yyyy-MM-dd");
//                    worksheet.Cell(i + 2, 8).Value = data[i].CCCD;
//                    worksheet.Cell(i + 2, 9).Value = data[i].Trang_Thai.HasValue ? (data[i].Trang_Thai.Value ? "Đang làm" : "Nghỉ") : "Không xác định";
//                    worksheet.Cell(i + 2, 10).Value = data[i].Ghi_Chu;
//                    worksheet.Cell(i + 2, 11).Value = data[i].AnhNhanVien;
//                }

//                var range = worksheet.RangeUsed();
//                var table = range.CreateTable();
//                table.Theme = XLTableTheme.TableStyleMedium9;
//                worksheet.Columns().AdjustToContents();

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    var content = stream.ToArray();
//                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuanLyNhanVien.xlsx");
//                }
//            }
//        }

//        [HttpGet]
//        public IActionResult Import()
//        {
//            return PartialView("_ImportExcel");
//        }

//        [HttpPost]
//        public async Task<IActionResult> ImportFromExcel(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                ModelState.AddModelError("", "Vui lòng chọn file Excel.");
//                return RedirectToAction("Index");
//            }

//            try
//            {
//                using (var stream = new MemoryStream())
//                {
//                    await file.CopyToAsync(stream);
//                    stream.Position = 0;
//                    using (var workbook = new XLWorkbook(stream))
//                    {
//                        var worksheet = workbook.Worksheet(1);
//                        var rows = worksheet.RowsUsed().Skip(1);

//                        foreach (var row in rows)
//                        {
//                            var entity = new NhanVien
//                            {
//                                Ho_Ten = row.Cell(2).GetString(),
//                                Email = row.Cell(3).GetString(),
//                                GioiTinh = row.Cell(4).GetString() == "Nam" ? true :
//                                           (row.Cell(4).GetString() == "Nữ" ? false : null),
//                                So_Dien_Thoai = row.Cell(5).GetString(),
//                                Dia_Chi = row.Cell(6).GetString(),
//                                NamSinh = row.Cell(7).DataType == XLDataType.DateTime
//                                            ? row.Cell(7).GetDateTime()
//                                            : DateTime.Parse(row.Cell(7).GetString()),
//                                CCCD = row.Cell(8).GetString(),
//                                Trang_Thai = row.Cell(9).GetString() == "Đang làm" ? true : false,
//                                Ghi_Chu = row.Cell(10).GetString(),
//                                AnhNhanVien = row.Cell(11).GetString(),
//                            };

//                            if (string.IsNullOrEmpty(entity.Ho_Ten))
//                                continue;

//                            await _service.AddAsync(entity);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", "Lỗi import: " + ex.Message);
//            }

//            return RedirectToAction("Index");
//        }


//        private bool IsValidEmail(string email)
//        {
//            if (string.IsNullOrEmpty(email)) return false;
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}
//using BE.models;
//using ClosedXML.Excel;
//using FE.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Service.IService;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FE.Controllers
//{
//    [RoleAuthorize(2)]
//    public class NhanVienController : Controller
//    {
//        private readonly INhanVienService _service;
//        private readonly ILogger<NhanVienController> _logger;

//        public NhanVienController(INhanVienService service, ILogger<NhanVienController> logger)
//        {
//            _service = service;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
//        {
//            var result = await _service.GetAllAsync();
//            var filteredResult = result.AsQueryable();

//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filteredResult = filteredResult.Where(nv =>
//                    nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.So_Dien_Thoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
//            }

//            if (statusFilter.HasValue)
//            {
//                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
//            }

//            ViewBag.TotalEmployees = result.Count();
//            ViewBag.WorkingEmployees = result.Count(nv => nv.Trang_Thai == true);
//            ViewBag.LeavingEmployees = result.Count(nv => nv.Trang_Thai == false);
//            ViewBag.NewEmployees = result.Count(nv => (DateTime.Now - nv.NamSinh).TotalDays / 365 < 1);

//            var totalItems = filteredResult.Count();
//            var paginatedResult = filteredResult
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
//            ViewBag.CurrentPage = page;
//            ViewBag.PageSize = pageSize;
//            ViewBag.SearchTerm = searchTerm;
//            ViewBag.StatusFilter = statusFilter;

//            return View(paginatedResult);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Details(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null)
//            {
//                return NotFound();
//            }
//            return PartialView("Details", result);
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(NhanVien entity, IFormFile anhNhanVien)
//        {
//            if (anhNhanVien != null && anhNhanVien.Length > 0)
//            {
//                // Validate file type
//                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
//                var extension = Path.GetExtension(anhNhanVien.FileName).ToLowerInvariant();
//                if (!allowedExtensions.Contains(extension))
//                {
//                    ModelState.AddModelError("anhNhanVien", "Chỉ hỗ trợ định dạng .jpg, .jpeg, .png");
//                    return View(entity);
//                }

//                // Validate file size (e.g., max 5MB)
//                if (anhNhanVien.Length > 5 * 1024 * 1024)
//                {
//                    ModelState.AddModelError("anhNhanVien", "Kích thước file không được vượt quá 5MB");
//                    return View(entity);
//                }

//                var fileName = Path.GetFileName(anhNhanVien.FileName);
//                var filePath = Path.Combine("wwwroot/uploads", fileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await anhNhanVien.CopyToAsync(stream);
//                }

//                entity.AnhNhanVien = "/uploads/" + fileName;
//            }

//            // Set default Trang_Thai to true (Đang làm)
//            entity.Trang_Thai = true;

//            await _service.AddAsync(entity);
//            return RedirectToAction("Index", "NhanVien");
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null)
//            {
//                return NotFound();
//            }
//            ViewBag.IsAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
//            return View(result);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, NhanVien entity, IFormFile anhNhanVien)
//        {
//            var currentEmployee = await _service.GetByIdAsync(id);
//            if (currentEmployee == null)
//            {
//                return NotFound();
//            }

//            if (anhNhanVien != null && anhNhanVien.Length > 0)
//            {
//                var fileName = Path.GetFileName(anhNhanVien.FileName);
//                var filePath = Path.Combine("wwwroot/uploads", fileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await anhNhanVien.CopyToAsync(stream);
//                }

//                entity.AnhNhanVien = "/uploads/" + fileName;
//            }
//            else
//            {
//                entity.AnhNhanVien = currentEmployee.AnhNhanVien;
//            }

//            entity.ID_Nhan_Vien = id;
//            await _service.UpdateAsync(id, entity);
//            return RedirectToAction("Index", "NhanVien");
//        }


//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                await _service.DeleteAsync(id);
//                TempData["Success"] = "Đã cập nhật thành công.";
//                return RedirectToAction(nameof(Index));
//            }
//            catch (KeyNotFoundException)
//            {
//                TempData["Error"] = "Không tìm thấy nhân viên.";
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi , thất bại.");
//                TempData["Error"] = $"Lỗi , thất bại: {ex.Message}";
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteSelected([FromBody] int[] ids)
//        {
//            try
//            {
//                if (ids == null || !ids.Any())
//                {
//                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một nhân viên." });
//                }

//                foreach (var id in ids)
//                {
//                    await _service.DeleteAsync(id);
//                }
//                return Json(new { success = true, message = "Xóa các nhân viên đã chọn thành công." });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xóa nhiều nhân viên.");
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusModel data)
//        {
//            try
//            {
//                if (data.Ids == null || !data.Ids.Any())
//                {
//                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một nhân viên." });
//                }

//                foreach (var id in data.Ids)
//                {
//                    var employee = await _service.GetByIdAsync(id);
//                    if (employee != null)
//                    {
//                        employee.Trang_Thai = data.Status;
//                        await _service.UpdateAsync(id, employee);
//                    }
//                }
//                return Json(new { success = true, message = "Cập nhật trạng thái thành công." });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái.");
//                return Json(new { success = false, message = ex.Message });
//            }
//        }

//        public class UpdateStatusModel
//        {
//            public int[] Ids { get; set; }
//            public bool? Status { get; set; }
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToExcel(string searchTerm = "", bool? statusFilter = null)
//        {
//            var result = await _service.GetAllAsync();
//            var filteredResult = result.AsQueryable();

//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                filteredResult = filteredResult.Where(nv =>
//                    nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
//                    nv.So_Dien_Thoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
//            }

//            if (statusFilter.HasValue)
//            {
//                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
//            }

//            var data = filteredResult.ToList();

//            using (var workbook = new XLWorkbook())
//            {
//                var worksheet = workbook.Worksheets.Add("NhanVien");

//                worksheet.Cell(1, 1).Value = "ID_Nhan_Vien";
//                worksheet.Cell(1, 2).Value = "Ho_Ten";
//                worksheet.Cell(1, 3).Value = "Email";
//                worksheet.Cell(1, 4).Value = "GioiTinh";
//                worksheet.Cell(1, 5).Value = "So_Dien_Thoai";
//                worksheet.Cell(1, 6).Value = "Dia_Chi";
//                worksheet.Cell(1, 7).Value = "NamSinh";
//                worksheet.Cell(1, 8).Value = "CCCD";
//                worksheet.Cell(1, 9).Value = "Trang_Thai";
//                worksheet.Cell(1, 10).Value = "Ghi_Chu";
//                worksheet.Cell(1, 11).Value = "AnhNhanVien";
//                worksheet.Cell(1, 12).Value = "AnhCCCD"; // Thêm lại AnhCCCD

//                for (int i = 0; i < data.Count; i++)
//                {
//                    worksheet.Cell(i + 2, 1).Value = data[i].ID_Nhan_Vien;
//                    worksheet.Cell(i + 2, 2).Value = data[i].Ho_Ten;
//                    worksheet.Cell(i + 2, 3).Value = data[i].Email;
//                    worksheet.Cell(i + 2, 4).Value = data[i].GioiTinh.HasValue ? (data[i].GioiTinh.Value ? "Nam" : "Nữ") : "Khác";
//                    worksheet.Cell(i + 2, 5).Value = data[i].So_Dien_Thoai;
//                    worksheet.Cell(i + 2, 6).Value = data[i].Dia_Chi;
//                    worksheet.Cell(i + 2, 7).Value = data[i].NamSinh.ToString("yyyy-MM-dd");
//                    worksheet.Cell(i + 2, 8).Value = data[i].CCCD;
//                    worksheet.Cell(i + 2, 9).Value = data[i].Trang_Thai.HasValue ? (data[i].Trang_Thai.Value ? "Đang làm" : "Nghỉ") : "Không xác định";
//                    worksheet.Cell(i + 2, 10).Value = data[i].Ghi_Chu;
//                    worksheet.Cell(i + 2, 11).Value = data[i].AnhNhanVien;
//                }

//                var range = worksheet.RangeUsed();
//                var table = range.CreateTable();
//                table.Theme = XLTableTheme.TableStyleMedium9;
//                worksheet.Columns().AdjustToContents();

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    var content = stream.ToArray();
//                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuanLyNhanVien.xlsx");
//                }
//            }
//        }

//        [HttpGet]
//        public IActionResult Import()
//        {
//            return PartialView("_ImportExcel");
//        }

//        [HttpPost]
//        public async Task<IActionResult> ImportFromExcel(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                TempData["Error"] = "Vui lòng chọn file Excel.";
//                return RedirectToAction("Index");
//            }

//            var errors = new List<string>();
//            var importedCount = 0;
//            var existingEmployees = await _service.GetAllAsync();
//            var existingNames = new HashSet<string>(existingEmployees.Select(e => e.Ho_Ten), StringComparer.OrdinalIgnoreCase);
//            var existingCCCDs = new HashSet<string>(existingEmployees.Select(e => e.CCCD).Where(c => !string.IsNullOrEmpty(c)));

//            try
//            {
//                using (var stream = new MemoryStream())
//                {
//                    await file.CopyToAsync(stream);
//                    stream.Position = 0;
//                    using (var workbook = new XLWorkbook(stream))
//                    {
//                        var worksheet = workbook.Worksheet(1);
//                        var rows = worksheet.RowsUsed().Skip(1);

//                        foreach (var row in rows)
//                        {
//                            var rowNumber = row.RowNumber();
//                            try
//                            {
//                                var hoTen = row.Cell(2).GetString()?.Trim();
//                                if (string.IsNullOrEmpty(hoTen))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: Bỏ qua vì họ tên rỗng.");
//                                    continue;
//                                }

//                                var email = row.Cell(3).GetString()?.Trim();
//                                if (!IsValidEmail(email))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: Email '{email}' không hợp lệ.");
//                                    continue;
//                                }

//                                var cccd = row.Cell(8).GetString()?.Trim();
//                                if (string.IsNullOrEmpty(cccd))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: CCCD rỗng.");
//                                    continue;
//                                }

//                                if (existingNames.Contains(hoTen))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: Trùng tên '{hoTen}'.");
//                                    continue;
//                                }
//                                if (existingCCCDs.Contains(cccd))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: Trùng mã nhân viên (CCCD) '{cccd}'.");
//                                    continue;
//                                }

//                                DateTime namSinh;
//                                if (row.Cell(7).DataType == XLDataType.DateTime)
//                                {
//                                    namSinh = row.Cell(7).GetDateTime();
//                                }
//                                else if (!DateTime.TryParse(row.Cell(7).GetString()?.Trim(), out namSinh))
//                                {
//                                    errors.Add($"Dòng {rowNumber}: Năm sinh không hợp lệ.");
//                                    continue;
//                                }

//                                var entity = new NhanVien
//                                {
//                                    Ho_Ten = hoTen,
//                                    Email = email,
//                                    GioiTinh = row.Cell(4).GetString() == "Nam" ? true :
//                                               (row.Cell(4).GetString() == "Nữ" ? false : null),
//                                    So_Dien_Thoai = row.Cell(5).GetString()?.Trim(),
//                                    Dia_Chi = row.Cell(6).GetString()?.Trim(),
//                                    NamSinh = namSinh,
//                                    CCCD = cccd,
//                                    Trang_Thai = row.Cell(9).GetString() == "Đang làm" ? true : false,
//                                    Ghi_Chu = row.Cell(10).GetString()?.Trim(),
//                                    AnhNhanVien = row.Cell(11).GetString()?.Trim()
//                                };

//                                await _service.AddAsync(entity);
//                                importedCount++;
//                                existingNames.Add(hoTen);
//                                existingCCCDs.Add(cccd);
//                            }
//                            catch (Exception ex)
//                            {
//                                errors.Add($"Dòng {rowNumber}: Lỗi xử lý dữ liệu - {ex.Message}");
//                            }
//                        }
//                    }
//                }

//                if (errors.Any())
//                {
//                    TempData["Error"] = string.Join("<br/>", errors);
//                    if (importedCount > 0)
//                    {
//                        TempData["Success"] = $"Import thành công {importedCount} bản ghi, nhưng có lỗi ở một số dòng.";
//                    }
//                }
//                else if (importedCount > 0)
//                {
//                    TempData["Success"] = $"Import thành công {importedCount} bản ghi.";
//                }
//                else
//                {
//                    TempData["Error"] = "Không có bản ghi nào được import.";
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi import file Excel.");
//                TempData["Error"] = $"Lỗi import toàn bộ file: {ex.Message}";
//            }

//            return RedirectToAction("Index");
//        }

//        private bool IsValidEmail(string email)
//        {
//            if (string.IsNullOrEmpty(email)) return false;
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}
using BE.models;
using ClosedXML.Excel;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FE.Controllers
{
    [RoleAuthorize(2)]
    public class NhanVienController : Controller
    {
        private readonly INhanVienService _service;
        private readonly ILogger<NhanVienController> _logger;
        private readonly string[] _allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
        private const long _maxImageBytes = 5 * 1024 * 1024; // 5MB

        public NhanVienController(INhanVienService service, ILogger<NhanVienController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Index + Details
        //[HttpGet]
        //public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
        //{
        //    var result = await _service.GetAllAsync();
        //    var filteredResult = result.AsQueryable();

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        filteredResult = filteredResult.Where(nv =>
        //            (nv.Ho_Ten ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            (nv.Email ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            (nv.So_Dien_Thoai ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (statusFilter.HasValue)
        //    {
        //        filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
        //    }

        //    ViewBag.TotalEmployees = result.Count();
        //    ViewBag.WorkingEmployees = result.Count(nv => nv.Trang_Thai == true);
        //    ViewBag.LeavingEmployees = result.Count(nv => nv.Trang_Thai == false);
        //    ViewBag.NewEmployees = result.Count(nv => (DateTime.Now - nv.NamSinh).TotalDays / 365 < 1);

        //    var totalItems = filteredResult.Count();
        //    var paginatedResult = filteredResult
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        //    ViewBag.CurrentPage = page;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.SearchTerm = searchTerm;
        //    ViewBag.StatusFilter = statusFilter;

        //    return View(paginatedResult);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
        {
            // bảo vệ giá trị
            if (page < 1) page = 1;
            pageSize = Math.Clamp(pageSize, 1, 100); // giới hạn 1..100 (thay đổi nếu muốn)

            var result = await _service.GetAllAsync();
            var filteredResult = result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(nv =>
                    (nv.Ho_Ten ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (nv.Email ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (nv.So_Dien_Thoai ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (statusFilter.HasValue)
            {
                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
            }

            ViewBag.TotalEmployees = result.Count();
            ViewBag.WorkingEmployees = result.Count(nv => nv.Trang_Thai == true);
            ViewBag.LeavingEmployees = result.Count(nv => nv.Trang_Thai == false);
            ViewBag.NewEmployees = result.Count(nv => (DateTime.Now - nv.NamSinh).TotalDays / 365 < 1);

            var totalItems = filteredResult.Count();
            var paginatedResult = filteredResult
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = statusFilter;

            return View(paginatedResult);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return PartialView("Details", result);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien entity, IFormFile anhNhanVien)
        {
            try
            {
                var allEmployees = await _service.GetAllAsync();

                if (entity.ID_Nhan_Vien != 0 && allEmployees.Any(e => e.ID_Nhan_Vien == entity.ID_Nhan_Vien))
                {
                    ModelState.AddModelError(nameof(entity.ID_Nhan_Vien), "Mã nhân viên đã tồn tại.");
                }

                if (string.IsNullOrWhiteSpace(entity.Ho_Ten))
                {
                    ModelState.AddModelError(nameof(entity.Ho_Ten), "Họ tên không được để trống.");
                }

                if (!string.IsNullOrWhiteSpace(entity.Email))
                {
                    if (!IsValidEmail(entity.Email))
                    {
                        ModelState.AddModelError(nameof(entity.Email), "Email không hợp lệ.");
                    }
                    else if (allEmployees.Any(e => string.Equals(e.Email, entity.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError(nameof(entity.Email), "Email đã tồn tại.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(entity.So_Dien_Thoai))
                {
                    if (!IsValidPhone(entity.So_Dien_Thoai))
                    {
                        ModelState.AddModelError(nameof(entity.So_Dien_Thoai), "Số điện thoại không hợp lệ.");
                    }
                    else if (allEmployees.Any(e => e.So_Dien_Thoai == entity.So_Dien_Thoai))
                    {
                        ModelState.AddModelError(nameof(entity.So_Dien_Thoai), "Số điện thoại đã tồn tại.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(entity.CCCD))
                {
                    if (allEmployees.Any(e => e.CCCD == entity.CCCD))
                    {
                        ModelState.AddModelError(nameof(entity.CCCD), "CCCD đã tồn tại.");
                    }
                }

                if (anhNhanVien != null && anhNhanVien.Length > 0)
                {
                    var uploadResult = await TrySaveImageAsync(anhNhanVien);
                    if (!uploadResult.Success)
                    {
                        ModelState.AddModelError("anhNhanVien", uploadResult.ErrorMessage);
                        return View(entity);
                    }

                    entity.AnhNhanVien = uploadResult.RelativePath;
                }

                entity.Trang_Thai = true;

                await _service.AddAsync(entity);

                TempData["Success"] = $"Thêm nhân viên {entity.Ho_Ten} thành công.";

                return RedirectToAction("Index", "NhanVien");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm nhân viên.");
                TempData["Error"] = $"Lỗi khi thêm nhân viên: {ex.Message}";
                return View(entity);
            }
        }

        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            ViewBag.IsAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien entity, IFormFile anhNhanVien)
        {
            try
            {
                var currentEmployee = await _service.GetByIdAsync(id);
                if (currentEmployee == null)
                {
                    return NotFound();
                }

                var allEmployees = await _service.GetAllAsync();

                if (string.IsNullOrWhiteSpace(entity.Ho_Ten))
                {
                    ModelState.AddModelError(nameof(entity.Ho_Ten), "Họ tên không được để trống.");
                }

                // Email
                if (!string.IsNullOrWhiteSpace(entity.Email))
                {
                    if (!IsValidEmail(entity.Email))
                    {
                        ModelState.AddModelError(nameof(entity.Email), "Email không hợp lệ.");
                    }
                    else if (allEmployees.Any(e => e.ID_Nhan_Vien != id &&
                                                   string.Equals(e.Email, entity.Email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError(nameof(entity.Email), "Email đã tồn tại.");
                    }
                }

                // SĐT
                if (!string.IsNullOrWhiteSpace(entity.So_Dien_Thoai))
                {
                    if (!IsValidPhone(entity.So_Dien_Thoai))
                    {
                        ModelState.AddModelError(nameof(entity.So_Dien_Thoai), "Số điện thoại không hợp lệ.");
                    }
                    else if (allEmployees.Any(e => e.ID_Nhan_Vien != id && e.So_Dien_Thoai == entity.So_Dien_Thoai))
                    {
                        ModelState.AddModelError(nameof(entity.So_Dien_Thoai), "Số điện thoại đã tồn tại.");
                    }
                }

                // CCCD
                if (!string.IsNullOrWhiteSpace(entity.CCCD))
                {
                    if (allEmployees.Any(e => e.ID_Nhan_Vien != id && e.CCCD == entity.CCCD))
                    {
                        ModelState.AddModelError(nameof(entity.CCCD), "CCCD đã tồn tại.");
                    }
                }

                // Upload ảnh mới (nếu có)
                if (anhNhanVien != null && anhNhanVien.Length > 0)
                {
                    var uploadResult = await TrySaveImageAsync(anhNhanVien);
                    if (!uploadResult.Success)
                    {
                        ModelState.AddModelError("anhNhanVien", uploadResult.ErrorMessage);
                        return View(entity);
                    }
                    entity.AnhNhanVien = uploadResult.RelativePath;
                }
                else
                {
                    entity.AnhNhanVien = currentEmployee.AnhNhanVien;
                }

                entity.ID_Nhan_Vien = id;
                await _service.UpdateAsync(id, entity);

                // ✅ Xác định các trường đã thay đổi
                var fieldsChanged = new List<string>();

                if (entity.Ho_Ten != currentEmployee.Ho_Ten) fieldsChanged.Add("Họ tên");
                if (entity.Email != currentEmployee.Email) fieldsChanged.Add("Email");
                if (entity.So_Dien_Thoai != currentEmployee.So_Dien_Thoai) fieldsChanged.Add("Số điện thoại");
                if (entity.CCCD != currentEmployee.CCCD) fieldsChanged.Add("CCCD");
                if (entity.Dia_Chi != currentEmployee.Dia_Chi) fieldsChanged.Add("Địa chỉ");
                if (entity.GioiTinh != currentEmployee.GioiTinh) fieldsChanged.Add("Giới tính");
                if (entity.NamSinh != currentEmployee.NamSinh) fieldsChanged.Add("Năm sinh");
                if (entity.Trang_Thai != currentEmployee.Trang_Thai) fieldsChanged.Add("Trạng thái");
                if (entity.Ghi_Chu != currentEmployee.Ghi_Chu) fieldsChanged.Add("Ghi chú");
                if (entity.AnhNhanVien != currentEmployee.AnhNhanVien) fieldsChanged.Add("Ảnh nhân viên");

                string changedMessage = fieldsChanged.Count > 0
                    ? $"Cập nhật {string.Join(", ", fieldsChanged)} thành công."
                    : "Không có thay đổi nào.";

                TempData["Success"] = changedMessage;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhân viên.");
                TempData["Error"] = $"Lỗi khi cập nhật nhân viên: {ex.Message}";
                return View(entity);
            }
        }

        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Đã xóa nhân viên thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Không tìm thấy nhân viên.";
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa nhân viên.");
                TempData["Error"] = $"Lỗi khi xóa nhân viên: {ex.Message}";
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected([FromBody] int[] ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một nhân viên." });
                }

                foreach (var id in ids)
                {
                    await _service.DeleteAsync(id);
                }
                return Json(new { success = true, message = "Xóa các nhân viên đã chọn thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa nhiều nhân viên.");
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Update Status
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusModel data)
        {
            try
            {
                if (data.Ids == null || !data.Ids.Any())
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một nhân viên." });
                }

                foreach (var id in data.Ids)
                {
                    var employee = await _service.GetByIdAsync(id);
                    if (employee != null)
                    {
                        employee.Trang_Thai = data.Status;
                        await _service.UpdateAsync(id, employee);
                    }
                }
                return Json(new { success = true, message = "Cập nhật trạng thái thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class UpdateStatusModel
        {
            public int[] Ids { get; set; }
            public bool? Status { get; set; }
        }
        #endregion

        #region Export + Import
        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string searchTerm = "", bool? statusFilter = null)
        {
            var result = await _service.GetAllAsync();
            var filteredResult = result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(nv =>
                    (nv.Ho_Ten ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (nv.Email ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (nv.So_Dien_Thoai ?? string.Empty).Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (statusFilter.HasValue)
            {
                filteredResult = filteredResult.Where(nv => nv.Trang_Thai == statusFilter.Value);
            }

            var data = filteredResult.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NhanVien");

                worksheet.Cell(1, 1).Value = "ID_Nhan_Vien";
                worksheet.Cell(1, 2).Value = "Ho_Ten";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "GioiTinh";
                worksheet.Cell(1, 5).Value = "So_Dien_Thoai";
                worksheet.Cell(1, 6).Value = "Dia_Chi";
                worksheet.Cell(1, 7).Value = "NamSinh";
                worksheet.Cell(1, 8).Value = "CCCD";
                worksheet.Cell(1, 9).Value = "Trang_Thai";
                worksheet.Cell(1, 10).Value = "Ghi_Chu";
                worksheet.Cell(1, 11).Value = "AnhNhanVien";
                worksheet.Cell(1, 12).Value = "AnhCCCD";

                for (int i = 0; i < data.Count; i++)
                {
                    var d = data[i];
                    worksheet.Cell(i + 2, 1).Value = d.ID_Nhan_Vien;
                    worksheet.Cell(i + 2, 2).Value = d.Ho_Ten;
                    worksheet.Cell(i + 2, 3).Value = d.Email;
                    worksheet.Cell(i + 2, 4).Value = d.GioiTinh.HasValue ? (d.GioiTinh.Value ? "Nam" : "Nữ") : "Khác";
                    worksheet.Cell(i + 2, 5).Value = d.So_Dien_Thoai;
                    worksheet.Cell(i + 2, 6).Value = d.Dia_Chi;
                    worksheet.Cell(i + 2, 7).Value = d.NamSinh.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 8).Value = d.CCCD;
                    worksheet.Cell(i + 2, 9).Value = d.Trang_Thai.HasValue ? (d.Trang_Thai.Value ? "Đang làm" : "Nghỉ") : "Không xác định";
                    worksheet.Cell(i + 2, 10).Value = d.Ghi_Chu;
                    worksheet.Cell(i + 2, 11).Value = d.AnhNhanVien;
                }

                var range = worksheet.RangeUsed();
                var table = range.CreateTable();
                table.Theme = XLTableTheme.TableStyleMedium9;
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuanLyNhanVien.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult Import()
        {
            return PartialView("_ImportExcel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn file Excel.";
                return RedirectToAction(nameof(Index));
            }

            var errors = new List<string>();
            var importedCount = 0;
            var existingEmployees = await _service.GetAllAsync();
            var existingNames = new HashSet<string>(existingEmployees.Select(e => e.Ho_Ten), StringComparer.OrdinalIgnoreCase);
            var existingCCCDs = new HashSet<string>(existingEmployees.Select(e => e.CCCD).Where(c => !string.IsNullOrEmpty(c)));

            try
            {
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
                            var rowNumber = row.RowNumber();
                            try
                            {
                                var hoTen = row.Cell(2).GetString()?.Trim();
                                if (string.IsNullOrEmpty(hoTen))
                                {
                                    errors.Add($"Dòng {rowNumber}: Bỏ qua vì họ tên rỗng.");
                                    continue;
                                }

                                var email = row.Cell(3).GetString()?.Trim();
                                if (!IsValidEmail(email))
                                {
                                    errors.Add($"Dòng {rowNumber}: Email '{email}' không hợp lệ.");
                                    continue;
                                }

                                var cccd = row.Cell(8).GetString()?.Trim();
                                if (string.IsNullOrEmpty(cccd))
                                {
                                    errors.Add($"Dòng {rowNumber}: CCCD rỗng.");
                                    continue;
                                }

                                if (existingNames.Contains(hoTen))
                                {
                                    errors.Add($"Dòng {rowNumber}: Trùng tên '{hoTen}'.");
                                    continue;
                                }
                                if (existingCCCDs.Contains(cccd))
                                {
                                    errors.Add($"Dòng {rowNumber}: Trùng CCCD '{cccd}'.");
                                    continue;
                                }

                                DateTime namSinh;
                                if (row.Cell(7).DataType == XLDataType.DateTime)
                                {
                                    namSinh = row.Cell(7).GetDateTime();
                                }
                                else if (!DateTime.TryParse(row.Cell(7).GetString()?.Trim(), out namSinh))
                                {
                                    errors.Add($"Dòng {rowNumber}: Năm sinh không hợp lệ.");
                                    continue;
                                }

                                var soDienThoai = row.Cell(5).GetString()?.Trim();
                                if (!string.IsNullOrEmpty(soDienThoai) && !IsValidPhone(soDienThoai))
                                {
                                    errors.Add($"Dòng {rowNumber}: SĐT '{soDienThoai}' không hợp lệ.");
                                    continue;
                                }

                                var entity = new NhanVien
                                {
                                    Ho_Ten = hoTen,
                                    Email = email,
                                    GioiTinh = row.Cell(4).GetString() == "Nam" ? true :
                                               (row.Cell(4).GetString() == "Nữ" ? false : null),
                                    So_Dien_Thoai = soDienThoai,
                                    Dia_Chi = row.Cell(6).GetString()?.Trim(),
                                    NamSinh = namSinh,
                                    CCCD = cccd,
                                    Trang_Thai = row.Cell(9).GetString() == "Đang làm" ? true : false,
                                    Ghi_Chu = row.Cell(10).GetString()?.Trim(),
                                    AnhNhanVien = row.Cell(11).GetString()?.Trim()
                                };

                                await _service.AddAsync(entity);
                                importedCount++;
                                existingNames.Add(hoTen);
                                existingCCCDs.Add(cccd);
                            }
                            catch (Exception exRow)
                            {
                                _logger.LogWarning(exRow, $"Lỗi xử lý dòng {rowNumber} trong file import.");
                                errors.Add($"Dòng {rowNumber}: Lỗi xử lý dữ liệu - {exRow.Message}");
                            }
                        }
                    }
                }

                if (errors.Any())
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    if (importedCount > 0)
                    {
                        TempData["Success"] = $"Import thành công {importedCount} bản ghi, nhưng có lỗi ở một số dòng.";
                    }
                }
                else if (importedCount > 0)
                {
                    TempData["Success"] = $"Import thành công {importedCount} bản ghi.";
                }
                else
                {
                    TempData["Error"] = "Không có bản ghi nào được import.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi import file Excel.");
                TempData["Error"] = $"Lỗi import toàn bộ file: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helpers
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Ví dụ: chấp nhận số gồm 9-12 chữ số, có thể bắt đầu bằng +84 hoặc 0
            var cleaned = phone.Trim();
            var pattern = @"^(\+?\d{9,12})$";
            return Regex.IsMatch(cleaned, pattern);
        }

        private async Task<(bool Success, string ErrorMessage, string RelativePath)> TrySaveImageAsync(IFormFile file)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedImageExtensions.Contains(extension))
                {
                    return (false, "Chỉ hỗ trợ định dạng .jpg, .jpeg, .png", null);
                }

                if (file.Length > _maxImageBytes)
                {
                    return (false, "Kích thước file không được vượt quá 5MB", null);
                }

                // đảm bảo thư mục tồn tại
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsRoot))
                {
                    Directory.CreateDirectory(uploadsRoot);
                }

                // đặt tên file an toàn: timestamp + GUID + extension
                var safeFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";
                var filePath = Path.Combine(uploadsRoot, safeFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relative = "/uploads/" + safeFileName;
                return (true, null, relative);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lưu file ảnh.");
                return (false, $"Lỗi lưu file ảnh: {ex.Message}", null);
            }
        }
        #endregion
    }
}
