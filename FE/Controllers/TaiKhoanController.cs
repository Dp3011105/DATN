using BE.models;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Linq;
using ClosedXML.Excel;
using System.IO;

namespace FE.Controllers
{
    [RoleAuthorize(2)] // Trang cho phép cả vai trò 2
    public class TaiKhoanController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;
        private readonly INhanVienService _nhanVienService;

        public TaiKhoanController(ITaiKhoanService taiKhoanService, INhanVienService nhanVienService)
        {
            _taiKhoanService = taiKhoanService;
            _nhanVienService = nhanVienService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var taiKhoans = await _taiKhoanService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                taiKhoans = taiKhoans
                    .Where(t =>
                        (t.NhanVien != null &&
                         (t.NhanVien.Ho_Ten.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                         (!string.IsNullOrEmpty(t.NhanVien.Email) && t.NhanVien.Email.Contains(search, StringComparison.OrdinalIgnoreCase))))
                        || (!string.IsNullOrEmpty(t.Ten_Nguoi_Dung) && t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            // Thống kê
            ViewBag.TongTaiKhoan = taiKhoans.Count();
            ViewBag.TaiKhoanHoatDong = taiKhoans.Count(t => t.Trang_Thai);
            ViewBag.TaiKhoanKhoa = taiKhoans.Count(t => !t.Trang_Thai);

            // Pagination
            int totalItems = taiKhoans.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range
            var paginatedTaiKhoans = taiKhoans
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;

            return View(paginatedTaiKhoans);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string? searchNhanVien)
        {
            var nhanViens = await _nhanVienService.GetAllAsync();
            var taiKhoans = await _taiKhoanService.GetAllAsync();

            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

            // Lọc ra nhân viên chưa có tài khoản
            nhanViens = nhanViens
                .Where(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
                .ToList();

            // Tìm kiếm nếu có search
            if (!string.IsNullOrWhiteSpace(searchNhanVien))
            {
                nhanViens = nhanViens
                    .Where(nv =>
                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.NhanViens = nhanViens;
            ViewBag.SearchNhanVien = searchNhanVien;

            // 📊 Thống kê
            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count();
            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

            return View(new TaiKhoan());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaiKhoan tk, string? searchNhanVien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                tk.Trang_Thai = true;
                await _taiKhoanService.AddAsync(tk);
                return RedirectToAction("Index", "TaiKhoan");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi thêm: {ex.Message}";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string? searchNhanVien)
        {
            var taiKhoan = await _taiKhoanService.GetByIdAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            var nhanViens = await _nhanVienService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchNhanVien))
            {
                nhanViens = nhanViens
                    .Where(nv =>
                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            ViewBag.NhanViens = nhanViens;
            ViewBag.SearchNhanVien = searchNhanVien;
            return View(taiKhoan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaiKhoan tk, string? searchNhanVien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                await _taiKhoanService.UpdateAsync(id, tk);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi cập nhật: {ex.Message}";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tk = await _taiKhoanService.GetByIdAsync(id);
            if (tk == null) return NotFound();

            var roles = tk.TaiKhoanVaiTros.Select(r => r.VaiTro.Ten_Vai_Tro).ToList();

            if (roles.Contains("Admin"))
            {
                TempData["Error"] = "Không được phép xóa tài khoản Admin.";
                return RedirectToAction("Index");
            }

            await _taiKhoanService.DeleteAsync(id);
            TempData["Success"] = "Xóa tài khoản thành công.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string? search)
        {
            var taiKhoans = await _taiKhoanService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                taiKhoans = taiKhoans
                    .Where(t =>
                        t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(t.Email) && t.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TaiKhoan");

                // Headers
                worksheet.Cell("A1").Value = "#";
                worksheet.Cell("B1").Value = "Tên người dùng";
                worksheet.Cell("C1").Value = "Email";
                worksheet.Cell("D1").Value = "Mật khẩu";
                worksheet.Cell("E1").Value = "Trạng thái";
                worksheet.Cell("F1").Value = "Ngày tạo";

                // Format headers
                worksheet.Range("A1:F1").Style.Font.Bold = true;
                worksheet.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                int stt = 1;
                foreach (var item in taiKhoans)
                {
                    worksheet.Cell(row, 1).Value = stt;
                    worksheet.Cell(row, 2).Value = item.Ten_Nguoi_Dung;
                    worksheet.Cell(row, 3).Value = item.Email;
                    worksheet.Cell(row, 4).Value = "******"; // Không xuất mật khẩu thực
                    worksheet.Cell(row, 5).Value = item.Trang_Thai ? "Hoạt động" : "Khóa";
                    worksheet.Cell(row, 6).Value = item.Ngay_Tao.ToString("dd/MM/yyyy");
                    row++;
                    stt++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachTaiKhoan.xlsx");
                }
            }
        }
    }
}