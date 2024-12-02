using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimController : ControllerBase
    {
        private readonly IOperationClaimService _OperationClaimService;

        public OperationClaimController(IOperationClaimService OperationClaimService)
        {
            _OperationClaimService = OperationClaimService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var OperationClaims = await _OperationClaimService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(OperationClaims);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var OperationClaim = await _OperationClaimService.GetAsync(x => x.Id == id);
            if (OperationClaim == null)
                return NotFound(new { Message = "OperationClaim not found." });

            return Ok(OperationClaim);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OperationClaim OperationClaim)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedOperationClaim = await _OperationClaimService.AddAsync(OperationClaim);
            return CreatedAtAction(nameof(GetById), new { id = addedOperationClaim.Id }, addedOperationClaim);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OperationClaim OperationClaim)
        {
            if (id != OperationClaim.Id)
                return BadRequest(new { Message = "OperationClaim ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedOperationClaim = await _OperationClaimService.UpdateAsync(OperationClaim);
                return Ok(updatedOperationClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "OperationClaim not found." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var OperationClaim = new OperationClaim { Id = id };
                var deletedOperationClaim = await _OperationClaimService.DeleteAsync(OperationClaim, permanent);
                return Ok(deletedOperationClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "OperationClaim not found." });
            }
        }


        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _OperationClaimService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}

