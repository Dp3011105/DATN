using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Repository cho từng model
builder.Services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
builder.Services.AddScoped<IChatSessionNhanVienRepository, ChatSessionNhanVienRepository>();
builder.Services.AddScoped<IDiaChiRepository, DiaChiRepository>();
builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IDoNgotRepository, DoNgotRepository>();
builder.Services.AddScoped<IGioHang_ChiTietRepository, GioHang_ChiTietRepository>();
builder.Services.AddScoped<IGioHangChiTiet_ToppingRepository, GioHangChiTiet_ToppingRepository>();
builder.Services.AddScoped<IGio_HangRepository, Gio_HangRepository>();
builder.Services.AddScoped<IHinhThucThanhToanRepository, HinhThucThanhToanRepository>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonChiTietRepository, HoaDonChiTietRepository>();
builder.Services.AddScoped<IHoaDonChiTietThueRepository, HoaDonChiTietThueRepository>();
builder.Services.AddScoped<IHoaDonChiTietToppingRepository, HoaDonChiTietToppingRepository>();
builder.Services.AddScoped<IHoaDonVoucherRepository, HoaDonVoucherRepository>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<IKhachHangDiaChiRepository, KhachHangDiaChiRepository>();
builder.Services.AddScoped<IKhachHangVoucherRepository, KhachHangVoucherRepository>();
builder.Services.AddScoped<ILichSuHoaDonRepository, LichSuHoaDonRepository>();
builder.Services.AddScoped<ILuongDaRepository, LuongDaRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<ISanPhamDoNgotRepository, SanPhamDoNgotRepository>();
builder.Services.AddScoped<ISanPhamLuongDaRepository, SanPhamLuongDaRepository>();
builder.Services.AddScoped<ISanPhamSizeRepository, SanPhamSizeRepository>();
builder.Services.AddScoped<ISanPhamToppingRepository, SanPhamToppingRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<ITaiKhoanVaiTroRepository, TaiKhoanVaiTroRepository>();
builder.Services.AddScoped<IThueRepository, ThueRepository>();
builder.Services.AddScoped<IToppingRepository, ToppingRepository>();
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

// Swagger & Controller
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
