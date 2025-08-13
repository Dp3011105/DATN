namespace BE.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using BE.models;
    using BE.Repository.IRepository;

    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;

        public HoaDonController(IHoaDonRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hoaDons = await _repository.GetAllAsync(); // trả DTO
            return Ok(hoaDons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoaDon = await _repository.GetByIdAsync(id); // trả entity
            if (hoaDon == null) return NotFound();
            return Ok(hoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null) return BadRequest("HoaDon cannot be null.");
            await _repository.AddAsync(hoaDon);
            return CreatedAtAction(nameof(GetById), new { id = hoaDon.ID_Hoa_Don }, hoaDon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || id != hoaDon.ID_Hoa_Don)
                return BadRequest("Invalid HoaDon data.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _repository.UpdateAsync(id, hoaDon);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
