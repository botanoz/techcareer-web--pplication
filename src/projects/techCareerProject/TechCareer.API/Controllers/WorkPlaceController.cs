using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.Models.Dtos.WorkPlace;
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPlaceController : ControllerBase
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;
        private readonly LoggerServiceBase _logger;

        public WorkPlaceController(IWorkPlaceRepository workPlaceRepository, LoggerServiceBase logger)
        {
            _workPlaceRepository = workPlaceRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkPlaces()
        {
            _logger.Info("GetAllWorkPlaces endpoint started.");
            try
            {
                var workPlaces = await _workPlaceRepository.GetListAsync();

                if (workPlaces == null || workPlaces.Count == 0)
                {
                    _logger.Warn("No work places found.");
                    return NotFound("No work places found.");
                }

                var workPlaceDtos = workPlaces.Select(workPlace => new WorkPlaceResponseDto
                {
                    Name = workPlace.Name
                }).ToList();

                _logger.Info($"Retrieved {workPlaceDtos.Count} work places.");
                return Ok(workPlaceDtos);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetAllWorkPlaces: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkPlace(int id)
        {
            _logger.Info($"GetWorkPlace endpoint started. ID: {id}");
            try
            {
                var workPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);

                if (workPlace == null)
                {
                    _logger.Warn($"Work place with id {id} not found.");
                    return NotFound($"Work place with id {id} not found.");
                }

                var workPlaceDto = new WorkPlaceResponseDto
                {
                    Name = workPlace.Name
                };

                _logger.Info($"Work place with id {id} retrieved successfully.");
                return Ok(workPlaceDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetWorkPlace: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkPlace([FromBody] WorkPlaceAddRequestDto workPlaceAddRequestDto)
        {
            _logger.Info("CreateWorkPlace endpoint started.");
            if (workPlaceAddRequestDto == null || string.IsNullOrWhiteSpace(workPlaceAddRequestDto.Name))
            {
                _logger.Warn("Work place name is required but null or empty was provided.");
                return BadRequest("Work place name is required.");
            }

            try
            {
                var workPlace = new WorkPlace
                {
                    Name = workPlaceAddRequestDto.Name
                };

                var createdWorkPlace = await _workPlaceRepository.AddAsync(workPlace);

                _logger.Info($"Work place created successfully. ID: {createdWorkPlace.Id}");
                return CreatedAtAction(nameof(GetWorkPlace), new { id = createdWorkPlace.Id }, new WorkPlaceResponseDto
                {
                    Name = workPlaceAddRequestDto.Name
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in CreateWorkPlace: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkPlace(int id, [FromBody] WorkPlaceUpdateRequestDto workPlaceUpdateRequestDto)
        {
            _logger.Info($"UpdateWorkPlace endpoint started. ID: {id}");
            if (workPlaceUpdateRequestDto == null || string.IsNullOrWhiteSpace(workPlaceUpdateRequestDto.Name))
            {
                _logger.Warn("Work place update data is invalid.");
                return BadRequest("Work place data is invalid.");
            }

            try
            {
                var existingWorkPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
                if (existingWorkPlace == null)
                {
                    _logger.Warn($"Work place with id {id} not found.");
                    return NotFound($"Work place with id {id} not found.");
                }

                existingWorkPlace.Name = workPlaceUpdateRequestDto.Name;

                var updatedWorkPlace = await _workPlaceRepository.UpdateAsync(existingWorkPlace);

                _logger.Info($"Work place with id {id} updated successfully.");
                return Ok(new WorkPlaceResponseDto
                {
                    Name = updatedWorkPlace.Name
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in UpdateWorkPlace: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkPlace(int id)
        {
            _logger.Info($"DeleteWorkPlace endpoint started. ID: {id}");
            try
            {
                var workPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
                if (workPlace == null)
                {
                    _logger.Warn($"Work place with id {id} not found.");
                    return NotFound($"Work place with id {id} not found.");
                }

                var workPlaceDto = new WorkPlaceResponseDto
                {
                    Name = workPlace.Name
                };

                await _workPlaceRepository.DeleteAsync(workPlace);
                _logger.Info($"Work place with id {id} deleted successfully.");

                return Ok(new
                {
                    Message = "Work place deleted successfully.",
                    DeletedWorkPlace = workPlaceDto
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in DeleteWorkPlace: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
