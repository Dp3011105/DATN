using BE.DTOs;
using BE.models;
using BE.Repository;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingController : ControllerBase
    {

        private readonly IToppingRepository _toppingRepository;

        public ToppingController(IToppingRepository toppingRepository)
        {
            _toppingRepository = toppingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToppingDTO>>> GetAllToppings()
        {
            var toppings = await _toppingRepository.GetAllAsync();
            return Ok(toppings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToppingDTO>> GetToppingById(int id)
        {
            var topping = await _toppingRepository.GetByIdAsync(id);
            if (topping == null)
            {
                return NotFound(new { message = "Topping không tồn tại" });
            }
            return Ok(topping);
        }

        [HttpPost]
        public async Task<ActionResult<ToppingDTO>> CreateTopping([FromBody] ToppingDTO toppingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTopping = await _toppingRepository.CreateAsync(toppingDTO);
            return CreatedAtAction(nameof(GetToppingById), new { id = createdTopping.ID_Topping }, createdTopping);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToppingDTO>> UpdateTopping(int id, [FromBody] ToppingDTO toppingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toppingDTO.ID_Topping)
            {
                return BadRequest(new { message = "ID topping không khớp" });
            }

            var updatedTopping = await _toppingRepository.UpdateAsync(id, toppingDTO);
            if (updatedTopping == null)
            {
                return NotFound(new { message = "Topping không tồn tại" });
            }

            return Ok(updatedTopping);

        }
    }
}