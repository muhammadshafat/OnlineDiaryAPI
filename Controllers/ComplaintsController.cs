using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly ComplaintRepository _repo;

        public ComplaintsController(ComplaintRepository repo)
        {
            _repo = repo;
        }
        // GET ALL PENDING COMPLAINTS
        [HttpGet("principalcomplaint")]
        public async Task<IActionResult> GetPendingComplaints()
        {
            var complaints = await _repo.GetPendingComplaints();

            if (complaints == null || !complaints.Any())
                return NotFound("No pending complaints found.");

            return Ok(complaints);
        }

        // GET PENDING COMPLAINTS BY TEACHER
        [HttpGet("teachercomplaint/{tid}")]
        public async Task<IActionResult> GetPendingComplaintsByTeacher(int tid)
        {
            var complaints = await _repo.GetPendingComplaintsByTeacher(tid);

            if (complaints == null || !complaints.Any())
                return NotFound("No pending complaints found for this teacher.");

            return Ok(complaints);
        }  // CREATE COMPLAINT
        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] ComplaintCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                return BadRequest("Description is required.");

            var result = await _repo.CreateComplaint(dto);

            if (!result)
                return StatusCode(500, "Failed to submit complaint.");

            return Ok(new
            {
                success = true,
                message = "Complaint submitted successfully"
            });
        }

        // GET ALL COMPLAINTS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAll();
            return Ok(data);
        }

        // GET BY STUDENT ID
        [HttpGet("student/{sid}")]
        public async Task<IActionResult> GetByStudent(int sid)
        {
            var data = await _repo.GetByStudent(sid);
            return Ok(data);
        }

        // DELETE COMPLAINT
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.Delete(id);

            if (!result)
                return NotFound("Complaint not found.");

            return Ok(new
            {
                success = true,
                message = "Complaint deleted successfully"
            });
        }
    }
}