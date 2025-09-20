//using BE.models;
//using FE.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Service.IService;
//using System.Linq;
//using ClosedXML.Excel;
//using System.IO;

//namespace FE.Controllers
//{
//    [RoleAuthorize(2)] // Trang cho phép cả vai trò 2
//    public class TaiKhoanController : Controller
//    {
//        private readonly ITaiKhoanService _taiKhoanService;
//        private readonly INhanVienService _nhanVienService;

//        public TaiKhoanController(ITaiKhoanService taiKhoanService, INhanVienService nhanVienService)
//        {
//            _taiKhoanService = taiKhoanService;
//            _nhanVienService = nhanVienService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
//        {
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                taiKhoans = taiKhoans
//                    .Where(t =>
//                        (t.NhanVien != null &&
//                         (t.NhanVien.Ho_Ten.Contains(search, StringComparison.OrdinalIgnoreCase) ||
//                         (!string.IsNullOrEmpty(t.NhanVien.Email) && t.NhanVien.Email.Contains(search, StringComparison.OrdinalIgnoreCase))))
//                        || (!string.IsNullOrEmpty(t.Ten_Nguoi_Dung) && t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            // Thống kê
//            ViewBag.TongTaiKhoan = taiKhoans.Count();
//            ViewBag.TaiKhoanHoatDong = taiKhoans.Count(t => t.Trang_Thai);
//            ViewBag.TaiKhoanKhoa = taiKhoans.Count(t => !t.Trang_Thai);

//            // Pagination
//            int totalItems = taiKhoans.Count();
//            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
//            page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range
//            var paginatedTaiKhoans = taiKhoans
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            ViewBag.CurrentPage = page;
//            ViewBag.PageSize = pageSize;
//            ViewBag.TotalPages = totalPages;
//            ViewBag.Search = search;

//            return View(paginatedTaiKhoans);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Create(string? searchNhanVien)
//        {
//            var nhanViens = await _nhanVienService.GetAllAsync();
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
//            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

//            // Lọc ra nhân viên chưa có tài khoản
//            nhanViens = nhanViens
//                .Where(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
//                .ToList();

//            // Tìm kiếm nếu có search
//            if (!string.IsNullOrWhiteSpace(searchNhanVien))
//            {
//                nhanViens = nhanViens
//                    .Where(nv =>
//                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            ViewBag.NhanViens = nhanViens;
//            ViewBag.SearchNhanVien = searchNhanVien;

//            // 📊 Thống kê
//            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count();
//            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

//            return View(new TaiKhoan());
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(TaiKhoan tk, string? searchNhanVien)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            try
//            {
//                tk.Trang_Thai = true;
//                await _taiKhoanService.AddAsync(tk);
//                return RedirectToAction("Index", "TaiKhoan");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = $"Lỗi khi thêm: {ex.Message}";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(int id, string? searchNhanVien)
//        {
//            var taiKhoan = await _taiKhoanService.GetByIdAsync(id); // Giả sử method này đã include NhanVien
//            if (taiKhoan == null)
//            {
//                return NotFound();
//            }

//            // Đảm bảo NhanVien được tải nếu chưa được include trong GetByIdAsync
//            if (taiKhoan.NhanVien == null && taiKhoan.ID_Nhan_Vien.HasValue)
//            {
//                taiKhoan.NhanVien = await _nhanVienService.GetByIdAsync(taiKhoan.ID_Nhan_Vien.Value);
//            }

//            var nhanViens = await _nhanVienService.GetAllAsync();
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
//            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

//            // Lọc ra nhân viên đã có tài khoản
//            nhanViens = nhanViens
//                .Where(nv => nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
//                .ToList();

//            // Tìm kiếm nếu có search
//            if (!string.IsNullOrWhiteSpace(searchNhanVien))
//            {
//                nhanViens = nhanViens
//                    .Where(nv =>
//                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            ViewBag.NhanViens = nhanViens;
//            ViewBag.SearchNhanVien = searchNhanVien;
//            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien)); // Thường sẽ là 0
//            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

