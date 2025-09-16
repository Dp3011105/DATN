using FE.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [RoleAuthorize(2, 3)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
