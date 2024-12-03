using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypOfWorkController : ControllerBase
    {
        private readonly ITypOfWorkRepository _typOfWorkRepository;

        public TypOfWorkController(ITypOfWorkRepository typOfWorkRepository)
        {
            _typOfWorkRepository = typOfWorkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypOfWorks()
        {
            var typOfWorks = await _typOfWorkRepository.GetListAsync();
            if (typOfWorks == null || typOfWorks.Count == 0)
            {
                return NotFound("No types of work found.");
            }
            return Ok(typOfWorks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTypOfWork(int id)
        {
            var typOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
            if (typOfWork == null)
            {
                return NotFound($"Type of work with id {id} not found.");
            }
            return Ok(typOfWork);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypOfWork([FromBody] TypOfWork typOfWork)
        {
            if (typOfWork == null)
            {
                return BadRequest("Type of work data is required.");
            }

            var createdTypOfWork = await _typOfWorkRepository.AddAsync(typOfWork);
            return CreatedAtAction(nameof(GetTypOfWork), new { id = createdTypOfWork.Id }, createdTypOfWork);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypOfWork(int id, [FromBody] TypOfWork typOfWork)
        {
            if (typOfWork == null || typOfWork.Id != id)
            {
                return BadRequest("Type of work data is invalid.");
            }

            var existingTypOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
            if (existingTypOfWork == null)
            {
                return NotFound($"Type of work with id {id} not found.");
            }

            var updatedTypOfWork = await _typOfWorkRepository.UpdateAsync(typOfWork);
            return Ok(updatedTypOfWork);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypOfWork(int id)
        {
            var typOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
            if (typOfWork == null)
            {
                return NotFound($"Type of work with id {id} not found.");
            }

            await _typOfWorkRepository.DeleteAsync(typOfWork);
            return NoContent(); 
        }
    }
}
