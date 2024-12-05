using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.Models.Dtos.WorkPlace;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPlaceController : ControllerBase
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;

        public WorkPlaceController(IWorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkPlaces()
        {
            var workPlaces = await _workPlaceRepository.GetListAsync();

            if (workPlaces == null || workPlaces.Count == 0)
            {
                return NotFound("No work places found.");
            }

            var workPlaceDtos = workPlaces.Select(workPlace => new WorkPlaceResponseDto
            {
                Name = workPlace.Name
            }).ToList();

            return Ok(workPlaceDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkPlace(int id)
        {
            var workPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);

            if (workPlace == null)
            {
                return NotFound($"Work place with id {id} not found.");
            }

            var workPlaceDto = new WorkPlaceResponseDto
            {
                Name = workPlace.Name
            };

            return Ok(workPlaceDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateWorkPlace([FromBody] WorkPlaceAddRequestDto workPlaceAddRequestDto)
        {
            if (workPlaceAddRequestDto == null || string.IsNullOrWhiteSpace(workPlaceAddRequestDto.Name))
            {
                return BadRequest("Work place name is required.");
            }

            var workPlace = new WorkPlace
            {
                Name = workPlaceAddRequestDto.Name
            };

            var createdWorkPlace = await _workPlaceRepository.AddAsync(workPlace);

            return CreatedAtAction(nameof(GetWorkPlace), new { id = createdWorkPlace.Id }, createdWorkPlace);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkPlace(int id, [FromBody] WorkPlaceUpdateRequestDto workPlaceUpdateRequestDto)
        {
            if (workPlaceUpdateRequestDto == null || string.IsNullOrWhiteSpace(workPlaceUpdateRequestDto.Name))
            {
                return BadRequest("Work place data is invalid.");
            }

            var existingWorkPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
            if (existingWorkPlace == null)
            {
                return NotFound($"Work place with id {id} not found.");
            }

            existingWorkPlace.Name = workPlaceUpdateRequestDto.Name;

            var updatedWorkPlace = await _workPlaceRepository.UpdateAsync(existingWorkPlace);

            return Ok(updatedWorkPlace);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkPlace(int id)
        {
            var workPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
            if (workPlace == null)
            {
                return NotFound($"Work place with id {id} not found.");
            }

            var workPlaceDto = new WorkPlaceResponseDto
            {
                Name = workPlace.Name
            };

            await _workPlaceRepository.DeleteAsync(workPlace);

            return Ok(new
            {
                Message = "Work place deleted successfully.",
                DeletedWorkPlace = workPlaceDto
            });
        }

    }
}
