using FE.Service;
using FE.Service.IService;
using FE.Services;
using Service;
using Service.IService;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IProductDetailsService, ProductDetailsService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IHoaDonChiTietService, HoaDonChiTietService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<IVaiTroService, VaiTroService>();
builder.Services.AddScoped<ITaiKhoanVaiTroService, TaiKhoanVaiTroService>();
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();

builder.Services.AddScoped<IQuanLyVaiTroService, QuanLyVaiTroService>();
builder.Services.AddScoped<IGanVoucherService, GanVoucherService>();
builder.Services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddHttpClient<IQLDonHangTkService, QLDonHangTkService>();


// SỬA LẠI: Base API URL phải trỏ đến BE, không phải FE
var baseApiUrl = "https://localhost:7169/"; // Đây là port của BE



// THÊM LẠI: Đăng ký GanVoucherService với HttpClient và timeout như bản cũ
builder.Services.AddHttpClient<IGanVoucherService, GanVoucherService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
    client.Timeout = TimeSpan.FromMinutes(2);
});

//Đăng ký các Service dùng Dependency Injection



builder.Services.AddHttpClient<IDiaChiService, DiaChiService>(client =>
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



builder.Services.AddHttpClient<ILuongDaService, LuongDaService>(client =>
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



builder.Services.AddHttpClient<IToppingService, ToppingService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

builder.Services.AddHttpClient<IVoucherService, VoucherService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(baseApiUrl);
});

// ĐĂNG KÝ CHO AUTH SERVICE CHỨC NĂNG ĐĂNG NHẬP đăng ký 

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

// SỬA LẠI: Route mặc định giống bản cũ để GanVoucher hoạt động
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();