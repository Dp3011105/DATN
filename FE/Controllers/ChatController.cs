using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }




        public async Task<IActionResult> ChatAI()
        {
            return View();
        }


    }
}
