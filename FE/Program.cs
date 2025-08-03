using FE.Service;
using FE.Service.IService;
using FE.Services;
using Service;
using Service.IService;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IProductDetailsService, ProductDetailsService>();


// Đăng ký các Service dùng Dependency Injection
//builder.Services.AddScoped<IChatSessionService, ChatSessionService>();
//builder.Services.AddScoped<IChatSessionNhanVienService, ChatSessionNhanVienService>();
//builder.Services.AddScoped<IDiaChiService, DiaChiService>();
//builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();
//builder.Services.AddScoped<IDoNgotService, DoNgotService>();
//builder.Services.AddScoped<IGioHang_ChiTietService, GioHang_ChiTietService>();
//builder.Services.AddScoped<IGioHangChiTiet_ToppingService, GioHangChiTiet_ToppingService>();
//builder.Services.AddScoped<IGio_HangService, Gio_HangService>();
//builder.Services.AddScoped<IHinhThucThanhToanService, HinhThucThanhToanService>();
//builder.Services.AddScoped<IHoaDonService, HoaDonService>();
//builder.Services.AddScoped<IHoaDonChiTietService, HoaDonChiTietService>();
//builder.Services.AddScoped<IHoaDonChiTietThueService, HoaDonChiTietThueService>();
//builder.Services.AddScoped<IHoaDonChiTietToppingService, HoaDonChiTietToppingService>();
//builder.Services.AddScoped<IHoaDonVoucherService, HoaDonVoucherService>();
//builder.Services.AddScoped<IKhachHangService, KhachHangService>();
//builder.Services.AddScoped<IKhachHangDiaChiService, KhachHangDiaChiService>();
//builder.Services.AddScoped<IKhachHangVoucherService, KhachHangVoucherService>();
//builder.Services.AddScoped<ILichSuHoaDonService, LichSuHoaDonService>();
//builder.Services.AddScoped<ILuongDaService, LuongDaService>();
//builder.Services.AddScoped<IMessageService, MessageService>();
//builder.Services.AddScoped<INhanVienService, NhanVienService>();
//builder.Services.AddScoped<ISanPhamService, SanPhamService>();
//builder.Services.AddScoped<ISanPhamDoNgotService, SanPhamDoNgotService>();
//builder.Services.AddScoped<ISanPhamLuongDaService, SanPhamLuongDaService>();
//builder.Services.AddScoped<ISanPhamSizeService, SanPhamSizeService>();
//builder.Services.AddScoped<ISanPhamToppingService, SanPhamToppingService>();
//builder.Services.AddScoped<ISizeService, SizeService>();
//builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
//builder.Services.AddScoped<ITaiKhoanVaiTroService, TaiKhoanVaiTroService>();
//builder.Services.AddScoped<IThueService, ThueService>();
//builder.Services.AddScoped<IToppingService, ToppingService>();
//builder.Services.AddScoped<IVaiTroService, VaiTroService>();
//builder.Services.AddScoped<IVoucherService, VoucherService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