//            return View(taiKhoan);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(int id, TaiKhoan tk, string? searchNhanVien)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            try
//            {
//                tk.ID_Tai_Khoan = id; // Đảm bảo ID không bị thay đổi
//                await _taiKhoanService.UpdateAsync(id, tk);
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = $"Lỗi khi cập nhật: {ex.Message}";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var tk = await _taiKhoanService.GetByIdAsync(id);
//            if (tk == null) return NotFound();

//            var roles = tk.TaiKhoanVaiTros.Select(r => r.VaiTro.Ten_Vai_Tro).ToList();

//            if (roles.Contains("Admin"))
//            {
//                TempData["Error"] = "Không được phép xóa tài khoản Admin.";
//                return RedirectToAction("Index");
//            }

//            await _taiKhoanService.DeleteAsync(id);
//            TempData["Success"] = "Xóa tài khoản thành công.";
//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToExcel(string? search)
//        {
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                taiKhoans = taiKhoans
//                    .Where(t =>
//                        t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(t.Email) && t.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            using (var workbook = new XLWorkbook())
//            {
//                var worksheet = workbook.Worksheets.Add("TaiKhoan");

//                // Headers
//                worksheet.Cell("A1").Value = "#";
//                worksheet.Cell("B1").Value = "Tên người dùng";
//                worksheet.Cell("C1").Value = "Email";
//                worksheet.Cell("D1").Value = "Mật khẩu";
//                worksheet.Cell("E1").Value = "Trạng thái";
//                worksheet.Cell("F1").Value = "Ngày tạo";

//                // Format headers
//                worksheet.Range("A1:F1").Style.Font.Bold = true;
//                worksheet.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.LightBlue;
//                worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

//                int row = 2;
//                int stt = 1;
//                foreach (var item in taiKhoans)
//                {
//                    worksheet.Cell(row, 1).Value = stt;
//                    worksheet.Cell(row, 2).Value = item.Ten_Nguoi_Dung;
//                    worksheet.Cell(row, 3).Value = item.Email;
//                    worksheet.Cell(row, 4).Value = "******"; // Không xuất mật khẩu thực
//                    worksheet.Cell(row, 5).Value = item.Trang_Thai ? "Hoạt động" : "Khóa";
//                    worksheet.Cell(row, 6).Value = item.Ngay_Tao.ToString("dd/MM/yyyy");
//                    row++;
//                    stt++;
//                }

//                // Auto-fit columns
//                worksheet.Columns().AdjustToContents();

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachTaiKhoan.xlsx");
//                }
//            }
//        }
//    }
//}
//using BE.models;
//using FE.Filters;
//using Microsoft.AspNetCore.Mvc;
//using Service.IService;
//using System.Linq;
//using ClosedXML.Excel;
//using System.IO;
//using System.Text.RegularExpressions;
//using System;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//// Nếu dùng BCrypt: cài BCrypt.Net-Next
//using BCryptNet = BCrypt.Net.BCrypt;

//namespace FE.Controllers
//{
//    [RoleAuthorize(2)] // Trang cho phép cả vai trò 2
//    public class TaiKhoanController : Controller
//    {
//        private readonly ITaiKhoanService _taiKhoanService;
//        private readonly INhanVienService _nhanVienService;

//        public TaiKhoanController(ITaiKhoanService taiKhoanService, INhanVienService nhanVienService)
//        {
//            _taiKhoanService = taiKhoanService;
//            _nhanVienService = nhanVienService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
//        {
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                taiKhoans = taiKhoans
//                    .Where(t =>
//                        (t.NhanVien != null &&
//                         (t.NhanVien.Ho_Ten.Contains(search, StringComparison.OrdinalIgnoreCase) ||
//                         (!string.IsNullOrEmpty(t.NhanVien.Email) && t.NhanVien.Email.Contains(search, StringComparison.OrdinalIgnoreCase))))
//                        || (!string.IsNullOrEmpty(t.Ten_Nguoi_Dung) && t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            // Thống kê
//            ViewBag.TongTaiKhoan = taiKhoans.Count();
//            ViewBag.TaiKhoanHoatDong = taiKhoans.Count(t => t.Trang_Thai);
//            ViewBag.TaiKhoanKhoa = taiKhoans.Count(t => !t.Trang_Thai);

