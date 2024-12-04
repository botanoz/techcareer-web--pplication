using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using Core.Security.Entities;

namespace TechCareer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoEducationController : ControllerBase
    {
        private readonly IVideoEducationService _videoEducationService;

        public VideoEducationController(IVideoEducationService videoEducationService)
        {
            _videoEducationService = videoEducationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int index = 0,
            [FromQuery] int size = 10,
            [FromQuery] bool include = false,
            [FromQuery] bool withDeleted = false,
            [FromQuery] bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var videoEducations = await _videoEducationService.GetPaginateAsync(
                predicate: null,
                orderBy: null,
                include: include,
                index: index,
                size: size,
                withDeleted: withDeleted,
                enableTracking: enableTracking,
                cancellationToken: cancellationToken
            );

            return Ok(videoEducations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var videoEducation = await _videoEducationService.GetAsync(
                predicate: x => x.Id == id,
                include: true,
                withDeleted: false,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            if (videoEducation == null)
            {
                return NotFound();
            }

            return Ok(videoEducation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VideoEducation videoEducation)
        {
            if (videoEducation == null)
            {
                return BadRequest("VideoEducation is required.");
            }

            var createdVideoEducation = await _videoEducationService.AddAsync(videoEducation);

            return CreatedAtAction(nameof(GetById), new { id = createdVideoEducation.Id }, createdVideoEducation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VideoEducation videoEducation)
        {
            if (videoEducation == null || videoEducation.Id != id)
            {
                return BadRequest("Invalid video education data.");
            }

            var updatedVideoEducation = await _videoEducationService.UpdateAsync(videoEducation);

            if (updatedVideoEducation == null)
            {
                return NotFound();
            }

            return Ok(updatedVideoEducation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            var videoEducation = await _videoEducationService.GetAsync(
                predicate: x => x.Id == id,
                include: true,
                withDeleted: false,
                enableTracking: true
            );

            if (videoEducation == null)
            {
                return NotFound();
            }

            var deletedVideoEducation = await _videoEducationService.DeleteAsync(videoEducation, permanent);

            return Ok(deletedVideoEducation);
        }
    }
}
