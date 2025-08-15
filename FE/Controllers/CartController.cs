using FE.Models;
using FE.Service;
using FE.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
// dành riêng em nó cho các chức năng liên quan đến giỏ hàng như add sản phẩm vào giỏ hàng , xóa sản phẩm khỏi giỏ hàng , linh tinh và mây mây
// Hiện chỉ thêm được vào giỏ hàng , chưa thể thanh toán 
//(Phước )
namespace FE.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
                    

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        


        [HttpPost]
        public async Task<IActionResult> AddToCart(
    int id, // ID_San_Pham
    int selectedSize,
    int selectedDoNgot,
    int selectedLuongDa,
    int so_Luong,
    string ghi_Chu = "string",
    List<int> selectedToppings = null)
        {
            // 1. Lấy UserData từ cookie
            var userDataJson = Request.Cookies["UserData"];
            if (string.IsNullOrEmpty(userDataJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var userData = JsonSerializer.Deserialize<LoginResponse>(userDataJson);
            if (userData?.ID_Khach_Hang == null || userData.ID_Khach_Hang <= 0)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Validate lựa chọn cơ bản
            if (selectedSize <= 0 || selectedDoNgot <= 0 || selectedLuongDa <= 0 || so_Luong <= 0)
            {
                TempData["Error"] = "Vui lòng chọn đầy đủ tùy chọn và số lượng > 0.";
                return RedirectToAction("Index", "Home");
            }

            // 3. Lấy thông tin sản phẩm từ DB
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // 4. Kiểm tra tồn kho
            if (so_Luong > product.So_Luong)
            {
                TempData["Error"] = $"Số lượng nhập ({so_Luong}) vượt quá tồn kho. Chỉ còn {product.So_Luong} sản phẩm.";
                return RedirectToAction("Index", "Home");
            }

            // 5. Tạo object để gửi API
            object cartItem;
            if (selectedToppings != null && selectedToppings.Any())
            {
                cartItem = new
                {
                    iD_Khach_Hang = userData.ID_Khach_Hang.Value,
                    iD_San_Pham = id,
                    iD_Size = selectedSize,
                    iD_DoNgot = selectedDoNgot,
                    iD_LuongDa = selectedLuongDa,
                    so_Luong = so_Luong,
                    ghi_Chu = ghi_Chu,
                    toppings = selectedToppings
                };
            }
            else
            {
                cartItem = new
                {
                    iD_Khach_Hang = userData.ID_Khach_Hang.Value,
                    iD_San_Pham = id,
                    iD_Size = selectedSize,
                    iD_DoNgot = selectedDoNgot,
                    iD_LuongDa = selectedLuongDa,
                    so_Luong = so_Luong,
                    ghi_Chu = ghi_Chu
                };
            }

            // 6. Gửi dữ liệu đến API
            var success = await _cartService.AddToCartAsync(cartItem);
            if (success)
            {
                TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng.";
            }
            else
            {
                TempData["Error"] = "Thêm vào giỏ hàng thất bại. Vui lòng thử lại.";
            }

            return RedirectToAction("Index", "Home");
        }



    }
}
