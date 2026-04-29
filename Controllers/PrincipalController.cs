using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.DTOs;
using OnlineDiaryAPI.Repositories;

namespace OnlineDiaryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly PrincipalRepository _repo;

        public PrincipalController(PrincipalRepository repo)
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
        public async Task<IActionResult> Add(PrincipalDto dto)
        {
            await _repo.Add(dto);
            return Ok("Principal Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(PrincipalDto dto)
        {
            await _repo.Update(dto);
            return Ok("Principal Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.Delete(id);
            return Ok("Principal Deleted");
        }
    }
}