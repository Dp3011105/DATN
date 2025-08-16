using BE.Data;
using BE.DTOs;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class QuanLyPhanQuyenNhanVienRepository : IQuanLyPhanQuyenNhanVienRepository
    {

        private readonly MyDbContext _context;

        public QuanLyPhanQuyenNhanVienRepository(MyDbContext context)
        {
            _context = context;
        }


        public async Task<List<NhanVienVoiTaiKhoan>> LayDanhSachNhanVienCoVaiTroAsync()
        {
            return await _context.Tai_Khoan
                .Where(tk => tk.ID_Nhan_Vien != null && tk.TaiKhoanVaiTros.Any())
                .Include(tk => tk.NhanVien)
                .Include(tk => tk.TaiKhoanVaiTros)
                .ThenInclude(tkvt => tkvt.VaiTro)
                .Select(tk => new NhanVienVoiTaiKhoan
                {
                    ID_Tai_Khoan = tk.ID_Tai_Khoan,
                    Ten_Dang_Nhap = tk.Ten_Nguoi_Dung,
                    Email = tk.Email,
                    Trang_Thai = tk.Trang_Thai,
                    Ngay_Tao = tk.Ngay_Tao,
                    Ngay_Cap_Nhat = tk.Ngay_Cap_Nhat,
                    ID_Nhan_Vien = tk.ID_Nhan_Vien.Value,
                    Ho_Ten = tk.NhanVien.Ho_Ten,
                    So_Dien_Thoai = tk.NhanVien.So_Dien_Thoai,
                    Dia_Chi = tk.NhanVien.Dia_Chi,
                    Nam_Sinh = tk.NhanVien.NamSinh,
                    CCCD = tk.NhanVien.CCCD,
                    Vai_Tros = tk.TaiKhoanVaiTros.Select(tkvt => new VaiTroDto
                    {
                        ID_Vai_Tro = tkvt.ID_Vai_Tro,
                        Ten_Vai_Tro = tkvt.VaiTro.Ten_Vai_Tro
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<NhanVienVoiTaiKhoan>> LayDanhSachNhanVienKhongCoVaiTroAsync()
        {
            return await _context.Tai_Khoan
                .Where(tk => tk.ID_Nhan_Vien != null && !tk.TaiKhoanVaiTros.Any())
                .Include(tk => tk.NhanVien)
                .Select(tk => new NhanVienVoiTaiKhoan
                {
                    ID_Tai_Khoan = tk.ID_Tai_Khoan,
                    Ten_Dang_Nhap = tk.Ten_Nguoi_Dung,
                    Email = tk.Email,
                    Trang_Thai = tk.Trang_Thai,
                    Ngay_Tao = tk.Ngay_Tao,
                    Ngay_Cap_Nhat = tk.Ngay_Cap_Nhat,
                    ID_Nhan_Vien = tk.ID_Nhan_Vien.Value,
                    Ho_Ten = tk.NhanVien.Ho_Ten,
                    So_Dien_Thoai = tk.NhanVien.So_Dien_Thoai,
                    Dia_Chi = tk.NhanVien.Dia_Chi,
                    Nam_Sinh = tk.NhanVien.NamSinh,
                    CCCD = tk.NhanVien.CCCD,
                    Vai_Tros = new List<VaiTroDto>()
                })
                .ToListAsync();
        }

        public async Task GanVaiTroChoTaiKhoanAsync(int accountId, List<int> roleIds)
        {
            var account = await _context.Tai_Khoan
                .Include(tk => tk.TaiKhoanVaiTros)
                .FirstOrDefaultAsync(tk => tk.ID_Tai_Khoan == accountId && tk.ID_Nhan_Vien != null);

            if (account == null)
            {
                throw new Exception("Tài khoản nhân viên không tồn tại.");
            }

            var existingRoles = account.TaiKhoanVaiTros.Select(tkvt => tkvt.ID_Vai_Tro).ToList();
            var newRoles = roleIds.Except(existingRoles).ToList();

            if (newRoles.Any())
            {
                foreach (var roleId in newRoles)
                {
                    var role = await _context.Vai_Tro.FindAsync(roleId);
                    if (role == null)
                    {
                        throw new Exception($"Vai trò với ID {roleId} không tồn tại.");
                    }

                    account.TaiKhoanVaiTros.Add(new BE.models.TaiKhoanVaiTro
                    {
                        ID_Tai_Khoan = accountId,
                        ID_Vai_Tro = roleId
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task CapNhatVaiTroChoTaiKhoanAsync(int accountId, List<int> roleIds)
        {
            var account = await _context.Tai_Khoan
                .Include(tk => tk.TaiKhoanVaiTros)
                .FirstOrDefaultAsync(tk => tk.ID_Tai_Khoan == accountId && tk.ID_Nhan_Vien != null);

            if (account == null)
            {
                throw new Exception("Tài khoản nhân viên không tồn tại.");
            }

            // Get current roles
            var existingRoles = account.TaiKhoanVaiTros.ToList();

            // Roles to remove (existing roles not in the new list)
            var rolesToRemove = existingRoles.Where(r => !roleIds.Contains(r.ID_Vai_Tro)).ToList();

            // Roles to add (new roles not in existing roles)
            var rolesToAdd = roleIds.Except(existingRoles.Select(r => r.ID_Vai_Tro)).ToList();

            // Remove roles
            if (rolesToRemove.Any())
            {
                _context.TaiKhoan_VaiTro.RemoveRange(rolesToRemove);
            }

            // Add new roles
            foreach (var roleId in rolesToAdd)
            {
                var role = await _context.Vai_Tro.FindAsync(roleId);
                if (role == null)
                {
                    throw new Exception($"Vai trò với ID {roleId} không tồn tại.");
                }

                account.TaiKhoanVaiTros.Add(new BE.models.TaiKhoanVaiTro
                {
                    ID_Tai_Khoan = accountId,
                    ID_Vai_Tro = roleId
                });
            }

            await _context.SaveChangesAsync();
        }




    }
}
