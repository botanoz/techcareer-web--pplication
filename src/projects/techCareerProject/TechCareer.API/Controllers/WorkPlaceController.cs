using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;

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
            return Ok(workPlaces);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkPlace(int id)
        {
            var workPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
            if (workPlace == null)
            {
                return NotFound($"Work place with id {id} not found.");
            }
            return Ok(workPlace);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkPlace([FromBody] WorkPlace workPlace)
        {
            if (workPlace == null)
            {
                return BadRequest("Work place data is required.");
            }

            var createdWorkPlace = await _workPlaceRepository.AddAsync(workPlace);
            return CreatedAtAction(nameof(GetWorkPlace), new { id = createdWorkPlace.Id }, createdWorkPlace);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkPlace(int id, [FromBody] WorkPlace workPlace)
        {
            if (workPlace == null || workPlace.Id != id)
            {
                return BadRequest("Work place data is invalid.");
            }

            var existingWorkPlace = await _workPlaceRepository.GetAsync(w => w.Id == id);
            if (existingWorkPlace == null)
            {
                return NotFound($"Work place with id {id} not found.");
            }

            var updatedWorkPlace = await _workPlaceRepository.UpdateAsync(workPlace);
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

            await _workPlaceRepository.DeleteAsync(workPlace);
            return NoContent(); 
        }
    }
}
