using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly TutorRepository _tutorRepository;

        public TutorController(TutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        // GET: api/Tutor
        [HttpGet]
        public async Task<IActionResult> GetAllTutors()
        {
            var tutors = await _tutorRepository.GetAllAsync();
            return Ok(tutors);
        }

        // GET: api/Tutor/by-class/1
        [HttpGet("by-class/{cid:int}")]
        public async Task<IActionResult> GetTutorsByClass(int cid)
        {
            var tutors = await _tutorRepository.GetByClassAsync(cid);
            return Ok(tutors);
        }

        // GET: api/Tutor/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTutorById(int id)
        {
            var tutor = await _tutorRepository.GetByIdAsync(id);

            if (tutor == null)
            {
                return NotFound("Tutor not found.");
            }

            return Ok(tutor);
        }

        // GET: api/Tutor/1/students
        [HttpGet("{tuid:int}/students")]
        public async Task<IActionResult> GetStudentsByTutor(int tuid)
        {
            var students = await _tutorRepository.GetStudentsByTutorAsync(tuid);
            return Ok(students);
        }

        // POST: api/Tutor
        [HttpPost]
        public async Task<IActionResult> CreateTutor(
            [FromBody] Tutor tutorDto)
        {
            var tutor = await _tutorRepository.CreateAsync(tutorDto);

            return CreatedAtAction(
                nameof(GetTutorById),
                new { id = tutor.Id },
                tutor
            );
        }

        // PUT: api/Tutor/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTutor(
            int id,
            [FromBody] Tutor tutorDto)
        {
            var updated = await _tutorRepository.UpdateAsync(id, tutorDto);

            if (!updated)
            {
                return NotFound("Tutor not found.");
            }

            return Ok("Tutor updated successfully.");
        }

        // DELETE: api/Tutor/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            var deleted = await _tutorRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Tutor not found.");
            }

            return Ok("Tutor deleted successfully.");
        }




    }
}