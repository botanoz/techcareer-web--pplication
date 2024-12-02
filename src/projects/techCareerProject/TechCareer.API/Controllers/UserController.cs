using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var Users = await _UserService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(Users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var User = await _UserService.GetAsync(x => x.Id == id);
            if (User == null)
                return NotFound(new { Message = "User not found." });

            return Ok(User);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User User)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedUser = await _UserService.AddAsync(User);
            return CreatedAtAction(nameof(GetById), new { id = addedUser.Id }, addedUser);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User User)
        {
            if (id != User.Id)
                return BadRequest(new { Message = "User ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUser = await _UserService.UpdateAsync(User);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found." });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var User = new User { Id = id };
                var deletedUser = await _UserService.DeleteAsync(User, permanent);
                return Ok(deletedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found." });
            }
        }


        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _UserService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}
