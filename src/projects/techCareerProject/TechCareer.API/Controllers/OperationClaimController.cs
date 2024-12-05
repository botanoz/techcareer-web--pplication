﻿using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Dtos.OperationClaim;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        // Get all operation claims
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var operationClaims = await _operationClaimService.GetListAsync(withDeleted: includeDeleted);
            return Ok(operationClaims);
        }

        // Get operation claim by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var operationClaim = await _operationClaimService.GetAsync(x => x.Id == id);

            if (operationClaim == null)
                return NotFound(new { Message = "OperationClaim not found." });

            return Ok(operationClaim);
        }

        // Add a new operation claim
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OperationClaimAddRequestDto operationClaimAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedOperationClaim = await _operationClaimService.AddAsync(operationClaimAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedOperationClaim.Id }, addedOperationClaim);
        }

        // Update an existing operation claim
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OperationClaimUpdateRequestDto operationClaimUpdateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the correct ID is passed
            operationClaimUpdateRequestDto.Id = id;

            try
            {
                var updatedOperationClaim = await _operationClaimService.UpdateAsync(operationClaimUpdateRequestDto);
                return Ok(updatedOperationClaim);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Delete an operation claim
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deletedOperationClaim = await _operationClaimService.DeleteAsync(new OperationClaimRequestDto { Id = id }, permanent);
                return Ok(deletedOperationClaim);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Get paginated operation claims
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var paginatedOperationClaims = await _operationClaimService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(paginatedOperationClaims);
        }
    }
}
