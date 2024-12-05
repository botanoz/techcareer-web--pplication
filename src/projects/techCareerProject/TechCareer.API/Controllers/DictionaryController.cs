using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using TechCareer.Models.Dtos.Dictionary;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryController(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDictionaries()
        {
            var dictionaries = await _dictionaryRepository.GetListAsync();

            if (dictionaries == null || dictionaries.Count == 0)
            {
                return NotFound("No dictionaries found.");
            }

            var dictionaryDtos = dictionaries.Select(dictionary => new DictionaryResponseDto
            {
                Title = dictionary.Title,
                Description = dictionary.Description
            }).ToList();

            return Ok(dictionaryDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDictionary(int id)
        {
            var dictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);

            if (dictionary == null)
            {
                return NotFound($"Dictionary with id {id} not found.");
            }

            var dictionaryDto = new DictionaryResponseDto
            {
                Title = dictionary.Title,
                Description = dictionary.Description
            };

            return Ok(dictionaryDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateDictionary([FromBody] DictionaryAddRequestDto dictionaryAddRequestDto)
        {
            if (dictionaryAddRequestDto == null)
            {
                return BadRequest("Dictionary data is required.");
            }

            
            var dictionary = new Dictionary
            {
                Title = dictionaryAddRequestDto.Title,
                Description = dictionaryAddRequestDto.Description,
           
            };

            var createdDictionary = await _dictionaryRepository.AddAsync(dictionary);
            return CreatedAtAction(nameof(GetDictionary), new { id = createdDictionary.Id }, createdDictionary);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDictionary(int id, [FromBody] DictionaryUpdateRequestDto dictionaryUpdateRequestDto)
        {
            if (dictionaryUpdateRequestDto == null)
            {
                return BadRequest("Dictionary data is required.");
            }

            var existingDictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
            if (existingDictionary == null)
            {
                return NotFound($"Dictionary with id {id} not found.");
            }

            
            existingDictionary.Title = dictionaryUpdateRequestDto.Title ?? existingDictionary.Title;
            existingDictionary.Description = dictionaryUpdateRequestDto.Description ?? existingDictionary.Description;
            

            var updatedDictionary = await _dictionaryRepository.UpdateAsync(existingDictionary);
            return Ok(updatedDictionary);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDictionary(int id)
        {
            var dictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
            if (dictionary == null)
            {
                return NotFound($"Dictionary with id {id} not found.");
            }

            var dictionaryDto = new DictionaryResponseDto
            {
                Title = dictionary.Title,
                Description = dictionary.Description
            };

            await _dictionaryRepository.DeleteAsync(dictionary);

            return Ok(new
            {
                Message = "Dictionary deleted successfully.",
                DeletedDictionary = dictionaryDto
            });
        }

    }
}