//            // Pagination
//            int totalItems = taiKhoans.Count();
//            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
//            if (totalPages == 0) totalPages = 1;
//            page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range
//            var paginatedTaiKhoans = taiKhoans
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            ViewBag.CurrentPage = page;
//            ViewBag.PageSize = pageSize;
//            ViewBag.TotalPages = totalPages;
//            ViewBag.Search = search;

//            return View(paginatedTaiKhoans);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Create(string? searchNhanVien)
//        {
//            var nhanViens = await _nhanVienService.GetAllAsync();
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
//            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

//            // Lọc ra nhân viên chưa có tài khoản
//            nhanViens = nhanViens
//                .Where(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
//                .ToList();

//            // Tìm kiếm nếu có search
//            if (!string.IsNullOrWhiteSpace(searchNhanVien))
//            {
//                nhanViens = nhanViens
//                    .Where(nv =>
//                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            ViewBag.NhanViens = nhanViens;
//            ViewBag.SearchNhanVien = searchNhanVien;

//            // 📊 Thống kê
//            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count();
//            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

//            return View(new TaiKhoan());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(TaiKhoan tk, string? searchNhanVien)
//        {
//            // Giữ nguyên hành vi: nếu model invalid, trả view
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            // Lấy danh sách tài khoản để kiểm tra unique
//            var allAccounts = await _taiKhoanService.GetAllAsync();

//            // Username (Ten_Nguoi_Dung) bắt buộc & unique
//            if (string.IsNullOrWhiteSpace(tk.Ten_Nguoi_Dung))
//            {
//                ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng bắt buộc.");
//            }
//            else
//            {
//                var existsUser = allAccounts.Any(a => string.Equals(a.Ten_Nguoi_Dung?.Trim(), tk.Ten_Nguoi_Dung.Trim(), StringComparison.OrdinalIgnoreCase));
//                if (existsUser)
//                {
//                    ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
//                }
//            }

//            // Email (nếu sử dụng) kiểm tra định dạng & unique
//            if (!string.IsNullOrWhiteSpace(tk.Email))
//            {
//                if (!IsValidEmail(tk.Email))
//                {
//                    ModelState.AddModelError(nameof(tk.Email), "Email không hợp lệ.");
//                }
//                else
//                {
//                    var existsEmail = allAccounts.Any(a => !string.IsNullOrEmpty(a.Email) && string.Equals(a.Email.Trim(), tk.Email.Trim(), StringComparison.OrdinalIgnoreCase));
//                    if (existsEmail)
//                    {
//                        ModelState.AddModelError(nameof(tk.Email), "Email đã được sử dụng bởi tài khoản khác.");
//                    }
//                }
//            }

//            // Mat_Khau bắt buộc và phải mạnh
//            if (string.IsNullOrWhiteSpace(tk.Mat_Khau))
//            {
//                ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu bắt buộc.");
//            }
//            else if (!IsPasswordStrong(tk.Mat_Khau))
//            {
//                ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu yếu. Mật khẩu phải ít nhất 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.");
//            }

//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Vui lòng sửa các lỗi trước khi lưu.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            try
//            {
//                tk.Trang_Thai = true;

//                // Hash mật khẩu trước khi lưu (nếu service chưa hash). Bạn có thể bỏ nếu service đã làm.
//                if (!string.IsNullOrWhiteSpace(tk.Mat_Khau))
//                {
//                    tk.Mat_Khau = BCryptNet.HashPassword(tk.Mat_Khau);
//                }

//                await _taiKhoanService.AddAsync(tk);
//                TempData["Success"] = "Tạo tài khoản thành công.";
//                return RedirectToAction("Index", "TaiKhoan");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = $"Lỗi khi thêm: {ex.Message}";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(int id, string? searchNhanVien)
//        {
//            var taiKhoan = await _taiKhoanService.GetByIdAsync(id); // Giả sử method này đã include NhanVien
//            if (taiKhoan == null)
//            {
//                return NotFound();
//            }

