using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
