//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;

//namespace BE.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ShippingController : ControllerBase
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _token = "3c6d36d0-ea92-11ef-a839-66afa442234f"; // copy từ GHN Portal
//        private readonly string _shopId = "5634551"; // ShopId của bạn

//        public ShippingController(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        // API: Lấy danh sách Tỉnh/Thành và lọc chỉ Hà Nội
//        [HttpGet("provinces/hanoi")]
//        public async Task<IActionResult> GetHanoiProvince()
//        {
//            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/province";

//            _httpClient.DefaultRequestHeaders.Clear();
//            _httpClient.DefaultRequestHeaders.Add("Token", _token);

//            var response = await _httpClient.GetAsync(url);
//            var body = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//                return BadRequest(new { error = body });

//            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

//            // lọc chỉ Hà Nội
//            var hanoi = json.GetProperty("data")
//                            .EnumerateArray()
//                            .FirstOrDefault(p => p.GetProperty("ProvinceName").GetString() == "Hà Nội");

//            return Ok(hanoi);
//        }

//        // API: Lấy danh sách Quận/Huyện theo ProvinceId (Hà Nội)
//        [HttpGet("districts/{provinceId}")]
//        public async Task<IActionResult> GetDistricts(int provinceId)
//        {
//            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";

//            _httpClient.DefaultRequestHeaders.Clear();
//            _httpClient.DefaultRequestHeaders.Add("Token", _token);

//            var response = await _httpClient.PostAsJsonAsync(url, new { province_id = provinceId });
//            var body = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//                return BadRequest(new { error = body });

//            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

//            return Ok(json);
//        }

//        // API: Lấy danh sách Phường/Xã theo DistrictId
//        [HttpGet("wards/{districtId}")]
//        public async Task<IActionResult> GetWards(int districtId)
//        {
//            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward";

//            _httpClient.DefaultRequestHeaders.Clear();
//            _httpClient.DefaultRequestHeaders.Add("Token", _token);

//            var response = await _httpClient.PostAsJsonAsync(url, new { district_id = districtId });
//            var body = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//                return BadRequest(new { error = body });

//            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

//            return Ok(json);
//        }
//    }
//}









using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = "3c6d36d0-ea92-11ef-a839-66afa442234f"; // thay bằng token thật
        private readonly string _shopId = "5634551";         // thay bằng ShopId thật

        public ShippingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ===== API LẤY TỈNH HÀ NỘI =====
        [HttpGet("provinces/hanoi")]
        public async Task<IActionResult> GetHanoiProvince()
        {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/province";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Token", _token);

            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { error = body });

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

            var hanoi = json.GetProperty("data")
                            .EnumerateArray()
                            .FirstOrDefault(p => p.GetProperty("ProvinceName").GetString() == "Hà Nội");

            return Ok(hanoi);
        }

        // ===== API LẤY QUẬN/HUYỆN =====
        [HttpGet("districts/{provinceId}")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Token", _token);

            var response = await _httpClient.PostAsJsonAsync(url, new { province_id = provinceId });
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { error = body });

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

            return Ok(json);
        }

        // ===== API LẤY PHƯỜNG/XÃ =====
        [HttpGet("wards/{districtId}")]
        public async Task<IActionResult> GetWards(int districtId)
        {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Token", _token);

            var response = await _httpClient.PostAsJsonAsync(url, new { district_id = districtId });
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { error = body });

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var json = JsonSerializer.Deserialize<JsonElement>(body, options);

            return Ok(json);
        }

        // ===== API TÍNH PHÍ SHIP =====
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateShipping([FromBody] ShippingRequest request)
        {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Token", _token);
            _httpClient.DefaultRequestHeaders.Add("ShopId", _shopId);

            var body = new
            {
                service_type_id = 2,                // dịch vụ giao hàng nhanh
                from_district_id = request.FromDistrictId,
                to_district_id = request.ToDistrictId,
                to_ward_code = request.ToWardCode,
                height = request.Height,
                length = request.Length,
                weight = request.Weight,
                width = request.Width,
                insurance_value = request.InsuranceValue,
            };

            var jsonBody = JsonSerializer.Serialize(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { error = responseString });

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<JsonElement>(responseString, options);

            return Ok(result);
        }
    }

    // ===== MODEL REQUEST =====
    public class ShippingRequest
    {
        public int FromDistrictId { get; set; }
        public int ToDistrictId { get; set; }
        public string ToWardCode { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int InsuranceValue { get; set; }
    }
}