//            // Đảm bảo NhanVien được tải nếu chưa được include trong GetByIdAsync
//            if (taiKhoan.NhanVien == null && taiKhoan.ID_Nhan_Vien.HasValue)
//            {
//                taiKhoan.NhanVien = await _nhanVienService.GetByIdAsync(taiKhoan.ID_Nhan_Vien.Value);
//            }

//            var nhanViens = await _nhanVienService.GetAllAsync();
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
//            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

//            // Lọc ra nhân viên đã có tài khoản
//            nhanViens = nhanViens
//                .Where(nv => nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
//                .ToList();

//            // Tìm kiếm nếu có search
//            if (!string.IsNullOrWhiteSpace(searchNhanVien))
//            {
//                nhanViens = nhanViens
//                    .Where(nv =>
//                        nv.Ho_Ten.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(nv.Email) && nv.Email.Contains(searchNhanVien, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            ViewBag.NhanViens = nhanViens;
//            ViewBag.SearchNhanVien = searchNhanVien;
//            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien)); // Thường sẽ là 0
//            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

//            return View(taiKhoan);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, TaiKhoan tk, string? searchNhanVien)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            var allAccounts = await _taiKhoanService.GetAllAsync();

//            // Username unique check (exclude current record)
//            if (string.IsNullOrWhiteSpace(tk.Ten_Nguoi_Dung))
//            {
//                ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng bắt buộc.");
//            }
//            else
//            {
//                var existsUser = allAccounts.Any(a => a.ID_Tai_Khoan != id &&
//                    string.Equals(a.Ten_Nguoi_Dung?.Trim(), tk.Ten_Nguoi_Dung.Trim(), StringComparison.OrdinalIgnoreCase));
//                if (existsUser)
//                    ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
//            }

//            // Email validation (optional)
//            if (!string.IsNullOrWhiteSpace(tk.Email))
//            {
//                if (!IsValidEmail(tk.Email))
//                    ModelState.AddModelError(nameof(tk.Email), "Email không hợp lệ.");
//                else
//                {
//                    var existsEmail = allAccounts.Any(a => a.ID_Tai_Khoan != id &&
//                        !string.IsNullOrEmpty(a.Email) && string.Equals(a.Email.Trim(), tk.Email.Trim(), StringComparison.OrdinalIgnoreCase));
//                    if (existsEmail)
//                        ModelState.AddModelError(nameof(tk.Email), "Email đã được sử dụng bởi tài khoản khác.");
//                }
//            }

//            // If password changed (non-empty), validate strength and hash
//            if (!string.IsNullOrWhiteSpace(tk.Mat_Khau))
//            {
//                if (!IsPasswordStrong(tk.Mat_Khau))
//                    ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu yếu. Mật khẩu phải ít nhất 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.");
//                else
//                {
//                    // Hash new password
//                    tk.Mat_Khau = BCryptNet.HashPassword(tk.Mat_Khau);
//                }
//            }
//            else
//            {
//                // Nếu mật khẩu rỗng trong form Edit => giữ nguyên mật khẩu hiện có.
//                var existing = await _taiKhoanService.GetByIdAsync(id);
//                if (existing != null) tk.Mat_Khau = existing.Mat_Khau;
//            }

//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Vui lòng sửa các lỗi trước khi lưu.";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }

//            try
//            {
//                tk.ID_Tai_Khoan = id;
//                await _taiKhoanService.UpdateAsync(id, tk);
//                TempData["Success"] = "Cập nhật tài khoản thành công.";
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = $"Lỗi khi cập nhật: {ex.Message}";
//                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
//                ViewBag.SearchNhanVien = searchNhanVien;
//                return View(tk);
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var tk = await _taiKhoanService.GetByIdAsync(id);
//            if (tk == null) return NotFound();

