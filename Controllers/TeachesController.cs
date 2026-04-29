using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachesController : ControllerBase
    {
        private readonly TeachesRepository _repo;

        public TeachesController(TeachesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _repo.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeachesDto dto)
        {
            await _repo.Add(dto);
            return Ok("Teaches Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(TeachesDto dto)
        {
            await _repo.Update(dto);
            return Ok("Teaches Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Teaches Deleted");
        }
    }
}