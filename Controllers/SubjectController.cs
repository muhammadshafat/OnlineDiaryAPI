using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectRepository _repo;

        public SubjectController(SubjectRepository repo)
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

        [HttpGet("Class/{id}")]
        
            public async Task<IActionResult> getSubjectByClass(int id)
            {
                return Ok(await _repo.getSubjectByClass(id));
            }
        
        [HttpPost]
        public async Task<IActionResult> Add(SubjectDto dto)
        {
            await _repo.Add(dto);
            return Ok("Subject Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(SubjectDto dto)
        {
            await _repo.Update(dto);
            return Ok("Subject Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Subject Deleted");
        }
    }
}