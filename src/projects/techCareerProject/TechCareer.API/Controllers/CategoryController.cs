using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Dtos.Category;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Get all categories
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var categories = await _categoryService.GetListAsync(withDeleted: includeDeleted);
            return Ok(categories);
        }

        // Get category by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.FindCategoryAsync(new CategoryRequestDto { Id = id });

            if (category == null)
                return NotFound(new { Message = "Category not found." });

            return Ok(category);
        }

        // Add a new category
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryAddRequestDto categoryAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedCategory = await _categoryService.AddAsync(categoryAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }

        // Update an existing category
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequestDto categoryUpdateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the correct ID is passed
            categoryUpdateRequestDto.Id = id;

            try
            {
                var updatedCategory = await _categoryService.UpdateAsync(categoryUpdateRequestDto);
                return Ok(updatedCategory);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Delete a category
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deletedCategory = await _categoryService.DeleteAsync(
                    new CategoryRequestDto { Id = id }, permanent);

                return Ok(deletedCategory);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Get paginated categories
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var paginatedCategories = await _categoryService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(paginatedCategories);
        }
    }
}
