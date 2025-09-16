using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FE.Controllers
{
    public class TraCuuController : Controller
    {
        private readonly HttpClient _httpClient;

        public TraCuuController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string maHoaDon, string soDienThoai)
        {
            var requestData = new
            {
                maHoaDon = maHoaDon,
                soDienThoai = soDienThoai
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://localhost:7169/api/TraCuuHoaDon/check-hoa-don", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Có lỗi xảy ra khi gọi API!";
                    return View();
                }

                // Nếu trả về message lỗi
                if (responseContent.Contains("message"))
                {
                    dynamic errorResponse = JsonConvert.DeserializeObject(responseContent);
                    ViewBag.Error = (string)errorResponse.message;
                    return View();
                }

                // Nếu trả về dữ liệu đúng
                dynamic successResponse = JsonConvert.DeserializeObject(responseContent);
                ViewBag.Result = successResponse;
                return View();
            }
            catch
            {
                ViewBag.Error = "Không thể kết nối đến API!";
                return View();
            }
        }


    }
}
