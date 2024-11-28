using Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {

        [HttpGet("getAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetListAsync();
            return Ok(categories);
        }


        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory()
        {
            List<Event> testEvent = new List<Event>();
            Event evnt = new Event()
            {
                ApplicationDeadline = DateTime.Now,
                Category = null,
                CategoryId = 1,
                CreatedDate = DateTime.Now,
                DeletedDate = DateTime.Now,
                Description = "Description",
                EndDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ImageUrl = "image string",
                ParticipationText = "participation text",
                StartDate = DateTime.Now,
                Title = "Title",
                UpdatedDate = DateTime.Now
            };

            testEvent.Add(evnt);

            Category testCategory = new Category
            {
                CreatedDate = DateTime.Now,
                DeletedDate = DateTime.Now,
                Events = testEvent,
                Name = "Test",
                UpdatedDate = DateTime.Now
            };

            var result = await _categoryService.AddAsync(testCategory);

            return Ok(result);

        }
    }

}
