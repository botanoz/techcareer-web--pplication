using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Category;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var Categorys = await _CategoryService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(Categorys);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var Category = await _CategoryService.GetAsync(x => x.Id == id);
            //if (Category == null)
            //    return NotFound(new { Message = "Category not found." });

            //return Ok(Category);

            return null;
        }

    
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryAddRequestDto categoryAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedCategory = await _CategoryService.AddAsync(categoryAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }

  
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequestDto categoryUpdateRequestDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCategory = await _CategoryService.UpdateAsync(categoryUpdateRequestDto);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Category not found." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var Category = new Category { Id = id };
                var deletedCategory = await _CategoryService.DeleteAsync(Category, permanent);
                return Ok(deletedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Category not found." });
            }
        }

      
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _CategoryService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}
