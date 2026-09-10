using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UpdateDiaryController : ControllerBase
    {
        // GET SUBJECTS OF TEACHER BASED ON CLASS
        [HttpGet("teacher-subjects/{tid}/{cid}")]
        public async Task<IActionResult> GetSubjectsByTeacher(int tid, int cid)
        {
            var data = await _repo.GetSubjectsByTeacher(tid, cid);
            if (data == null || !data.Any())
                return NotFound("No data found");
            return Ok(data);
        }
        // GET CLASSES OF LOGGED-IN TEACHER
        [HttpGet("teacher-classes/{tid}")]
        public async Task<IActionResult> GetClassesByTeacher(int tid)
        {
            var data = await _repo.GetClassesByTeacher(tid);
            if (data == null || !data.Any())
                return NotFound("No data found");
            return Ok(data);
        }
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
        public async Task<IActionResult> UpdateDiary([FromBody] UpdateDiaryDto dto)
        {
            try
            {
                var message = await _repo.UpdateDiary(dto);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //  [HttpDelete("{id}")]
        /*    public async Task<IActionResult> Delete(int id)
            {
                await _repo.Delete(id);
                return Ok("Deleted");
            }*/

        [HttpGet("parent-view-diary/{sid}/{category}")]
        public async Task<IActionResult> parentViewDiary(int sid, string category)
        {
            var data = await _repo.parentViewDiary(sid, category);
            return Ok(data);
        }
        [HttpPost("add-note")]
        public async Task<IActionResult> AddNote([FromBody] StdDiaryDto dto)
        {
            var did = await _repo.AddNoteDiary(dto);
            return Ok(new { message = "Note added", did });
        }
    }
}