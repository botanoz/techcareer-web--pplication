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
        private readonly LoggerServiceBase _logger;

        public JobController(IJobService jobService, LoggerServiceBase logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        // Get all jobs
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            _logger.Info("Info log: Bu bir test logudur.");
            _logger.Warn("Warn log: Bu bir uyarı mesajıdır.");
            _logger.Error("Error log: Bu bir hata mesajıdır.");
            var jobs = await _jobService.GetListAsync(withDeleted: includeDeleted);
            return Ok(jobs);
        }

        // Get job by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetAsync(job => job.Id == id);

            if (job == null)
                return NotFound(new { Message = "Job not found." });

            return Ok(job);
        }

        // Add a new job
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JobAddRequestDto jobAddRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addedJob = await _jobService.AddAsync(jobAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedJob.Id }, addedJob);
        }

        // Update an existing job
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobUpdateRequestDto jobUpdateRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the correct ID is passed
            jobUpdateRequestDto.Id = id;

            try
            {
                var updatedJob = await _jobService.UpdateAsync(jobUpdateRequestDto);
                return Ok(updatedJob);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // Delete a job
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            try
            {
                var deletedJob = await _jobService.DeleteAsync(
                    new JobRequestDto { Id = id }, permanent);

                return Ok(deletedJob);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
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
