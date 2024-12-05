using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Models.Dtos.TypeOfWork;

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
        public async Task<IActionResult> CreateTypeOfWork([FromBody] TypeOfWorkAddRequestDto typeOfWorkAddRequestDto)
        {
            if (typeOfWorkAddRequestDto == null || string.IsNullOrWhiteSpace(typeOfWorkAddRequestDto.Name))
            {
                return BadRequest("Type of work name is required.");
            }

            var typeOfWork = new TypOfWork
            {
                Name = typeOfWorkAddRequestDto.Name
            };

            var createdTypeOfWork = await _typOfWorkRepository.AddAsync(typeOfWork);

            return CreatedAtAction(nameof(GetTypOfWork), new { id = createdTypeOfWork.Id }, createdTypeOfWork);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeOfWork(int id, [FromBody] TypeOfWorkUpdateRequestDto typeOfWorkUpdateRequestDto)
        {
            if (typeOfWorkUpdateRequestDto == null || string.IsNullOrWhiteSpace(typeOfWorkUpdateRequestDto.Name))
            {
                return BadRequest("Type of work data is invalid.");
            }

            var existingTypeOfWork = await _typOfWorkRepository.GetAsync(t => t.Id == id);
            if (existingTypeOfWork == null)
            {
                return NotFound($"Type of work with id {id} not found.");
            }

            existingTypeOfWork.Name = typeOfWorkUpdateRequestDto.Name;

            var updatedTypeOfWork = await _typOfWorkRepository.UpdateAsync(existingTypeOfWork);

            return Ok(updatedTypeOfWork);
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
