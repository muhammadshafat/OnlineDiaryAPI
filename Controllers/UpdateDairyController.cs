using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateDiaryController : ControllerBase
    {
        private readonly UpdateDiaryRepository _repo;

        public UpdateDiaryController(UpdateDiaryRepository repo)
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
        public async Task<IActionResult> Add(UpdateDiaryDto dto)
        {
            await _repo.Add(dto);
            return Ok("UpdateDiary Added");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Deleted");
        }
    }
}