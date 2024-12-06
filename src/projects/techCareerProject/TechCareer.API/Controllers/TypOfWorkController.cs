using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.TypeOfWork;
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypOfWorkController : ControllerBase
    {
        private readonly ITypOfWorkRepository _typOfWorkRepository;
        private readonly LoggerServiceBase _logger;

        public TypOfWorkController(ITypOfWorkRepository typOfWorkRepository, LoggerServiceBase logger)
        {
            _typOfWorkRepository = typOfWorkRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypOfWorks()
        {
            _logger.Info("GetAllTypOfWorks endpoint started.");
            try
            {
                var typOfWorks = await _typOfWorkRepository.GetListAsync();

                if (typOfWorks == null || typOfWorks.Count == 0)
                {
                    _logger.Warn("No types of work found.");
                    return NotFound("No types of work found.");
                }

                var typOfWorkDtos = typOfWorks.Select(typOfWork => new TypeOfWorkResponseDto
                {
                    Name = typOfWork.Name
                }).ToList();

                _logger.Info($"Retrieved {typOfWorkDtos.Count} types of work.");
                return Ok(typOfWorkDtos);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetAllTypOfWorks: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTypOfWork(int id)
        {
            _logger.Info($"GetTypOfWork endpoint started. ID: {id}");
            try
            {
                var typOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);

                if (typOfWork == null)
                {
                    _logger.Warn($"Type of work with id {id} not found.");
                    return NotFound($"Type of work with id {id} not found.");
                }

                var typOfWorkDto = new TypeOfWorkResponseDto
                {
                    Name = typOfWork.Name
                };

                _logger.Info($"Type of work with id {id} retrieved successfully.");
                return Ok(typOfWorkDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetTypOfWork: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTypeOfWork([FromBody] TypeOfWorkAddRequestDto typeOfWorkAddRequestDto)
        {
            _logger.Info("CreateTypeOfWork endpoint started.");
            if (typeOfWorkAddRequestDto == null || string.IsNullOrWhiteSpace(typeOfWorkAddRequestDto.Name))
            {
                _logger.Warn("Type of work name is required but null or empty was provided.");
                return BadRequest("Type of work name is required.");
            }

            try
            {
                var typeOfWork = new TypOfWork
                {
                    Name = typeOfWorkAddRequestDto.Name
                };

                var createdTypeOfWork = await _typOfWorkRepository.AddAsync(typeOfWork);

                _logger.Info($"Type of work created successfully. ID: {createdTypeOfWork.Id}");
                return CreatedAtAction(nameof(GetTypOfWork), new { id = createdTypeOfWork.Id }, new TypeOfWorkResponseDto
                {
                    Name = typeOfWorkAddRequestDto.Name
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in CreateTypeOfWork: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeOfWork(int id, [FromBody] TypeOfWorkUpdateRequestDto typeOfWorkUpdateRequestDto)
        {
            _logger.Info($"UpdateTypeOfWork endpoint started. ID: {id}");
            if (typeOfWorkUpdateRequestDto == null || string.IsNullOrWhiteSpace(typeOfWorkUpdateRequestDto.Name))
            {
                _logger.Warn("Type of work update data is invalid.");
                return BadRequest("Type of work data is invalid.");
            }

            try
            {
                var existingTypeOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
                if (existingTypeOfWork == null)
                {
                    _logger.Warn($"Type of work with id {id} not found.");
                    return NotFound($"Type of work with id {id} not found.");
                }

                existingTypeOfWork.Name = typeOfWorkUpdateRequestDto.Name;

                var updatedTypeOfWork = await _typOfWorkRepository.UpdateAsync(existingTypeOfWork);

                _logger.Info($"Type of work with id {id} updated successfully.");
                return Ok(new TypeOfWorkResponseDto
                {
                    Name = updatedTypeOfWork.Name
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in UpdateTypeOfWork: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypOfWork(int id)
        {
            _logger.Info($"DeleteTypOfWork endpoint started. ID: {id}");
            try
            {
                var typOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
                if (typOfWork == null)
                {
                    _logger.Warn($"Type of work with id {id} not found.");
                    return NotFound($"Type of work with id {id} not found.");
                }

                var typOfWorkDto = new TypeOfWorkResponseDto
                {
                    Name = typOfWork.Name
                };

                await _typOfWorkRepository.DeleteAsync(typOfWork);
                _logger.Info($"Type of work with id {id} deleted successfully.");

                return Ok(new
                {
                    Message = "Type of work deleted successfully.",
                    DeletedTypeOfWork = typOfWorkDto
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in DeleteTypOfWork: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
