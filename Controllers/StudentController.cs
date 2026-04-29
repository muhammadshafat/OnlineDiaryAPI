using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _repo;

        public StudentController(StudentRepository repo)
        {
            _repo = repo;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAll();
            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetById(id);
            return Ok(result);
        }

        // INSERT
        [HttpPost]
        public async Task<IActionResult> Add(StudentDto dto)
        {
            await _repo.Add(dto);
            return Ok("Student Added");
        }

        // UPDATE
        [HttpPut]
        public async Task<IActionResult> Update(StudentDto dto)
        {
            await _repo.Update(dto);
            return Ok("Student Updated");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Student Deleted");
        }
    }
}