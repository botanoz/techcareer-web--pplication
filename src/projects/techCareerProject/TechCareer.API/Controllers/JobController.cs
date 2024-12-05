using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Dtos.Job;
using Core.CrossCuttingConcerns.Serilog;

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

        // Get all jobs
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {

            var jobs = await _jobService.GetListAsync(withDeleted: includeDeleted);
            return Ok(jobs);
        }

        // Get job by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetAsync(job => job.Id == id);
            return Ok(job);
        }

        // Add a new job
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JobAddRequestDto jobAddRequestDto)
        {
            var addedJob = await _jobService.AddAsync(jobAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedJob.Id }, addedJob);
        }

        // Update an existing job
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobUpdateRequestDto jobUpdateRequestDto)
        {
            jobUpdateRequestDto.Id = id;

            var updatedJob = await _jobService.UpdateAsync(jobUpdateRequestDto);
            return Ok(updatedJob);
        }

        // Delete a job
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {

            var deletedJob = await _jobService.DeleteAsync(
                    new JobRequestDto { Id = id }, permanent);
            return Ok(deletedJob);

        }

        // Get paginated jobs
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var paginatedJobs = await _jobService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(paginatedJobs);
        }
    }
}
