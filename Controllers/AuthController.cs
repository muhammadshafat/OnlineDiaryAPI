using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _repo;

        public AuthController(AuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("principal-login")]
        public async Task<IActionResult> PrincipalLogin(LoginDto dto)
        {
            var result = await _repo.PrincipalLogin(dto.EmailOrName, dto.Password);

            if (result == null)
                return BadRequest("Invalid Login");

            return Ok(result);
        }

        [HttpPost("Tutor-login")]
        public async Task<IActionResult> TutorLogin(LoginDto dto)
        {
            var result = await _repo.TutorLogin(dto.EmailOrName, dto.Password);

            if (result == null)
                return BadRequest("Invalid Login");

            return Ok(result);
        }

        [HttpPost("teacher-login")]
        public async Task<IActionResult> TeacherLogin(LoginDto dto)
        {
            var result = await _repo.TeacherLogin(dto.EmailOrName, dto.Password);

            if (result == null)
                return BadRequest("Invalid Login");

            return Ok(result);
        }

        [HttpPost("parent-login")]
        public async Task<IActionResult> ParentLogin(LoginDto dto)
        {
            var result = await _repo.ParentLogin(dto.EmailOrName, dto.Password);

            if (result == null)
                return BadRequest("Invalid Login");

            return Ok(result);
        }
    }
}