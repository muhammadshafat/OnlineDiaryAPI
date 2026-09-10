using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GardenerController : ControllerBase
    {
        private readonly GardenerRepository _repo;

        public GardenerController(GardenerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Add(GardenerDto dto)
        {
            await _repo.Add(dto);
            return Ok("Gardener Added");
        }
    }
}