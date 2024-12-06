using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.Models.Dtos.Dictionary;
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly LoggerServiceBase _logger;

        public DictionaryController(IDictionaryRepository dictionaryRepository, LoggerServiceBase logger)
        {
            _dictionaryRepository = dictionaryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDictionaries()
        {
            _logger.Info("GetAllDictionaries endpoint started.");
            try
            {
                var dictionaries = await _dictionaryRepository.GetListAsync();

                if (dictionaries == null || dictionaries.Count == 0)
                {
                    _logger.Warn("No dictionaries found.");
                    return NotFound("No dictionaries found.");
                }

                var dictionaryDtos = dictionaries.Select(dictionary => new DictionaryResponseDto
                {
                    Title = dictionary.Title,
                    Description = dictionary.Description
                }).ToList();

                _logger.Info($"Retrieved {dictionaryDtos.Count} dictionaries.");
                return Ok(dictionaryDtos);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetAllDictionaries: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDictionary(int id)
        {
            _logger.Info($"GetDictionary endpoint started. ID: {id}");
            try
            {
                var dictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);

                if (dictionary == null)
                {
                    _logger.Warn($"Dictionary with id {id} not found.");
                    return NotFound($"Dictionary with id {id} not found.");
                }

                var dictionaryDto = new DictionaryResponseDto
                {
                    Title = dictionary.Title,
                    Description = dictionary.Description
                };

                _logger.Info($"Dictionary with id {id} retrieved successfully.");
                return Ok(dictionaryDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetDictionary: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDictionary([FromBody] DictionaryAddRequestDto dictionaryAddRequestDto)
        {
            _logger.Info("CreateDictionary endpoint started.");
            if (dictionaryAddRequestDto == null)
            {
                _logger.Warn("Dictionary data is required but null was provided.");
                return BadRequest("Dictionary data is required.");
            }

            try
            {
                var dictionary = new Dictionary
                {
                    Title = dictionaryAddRequestDto.Title,
                    Description = dictionaryAddRequestDto.Description,
                };

                var createdDictionary = await _dictionaryRepository.AddAsync(dictionary);
                _logger.Info($"Dictionary created successfully. ID: {createdDictionary.Id}");

                return CreatedAtAction(nameof(GetDictionary), new { id = createdDictionary.Id }, new DictionaryResponseDto
                {
                    Title = dictionaryAddRequestDto.Title,
                    Description = dictionaryAddRequestDto.Description,
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in CreateDictionary: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDictionary(int id, [FromBody] DictionaryUpdateRequestDto dictionaryUpdateRequestDto)
        {
            _logger.Info($"UpdateDictionary endpoint started. ID: {id}");
            if (dictionaryUpdateRequestDto == null)
            {
                _logger.Warn("Dictionary update data is required but null was provided.");
                return BadRequest("Dictionary data is required.");
            }

            try
            {
                var existingDictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
                if (existingDictionary == null)
                {
                    _logger.Warn($"Dictionary with id {id} not found.");
                    return NotFound($"Dictionary with id {id} not found.");
                }

                existingDictionary.Title = dictionaryUpdateRequestDto.Title ?? existingDictionary.Title;
                existingDictionary.Description = dictionaryUpdateRequestDto.Description ?? existingDictionary.Description;

                var updatedDictionary = await _dictionaryRepository.UpdateAsync(existingDictionary);
                _logger.Info($"Dictionary with id {id} updated successfully.");

                return Ok(new DictionaryResponseDto
                {
                    Title = updatedDictionary.Title,
                    Description = updatedDictionary.Description,
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in UpdateDictionary: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDictionary(int id)
        {
            _logger.Info($"DeleteDictionary endpoint started. ID: {id}");
            try
            {
                var dictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
                if (dictionary == null)
                {
                    _logger.Warn($"Dictionary with id {id} not found.");
                    return NotFound($"Dictionary with id {id} not found.");
                }

                var dictionaryDto = new DictionaryResponseDto
                {
                    Title = dictionary.Title,
                    Description = dictionary.Description
                };

                await _dictionaryRepository.DeleteAsync(dictionary);
                _logger.Info($"Dictionary with id {id} deleted successfully.");

                return Ok(new
                {
                    Message = "Dictionary deleted successfully.",
                    DeletedDictionary = dictionaryDto
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in DeleteDictionary: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
