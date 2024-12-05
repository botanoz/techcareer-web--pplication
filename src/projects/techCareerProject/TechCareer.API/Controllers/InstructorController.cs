using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using TechCareer.Service.Abstracts;
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

        // Tüm eğitmenleri getirir
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var instructors = await _instructorService.GetListAsync(withDeleted: includeDeleted);
            return Ok(instructors);
        }

        // ID ile eğitmen getirir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var instructor = await _instructorService.GetAsync(x => x.Id == id);

            if (instructor == null)
                return NotFound(new { Message = "Instructor not found." });

            return Ok(instructor);
        }

        // Yeni eğitmen ekler
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InstructorAddRequestDto instructorAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedInstructor = await _instructorService.AddAsync(instructorAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedInstructor.Id }, addedInstructor);
        }

        // Eğitmen günceller
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InstructorUpdateRequestDto instructorUpdateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedInstructor = await _instructorService.UpdateAsync(instructorUpdateRequestDto);
                return Ok(updatedInstructor);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Eğitmen siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deletedInstructor = await _instructorService.DeleteAsync(
                    new InstructorRequestDto { Id = id }, permanent);

                return Ok(deletedInstructor);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Sayfalandırılmış eğitmen listesi
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
