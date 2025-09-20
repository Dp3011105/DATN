# 📘 Dự án Tốt nghiệp – Hệ thống Quản lý & Bán hàng

## 📝 Giới thiệu
Dự án này được phát triển trong khuôn khổ **Đồ án Tốt nghiệp (DATN)** của nhóm sinh viên trường FPT Polytechnic.  
Mục tiêu: Xây dựng hệ thống quản lý bán hàng đa nền tảng, hỗ trợ quản lý sản phẩm, đơn hàng, khách hàng, khuyến mãi, và thanh toán.

## 👨‍💻 Thành viên nhóm
- **Vũ Xuân Phúc** – MSSV: *PH54760*
- **Nguyễn Đức Phương** – MSSV: *PH53969*
- **Đào Hiệp Hưng** – MSSV: *PH53856*
- **Nguyễn Đức Phước** – MSSV: *PH53971*


## ⚙️ Công nghệ sử dụng
- **Back-end**: ASP.NET Core (.NET 8 `.csproj`)
- **Entity Framework Core**: ORM quản lý database
- **SQL Server**: Cơ sở dữ liệu chính
- **Front-end**: ASP.NET MVC (Views Razor)
- **Authentication**: JWT / Identity
- **API**: RESTful API
- **Khác**: LINQ, AutoMapper, Dependency Injection

## 📂 Cấu trúc thư mục chính
```
DATN.sln                → Solution tổng
BE/                     → Back-end API
  Controllers/          → Các controller cho API (HoaDon, SanPham, KhachHang…)
  DTOs/                 → Data Transfer Objects
  Models/               → Các lớp entity (EF Core)
  Repository/           → Repository pattern
  Service/              → Business logic
FE/                     → Front-end (MVC)
  Controllers/          → Controller giao diện
  Views/                → Razor Views
  wwwroot/              → CSS, JS, assets
```

## 🚀 Cài đặt & Chạy dự án

### 1. Yêu cầu hệ thống
- .NET SDK (6.0+)
- SQL Server (2019 hoặc mới hơn)
- Visual Studio 2022 (hoặc Rider)

### 2. Clone & mở project
```bash
git clone https://github.com/Dp3011105/DATN.git
cd DATN
```
Hoặc tải file ZIP và giải nén.

### 3. Cấu hình database
- Mở file `appsettings.json`
- Sửa chuỗi kết nối `"ConnectionStrings:DefaultConnection"`
- Chạy lệnh migrations (nếu cần):
```bash
dotnet ef database update
```

### 4. Chạy Back-end
```bash
cd BE
dotnet run
```
API sẽ chạy tại `https://localhost:5001` (tuỳ cấu hình).

### 5. Chạy Front-end
```bash
cd FE
dotnet run
```
Mở trình duyệt tại `https://localhost:5002` để truy cập giao diện.

## 🧪 Tính năng chính
- Quản lý sản phẩm (CRUD, size, topping…)
- Quản lý khách hàng & nhân viên
- Quản lý đơn hàng, giỏ hàng
- Thanh toán + tích hợp voucher, khuyến mãi
- Báo cáo thống kê doanh thu
- Phân quyền tài khoản (Admin, Nhân viên, Khách hàng)



