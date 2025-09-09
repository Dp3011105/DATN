using BE.models;
using Microsoft.EntityFrameworkCore;

namespace BE.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<VaiTro> Vai_Tro { get; set; }
        public DbSet<NhanVien> Nhan_Vien { get; set; }
        public DbSet<KhachHang> Khach_Hang { get; set; }
        public DbSet<DiaChi> Dia_Chi { get; set; }
        public DbSet<KhachHangDiaChi> KhachHang_DiaChi { get; set; }
        public DbSet<TaiKhoan> Tai_Khoan { get; set; }
        public DbSet<TaiKhoanVaiTro> TaiKhoan_VaiTro { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<KhachHangVoucher> KhachHang_Voucher { get; set; }
        public DbSet<DoNgot> DoNgot { get; set; }
        public DbSet<SanPham> San_Pham { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<SanPhamSize> SanPham_Size { get; set; }
        public DbSet<Topping> Topping { get; set; }
        public DbSet<SanPhamTopping> SanPham_Topping { get; set; }
        public DbSet<Thue> Thue { get; set; }
        public DbSet<HoaDon> Hoa_Don { get; set; }
        public DbSet<HoaDonChiTiet> HoaDon_ChiTiet { get; set; }
        public DbSet<HoaDonChiTietTopping> HoaDonChiTiet_Topping { get; set; }
        public DbSet<HoaDonChiTietThue> HoaDonChiTiet_Thue { get; set; }
        public DbSet<DiemDanh> Diem_Danh { get; set; }
        public DbSet<LichSuHoaDon> Lich_Su_Hoa_Don { get; set; }
        public DbSet<HinhThucThanhToan> Hinh_Thuc_Thanh_Toan { get; set; }
        public DbSet<Gio_Hang> Gio_Hang { get; set; }
        public DbSet<GioHang_ChiTiet> GioHang_ChiTiet { get; set; }
        public DbSet<GioHangChiTiet_Topping> GioHangChiTiet_Topping { get; set; }
        public DbSet<LuongDa> LuongDa { get; set; }
        public DbSet<SanPhamLuongDa> SanPhamLuongDa { get; set; }
        public DbSet<ChatSession> Chat_Session { get; set; }
        public DbSet<ChatSessionNhanVien> Chat_Session_Nhan_Vien { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<SanPhamDoNgot> SanPham_DoNgot { get; set; }
        public DbSet<HoaDonVoucher> HoaDonVouchers { get; set; }
        public DbSet<SanPhamKhuyenMai> SanPhamKhuyenMai { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Vai_Tro
            modelBuilder.Entity<VaiTro>()
                .HasKey(v => v.ID_Vai_Tro);
            modelBuilder.Entity<VaiTro>()
                .Property(v => v.Ten_Vai_Tro)
                .IsRequired(true);

            // 2. Nhan_Vien
            modelBuilder.Entity<NhanVien>()
                .HasKey(n => n.ID_Nhan_Vien);
            modelBuilder.Entity<NhanVien>()
                .HasIndex(n => n.Email)
                .IsUnique();
            modelBuilder.Entity<NhanVien>()
                .Property(n => n.Ho_Ten)
                .IsRequired(true);
            modelBuilder.Entity<NhanVien>()
                .Property(n => n.Ghi_Chu)
                .IsRequired(false);
            // Add configurations for new image fields
            modelBuilder.Entity<NhanVien>()
                .Property(n => n.AnhNhanVien)
                .IsRequired(false) // Optional field
                .HasMaxLength(255);

            modelBuilder.Entity<NhanVien>()
                .Property(n => n.AnhCCCD)
                .IsRequired(false) // Optional field
                .HasMaxLength(255);

            // 3. Khach_Hang
            modelBuilder.Entity<KhachHang>()
                .HasKey(k => k.ID_Khach_Hang);
            modelBuilder.Entity<KhachHang>()
                .Property(k => k.So_Dien_Thoai)
                .IsRequired(true)
                .HasDefaultValue("");
            modelBuilder.Entity<KhachHang>()
                .Property(k => k.Ghi_Chu)
                .IsRequired(true)
                .HasDefaultValue("");

            // 4. Dia_Chi
            modelBuilder.Entity<DiaChi>()
                .HasKey(d => d.ID_Dia_Chi);
            modelBuilder.Entity<DiaChi>()
                .Property(d => d.Ghi_Chu)
                .IsRequired(false);
            modelBuilder.Entity<DiaChi>()
                .HasMany(d => d.HoaDons)
                .WithOne(h => h.DiaChi)
                .HasForeignKey(h => h.ID_Dia_Chi);
            modelBuilder.Entity<DiaChi>()
                .HasMany(d => d.KhachHangDiaChis)
                .WithOne(k => k.DiaChi)
                .HasForeignKey(k => k.ID_Dia_Chi);

            // 5. KhachHang_DiaChi
            modelBuilder.Entity<KhachHangDiaChi>()
                .HasKey(kd => new { kd.ID_Dia_Chi, kd.KhachHang_ID });
            modelBuilder.Entity<KhachHangDiaChi>()
                .HasOne(kd => kd.DiaChi)
                .WithMany(d => d.KhachHangDiaChis)
                .HasForeignKey(kd => kd.ID_Dia_Chi);
            modelBuilder.Entity<KhachHangDiaChi>()
                .HasOne(kd => kd.KhachHang)
                .WithMany(k => k.KhachHangDiaChis)
                .HasForeignKey(kd => kd.KhachHang_ID);
            modelBuilder.Entity<KhachHangDiaChi>()
                .Property(kd => kd.Ghi_Chu)
                .IsRequired(false);

            // 6. Tai_Khoan
            modelBuilder.Entity<TaiKhoan>()
                .HasKey(t => t.ID_Tai_Khoan);
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.KhachHang)
                .WithMany(k => k.TaiKhoans)
                .HasForeignKey(t => t.ID_Khach_Hang)
                .IsRequired(false);
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.NhanVien)
                .WithMany(n => n.TaiKhoans)
                .HasForeignKey(t => t.ID_Nhan_Vien)
                .IsRequired(false);
            modelBuilder.Entity<TaiKhoan>()
                .Property(t => t.Ten_Nguoi_Dung)
                .IsRequired(true);
            modelBuilder.Entity<TaiKhoan>()
                .Property(t => t.Mat_Khau)
                .IsRequired(true);

            // Khóa chính tổng hợp
            modelBuilder.Entity<TaiKhoanVaiTro>()
                .HasKey(x => new { x.ID_Tai_Khoan, x.ID_Vai_Tro });

            // Quan hệ n-n (bắc cầu qua bảng TaiKhoanVaiTro)
            modelBuilder.Entity<TaiKhoanVaiTro>()
                .HasOne(x => x.TaiKhoan)
                .WithMany(tk => tk.TaiKhoanVaiTros)
                .HasForeignKey(x => x.ID_Tai_Khoan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaiKhoanVaiTro>()
                .HasOne(x => x.VaiTro)
                .WithMany(vt => vt.TaiKhoanVaiTros)
                .HasForeignKey(x => x.ID_Vai_Tro)
                .OnDelete(DeleteBehavior.Cascade);

            // Index để tăng tốc tra cứu
            modelBuilder.Entity<TaiKhoanVaiTro>()
                .HasIndex(x => new { x.ID_Tai_Khoan, x.ID_Vai_Tro })
                .IsUnique();

            modelBuilder.Entity<TaiKhoan>().HasIndex(t => t.Email)
                .IsUnique(false);
            modelBuilder.Entity<TaiKhoan>().HasIndex(t => t.Ten_Nguoi_Dung)
                .IsUnique();

            // 8. Voucher
            modelBuilder.Entity<Voucher>()
                .HasKey(v => v.ID_Voucher);
            modelBuilder.Entity<Voucher>()
                .Property(v => v.Ma_Voucher)
                .IsRequired(true);
            modelBuilder.Entity<Voucher>()
                .Property(v => v.Gia_Tri_Giam)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Voucher>()
                .Property(v => v.So_Tien_Dat_Yeu_Cau)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Voucher>()
                .Property(v => v.Trang_Thai)
                .IsRequired(true)
                .HasDefaultValue(true);

            // 9. KhachHang_Voucher
            modelBuilder.Entity<KhachHangVoucher>()
                .HasKey(kv => new { kv.ID_Khach_Hang, kv.ID_Voucher });
            modelBuilder.Entity<KhachHangVoucher>()
                .HasOne(kv => kv.KhachHang)
                .WithMany(k => k.KhachHangVouchers)
                .HasForeignKey(kv => kv.ID_Khach_Hang);
            modelBuilder.Entity<KhachHangVoucher>()
                .HasOne(kv => kv.Voucher)
                .WithMany(v => v.KhachHangVouchers)
                .HasForeignKey(kv => kv.ID_Voucher);
            modelBuilder.Entity<KhachHangVoucher>()
                .Property(kv => kv.Ghi_Chu)
                .IsRequired(true)
                .HasDefaultValue("");

            // 10. SanPham_DoNgot
            modelBuilder.Entity<SanPhamDoNgot>()
                .HasKey(spdn => new { spdn.ID_San_Pham, spdn.ID_DoNgot });
            modelBuilder.Entity<SanPhamDoNgot>()
                .HasOne(spdn => spdn.SanPham)
                .WithMany(sp => sp.SanPhamDoNgots)
                .HasForeignKey(spdn => spdn.ID_San_Pham);
            modelBuilder.Entity<SanPhamDoNgot>()
                .HasOne(spdn => spdn.DoNgot)
                .WithMany(dn => dn.SanPhamDoNgots)
                .HasForeignKey(spdn => spdn.ID_DoNgot);
            modelBuilder.Entity<SanPhamDoNgot>()
                .Property(spdn => spdn.Ghi_Chu)
                .IsRequired(false);

            // 11. San_Pham
            modelBuilder.Entity<SanPham>()
                .HasKey(sp => sp.ID_San_Pham);
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.Ten_San_Pham)
                .IsRequired(true);
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.So_Luong)
                .IsRequired(true);
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.Gia)
                .HasPrecision(18, 2);
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.Hinh_Anh)
                .IsRequired(false);
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.Mo_Ta)
                .IsRequired(false);

            // 12. Size
            modelBuilder.Entity<Size>()
                .HasKey(s => s.ID_Size);
            modelBuilder.Entity<Size>()
                .Property(s => s.Gia)
                .HasPrecision(18, 2);

            // 13. SanPham_Size
            modelBuilder.Entity<SanPhamSize>()
                .HasKey(ss => new { ss.ID_Size, ss.ID_San_Pham });
            modelBuilder.Entity<SanPhamSize>()
                .HasOne(ss => ss.Size)
                .WithMany(s => s.SanPhamSizes)
                .HasForeignKey(ss => ss.ID_Size);
            modelBuilder.Entity<SanPhamSize>()
                .HasOne(ss => ss.SanPham)
                .WithMany(sp => sp.SanPhamSizes)
                .HasForeignKey(ss => ss.ID_San_Pham);
            modelBuilder.Entity<SanPhamSize>()
                .Property(ss => ss.Ghi_Chu)
                .IsRequired(false);
            modelBuilder.Entity<SanPhamSize>()
               .Property(ss => ss.Mo_Ta)
               .IsRequired(false);

            // 14. Topping
            modelBuilder.Entity<Topping>()
                .HasKey(t => t.ID_Topping);
            modelBuilder.Entity<Topping>()
                .Property(t => t.Gia)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Topping>()
                .Property(t => t.Ghi_Chu)
                .IsRequired(false);
            modelBuilder.Entity<Topping>()
                .Property(t => t.Hinh_Anh)
                .HasMaxLength(255); // Hoặc chiều dài tối đa khác nếu cần

            // 15. SanPham_Topping
            modelBuilder.Entity<SanPhamTopping>()
                .HasKey(st => new { st.ID_Topping, st.ID_San_Pham });
            modelBuilder.Entity<SanPhamTopping>()
                .HasOne(st => st.Topping)
                .WithMany(t => t.SanPhamToppings)
                .HasForeignKey(st => st.ID_Topping);
            modelBuilder.Entity<SanPhamTopping>()
                .HasOne(st => st.SanPham)
                .WithMany(sp => sp.SanPhamToppings)
                .HasForeignKey(st => st.ID_San_Pham);
            modelBuilder.Entity<SanPhamTopping>()
               .Property(ss => ss.Mo_Ta)
               .IsRequired(false);
            // 16. LuongDa
            modelBuilder.Entity<LuongDa>()
                .HasKey(ld => ld.ID_LuongDa);
            modelBuilder.Entity<LuongDa>()
                .Property(ld => ld.Ten_LuongDa)
                .IsRequired(true)
                .HasMaxLength(50);



            // 17. SanPhamLuongDa
            modelBuilder.Entity<SanPhamLuongDa>()
                .HasKey(spld => new { spld.ID_San_Pham, spld.ID_LuongDa });
            modelBuilder.Entity<SanPhamLuongDa>()
                .HasOne(spld => spld.SanPham)
                .WithMany(sp => sp.SanPhamLuongDas)
                .HasForeignKey(spld => spld.ID_San_Pham);
            modelBuilder.Entity<SanPhamLuongDa>()
                .HasOne(spld => spld.LuongDa)
                .WithMany(ld => ld.SanPhamLuongDas)
                .HasForeignKey(spld => spld.ID_LuongDa);

            // 18. Thue
            modelBuilder.Entity<Thue>()
                .HasKey(t => t.ID_Thue);
            modelBuilder.Entity<Thue>()
                .Property(t => t.Ty_Le)
                .HasPrecision(5, 2);

            // 19. Hoa_Don
            modelBuilder.Entity<HoaDon>(e =>
            {
                e.HasKey(hd => hd.ID_Hoa_Don);

                // Optional FK (tránh lỗi bắt buộc khi bạn gán ID mặc định/để null)
                e.HasOne(hd => hd.NhanVien)
                 .WithMany(n => n.HoaDons)
                 .HasForeignKey(hd => hd.ID_Nhan_Vien)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(hd => hd.KhachHang)
                 .WithMany(k => k.HoaDons)
                 .HasForeignKey(hd => hd.ID_Khach_Hang)
                 .IsRequired(false)                // cho phép null nếu là khách lẻ
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(hd => hd.HinhThucThanhToan)
                 .WithMany(ht => ht.HoaDons)
                 .HasForeignKey(hd => hd.ID_Hinh_Thuc_Thanh_Toan)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(hd => hd.DiaChi)
                 .WithMany(d => d.HoaDons)
                 .HasForeignKey(hd => hd.ID_Dia_Chi)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                // 1-n: HoaDon -> HoaDonChiTiet
                e.HasMany(hd => hd.HoaDonChiTiets)
                 .WithOne(ct => ct.HoaDon!)
                 .HasForeignKey(ct => ct.ID_Hoa_Don)
                 .OnDelete(DeleteBehavior.Cascade);

                // Số liệu & mặc định
                e.Property(hd => hd.Tong_Tien).HasPrecision(18, 2);
                e.Property(hd => hd.Phi_Ship).HasPrecision(18, 2).IsRequired(false);

                e.Property(hd => hd.Trang_Thai)
                 .HasMaxLength(50)
                 .HasDefaultValue("Chua_Xac_Nhan");

                e.Property(hd => hd.Loai_Hoa_Don)
                 .HasMaxLength(50)
                 .HasDefaultValue("TaiQuay");

                e.Property(hd => hd.Ma_Hoa_Don)
                 .HasMaxLength(50)
                 .IsRequired();

                e.Property(hd => hd.Ghi_Chu).IsRequired(false);
                e.Property(hd => hd.Dia_Chi_Tu_Nhap).IsRequired(false);
                e.Property(hd => hd.LyDoHuyDon).HasMaxLength(500).IsRequired(false);
                e.Property(hd => hd.LyDoDonHangCoVanDe).HasMaxLength(500).IsRequired(false);
            });

            // 20. HoaDon_ChiTiet
            modelBuilder.Entity<HoaDonChiTiet>(e =>
            {
                e.HasKey(ct => ct.ID_HoaDon_ChiTiet);

                // Quan hệ đến SanPham/Size/DoNgot/LuongDa
                e.HasOne(ct => ct.SanPham)
                 .WithMany(sp => sp.HoaDonChiTiets)
                 .HasForeignKey(ct => ct.ID_San_Pham)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ct => ct.Size)
                 .WithMany(s => s.HoaDonChiTiets)
                 .HasForeignKey(ct => ct.ID_Size)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ct => ct.DoNgot)
                 .WithMany(dn => dn.HoaDonChiTiets)
                 .HasForeignKey(ct => ct.ID_SanPham_DoNgot)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ct => ct.LuongDa)
                 .WithMany(ld => ld.HoaDonChiTiets)
                 .HasForeignKey(ct => ct.ID_LuongDa)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Restrict);

                // Số liệu
                e.Property(ct => ct.Gia_San_Pham).HasPrecision(18, 2);
                e.Property(ct => ct.Gia_Them_Size).HasPrecision(18, 2);
                e.Property(ct => ct.Tong_Tien).HasPrecision(18, 2);

                e.Property(ct => ct.Ghi_Chu).IsRequired(false);
                e.Property(ct => ct.Ma_HoaDon_ChiTiet).IsRequired();
                e.Property(ct => ct.So_Luong).IsRequired();

                // default Ngay_Tao ở DB
                e.Property(ct => ct.Ngay_Tao)
                 .IsRequired()
                 .HasDefaultValueSql("GETDATE()");

                // Unique theo đơn + mã chi tiết (tránh trùng)
                e.HasIndex(ct => new { ct.ID_Hoa_Don, ct.Ma_HoaDon_ChiTiet }).IsUnique();
            });

            // (Giữ nguyên phần seed HinhThucThanhToan của bạn)
            modelBuilder.Entity<HinhThucThanhToan>().HasData(
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 1, Phuong_Thuc_Thanh_Toan = "TienMat", Cong_Thanh_Toan = "Cash", Trang_Thai = true },
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 2, Phuong_Thuc_Thanh_Toan = "The", Cong_Thanh_Toan = "Card", Trang_Thai = true },
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 3, Phuong_Thuc_Thanh_Toan = "ChuyenKhoan", Cong_Thanh_Toan = "Bank", Trang_Thai = true },
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 4, Phuong_Thuc_Thanh_Toan = "Thanh Toán Khi Nhận Hàng", Cong_Thanh_Toan = "Cash", Trang_Thai = true },
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 5, Phuong_Thuc_Thanh_Toan = "Thanh Toán MOMO", Cong_Thanh_Toan = "Bank", Trang_Thai = true },
                new HinhThucThanhToan { ID_Hinh_Thuc_Thanh_Toan = 6, Phuong_Thuc_Thanh_Toan = "Chuyển Khoản Mã QR", Cong_Thanh_Toan = "Bank", Trang_Thai = true }
            );
            // Size
            modelBuilder.Entity<Size>().HasData(
                new Size { ID_Size = 1, SizeName = "Cơ bản", Gia = 0m, Trang_Thai = true },
                new Size { ID_Size = 2, SizeName = "Large", Gia = 10000m, Trang_Thai = true },
                new Size { ID_Size = 3, SizeName = "X-Large", Gia = 15000m, Trang_Thai = true }
            );

            // Lượng đá
            modelBuilder.Entity<LuongDa>().HasData(
                new LuongDa { ID_LuongDa = 1, Ten_LuongDa = "Mặc Định", Trang_Thai = true },
                new LuongDa { ID_LuongDa = 2, Ten_LuongDa = "Nhiều Đá", Trang_Thai = true },
                new LuongDa { ID_LuongDa = 3, Ten_LuongDa = "Ít Đá", Trang_Thai = true }
            );

            // Độ ngọt
            modelBuilder.Entity<DoNgot>().HasData(
                new DoNgot { ID_DoNgot = 1, Muc_Do = "Mặc Định", Ghi_Chu = "asd", Trang_Thai = true },
                new DoNgot { ID_DoNgot = 2, Muc_Do = "Thêm Đường", Ghi_Chu = "asd", Trang_Thai = true },
                new DoNgot { ID_DoNgot = 3, Muc_Do = "Ít Đường", Ghi_Chu = "asd", Trang_Thai = true }
            );

            // 21. HoaDonChiTiet_Topping
            modelBuilder.Entity<HoaDonChiTietTopping>()
                .HasKey(hdctt => hdctt.ID);
            modelBuilder.Entity<HoaDonChiTietTopping>()
                .HasOne(hdctt => hdctt.HoaDonChiTiet)
                .WithMany(hdct => hdct.HoaDonChiTietToppings)
                .HasForeignKey(hdctt => hdctt.ID_HoaDon_ChiTiet);
            modelBuilder.Entity<HoaDonChiTietTopping>()
                .HasOne(hdctt => hdctt.Topping)
                .WithMany(t => t.HoaDonChiTietToppings)
                .HasForeignKey(hdctt => hdctt.ID_Topping);
            modelBuilder.Entity<HoaDonChiTietTopping>()
                .Property(hdctt => hdctt.Gia_Topping)
                .HasPrecision(18, 2);

            // 22. HoaDonChiTiet_Thue
            modelBuilder.Entity<HoaDonChiTietThue>()
                .HasKey(hdctt => new { hdctt.ID_HoaDon_ChiTiet, hdctt.ID_Thue });
            modelBuilder.Entity<HoaDonChiTietThue>()
                .HasOne(hdctt => hdctt.HoaDonChiTiet)
                .WithMany(hdct => hdct.HoaDonChiTietThues)
                .HasForeignKey(hdctt => hdctt.ID_HoaDon_ChiTiet);
            modelBuilder.Entity<HoaDonChiTietThue>()
                .HasOne(hdctt => hdctt.Thue)
                .WithMany(t => t.HoaDonChiTietThues)
                .HasForeignKey(hdctt => hdctt.ID_Thue);
            modelBuilder.Entity<HoaDonChiTietThue>()
                .Property(hdctt => hdctt.Ghi_Chu)
                .IsRequired(false);

            // 23. Diem_Danh
            modelBuilder.Entity<DiemDanh>()
                .HasKey(dd => dd.ID_Diem_Danh);
            modelBuilder.Entity<DiemDanh>()
                .HasOne(dd => dd.NhanVien)
                .WithMany(n => n.DiemDanhs)
                .HasForeignKey(dd => dd.NhanVien_ID);
            modelBuilder.Entity<DiemDanh>()
                .Property(dd => dd.Ghi_Chu)
                .IsRequired(false);

            // 24. Lich_Su_Hoa_Don
            modelBuilder.Entity<LichSuHoaDon>()
                .HasKey(lshd => lshd.ID_Lich_Su_Hoa_Don);
            modelBuilder.Entity<LichSuHoaDon>()
                .HasOne(lshd => lshd.HoaDon)
                .WithMany(hd => hd.LichSuHoaDons)
                .HasForeignKey(lshd => lshd.ID_Hoa_Don);
            modelBuilder.Entity<LichSuHoaDon>()
                .Property(lshd => lshd.Ghi_Chu)
                .IsRequired(false);

            // 25. Hinh_Thuc_Thanh_Toan
            modelBuilder.Entity<HinhThucThanhToan>()
                .HasKey(httt => httt.ID_Hinh_Thuc_Thanh_Toan);
            modelBuilder.Entity<HinhThucThanhToan>()
                .Property(httt => httt.Ghi_Chu)
                .IsRequired(false);

            // 26. Gio_Hang
            modelBuilder.Entity<Gio_Hang>()
                .HasKey(gh => gh.ID_Gio_Hang);
            modelBuilder.Entity<Gio_Hang>()
                .HasOne(gh => gh.Khach_Hang)
                .WithMany(kh => kh.Gio_Hangs)
                .HasForeignKey(gh => gh.ID_Khach_Hang)
                .IsRequired(true);
            modelBuilder.Entity<Gio_Hang>()
                .Property(gh => gh.Ngay_Tao)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Gio_Hang>()
                .Property(gh => gh.Ngay_Cap_Nhat)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Gio_Hang>()
                .Property(gh => gh.Trang_Thai)
                .IsRequired(true)
                .HasDefaultValue(true);
            modelBuilder.Entity<Gio_Hang>()
                .HasIndex(gh => gh.ID_Khach_Hang);

            // 27. GioHang_ChiTiet
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasKey(ghct => ghct.ID_GioHang_ChiTiet);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasOne(ghct => ghct.Gio_Hang)
                .WithMany(gh => gh.GioHang_ChiTiets)
                .HasForeignKey(ghct => ghct.ID_Gio_Hang);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasOne(ghct => ghct.San_Pham)
                .WithMany(sp => sp.GioHang_ChiTiets)
                .HasForeignKey(ghct => ghct.ID_San_Pham);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasOne(ghct => ghct.Size)
                .WithMany(s => s.GioHang_ChiTiets)
                .HasForeignKey(ghct => ghct.ID_Size)
                .IsRequired(false);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasOne(ghct => ghct.DoNgot)
                .WithMany(dn => dn.GioHang_ChiTiets)
                .HasForeignKey(ghct => ghct.ID_SanPham_DoNgot)
                .IsRequired(false);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasOne(ghct => ghct.LuongDa)
                .WithMany(ld => ld.GioHang_ChiTiets)
                .HasForeignKey(ghct => ghct.ID_LuongDa)
                .IsRequired(false);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .Property(ghct => ghct.Ma_GioHang_ChiTiet)
                .IsRequired(true);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .Property(ghct => ghct.So_Luong)
                .IsRequired(true);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .Property(ghct => ghct.Ghi_Chu)
                .IsRequired(false);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .Property(ghct => ghct.Ngay_Tao)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasIndex(ghct => ghct.ID_Gio_Hang);
            modelBuilder.Entity<GioHang_ChiTiet>()
                .HasIndex(ghct => ghct.Ma_GioHang_ChiTiet)
                .IsUnique();
   
            // 28. GioHangChiTiet_Topping
            modelBuilder.Entity<GioHangChiTiet_Topping>()
                .HasKey(ghctt => ghctt.ID_GioHangChiTiet_Topping);
            modelBuilder.Entity<GioHangChiTiet_Topping>()
                .HasOne(ghctt => ghctt.GioHang_ChiTiet)
                .WithMany(ghct => ghct.GioHangChiTiet_Toppings)
                .HasForeignKey(ghctt => ghctt.ID_GioHang_ChiTiet);
            modelBuilder.Entity<GioHangChiTiet_Topping>()
                .HasOne(ghctt => ghctt.Topping)
                .WithMany(t => t.GioHangChiTiet_Toppings)
                .HasForeignKey(ghctt => ghctt.ID_Topping);
            modelBuilder.Entity<GioHangChiTiet_Topping>()
                .HasIndex(ghctt => ghctt.ID_GioHang_ChiTiet);

            // 29. Chat_Session
            modelBuilder.Entity<ChatSession>()
                .HasKey(cs => cs.ID_Chat_Session);
            modelBuilder.Entity<ChatSession>()
                .HasOne(cs => cs.KhachHang)
                .WithMany(k => k.ChatSessions)
                .HasForeignKey(cs => cs.ID_Khach_Hang)
                .IsRequired(true);
            modelBuilder.Entity<ChatSession>()
                .Property(cs => cs.Tieu_De)
                .IsRequired(false);
            modelBuilder.Entity<ChatSession>()
                .Property(cs => cs.Trang_Thai)
                .IsRequired(true)
                .HasDefaultValue(true);

            // 30. Chat_Session_Nhan_Vien
            modelBuilder.Entity<ChatSessionNhanVien>()
                .HasKey(csn => new { csn.ID_Chat_Session, csn.ID_Nhan_Vien });
            modelBuilder.Entity<ChatSessionNhanVien>()
                .HasOne(csn => csn.ChatSession)
                .WithMany(cs => cs.ChatSessionNhanViens)
                .HasForeignKey(csn => csn.ID_Chat_Session);
            modelBuilder.Entity<ChatSessionNhanVien>()
                .HasOne(csn => csn.NhanVien)
                .WithMany(n => n.ChatSessionNhanViens)
                .HasForeignKey(csn => csn.ID_Nhan_Vien);

            // 31. Message
            modelBuilder.Entity<Message>()
                .HasKey(m => m.ID_Message);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ChatSession)
                .WithMany(cs => cs.Messages)
                .HasForeignKey(m => m.ID_Chat_Session)
                .IsRequired(true);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.KhachHang)
                .WithMany(k => k.Messages)
                .HasForeignKey(m => m.ID_Khach_Hang)
                .IsRequired(false);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.NhanVien)
                .WithMany(n => n.Messages)
                .HasForeignKey(m => m.ID_Nhan_Vien)
                .IsRequired(false);
            modelBuilder.Entity<Message>()
                .Property(m => m.Noi_Dung)
                .IsRequired(true);
            modelBuilder.Entity<Message>()
                .Property(m => m.Thoi_Gian_Gui)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Message>()
                .Property(m => m.Trang_Thai)
                .IsRequired(true)
                .HasDefaultValue(true);

           
            // 32. HoaDonVoucher
            modelBuilder.Entity<HoaDonVoucher>()
                .HasKey(hdv => hdv.ID_HoaDonVoucher);
            modelBuilder.Entity<HoaDonVoucher>()
                .HasOne(hdv => hdv.HoaDon)
                .WithMany(hd => hd.HoaDonVouchers)
                .HasForeignKey(hdv => hdv.ID_Hoa_Don);
            modelBuilder.Entity<HoaDonVoucher>()
                .HasOne(hdv => hdv.Voucher)
                .WithMany(v => v.HoaDonVouchers)
                .HasForeignKey(hdv => hdv.ID_Voucher);
            modelBuilder.Entity<HoaDonVoucher>()
                .Property(hdv => hdv.Gia_Tri_Giam)
                .HasPrecision(18, 2);


            // 33. SanPhamKhuyenMais 
            modelBuilder.Entity<SanPhamKhuyenMai>()
            .Property(p => p.Gia_Giam)
            .HasColumnType("decimal(18,2)"); // Chỉ định kiểu dữ liệu cho Gia_Giam

            modelBuilder.Entity<SanPhamKhuyenMai>()
                .Property(p => p.Phan_Tram_Giam)
                .HasColumnType("decimal(5,2)"); // Chỉ định kiểu dữ liệu cho Phan_Tram_Giam
        }
    }
}
