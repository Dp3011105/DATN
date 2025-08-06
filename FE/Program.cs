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
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IHoaDonChiTietService, HoaDonChiTietService>();
builder.Services.AddScoped<IHoaDonChiTietThueService, HoaDonChiTietThueService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var baseApiUrl = "https://localhost:7081/";
//Đăng ký các Service dùng Dependency Injection

builder.Services.AddHttpClient<IChatSessionService, ChatSessionService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IChatSessionNhanVienService, ChatSessionNhanVienService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IDiaChiService, DiaChiService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IDiemDanhService, DiemDanhService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IDoNgotService, DoNgotService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IGioHang_ChiTietService, GioHang_ChiTietService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IGioHangChiTiet_ToppingService, GioHangChiTiet_ToppingService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IGio_HangService, Gio_HangService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IHinhThucThanhToanService, HinhThucThanhToanService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});
builder.Services.AddHttpClient<IHoaDonChiTietToppingService, HoaDonChiTietToppingService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IHoaDonVoucherService, HoaDonVoucherService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IKhachHangDiaChiService, KhachHangDiaChiService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IKhachHangVoucherService, KhachHangVoucherService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ILichSuHoaDonService, LichSuHoaDonService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ILuongDaService, LuongDaService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IMessageService, MessageService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISanPhamService, SanPhamService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISanPhamDoNgotService, SanPhamDoNgotService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISanPhamLuongDaService, SanPhamLuongDaService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISanPhamSizeService, SanPhamSizeService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISanPhamToppingService, SanPhamToppingService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ISizeService, SizeService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ITaiKhoanService, TaiKhoanService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<ITaiKhoanVaiTroService, TaiKhoanVaiTroService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IThueService, ThueService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IToppingService, ToppingService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IVaiTroService, VaiTroService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IVoucherService, VoucherService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles(); // Cho phép truy cập file trong wwwroot
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ThongKe}/{action=Index}/{id?}");

app.Run();
