using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimController : ControllerBase
    {
        private readonly IUserOperationClaimService _UserOperationClaimService;

        public UserOperationClaimController(IUserOperationClaimService UserOperationClaimService)
        {
            _UserOperationClaimService = UserOperationClaimService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var UserOperationClaims = await _UserOperationClaimService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(UserOperationClaims);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var UserOperationClaim = await _UserOperationClaimService.GetAsync(x => x.Id == id);
            if (UserOperationClaim == null)
                return NotFound(new { Message = "UserOperationClaim not found." });

            return Ok(UserOperationClaim);
        }

  
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserOperationClaim UserOperationClaim)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedUserOperationClaim = await _UserOperationClaimService.AddAsync(UserOperationClaim);
            return CreatedAtAction(nameof(GetById), new { id = addedUserOperationClaim.Id }, addedUserOperationClaim);
        }

   
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserOperationClaim UserOperationClaim)
        {
            if (id != UserOperationClaim.Id)
                return BadRequest(new { Message = "UserOperationClaim ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUserOperationClaim = await _UserOperationClaimService.UpdateAsync(UserOperationClaim);
                return Ok(updatedUserOperationClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "UserOperationClaim not found." });
            }
        }

  
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        //{
        //    try
        //    {
        //        var UserOperationClaim = new UserOperationClaim {};
        //        var deletedUserOperationClaim = await _UserOperationClaimService.DeleteAsync(UserOperationClaim, permanent);
        //        return Ok(deletedUserOperationClaim);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound(new { Message = "UserOperationClaim not found." });
        //    }
        //}

  
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _UserOperationClaimService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}
