using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowerController : ControllerBase
    {
        private readonly FlowerRepository _repo;

        public FlowerController(FlowerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Add(FlowerDto dto)
        {
            await _repo.Add(dto);
            return Ok("Flower Added");
        }

        [HttpGet("byprice/{price}")]
        public async Task<IActionResult> GetByPrice(decimal price)
        {
            return Ok(await _repo.GetByPrice(price));
        }

        [HttpGet("bycolorandprice/{color}/{price}")]
        public async Task<IActionResult> GetByColorAndPrice(string color, decimal price)
        {
            return Ok(await _repo.GetByColorAndPrice(color, price));
        }

        [HttpGet("redoryellow")]
        public async Task<IActionResult> GetRedOrYellow()
        {
            return Ok(await _repo.GetRedOrYellow());
        }

        [HttpGet("redoryellowandprice/{price}")]
        public async Task<IActionResult> GetRedOrYellowAndPrice(decimal price)
        {
            return Ok(await _repo.GetRedOrYellowAndPrice(price));
        }
    }
}