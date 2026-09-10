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
        [HttpGet("class/{cid}")]
        public async Task<IActionResult> GetTeachersByClass(int cid)
        {
            try
            {
                var teachers = await _repo.GetTeachersByClass(cid);

                if (teachers == null || !teachers.Any())
                {
                    return NotFound(new
                    {
                        message = "No teachers found for this class."
                    });
                }

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching teachers.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("subject/{cid}/{tid}")]
        public async Task<IActionResult> GetsubjectByteacher(int cid, int tid)
        {
            try
            {
                var teachers = await _repo.GetsubjectByteacher(cid, tid);

                if (teachers == null || !teachers.Any())
                {
                    return NotFound(new
                    {
                        message = "No teachers found for this class."
                    });
                }

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching teachers.",
                    error = ex.Message
                });
            }
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