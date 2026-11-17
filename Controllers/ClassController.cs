using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ClassRepository _repo;

        public ClassController(ClassRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [HttpGet("school/{id}")]
        public async Task<IActionResult> GetByschoolId(int id)
        {
            return Ok(await _repo.GetByschoolId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClassDto dto)
        {
            await _repo.Add(dto);
            return Ok("Class Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(ClassDto dto)
        {
            await _repo.Update(dto);
            return Ok("Class Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Class Deleted");
        }
    }
}