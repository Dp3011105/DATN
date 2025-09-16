using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace FE.Controllers
{
    public class CheckoutCkController : Controller
    {
        private readonly HttpClient _httpClient;

        public CheckoutCkController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7169/");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