//            var roles = tk.TaiKhoanVaiTros?.Select(r => r.VaiTro.Ten_Vai_Tro).ToList() ?? new List<string>();

//            if (roles.Contains("Admin"))
//            {
//                TempData["Error"] = "Không được phép xóa tài khoản Admin.";
//                return RedirectToAction("Index");
//            }

//            await _taiKhoanService.DeleteAsync(id);
//            TempData["Success"] = "Xóa tài khoản thành công.";
//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        public async Task<IActionResult> ExportToExcel(string? search)
//        {
//            var taiKhoans = await _taiKhoanService.GetAllAsync();

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                taiKhoans = taiKhoans
//                    .Where(t =>
//                        t.Ten_Nguoi_Dung.Contains(search, StringComparison.OrdinalIgnoreCase) ||
//                        (!string.IsNullOrEmpty(t.Email) && t.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
//                    )
//                    .ToList();
//            }

//            using (var workbook = new XLWorkbook())
//            {
//                var worksheet = workbook.Worksheets.Add("TaiKhoan");

//                // Headers
//                worksheet.Cell("A1").Value = "#";
//                worksheet.Cell("B1").Value = "Tên người dùng";
//                worksheet.Cell("C1").Value = "Email";
//                worksheet.Cell("D1").Value = "Mật khẩu";
//                worksheet.Cell("E1").Value = "Trạng thái";
//                worksheet.Cell("F1").Value = "Ngày tạo";

//                // Format headers
//                worksheet.Range("A1:F1").Style.Font.Bold = true;
//                worksheet.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.LightBlue;
//                worksheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

//                int row = 2;
//                int stt = 1;
//                foreach (var item in taiKhoans)
//                {
//                    worksheet.Cell(row, 1).Value = stt;
//                    worksheet.Cell(row, 2).Value = item.Ten_Nguoi_Dung;
//                    worksheet.Cell(row, 3).Value = item.Email;
//                    worksheet.Cell(row, 4).Value = "******"; // Không xuất mật khẩu thực
//                    worksheet.Cell(row, 5).Value = item.Trang_Thai ? "Hoạt động" : "Khóa";
//                    worksheet.Cell(row, 6).Value = item.Ngay_Tao.ToString("dd/MM/yyyy");
//                    row++;
//                    stt++;
//                }

//                // Auto-fit columns
//                worksheet.Columns().AdjustToContents();

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachTaiKhoan.xlsx");
//                }
//            }
//        }

//        // Helper: validate email
//        private bool IsValidEmail(string email)
//        {
//            if (string.IsNullOrWhiteSpace(email)) return false;
//            try
//            {
//                return Regex.IsMatch(email,
//                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
//                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Helper: strong password check
//        private bool IsPasswordStrong(string password)
//        {
//            if (string.IsNullOrEmpty(password) || password.Length < 8) return false;

//            bool hasUpper = password.Any(char.IsUpper);
//            bool hasLower = password.Any(char.IsLower);
//            bool hasDigit = password.Any(char.IsDigit);
//            bool hasSpecial = Regex.IsMatch(password, @"[\W_]"); // non-word char or underscore

//            return hasUpper && hasLower && hasDigit && hasSpecial;
//        }
//    }
//}
using BE.models;
using FE.Filters;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Linq;
using ClosedXML.Excel;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

