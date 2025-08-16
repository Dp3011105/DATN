using BE.Data;
using BE.Repository;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
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
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<IKhachHangDiaChiRepository, KhachHangDiaChiRepository>();
builder.Services.AddScoped<IKhachHangVoucherRepository, KhachHangVoucherRepository>();
builder.Services.AddScoped<ILichSuHoaDonRepository, LichSuHoaDonRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ISanPhamDoNgotRepository, SanPhamDoNgotRepository>();
builder.Services.AddScoped<ISanPhamSizeRepository, SanPhamSizeRepository>();
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<ITaiKhoanVaiTroRepository, TaiKhoanVaiTroRepository>();
builder.Services.AddScoped<IThueRepository, ThueRepository>();
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();



//phước
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<ILuongDaRepository, LuongDaRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IDoNgotRepository, DoNgotRepository>();
builder.Services.AddScoped<IToppingRepository, ToppingRepository>();
builder.Services.AddScoped<ISanPhamLuongDaRepository, SanPhamLuongDaRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
// Register Repositories chức năng đăng ký tài khoản 
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
// Register chức năng ghép Tài khoản và Vai trò của nhân viên với nhau
builder.Services.AddScoped<IQuanLyPhanQuyenNhanVienRepository, QuanLyPhanQuyenNhanVienRepository>();
// Đăng ký EmailService
builder.Services.AddSingleton<EmailService>();



// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:7081", "https://localhost:7081") // AE thay đường dẫn của FE ae vào 
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Access-Control-Allow-Origin"); // Expose CORS headers for debugging
    });
});


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

// Add middleware to log CORS headers
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        Console.WriteLine($"Response Headers: {string.Join(", ", context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
        return Task.CompletedTask;
    });
    await next.Invoke();
});


app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();