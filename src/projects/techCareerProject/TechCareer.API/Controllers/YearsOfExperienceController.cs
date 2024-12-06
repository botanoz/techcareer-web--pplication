using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.Models.Dtos.YearsOfExperience;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearsOfExperienceController : ControllerBase
    {
        private readonly IYearsOfExperienceRepository _yearsOfExperienceRepository;
        private readonly ILogger<YearsOfExperienceController> _logger;

        public YearsOfExperienceController(
            IYearsOfExperienceRepository yearsOfExperienceRepository,
            ILogger<YearsOfExperienceController> logger)
        {
            _yearsOfExperienceRepository = yearsOfExperienceRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllYearsOfExperience()
        {
            _logger.LogInformation("Fetching all years of experience.");
            var yearsOfExperience = await _yearsOfExperienceRepository.GetListAsync();

            if (yearsOfExperience == null || yearsOfExperience.Count == 0)
            {
                _logger.LogWarning("No years of experience found.");
                return NotFound("No years of experience found.");
            }

            var yearsOfExperienceDtos = yearsOfExperience.Select(yoe => new YearsOfExperienceResponseDto
            {
                Name = yoe.Name
            }).ToList();

            _logger.LogInformation("{Count} years of experience found.", yearsOfExperienceDtos.Count);
            return Ok(yearsOfExperienceDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetYearsOfExperience(int id)
        {
            _logger.LogInformation("Fetching years of experience with ID: {Id}", id);
            var yearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (yearsOfExperience == null)
            {
                _logger.LogWarning("Years of experience with ID: {Id} not found.", id);
                return NotFound($"Years of experience with id {id} not found.");
            }

            var yearsOfExperienceDto = new YearsOfExperienceResponseDto
            {
                Name = yearsOfExperience.Name
            };

            _logger.LogInformation("Years of experience with ID: {Id} found.", id);
            return Ok(yearsOfExperienceDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateYearsOfExperience([FromBody] YearsOfExperienceAddRequestDto yearsOfExperienceAddRequestDto)
        {
            if (yearsOfExperienceAddRequestDto == null || string.IsNullOrWhiteSpace(yearsOfExperienceAddRequestDto.Name))
            {
                _logger.LogWarning("Invalid years of experience data received.");
                return BadRequest("Years of experience name is required.");
            }

            var yearsOfExperience = new YearsOfExperience
            {
                Name = yearsOfExperienceAddRequestDto.Name
            };

            _logger.LogInformation("Creating a new years of experience: {Name}", yearsOfExperience.Name);
            var createdYearsOfExperience = await _yearsOfExperienceRepository.AddAsync(yearsOfExperience);

            _logger.LogInformation("Years of experience created with ID: {Id}", createdYearsOfExperience.Id);
            return CreatedAtAction(nameof(GetYearsOfExperience), new { id = createdYearsOfExperience.Id }, new YearsOfExperienceResponseDto
            {
                Name = yearsOfExperienceAddRequestDto.Name
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateYearsOfExperience(int id, [FromBody] YearsOfExperienceUpdateRequestDto requestDto)
        {
            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.Name))
            {
                _logger.LogWarning("Invalid data for updating years of experience with ID: {Id}", id);
                return BadRequest("Years of experience data is invalid.");
            }

            _logger.LogInformation("Updating years of experience with ID: {Id}", id);
            var existingYearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (existingYearsOfExperience == null)
            {
                _logger.LogWarning("Years of experience with ID: {Id} not found.", id);
                return NotFound($"Years of experience with id {id} not found.");
            }

            existingYearsOfExperience.Name = requestDto.Name;
            var updatedYearsOfExperience = await _yearsOfExperienceRepository.UpdateAsync(existingYearsOfExperience);

            _logger.LogInformation("Years of experience with ID: {Id} successfully updated.", id);
            return Ok(new YearsOfExperienceResponseDto
            {
                Name = updatedYearsOfExperience.Name
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteYearsOfExperience(int id)
        {
            _logger.LogInformation("Deleting years of experience with ID: {Id}", id);
            var yearsOfExperience = await _yearsOfExperienceRepository.GetAsync(y => y.Id == id);
            if (yearsOfExperience == null)
            {
                _logger.LogWarning("Years of experience with ID: {Id} not found.", id);
                return NotFound($"Years of experience with id {id} not found.");
            }

            var yearsOfExperienceDto = new YearsOfExperienceResponseDto
            {
                Name = yearsOfExperience.Name
            };

            await _yearsOfExperienceRepository.DeleteAsync(yearsOfExperience);

            _logger.LogInformation("Years of experience with ID: {Id} deleted successfully.", id);
            return Ok(new
            {
                Message = "Years of experience deleted successfully.",
                DeletedYearsOfExperience = yearsOfExperienceDto
            });
        }
    }
}
