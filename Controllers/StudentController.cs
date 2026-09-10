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

        // =========================
        // GET ALL STUDENTS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAll();
            return Ok(result);
        }

        // =========================
        // GET STUDENT BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetById(id);
            return Ok(result);
        }

        // =========================
        // PARENT CHILDREN
        // =========================
        [HttpGet("parent-children/{pid}")]
        public async Task<IActionResult> GetChildren(int pid)
        {
            var data = await _repo.GetChildrenByParent(pid);
            return Ok(data);
        }

        // =========================
        // STUDENTS BY CLASS
        // =========================
        [HttpGet("class/{cid}")]
        public async Task<IActionResult> GetStudentsByClass(int cid)
        {
            var data = await _repo.GetStudentsByClassAsync(cid);

            if (data == null || !data.Any())
                return NotFound(new { message = "No students found for this class" });

            return Ok(data);
        }

        // =========================
        // ADD STUDENT
        // =========================
        [HttpPost]
        public async Task<IActionResult> Add(StudentDto dto)
        {
            await _repo.Add(dto);
            return Ok(new { message = "Student Added" });
        }

        // =========================
        // UPDATE STUDENT
        // =========================
        [HttpPut]
        public async Task<IActionResult> Update(StudentDto dto)
        {
            await _repo.Update(dto);
            return Ok(new { message = "Student Updated" });
        }

        // =========================
        // DELETE STUDENT
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok(new { message = "Student Deleted" });
        }
    }
}