// Nếu dùng BCrypt: cài BCrypt.Net-Next
using BCryptNet = BCrypt.Net.BCrypt;

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
            if (totalPages == 0) totalPages = 1;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoan tk, string? searchNhanVien)
        {
            // Giữ nguyên hành vi: nếu model invalid, trả view
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            // Lấy danh sách tài khoản để kiểm tra unique
            var allAccounts = await _taiKhoanService.GetAllAsync();

            // Username (Ten_Nguoi_Dung) bắt buộc & unique
            if (string.IsNullOrWhiteSpace(tk.Ten_Nguoi_Dung))
            {
                ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng bắt buộc.");
            }
            else
            {
                var existsUser = allAccounts.Any(a => string.Equals(a.Ten_Nguoi_Dung?.Trim(), tk.Ten_Nguoi_Dung.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existsUser)
                {
                    ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
                }
            }

            // Email (nếu sử dụng) kiểm tra định dạng & unique
            if (!string.IsNullOrWhiteSpace(tk.Email))
            {
                if (!IsValidEmail(tk.Email))
                {
                    ModelState.AddModelError(nameof(tk.Email), "Email không hợp lệ.");
                }
                else
                {
                    var existsEmail = allAccounts.Any(a => !string.IsNullOrEmpty(a.Email) && string.Equals(a.Email.Trim(), tk.Email.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (existsEmail)
                    {
                        ModelState.AddModelError(nameof(tk.Email), "Email đã được sử dụng bởi tài khoản khác.");
                    }
                }
            }

            // Mat_Khau bắt buộc và phải mạnh
            if (string.IsNullOrWhiteSpace(tk.Mat_Khau))
            {
                ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu bắt buộc.");
            }
            else if (!IsPasswordStrong(tk.Mat_Khau))
            {
                ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu yếu. Mật khẩu phải ít nhất 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng sửa các lỗi trước khi lưu.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                tk.Trang_Thai = true;

                // Hash mật khẩu trước khi lưu (nếu service chưa hash). Bạn có thể bỏ nếu service đã làm.
                if (!string.IsNullOrWhiteSpace(tk.Mat_Khau))
                {
                    tk.Mat_Khau = BCryptNet.HashPassword(tk.Mat_Khau);
                }

                await _taiKhoanService.AddAsync(tk);

                // Ghi thông báo chi tiết vào TempData
                var userDisplay = string.IsNullOrWhiteSpace(tk.Ten_Nguoi_Dung) ? "(không có tên)" : tk.Ten_Nguoi_Dung.Trim();
                TempData["Success"] = $"Tạo tài khoản thành công cho  {userDisplay}";

                return RedirectToAction("Index", "TaiKhoan");
            }
            catch (Exception ex)
            {
                // Ghi lỗi vào cả ViewBag và TempData để UI có thể show toast + detail
                ViewBag.Error = $"Lỗi khi thêm {ex.Message}";
                TempData["Error"] = $"Lỗi khi thêm tài khoản {ex.Message}";

                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string? searchNhanVien)
        {
            var taiKhoan = await _taiKhoanService.GetByIdAsync(id); // Giả sử method này đã include NhanVien
            if (taiKhoan == null)
            {
                return NotFound();
            }

            // Đảm bảo NhanVien được tải nếu chưa được include trong GetByIdAsync
            if (taiKhoan.NhanVien == null && taiKhoan.ID_Nhan_Vien.HasValue)
            {
                taiKhoan.NhanVien = await _nhanVienService.GetByIdAsync(taiKhoan.ID_Nhan_Vien.Value);
            }

            var nhanViens = await _nhanVienService.GetAllAsync();
            var taiKhoans = await _taiKhoanService.GetAllAsync();

            // Lấy danh sách ID_Nhan_Vien đã có tài khoản
            var nhanVienDaCoTK = taiKhoans.Select(t => t.ID_Nhan_Vien).ToHashSet();

            // Lọc ra nhân viên đã có tài khoản
            nhanViens = nhanViens
                .Where(nv => nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien))
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
            ViewBag.SoNhanVienChuaCoTK = nhanViens.Count(nv => !nhanVienDaCoTK.Contains(nv.ID_Nhan_Vien)); // Thường sẽ là 0
            ViewBag.SoNhanVienDaCoTK = taiKhoans.Count();

            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaiKhoan tk, string? searchNhanVien)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ. Vui lòng kiểm tra các trường bắt buộc.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            // Load existing before changes để so sánh các trường đã thay đổi (giữ nguyên logic khác)
            var existingBefore = await _taiKhoanService.GetByIdAsync(id);
            if (existingBefore == null)
            {
                return NotFound();
            }

            var allAccounts = await _taiKhoanService.GetAllAsync();

            // Username unique check (exclude current record)
            if (string.IsNullOrWhiteSpace(tk.Ten_Nguoi_Dung))
            {
                ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng bắt buộc.");
            }
            else
            {
                var existsUser = allAccounts.Any(a => a.ID_Tai_Khoan != id &&
                    string.Equals(a.Ten_Nguoi_Dung?.Trim(), tk.Ten_Nguoi_Dung.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existsUser)
                    ModelState.AddModelError(nameof(tk.Ten_Nguoi_Dung), "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
            }

            // Email validation (optional)
            if (!string.IsNullOrWhiteSpace(tk.Email))
            {
                if (!IsValidEmail(tk.Email))
                    ModelState.AddModelError(nameof(tk.Email), "Email không hợp lệ.");
                else
                {
                    var existsEmail = allAccounts.Any(a => a.ID_Tai_Khoan != id &&
                        !string.IsNullOrEmpty(a.Email) && string.Equals(a.Email.Trim(), tk.Email.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (existsEmail)
                        ModelState.AddModelError(nameof(tk.Email), "Email đã được sử dụng bởi tài khoản khác.");
                }
            }

            // If password changed (non-empty), validate strength and hash
            bool passwordWillChange = !string.IsNullOrWhiteSpace(tk.Mat_Khau);
            if (passwordWillChange)
            {
                if (!IsPasswordStrong(tk.Mat_Khau))
                    ModelState.AddModelError(nameof(tk.Mat_Khau), "Mật khẩu yếu. Mật khẩu phải ít nhất 8 ký tự, gồm chữ hoa, chữ thường, chữ số và ký tự đặc biệt.");
                else
                {
                    // Hash new password
                    tk.Mat_Khau = BCryptNet.HashPassword(tk.Mat_Khau);
                }
            }
            else
            {
                // Nếu mật khẩu rỗng trong form Edit => giữ nguyên mật khẩu hiện có.
                var existing = await _taiKhoanService.GetByIdAsync(id);
                if (existing != null) tk.Mat_Khau = existing.Mat_Khau;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng sửa các lỗi trước khi lưu.";
                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }

            try
            {
                tk.ID_Tai_Khoan = id;
                await _taiKhoanService.UpdateAsync(id, tk);

                // Build list of changed fields for message
                var changedFields = new List<string>();
                if (!string.Equals(existingBefore.Ten_Nguoi_Dung?.Trim(), tk.Ten_Nguoi_Dung?.Trim(), StringComparison.OrdinalIgnoreCase))
                    changedFields.Add("Tên người dùng");
                if (!string.Equals(existingBefore.Email?.Trim(), tk.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    changedFields.Add("Email");
                // passwordWillChange indicates mật khẩu bị đổi
                if (passwordWillChange) changedFields.Add("Mật khẩu");
                if (existingBefore.Trang_Thai != tk.Trang_Thai) changedFields.Add("Trạng thái");
                if (existingBefore.ID_Nhan_Vien != tk.ID_Nhan_Vien) changedFields.Add("Nhân viên");

                string successMsg;
                if (changedFields.Any())
                {
                    successMsg = $"Cập nhật {string.Join(", ", changedFields)} thành công.";
                }
                else
                {
                    successMsg = "Cập nhật tài khoản thành công.";
                }

                TempData["Success"] = successMsg;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi cập nhật {ex.Message}";
                TempData["Error"] = $"Lỗi khi cập nhật tài khoản  {ex.Message}";

                ViewBag.NhanViens = await _nhanVienService.GetAllAsync();
                ViewBag.SearchNhanVien = searchNhanVien;
                return View(tk);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tk = await _taiKhoanService.GetByIdAsync(id);
            if (tk == null) return NotFound();

            var roles = tk.TaiKhoanVaiTros?.Select(r => r.VaiTro.Ten_Vai_Tro).ToList() ?? new List<string>();

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

        // Helper: validate email
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        // Helper: strong password check
        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8) return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = Regex.IsMatch(password, @"[\W_]"); // non-word char or underscore

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}

