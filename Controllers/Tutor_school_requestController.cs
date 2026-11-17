
using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tutor_school_requestController : ControllerBase
    {
        private readonly Tutor_school_requestRepository _repo;

        public Tutor_school_requestController(
            Tutor_school_requestRepository repo)
        {
            _repo = repo;
        }

        // 1. CREATE MULTIPLE COURSE REQUESTS
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Tutor_school_requestDto requestDto)
        {
            try
            {
                var result = await _repo.CreateAsync(requestDto);

                return Ok(new
                {
                    message = "Tutor school requests added successfully.",
                    success = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error creating tutor school request.",
                    error = ex.Message
                });
            }
        }

        // 2. GET REQUESTS BY SCHOOL ID
        [HttpGet("school/{schoolId:int}/{status}")]
public async Task<IActionResult> GetBySchoolId(
    int schoolId,
    string status)
{
    try
    {
        if (schoolId <= 0)
            return BadRequest("Invalid School ID.");

        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");

        var allowedStatuses = new[] { "pending", "accept", "reject" };

        if (!allowedStatuses.Contains(status.ToLower()))
            return BadRequest("Invalid status.");

        var requests = await _repo.GetBySchoolIdAsync(
            schoolId,
            status.ToLower()
        );

        return Ok(requests);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Error retrieving school requests.",
            error = ex.Message
        });
    }
}

        // 3. GET REQUESTS BY TUTOR ID
        [HttpGet("tutor/{tutorId:int}")]
        public async Task<IActionResult> GetByTutorId(int tutorId)
        {
            try
            {
                if (tutorId <= 0)
                    return BadRequest("Invalid Tutor ID.");

                var requests = await _repo.GetByTutorIdAsync(tutorId);

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving tutor requests.",
                    error = ex.Message
                });
            }
        }
        [HttpPost("school/status/{id:int}/{status}")]
public async Task<IActionResult> UpdateSchoolTutorRequestStatus(
    int id,
    string status)
{
    try
    {
        if (id <= 0)
            return BadRequest("Invalid request ID.");

        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("Status is required.");

        status = status.Trim().ToLower();

        if (status != "accept" && status != "reject")
            return BadRequest("Status must be accept or reject.");

        var updated = await _repo.UpdateSchoolTutorRequestStatusAsync(
            id, status);

        if (!updated)
            return NotFound("Tutor request not found.");

        return Ok(new
        {
            message = "Tutor request status updated successfully.",
            id = id,
            status = status
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "Error updating tutor request status.",
            error = ex.Message
        });
    }
}
    }
}
