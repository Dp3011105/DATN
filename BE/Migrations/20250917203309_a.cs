using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dia_Chi",
                columns: table => new
                {
                    ID_Dia_Chi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dia_Chi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tinh_Thanh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dia_Chi", x => x.ID_Dia_Chi);
                });

            migrationBuilder.CreateTable(
                name: "DoNgot",
                columns: table => new
                {
                    ID_DoNgot = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Muc_Do = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoNgot", x => x.ID_DoNgot);
                });

            migrationBuilder.CreateTable(
                name: "Hinh_Thuc_Thanh_Toan",
                columns: table => new
                {
                    ID_Hinh_Thuc_Thanh_Toan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phuong_Thuc_Thanh_Toan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cong_Thanh_Toan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hinh_Thuc_Thanh_Toan", x => x.ID_Hinh_Thuc_Thanh_Toan);
                });

            migrationBuilder.CreateTable(
                name: "Khach_Hang",
                columns: table => new
                {
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ho_Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    So_Dien_Thoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khach_Hang", x => x.ID_Khach_Hang);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    ID_Khuyen_Mai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Khuyen_Mai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ngay_Bat_Dau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ngay_Ket_Thuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mo_Ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.ID_Khuyen_Mai);
                });

            migrationBuilder.CreateTable(
                name: "LuongDa",
                columns: table => new
                {
                    ID_LuongDa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_LuongDa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuongDa", x => x.ID_LuongDa);
                });

            migrationBuilder.CreateTable(
                name: "Nhan_Vien",
                columns: table => new
                {
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ho_Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    So_Dien_Thoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Dia_Chi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NamSinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AnhNhanVien = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AnhCCCD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nhan_Vien", x => x.ID_Nhan_Vien);
                });

            migrationBuilder.CreateTable(
                name: "San_Pham",
                columns: table => new
                {
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_San_Pham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    Hinh_Anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Mo_Ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_San_Pham", x => x.ID_San_Pham);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    ID_Size = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.ID_Size);
                });

            migrationBuilder.CreateTable(
                name: "Topping",
                columns: table => new
                {
                    ID_Topping = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    So_Luong = table.Column<int>(type: "int", nullable: true),
                    Hinh_Anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topping", x => x.ID_Topping);
                });

            migrationBuilder.CreateTable(
                name: "Vai_Tro",
                columns: table => new
                {
                    ID_Vai_Tro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Vai_Tro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vai_Tro", x => x.ID_Vai_Tro);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    ID_Voucher = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_Voucher = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: true),
                    Gia_Tri_Giam = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    So_Tien_Dat_Yeu_Cau = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Ngay_Bat_Dau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ngay_Ket_Thuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.ID_Voucher);
                });

            migrationBuilder.CreateTable(
                name: "Gio_Hang",
                columns: table => new
                {
                    ID_Gio_Hang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: false),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Ngay_Cap_Nhat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gio_Hang", x => x.ID_Gio_Hang);
                    table.ForeignKey(
                        name: "FK_Gio_Hang_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang_DiaChi",
                columns: table => new
                {
                    ID_Dia_Chi = table.Column<int>(type: "int", nullable: false),
                    KhachHang_ID = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang_DiaChi", x => new { x.ID_Dia_Chi, x.KhachHang_ID });
                    table.ForeignKey(
                        name: "FK_KhachHang_DiaChi_Dia_Chi_ID_Dia_Chi",
                        column: x => x.ID_Dia_Chi,
                        principalTable: "Dia_Chi",
                        principalColumn: "ID_Dia_Chi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KhachHang_DiaChi_Khach_Hang_KhachHang_ID",
                        column: x => x.KhachHang_ID,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hoa_Don",
                columns: table => new
                {
                    ID_Hoa_Don = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: true),
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: true),
                    ID_Hinh_Thuc_Thanh_Toan = table.Column<int>(type: "int", nullable: true),
                    ID_Dia_Chi = table.Column<int>(type: "int", nullable: true),
                    ID_Phi_Ship = table.Column<int>(type: "int", nullable: true),
                    Dia_Chi_Tu_Nhap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tong_Tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Phi_Ship = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Trang_Thai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Chua_Xac_Nhan"),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ma_Hoa_Don = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Loai_Hoa_Don = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "TaiQuay"),
                    LyDoHuyDon = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LyDoDonHangCoVanDe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoa_Don", x => x.ID_Hoa_Don);
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Dia_Chi_ID_Dia_Chi",
                        column: x => x.ID_Dia_Chi,
                        principalTable: "Dia_Chi",
                        principalColumn: "ID_Dia_Chi",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Hinh_Thuc_Thanh_Toan_ID_Hinh_Thuc_Thanh_Toan",
                        column: x => x.ID_Hinh_Thuc_Thanh_Toan,
                        principalTable: "Hinh_Thuc_Thanh_Toan",
                        principalColumn: "ID_Hinh_Thuc_Thanh_Toan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Nhan_Vien_ID_Nhan_Vien",
                        column: x => x.ID_Nhan_Vien,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tai_Khoan",
                columns: table => new
                {
                    ID_Tai_Khoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: true),
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: true),
                    Ten_Nguoi_Dung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mat_Khau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ngay_Cap_Nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tai_Khoan", x => x.ID_Tai_Khoan);
                    table.ForeignKey(
                        name: "FK_Tai_Khoan_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang");
                    table.ForeignKey(
                        name: "FK_Tai_Khoan_Nhan_Vien_ID_Nhan_Vien",
                        column: x => x.ID_Nhan_Vien,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien");
                });

            migrationBuilder.CreateTable(
                name: "SanPham_DoNgot",
                columns: table => new
                {
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    ID_DoNgot = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham_DoNgot", x => new { x.ID_San_Pham, x.ID_DoNgot });
                    table.ForeignKey(
                        name: "FK_SanPham_DoNgot_DoNgot_ID_DoNgot",
                        column: x => x.ID_DoNgot,
                        principalTable: "DoNgot",
                        principalColumn: "ID_DoNgot",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPham_DoNgot_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamKhuyenMai",
                columns: table => new
                {
                    ID_San_Pham_Khuyen_Mai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    ID_Khuyen_Mai = table.Column<int>(type: "int", nullable: false),
                    Phan_Tram_Giam = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Gia_Giam = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamKhuyenMai", x => x.ID_San_Pham_Khuyen_Mai);
                    table.ForeignKey(
                        name: "FK_SanPhamKhuyenMai_KhuyenMai_ID_Khuyen_Mai",
                        column: x => x.ID_Khuyen_Mai,
                        principalTable: "KhuyenMai",
                        principalColumn: "ID_Khuyen_Mai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamKhuyenMai_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamLuongDa",
                columns: table => new
                {
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    ID_LuongDa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamLuongDa", x => new { x.ID_San_Pham, x.ID_LuongDa });
                    table.ForeignKey(
                        name: "FK_SanPhamLuongDa_LuongDa_ID_LuongDa",
                        column: x => x.ID_LuongDa,
                        principalTable: "LuongDa",
                        principalColumn: "ID_LuongDa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamLuongDa_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham_Size",
                columns: table => new
                {
                    ID_Size = table.Column<int>(type: "int", nullable: false),
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    Mo_Ta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham_Size", x => new { x.ID_Size, x.ID_San_Pham });
                    table.ForeignKey(
                        name: "FK_SanPham_Size_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPham_Size_Size_ID_Size",
                        column: x => x.ID_Size,
                        principalTable: "Size",
                        principalColumn: "ID_Size",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham_Topping",
                columns: table => new
                {
                    ID_Topping = table.Column<int>(type: "int", nullable: false),
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    Mo_Ta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham_Topping", x => new { x.ID_Topping, x.ID_San_Pham });
                    table.ForeignKey(
                        name: "FK_SanPham_Topping_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPham_Topping_Topping_ID_Topping",
                        column: x => x.ID_Topping,
                        principalTable: "Topping",
                        principalColumn: "ID_Topping",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang_Voucher",
                columns: table => new
                {
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: false),
                    ID_Voucher = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang_Voucher", x => new { x.ID_Khach_Hang, x.ID_Voucher });
                    table.ForeignKey(
                        name: "FK_KhachHang_Voucher_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KhachHang_Voucher_Voucher_ID_Voucher",
                        column: x => x.ID_Voucher,
                        principalTable: "Voucher",
                        principalColumn: "ID_Voucher",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang_ChiTiet",
                columns: table => new
                {
                    ID_GioHang_ChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Gio_Hang = table.Column<int>(type: "int", nullable: false),
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    ID_Size = table.Column<int>(type: "int", nullable: true),
                    ID_SanPham_DoNgot = table.Column<int>(type: "int", nullable: true),
                    ID_LuongDa = table.Column<int>(type: "int", nullable: true),
                    Ma_GioHang_ChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang_ChiTiet", x => x.ID_GioHang_ChiTiet);
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTiet_DoNgot_ID_SanPham_DoNgot",
                        column: x => x.ID_SanPham_DoNgot,
                        principalTable: "DoNgot",
                        principalColumn: "ID_DoNgot");
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTiet_Gio_Hang_ID_Gio_Hang",
                        column: x => x.ID_Gio_Hang,
                        principalTable: "Gio_Hang",
                        principalColumn: "ID_Gio_Hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTiet_LuongDa_ID_LuongDa",
                        column: x => x.ID_LuongDa,
                        principalTable: "LuongDa",
                        principalColumn: "ID_LuongDa");
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTiet_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHang_ChiTiet_Size_ID_Size",
                        column: x => x.ID_Size,
                        principalTable: "Size",
                        principalColumn: "ID_Size");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_ChiTiet",
                columns: table => new
                {
                    ID_HoaDon_ChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Hoa_Don = table.Column<int>(type: "int", nullable: false),
                    ID_San_Pham = table.Column<int>(type: "int", nullable: false),
                    ID_Size = table.Column<int>(type: "int", nullable: true),
                    ID_SanPham_DoNgot = table.Column<int>(type: "int", nullable: true),
                    ID_LuongDa = table.Column<int>(type: "int", nullable: true),
                    Ma_HoaDon_ChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gia_Them_Size = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Gia_San_Pham = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Tong_Tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_ChiTiet", x => x.ID_HoaDon_ChiTiet);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_DoNgot_ID_SanPham_DoNgot",
                        column: x => x.ID_SanPham_DoNgot,
                        principalTable: "DoNgot",
                        principalColumn: "ID_DoNgot",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_Hoa_Don_ID_Hoa_Don",
                        column: x => x.ID_Hoa_Don,
                        principalTable: "Hoa_Don",
                        principalColumn: "ID_Hoa_Don",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_LuongDa_ID_LuongDa",
                        column: x => x.ID_LuongDa,
                        principalTable: "LuongDa",
                        principalColumn: "ID_LuongDa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_Size_ID_Size",
                        column: x => x.ID_Size,
                        principalTable: "Size",
                        principalColumn: "ID_Size",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonVouchers",
                columns: table => new
                {
                    ID_HoaDonVoucher = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Hoa_Don = table.Column<int>(type: "int", nullable: false),
                    ID_Voucher = table.Column<int>(type: "int", nullable: false),
                    Gia_Tri_Giam = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonVouchers", x => x.ID_HoaDonVoucher);
                    table.ForeignKey(
                        name: "FK_HoaDonVouchers_Hoa_Don_ID_Hoa_Don",
                        column: x => x.ID_Hoa_Don,
                        principalTable: "Hoa_Don",
                        principalColumn: "ID_Hoa_Don",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonVouchers_Voucher_ID_Voucher",
                        column: x => x.ID_Voucher,
                        principalTable: "Voucher",
                        principalColumn: "ID_Voucher",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan_VaiTro",
                columns: table => new
                {
                    ID_Vai_Tro = table.Column<int>(type: "int", nullable: false),
                    ID_Tai_Khoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan_VaiTro", x => new { x.ID_Tai_Khoan, x.ID_Vai_Tro });
                    table.ForeignKey(
                        name: "FK_TaiKhoan_VaiTro_Tai_Khoan_ID_Tai_Khoan",
                        column: x => x.ID_Tai_Khoan,
                        principalTable: "Tai_Khoan",
                        principalColumn: "ID_Tai_Khoan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_VaiTro_Vai_Tro_ID_Vai_Tro",
                        column: x => x.ID_Vai_Tro,
                        principalTable: "Vai_Tro",
                        principalColumn: "ID_Vai_Tro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangChiTiet_Topping",
                columns: table => new
                {
                    ID_GioHangChiTiet_Topping = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_GioHang_ChiTiet = table.Column<int>(type: "int", nullable: false),
                    ID_Topping = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTiet_Topping", x => x.ID_GioHangChiTiet_Topping);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_Topping_GioHang_ChiTiet_ID_GioHang_ChiTiet",
                        column: x => x.ID_GioHang_ChiTiet,
                        principalTable: "GioHang_ChiTiet",
                        principalColumn: "ID_GioHang_ChiTiet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_Topping_Topping_ID_Topping",
                        column: x => x.ID_Topping,
                        principalTable: "Topping",
                        principalColumn: "ID_Topping",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiet_Topping",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_HoaDon_ChiTiet = table.Column<int>(type: "int", nullable: false),
                    ID_Topping = table.Column<int>(type: "int", nullable: false),
                    So_Luong = table.Column<int>(type: "int", nullable: true),
                    Gia_Topping = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiet_Topping", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_Topping_HoaDon_ChiTiet_ID_HoaDon_ChiTiet",
                        column: x => x.ID_HoaDon_ChiTiet,
                        principalTable: "HoaDon_ChiTiet",
                        principalColumn: "ID_HoaDon_ChiTiet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_Topping_Topping_ID_Topping",
                        column: x => x.ID_Topping,
                        principalTable: "Topping",
                        principalColumn: "ID_Topping",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DoNgot",
                columns: new[] { "ID_DoNgot", "Ghi_Chu", "Muc_Do", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, "asd", "Mặc Định", true },
                    { 2, "asd", "Thêm Đường", true },
                    { 3, "asd", "Ít Đường", true }
                });

            migrationBuilder.InsertData(
                table: "Hinh_Thuc_Thanh_Toan",
                columns: new[] { "ID_Hinh_Thuc_Thanh_Toan", "Cong_Thanh_Toan", "Ghi_Chu", "Phuong_Thuc_Thanh_Toan", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, "Cash", null, "TienMat", true },
                    { 2, "Card", null, "The", true },
                    { 3, "Bank", null, "ChuyenKhoan", true },
                    { 4, "Cash", null, "Thanh Toan Khi Nhan Hang", true },
                    { 5, "Bank", null, "Thanh Toán VNPAY", true }
                });

            migrationBuilder.InsertData(
                table: "Khach_Hang",
                columns: new[] { "ID_Khach_Hang", "Email", "Ghi_Chu", "GioiTinh", "Ho_Ten", "So_Dien_Thoai", "Trang_Thai" },
                values: new object[] { 1, "nguyenvana@example.com", "Khách hàng mặc định", true, "Nguyen Van A", "0123456789", true });

            migrationBuilder.InsertData(
                table: "LuongDa",
                columns: new[] { "ID_LuongDa", "Ten_LuongDa", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, "Mặc Định", true },
                    { 2, "Nhiều Đá", true },
                    { 3, "Ít Đá", true }
                });

            migrationBuilder.InsertData(
                table: "Nhan_Vien",
                columns: new[] { "ID_Nhan_Vien", "AnhCCCD", "AnhNhanVien", "CCCD", "Dia_Chi", "Email", "Ghi_Chu", "GioiTinh", "Ho_Ten", "NamSinh", "So_Dien_Thoai", "Trang_Thai" },
                values: new object[] { 1, null, null, "123456789012", "123 Đường Láng, Đống Đa, Hà Nội", "tranvanb@example.com", "Nhân viên Admin", true, "Tran Van B", new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0987654321", true });

            migrationBuilder.InsertData(
                table: "Size",
                columns: new[] { "ID_Size", "Gia", "SizeName", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, 0m, "Cơ bản", true },
                    { 2, 10000m, "Large", true },
                    { 3, 15000m, "X-Large", true }
                });

            migrationBuilder.InsertData(
                table: "Vai_Tro",
                columns: new[] { "ID_Vai_Tro", "Ten_Vai_Tro" },
                values: new object[,]
                {
                    { 1, "Khách Hàng" },
                    { 2, "Admin" },
                    { 3, "Nhân Viên" }
                });

            migrationBuilder.InsertData(
                table: "Tai_Khoan",
                columns: new[] { "ID_Tai_Khoan", "Email", "ID_Khach_Hang", "ID_Nhan_Vien", "Mat_Khau", "Ngay_Cap_Nhat", "Ngay_Tao", "Ten_Nguoi_Dung", "Trang_Thai" },
                values: new object[,]
                {
                    { 1, "nguyenvana@example.com", 1, null, "hashed_password_here", null, new DateTime(2025, 9, 18, 3, 33, 8, 845, DateTimeKind.Local).AddTicks(5858), "nguyenvana", true },
                    { 2, "tranvanb@example.com", null, 1, "admin", null, new DateTime(2025, 9, 18, 3, 33, 8, 845, DateTimeKind.Local).AddTicks(5861), "admin", true }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan_VaiTro",
                columns: new[] { "ID_Tai_Khoan", "ID_Vai_Tro" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gio_Hang_ID_Khach_Hang",
                table: "Gio_Hang",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_ID_Gio_Hang",
                table: "GioHang_ChiTiet",
                column: "ID_Gio_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_ID_LuongDa",
                table: "GioHang_ChiTiet",
                column: "ID_LuongDa");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_ID_San_Pham",
                table: "GioHang_ChiTiet",
                column: "ID_San_Pham");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_ID_SanPham_DoNgot",
                table: "GioHang_ChiTiet",
                column: "ID_SanPham_DoNgot");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_ID_Size",
                table: "GioHang_ChiTiet",
                column: "ID_Size");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_ChiTiet_Ma_GioHang_ChiTiet",
                table: "GioHang_ChiTiet",
                column: "Ma_GioHang_ChiTiet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_Topping_ID_GioHang_ChiTiet",
                table: "GioHangChiTiet_Topping",
                column: "ID_GioHang_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_Topping_ID_Topping",
                table: "GioHangChiTiet_Topping",
                column: "ID_Topping");

            migrationBuilder.CreateIndex(
                name: "IX_Hoa_Don_ID_Dia_Chi",
                table: "Hoa_Don",
                column: "ID_Dia_Chi");

            migrationBuilder.CreateIndex(
                name: "IX_Hoa_Don_ID_Hinh_Thuc_Thanh_Toan",
                table: "Hoa_Don",
                column: "ID_Hinh_Thuc_Thanh_Toan");

            migrationBuilder.CreateIndex(
                name: "IX_Hoa_Don_ID_Khach_Hang",
                table: "Hoa_Don",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_Hoa_Don_ID_Nhan_Vien",
                table: "Hoa_Don",
                column: "ID_Nhan_Vien");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_ID_Hoa_Don_Ma_HoaDon_ChiTiet",
                table: "HoaDon_ChiTiet",
                columns: new[] { "ID_Hoa_Don", "Ma_HoaDon_ChiTiet" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_ID_LuongDa",
                table: "HoaDon_ChiTiet",
                column: "ID_LuongDa");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_ID_San_Pham",
                table: "HoaDon_ChiTiet",
                column: "ID_San_Pham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_ID_SanPham_DoNgot",
                table: "HoaDon_ChiTiet",
                column: "ID_SanPham_DoNgot");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ChiTiet_ID_Size",
                table: "HoaDon_ChiTiet",
                column: "ID_Size");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_Topping_ID_HoaDon_ChiTiet",
                table: "HoaDonChiTiet_Topping",
                column: "ID_HoaDon_ChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_Topping_ID_Topping",
                table: "HoaDonChiTiet_Topping",
                column: "ID_Topping");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonVouchers_ID_Hoa_Don",
                table: "HoaDonVouchers",
                column: "ID_Hoa_Don");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonVouchers_ID_Voucher",
                table: "HoaDonVouchers",
                column: "ID_Voucher");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_DiaChi_KhachHang_ID",
                table: "KhachHang_DiaChi",
                column: "KhachHang_ID");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Voucher_ID_Voucher",
                table: "KhachHang_Voucher",
                column: "ID_Voucher");

            migrationBuilder.CreateIndex(
                name: "IX_Nhan_Vien_Email",
                table: "Nhan_Vien",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_DoNgot_ID_DoNgot",
                table: "SanPham_DoNgot",
                column: "ID_DoNgot");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_Size_ID_San_Pham",
                table: "SanPham_Size",
                column: "ID_San_Pham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_Topping_ID_San_Pham",
                table: "SanPham_Topping",
                column: "ID_San_Pham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamKhuyenMai_ID_Khuyen_Mai",
                table: "SanPhamKhuyenMai",
                column: "ID_Khuyen_Mai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamKhuyenMai_ID_San_Pham",
                table: "SanPhamKhuyenMai",
                column: "ID_San_Pham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamLuongDa_ID_LuongDa",
                table: "SanPhamLuongDa",
                column: "ID_LuongDa");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_Email",
                table: "Tai_Khoan",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_ID_Khach_Hang",
                table: "Tai_Khoan",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_ID_Nhan_Vien",
                table: "Tai_Khoan",
                column: "ID_Nhan_Vien");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_Ten_Nguoi_Dung",
                table: "Tai_Khoan",
                column: "Ten_Nguoi_Dung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_VaiTro_ID_Tai_Khoan_ID_Vai_Tro",
                table: "TaiKhoan_VaiTro",
                columns: new[] { "ID_Tai_Khoan", "ID_Vai_Tro" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_VaiTro_ID_Vai_Tro",
                table: "TaiKhoan_VaiTro",
                column: "ID_Vai_Tro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GioHangChiTiet_Topping");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet_Topping");

            migrationBuilder.DropTable(
                name: "HoaDonVouchers");

            migrationBuilder.DropTable(
                name: "KhachHang_DiaChi");

            migrationBuilder.DropTable(
                name: "KhachHang_Voucher");

            migrationBuilder.DropTable(
                name: "SanPham_DoNgot");

            migrationBuilder.DropTable(
                name: "SanPham_Size");

            migrationBuilder.DropTable(
                name: "SanPham_Topping");

            migrationBuilder.DropTable(
                name: "SanPhamKhuyenMai");

            migrationBuilder.DropTable(
                name: "SanPhamLuongDa");

            migrationBuilder.DropTable(
                name: "TaiKhoan_VaiTro");

            migrationBuilder.DropTable(
                name: "GioHang_ChiTiet");

            migrationBuilder.DropTable(
                name: "HoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Topping");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "Tai_Khoan");

            migrationBuilder.DropTable(
                name: "Vai_Tro");

            migrationBuilder.DropTable(
                name: "Gio_Hang");

            migrationBuilder.DropTable(
                name: "DoNgot");

            migrationBuilder.DropTable(
                name: "Hoa_Don");

            migrationBuilder.DropTable(
                name: "LuongDa");

            migrationBuilder.DropTable(
                name: "San_Pham");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Dia_Chi");

            migrationBuilder.DropTable(
                name: "Hinh_Thuc_Thanh_Toan");

            migrationBuilder.DropTable(
                name: "Khach_Hang");

            migrationBuilder.DropTable(
                name: "Nhan_Vien");
        }
    }
}
