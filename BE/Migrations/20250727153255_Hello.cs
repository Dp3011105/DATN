using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE.Migrations
{
    /// <inheritdoc />
    public partial class Hello : Migration
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
                    GioiTinh = table.Column<bool>(type: "bit", nullable: true),
                    So_Dien_Thoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khach_Hang", x => x.ID_Khach_Hang);
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
                    GioiTinh = table.Column<bool>(type: "bit", nullable: true),
                    So_Dien_Thoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Dia_Chi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NamSinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true),
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
                name: "Thue",
                columns: table => new
                {
                    ID_Thue = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Thue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ty_Le = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Mo_Ta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thue", x => x.ID_Thue);
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
                name: "Chat_Session",
                columns: table => new
                {
                    ID_Chat_Session = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: false),
                    Tieu_De = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_Session", x => x.ID_Chat_Session);
                    table.ForeignKey(
                        name: "FK_Chat_Session_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Diem_Danh",
                columns: table => new
                {
                    ID_Diem_Danh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NhanVien_ID = table.Column<int>(type: "int", nullable: false),
                    Vi_Tri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ngay_Diem_Danh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gio_Bat_Dau = table.Column<TimeSpan>(type: "time", nullable: true),
                    Gio_Ket_Thuc = table.Column<TimeSpan>(type: "time", nullable: true),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diem_Danh", x => x.ID_Diem_Danh);
                    table.ForeignKey(
                        name: "FK_Diem_Danh_Nhan_Vien_NhanVien_ID",
                        column: x => x.NhanVien_ID,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hoa_Don",
                columns: table => new
                {
                    ID_Hoa_Don = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: false),
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: true),
                    ID_Hinh_Thuc_Thanh_Toan = table.Column<int>(type: "int", nullable: true),
                    ID_Dia_Chi = table.Column<int>(type: "int", nullable: true),
                    ID_Phi_Ship = table.Column<int>(type: "int", nullable: true),
                    Dia_Chi_Tu_Nhap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tong_Tien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Phi_Ship = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Trang_Thai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ma_Hoa_Don = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Loai_Hoa_Don = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        principalColumn: "ID_Dia_Chi");
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Hinh_Thuc_Thanh_Toan_ID_Hinh_Thuc_Thanh_Toan",
                        column: x => x.ID_Hinh_Thuc_Thanh_Toan,
                        principalTable: "Hinh_Thuc_Thanh_Toan",
                        principalColumn: "ID_Hinh_Thuc_Thanh_Toan");
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hoa_Don_Nhan_Vien_ID_Nhan_Vien",
                        column: x => x.ID_Nhan_Vien,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien");
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
                    Mat_Khau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: true)
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
                    Mo_Ta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    Mo_Ta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                name: "Chat_Session_Nhan_Vien",
                columns: table => new
                {
                    ID_Chat_Session = table.Column<int>(type: "int", nullable: false),
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_Session_Nhan_Vien", x => new { x.ID_Chat_Session, x.ID_Nhan_Vien });
                    table.ForeignKey(
                        name: "FK_Chat_Session_Nhan_Vien_Chat_Session_ID_Chat_Session",
                        column: x => x.ID_Chat_Session,
                        principalTable: "Chat_Session",
                        principalColumn: "ID_Chat_Session",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chat_Session_Nhan_Vien_Nhan_Vien_ID_Nhan_Vien",
                        column: x => x.ID_Nhan_Vien,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    ID_Message = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Chat_Session = table.Column<int>(type: "int", nullable: false),
                    ID_Khach_Hang = table.Column<int>(type: "int", nullable: true),
                    ID_Nhan_Vien = table.Column<int>(type: "int", nullable: true),
                    Noi_Dung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thoi_Gian_Gui = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.ID_Message);
                    table.ForeignKey(
                        name: "FK_Message_Chat_Session_ID_Chat_Session",
                        column: x => x.ID_Chat_Session,
                        principalTable: "Chat_Session",
                        principalColumn: "ID_Chat_Session",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Khach_Hang_ID_Khach_Hang",
                        column: x => x.ID_Khach_Hang,
                        principalTable: "Khach_Hang",
                        principalColumn: "ID_Khach_Hang");
                    table.ForeignKey(
                        name: "FK_Message_Nhan_Vien_ID_Nhan_Vien",
                        column: x => x.ID_Nhan_Vien,
                        principalTable: "Nhan_Vien",
                        principalColumn: "ID_Nhan_Vien");
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
                    Ma_HoaDon_ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        principalColumn: "ID_DoNgot");
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
                        principalColumn: "ID_LuongDa");
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_San_Pham_ID_San_Pham",
                        column: x => x.ID_San_Pham,
                        principalTable: "San_Pham",
                        principalColumn: "ID_San_Pham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_ChiTiet_Size_ID_Size",
                        column: x => x.ID_Size,
                        principalTable: "Size",
                        principalColumn: "ID_Size");
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
                name: "Lich_Su_Hoa_Don",
                columns: table => new
                {
                    ID_Lich_Su_Hoa_Don = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Hoa_Don = table.Column<int>(type: "int", nullable: false),
                    Ngay_Tao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nguoi_Tao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ngay_Cap_Nhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nguoi_Cap_Nhat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Hanh_Dong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lich_Su_Hoa_Don", x => x.ID_Lich_Su_Hoa_Don);
                    table.ForeignKey(
                        name: "FK_Lich_Su_Hoa_Don_Hoa_Don_ID_Hoa_Don",
                        column: x => x.ID_Hoa_Don,
                        principalTable: "Hoa_Don",
                        principalColumn: "ID_Hoa_Don",
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
                    table.PrimaryKey("PK_TaiKhoan_VaiTro", x => new { x.ID_Vai_Tro, x.ID_Tai_Khoan });
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
                name: "HoaDonChiTiet_Thue",
                columns: table => new
                {
                    ID_HoaDon_ChiTiet = table.Column<int>(type: "int", nullable: false),
                    ID_Thue = table.Column<int>(type: "int", nullable: false),
                    Ghi_Chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trang_Thai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiet_Thue", x => new { x.ID_HoaDon_ChiTiet, x.ID_Thue });
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_Thue_HoaDon_ChiTiet_ID_HoaDon_ChiTiet",
                        column: x => x.ID_HoaDon_ChiTiet,
                        principalTable: "HoaDon_ChiTiet",
                        principalColumn: "ID_HoaDon_ChiTiet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_Thue_Thue_ID_Thue",
                        column: x => x.ID_Thue,
                        principalTable: "Thue",
                        principalColumn: "ID_Thue",
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

            migrationBuilder.CreateIndex(
                name: "IX_Chat_Session_ID_Khach_Hang",
                table: "Chat_Session",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_Session_Nhan_Vien_ID_Nhan_Vien",
                table: "Chat_Session_Nhan_Vien",
                column: "ID_Nhan_Vien");

            migrationBuilder.CreateIndex(
                name: "IX_Diem_Danh_NhanVien_ID",
                table: "Diem_Danh",
                column: "NhanVien_ID");

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
                name: "IX_HoaDon_ChiTiet_ID_Hoa_Don",
                table: "HoaDon_ChiTiet",
                column: "ID_Hoa_Don");

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
                name: "IX_HoaDonChiTiet_Thue_ID_Thue",
                table: "HoaDonChiTiet_Thue",
                column: "ID_Thue");

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
                name: "IX_Lich_Su_Hoa_Don_ID_Hoa_Don",
                table: "Lich_Su_Hoa_Don",
                column: "ID_Hoa_Don");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ID_Chat_Session",
                table: "Message",
                column: "ID_Chat_Session");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ID_Khach_Hang",
                table: "Message",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ID_Nhan_Vien",
                table: "Message",
                column: "ID_Nhan_Vien");

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
                name: "IX_SanPhamLuongDa_ID_LuongDa",
                table: "SanPhamLuongDa",
                column: "ID_LuongDa");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_ID_Khach_Hang",
                table: "Tai_Khoan",
                column: "ID_Khach_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_Tai_Khoan_ID_Nhan_Vien",
                table: "Tai_Khoan",
                column: "ID_Nhan_Vien");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_VaiTro_ID_Tai_Khoan",
                table: "TaiKhoan_VaiTro",
                column: "ID_Tai_Khoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chat_Session_Nhan_Vien");

            migrationBuilder.DropTable(
                name: "Diem_Danh");

            migrationBuilder.DropTable(
                name: "GioHangChiTiet_Topping");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet_Thue");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet_Topping");

            migrationBuilder.DropTable(
                name: "HoaDonVouchers");

            migrationBuilder.DropTable(
                name: "KhachHang_DiaChi");

            migrationBuilder.DropTable(
                name: "KhachHang_Voucher");

            migrationBuilder.DropTable(
                name: "Lich_Su_Hoa_Don");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "SanPham_DoNgot");

            migrationBuilder.DropTable(
                name: "SanPham_Size");

            migrationBuilder.DropTable(
                name: "SanPham_Topping");

            migrationBuilder.DropTable(
                name: "SanPhamLuongDa");

            migrationBuilder.DropTable(
                name: "TaiKhoan_VaiTro");

            migrationBuilder.DropTable(
                name: "GioHang_ChiTiet");

            migrationBuilder.DropTable(
                name: "Thue");

            migrationBuilder.DropTable(
                name: "HoaDon_ChiTiet");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Chat_Session");

            migrationBuilder.DropTable(
                name: "Topping");

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
