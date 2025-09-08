using BE.Data;
using BE.Repository;
using BE.Repository.IRepository;
using BE.Service; // Thêm này để sử dụng EmailService
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Database Context
builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

// Repository Registrations
builder.Services.AddScoped<IGanVoucherRepository, GanVoucherRepository>();
builder.Services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
builder.Services.AddScoped<IChatSessionNhanVienRepository, ChatSessionNhanVienRepository>();
builder.Services.AddScoped<IDiaChiRepository, DiaChiRepository>();
builder.Services.AddScoped<IDiemDanhRepository, DiemDanhRepository>();
builder.Services.AddScoped<IGioHang_ChiTietRepository, GioHang_ChiTietRepository>();
builder.Services.AddScoped<IGioHangChiTiet_ToppingRepository, GioHangChiTiet_ToppingRepository>();
builder.Services.AddScoped<IGio_HangRepository, Gio_HangRepository>();
builder.Services.AddScoped<IHinhThucThanhToanRepository, HinhThucThanhToanRepository>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonChiTietRepository, HoaDonChiTietRepository>();
builder.Services.AddScoped<IHoaDonChiTietThueRepository, HoaDonChiTietThueRepository>();
builder.Services.AddScoped<IHoaDonChiTietToppingRepository, HoaDonChiTietToppingRepository>();
builder.Services.AddScoped<IHoaDonVoucherRepository, HoaDonVoucherRepository>();

// THÊM LẠI: KhachHangRepository bị comment trong bản mới
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();

builder.Services.AddScoped<IKhachHangDiaChiRepository, KhachHangDiaChiRepository>();
builder.Services.AddScoped<IKhachHangVoucherRepository, KhachHangVoucherRepository>();
builder.Services.AddScoped<ILichSuHoaDonRepository, LichSuHoaDonRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ISanPhamDoNgotRepository, SanPhamDoNgotRepository>();
builder.Services.AddScoped<ISanPhamLuongDaRepository, SanPhamLuongDaRepository>();
builder.Services.AddScoped<ISanPhamSizeRepository, SanPhamSizeRepository>();
builder.Services.AddScoped<ISanPhamToppingRepository, SanPhamToppingRepository>();
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<ITaiKhoanVaiTroRepository, TaiKhoanVaiTroRepository>();
builder.Services.AddScoped<IThueRepository, ThueRepository>();
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<ILuongDaRepository, LuongDaRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IDoNgotRepository, DoNgotRepository>();
builder.Services.AddScoped<IToppingRepository, ToppingRepository>();

// THÊM LẠI: Các repository từ bản cũ cần thiết cho GanVoucher
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IQuanLyPhanQuyenNhanVienRepository, QuanLyPhanQuyenNhanVienRepository>();
builder.Services.AddScoped<IKhuyenMaiSanPhamRepository, KhuyenMaiSanPhamRepository>();
builder.Services.AddScoped<IKhuyenMaiRepository, KhuyenMaiRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

// THÊM LẠI: EmailService cần thiết cho GanVoucher
builder.Services.AddSingleton<EmailService>();

// Configure CORS - Cập nhật để hỗ trợ FE
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:7081",
            "https://localhost:7081",
            "http://localhost:5000",
            "https://localhost:5001"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials() // Cần thiết cho authentication
        .WithExposedHeaders("Access-Control-Allow-Origin"); // Thêm từ bản cũ
    });
});

// Controllers và Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// THÊM LẠI: Middleware để log CORS headers (từ bản cũ)
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        Console.WriteLine($"Response Headers: {string.Join(", ", context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
        return Task.CompletedTask;
    });
    await next.Invoke();
});

// Middleware pipeline - Thứ tự quan trọng!
app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // CORS phải đặt trước Authorization
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();