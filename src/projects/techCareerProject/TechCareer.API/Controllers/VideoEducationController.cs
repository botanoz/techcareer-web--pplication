using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Models.Dtos.VideoEducation;

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
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
        {
            var videoEducations = await _videoEducationService.GetListAsync(withDeleted: includeDeleted);
            return Ok(videoEducations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var videoEducation = await _videoEducationService.GetAsync(education => education.Id == id);
            return Ok(videoEducation);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VideoEducationAddRequestDto videoEducationAddRequestDto)
        {
            var addedVideoEducation = await _videoEducationService.AddAsync(videoEducationAddRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = addedVideoEducation.Id }, addedVideoEducation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VideoEducationUpdateRequestDto videoEducationUpdateRequestDto)
        {
            videoEducationUpdateRequestDto.Id = id;
            var updatedVideoEducation = await _videoEducationService.UpdateAsync(videoEducationUpdateRequestDto);
            return Ok(updatedVideoEducation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool permanent = false)
        {
            var deletedVideoEducation = await _videoEducationService.DeleteAsync(
                new VideoEducationRequestDto { Id = id }, permanent);

            return Ok(deletedVideoEducation);
        }

        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDeleted = false)
        {
            var paginatedVideoEducations = await _videoEducationService.GetPaginateAsync(
                index: pageIndex,
                size: pageSize,
                withDeleted: includeDeleted);

            return Ok(paginatedVideoEducations);
        }
    }
}
