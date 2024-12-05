using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using TechCareer.Models.Dtos.OperationClaim;

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
        public async Task<IActionResult> AddOperationClaim([FromBody] OperationClaimAddRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var operationClaim = new OperationClaim
                {
                    Name = requestDto.Name
                };

                var addedClaim = await _OperationClaimService.AddAsync(operationClaim);
                return CreatedAtAction(nameof(GetById), new { id = addedClaim.Id }, addedClaim);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the operation claim.", Details = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OperationClaimUpdateRequestDto operationClaimUpdateRequestDto)
        {
            if (id != operationClaimUpdateRequestDto.Id)
                return BadRequest(new { Message = "Operation claim ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedClaim = await _OperationClaimService.UpdateAsync(operationClaimUpdateRequestDto);

                return Ok(updatedClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Operation claim not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the operation claim.", Details = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deleteRequestDto = new OperationClaimDeleteRequestDto
                {
                    Id = id,
                    Permanent = permanent
                };

                var deletedClaim = await _OperationClaimService.DeleteAsync(deleteRequestDto);

                return Ok(deletedClaim);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Operation claim not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the operation claim.", Details = ex.Message });
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

