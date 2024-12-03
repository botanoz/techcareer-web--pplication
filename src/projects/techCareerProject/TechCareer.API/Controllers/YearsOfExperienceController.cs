using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearsOfExperienceController : ControllerBase
    {
        private readonly IYearsOfExperienceRepository _yearsOfExperienceRepository;

        public YearsOfExperienceController(IYearsOfExperienceRepository yearsOfExperienceRepository)
        {
            _yearsOfExperienceRepository = yearsOfExperienceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllYearsOfExperience()
        {
            var yearsOfExperience = await _yearsOfExperienceRepository.GetListAsync();
            if (yearsOfExperience == null || yearsOfExperience.Count == 0)
            {
                return NotFound("No years of experience found.");
            }
            return Ok(yearsOfExperience);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetYearsOfExperience(int id)
        {
            var yearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (yearsOfExperience == null)
            {
                return NotFound($"Years of experience with id {id} not found.");
            }
            return Ok(yearsOfExperience);
        }

        [HttpPost]
        public async Task<IActionResult> CreateYearsOfExperience([FromBody] YearsOfExperience yearsOfExperience)
        {
            if (yearsOfExperience == null)
            {
                return BadRequest("Years of experience data is required.");
            }

            var createdYearsOfExperience = await _yearsOfExperienceRepository.AddAsync(yearsOfExperience);
            return CreatedAtAction(nameof(GetYearsOfExperience), new { id = createdYearsOfExperience.Id }, createdYearsOfExperience);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateYearsOfExperience(int id, [FromBody] YearsOfExperience yearsOfExperience)
        {
            if (yearsOfExperience == null || yearsOfExperience.Id != id)
            {
                return BadRequest("Years of experience data is invalid.");
            }

            var existingYearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (existingYearsOfExperience == null)
            {
                return NotFound($"Years of experience with id {id} not found.");
            }

            var updatedYearsOfExperience = await _yearsOfExperienceRepository.UpdateAsync(yearsOfExperience);
            return Ok(updatedYearsOfExperience);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteYearsOfExperience(int id)
        {
            var yearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (yearsOfExperience == null)
            {
                return NotFound($"Years of experience with id {id} not found.");
            }

            await _yearsOfExperienceRepository.DeleteAsync(yearsOfExperience);
            return NoContent(); 
        }
    }
}
