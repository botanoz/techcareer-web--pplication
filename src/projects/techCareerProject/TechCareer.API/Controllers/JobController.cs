using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Job;
using TechCareer.Models.Dtos.Event;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var jobs = await _jobService.GetListAsync(
                withDeleted: includeDeleted);
            return Ok(jobs);
        }

 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetAsync(x => x.Id == id);
            if (job == null)
                return NotFound(new { Message = "Job not found." });

            return Ok(job);
        }

 
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JobAddRequestDto jobAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedJob = await _jobService.AddAsync(jobAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedJob.Id }, addedJob);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] JobUpdateRequestDto jobUpdateRequestDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var UpdatedEvent = await _jobService.UpdateAsync(jobUpdateRequestDto);
                return Ok(UpdatedEvent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Event not found." });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var job = new Job { Id = id };
                var deletedJob = await _jobService.DeleteAsync(job, permanent);
                return Ok(deletedJob);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Job not found." });
            }
        }


        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var result = await _jobService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(result);
        }
    }
}

