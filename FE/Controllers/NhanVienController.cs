using BE.models;
using ClosedXML.Excel;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using Service.IService;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FE.Controllers
{
    [RoleAuthorize(2)]// Trang chỉ cho vai trò 2 truy cập// Phương thức này đươc để trong thư mục Filters nhé ae

    public class NhanVienController : Controller
    {
        private readonly INhanVienService _service;

        public NhanVienController(INhanVienService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "", bool? statusFilter = null, int page = 1, int pageSize = 10)
        {
            var result = await _service.GetAllAsync();
            var filteredResult = result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(nv => nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVien entity, IFormFile anhNhanVien)
        {
            if (anhNhanVien != null && anhNhanVien.Length > 0)
            {
                var fileName = Path.GetFileName(anhNhanVien.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhNhanVien.CopyToAsync(stream);
                }

                entity.AnhNhanVien = "/uploads/" + fileName;
            }

            await _service.AddAsync(entity);
            return RedirectToAction("Index", "NhanVien");
        }

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
            var currentEmployee = await _service.GetByIdAsync(id);
            if (currentEmployee == null)
            {
                return NotFound();
            }

            if (anhNhanVien != null && anhNhanVien.Length > 0)
            {
                var fileName = Path.GetFileName(anhNhanVien.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhNhanVien.CopyToAsync(stream);
                }

                entity.AnhNhanVien = "/uploads/" + fileName;
            }
            else
            {
                entity.AnhNhanVien = currentEmployee.AnhNhanVien;
            }

            entity.ID_Nhan_Vien = id;
            await _service.UpdateAsync(id, entity);
            return RedirectToAction("Index", "NhanVien");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSelected([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    await _service.DeleteAsync(id);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] dynamic data)
        {
            try
            {
                var ids = data.ids.ToObject<int[]>();
                var status = data.status.ToObject<bool?>();
                foreach (var id in ids)
                {
                    var employee = await _service.GetByIdAsync(id);
                    if (employee != null)
                    {
                        employee.Trang_Thai = status;
                        await _service.UpdateAsync(id, employee);
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string searchTerm = "", bool? statusFilter = null)
        {
            var result = await _service.GetAllAsync();
            var filteredResult = result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredResult = filteredResult.Where(nv => nv.Ho_Ten.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
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
                    worksheet.Cell(i + 2, 1).Value = data[i].ID_Nhan_Vien;
                    worksheet.Cell(i + 2, 2).Value = data[i].Ho_Ten;
                    worksheet.Cell(i + 2, 3).Value = data[i].Email;
                    worksheet.Cell(i + 2, 4).Value = data[i].GioiTinh.HasValue ? (data[i].GioiTinh.Value ? "Nam" : "Nữ") : "Khác";
                    worksheet.Cell(i + 2, 5).Value = data[i].So_Dien_Thoai;
                    worksheet.Cell(i + 2, 6).Value = data[i].Dia_Chi;
                    worksheet.Cell(i + 2, 7).Value = data[i].NamSinh.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 8).Value = data[i].CCCD;
                    worksheet.Cell(i + 2, 9).Value = data[i].Trang_Thai.HasValue ? (data[i].Trang_Thai.Value ? "Đang làm" : "Nghỉ") : "Không xác định";
                    worksheet.Cell(i + 2, 10).Value = data[i].Ghi_Chu;
                    worksheet.Cell(i + 2, 11).Value = data[i].AnhNhanVien;
                    worksheet.Cell(i + 2, 12).Value = data[i].AnhCCCD;
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
                        var entity = new NhanVien
                        {
                            Ho_Ten = row.Cell(2).GetString(),
                            Email = row.Cell(3).GetString(),
                            GioiTinh = row.Cell(4).GetString() == "Nam" ? true : (row.Cell(4).GetString() == "Nữ" ? false : null),
                            So_Dien_Thoai = row.Cell(5).GetString(),
                            Dia_Chi = row.Cell(6).GetString(),
                            NamSinh = DateTime.Parse(row.Cell(7).GetString()),
                            CCCD = row.Cell(8).GetString(),
                            Trang_Thai = row.Cell(9).GetString() == "Đang làm" ? true : false,
                            Ghi_Chu = row.Cell(10).GetString(),
                            AnhNhanVien = row.Cell(11).GetString(),
                            AnhCCCD = row.Cell(12).GetString()
                        };

                        // Validate cơ bản
                        if (string.IsNullOrEmpty(entity.Ho_Ten))
                            continue; // Bỏ qua row lỗi

                        await _service.AddAsync(entity);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        // Hàm hỗ trợ kiểm tra email
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
    }
}