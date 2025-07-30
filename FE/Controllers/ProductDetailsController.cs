using FE.Service.IService;
using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly IProductDetailsService _service;

        public ProductDetailsController(IProductDetailsService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        // DoNgot
        public async Task<IActionResult> DoNgotIndex()
        {
            var doNgots = await _service.GetAllDoNgotsAsync();
            return PartialView("_DoNgotIndex", doNgots);
        }

        public async Task<IActionResult> DoNgotDetails(int id)
        {
            var doNgot = await _service.GetDoNgotByIdAsync(id);
            if (doNgot == null) return NotFound();
            return PartialView("_DoNgotDetails", doNgot);
        }

        public IActionResult DoNgotCreate()
        {
            return PartialView("_DoNgotCreate");
        }

        [HttpPost]
        public async Task<IActionResult> DoNgotCreate(DoNgot doNgot)
        {
            if (ModelState.IsValid)
            {
                doNgot.ID_DoNgot = 0;
                await _service.CreateDoNgotAsync(doNgot);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_DoNgotCreate", doNgot);
        }

        public async Task<IActionResult> DoNgotEdit(int id)
        {
            var doNgot = await _service.GetDoNgotByIdAsync(id);
            if (doNgot == null) return NotFound();
            return PartialView("_DoNgotEdit", doNgot);
        }

        [HttpPost]
        public async Task<IActionResult> DoNgotEdit(int id, DoNgot doNgot)
        {
            if (id != doNgot.ID_DoNgot) return BadRequest();
            if (ModelState.IsValid)
            {
                await _service.UpdateDoNgotAsync(doNgot);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_DoNgotEdit", doNgot);
        }

        // Size
        public async Task<IActionResult> SizeIndex()
        {
            var sizes = await _service.GetAllSizesAsync();
            return PartialView("_SizeIndex", sizes);
        }

        public async Task<IActionResult> SizeDetails(int id)
        {
            var size = await _service.GetSizeByIdAsync(id);
            if (size == null) return NotFound();
            return PartialView("_SizeDetails", size);
        }

        public IActionResult SizeCreate()
        {
            return PartialView("_SizeCreate");
        }

        [HttpPost]
        public async Task<IActionResult> SizeCreate(Size size)
        {
            if (ModelState.IsValid)
            {
                size.ID_Size = 0;
                await _service.CreateSizeAsync(size);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_SizeCreate", size);
        }

        public async Task<IActionResult> SizeEdit(int id)
        {
            var size = await _service.GetSizeByIdAsync(id);
            if (size == null) return NotFound();
            return PartialView("_SizeEdit", size);
        }

        [HttpPost]
        public async Task<IActionResult> SizeEdit(int id, Size size)
        {
            if (id != size.ID_Size) return BadRequest();
            if (ModelState.IsValid)
            {
                await _service.UpdateSizeAsync(size);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_SizeEdit", size);
        }

        // Topping
        public async Task<IActionResult> ToppingIndex()
        {
            var toppings = await _service.GetAllToppingsAsync();
            return PartialView("_ToppingIndex", toppings);
        }

        public async Task<IActionResult> ToppingDetails(int id)
        {
            var topping = await _service.GetToppingByIdAsync(id);
            if (topping == null) return NotFound();
            return PartialView("_ToppingDetails", topping);
        }

        public IActionResult ToppingCreate()
        {
            return PartialView("_ToppingCreate");
        }

        [HttpPost]
        public async Task<IActionResult> ToppingCreate(Topping topping)
        {
            if (ModelState.IsValid)
            {
                topping.ID_Topping = 0;
                await _service.CreateToppingAsync(topping);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_ToppingCreate", topping);
        }

        public async Task<IActionResult> ToppingEdit(int id)
        {
            var topping = await _service.GetToppingByIdAsync(id);
            if (topping == null) return NotFound();
            return PartialView("_ToppingEdit", topping);
        }

        [HttpPost]
        public async Task<IActionResult> ToppingEdit(int id, Topping topping)
        {
            if (id != topping.ID_Topping) return BadRequest();
            if (ModelState.IsValid)
            {
                await _service.UpdateToppingAsync(topping);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_ToppingEdit", topping);
        }

        // LuongDa
        public async Task<IActionResult> LuongDaIndex()
        {
            var luongDas = await _service.GetAllLuongDasAsync();
            return PartialView("_LuongDaIndex", luongDas);
        }

        public async Task<IActionResult> LuongDaDetails(int id)
        {
            var luongDa = await _service.GetLuongDaByIdAsync(id);
            if (luongDa == null) return NotFound();
            return PartialView("_LuongDaDetails", luongDa);
        }

        public IActionResult LuongDaCreate()
        {
            return PartialView("_LuongDaCreate");
        }

        [HttpPost]
        public async Task<IActionResult> LuongDaCreate(LuongDa luongDa)
        {
            if (ModelState.IsValid)
            {
                luongDa.ID_LuongDa = 0;
                await _service.CreateLuongDaAsync(luongDa);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_LuongDaCreate", luongDa);
        }

        public async Task<IActionResult> LuongDaEdit(int id)
        {
            var luongDa = await _service.GetLuongDaByIdAsync(id);
            if (luongDa == null) return NotFound();
            return PartialView("_LuongDaEdit", luongDa);
        }

        [HttpPost]
        public async Task<IActionResult> LuongDaEdit(int id, LuongDa luongDa)
        {
            if (id != luongDa.ID_LuongDa) return BadRequest();
            if (ModelState.IsValid)
            {
                await _service.UpdateLuongDaAsync(luongDa);
                return RedirectToAction(nameof(Index));
            }
            return PartialView("_LuongDaEdit", luongDa);
        }
    }
}
