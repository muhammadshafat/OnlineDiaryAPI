using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentController : ControllerBase
    {
        private readonly ParentRepository _repo;

        public ParentController(ParentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _repo.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Add(ParentDto dto)
        {
            await _repo.Add(dto);
            return Ok("Parent Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(ParentDto dto)
        {
            await _repo.Update(dto);
            return Ok("Parent Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Parent Deleted");
        }
    }
}