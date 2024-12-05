using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Instructor;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var instructors = await _instructorService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(instructors);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var instructor = await _instructorService.GetAsync(x => x.Id == id);
            if (instructor == null)
                return NotFound(new { Message = "Instructor not found." });

            return Ok(instructor);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InstructorAddRequestDto instructorAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructor = new Instructor
            {
                Name = instructorAddRequestDto.Name,
                About = instructorAddRequestDto.About,

            };

            var addedInstructor = await _instructorService.AddAsync(instructor);
            return CreatedAtAction(nameof(GetById), new { id = addedInstructor.Id }, addedInstructor);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InstructorUpdateRequestDto instructorUpdateRequestDto)
        {
            if (instructorUpdateRequestDto == null)
                return BadRequest(new { Message = "Instructor data is required." });

            var existingInstructor = await _instructorService.GetByIdAsync(id);
            if (existingInstructor == null)
                return NotFound(new { Message = "Instructor not found." });

            existingInstructor.Name = instructorUpdateRequestDto.Name ?? existingInstructor.Name;
            existingInstructor.About = instructorUpdateRequestDto.About ?? existingInstructor.About;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedInstructor = await _instructorService.UpdateAsync(existingInstructor);
                return Ok(updatedInstructor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Instructor not found." });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deleteRequest = new InstructorDeleteRequestDto
                {
                    Id = id,
                    Permanent = permanent
                };

                var deletedInstructor = await _instructorService.DeleteAsync(deleteRequest);

                return Ok(deletedInstructor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Instructor not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the instructor.", Details = ex.Message });
            }
        }



        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _instructorService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}

