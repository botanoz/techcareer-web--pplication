using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;

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
            return Ok(dictionaries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDictionary(int id)
        {
            var dictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
            if (dictionary == null)
            {
                return NotFound($"Dictionary with id {id} not found.");
            }
            return Ok(dictionary);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDictionary([FromBody] Dictionary dictionary)
        {
            if (dictionary == null)
            {
                return BadRequest("Dictionary data is required.");
            }

            var createdDictionary = await _dictionaryRepository.AddAsync(dictionary);
            return CreatedAtAction(nameof(GetDictionary), new { id = createdDictionary.Id }, createdDictionary);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDictionary(int id, [FromBody] Dictionary dictionary)
        {
            if (dictionary == null || dictionary.Id != id)
            {
                return BadRequest("Dictionary data is invalid.");
            }

            var existingDictionary = await _dictionaryRepository.GetAsync(d => d.Id == id);
            if (existingDictionary == null)
            {
                return NotFound($"Dictionary with id {id} not found.");
            }

            var updatedDictionary = await _dictionaryRepository.UpdateAsync(dictionary);
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

            await _dictionaryRepository.DeleteAsync(dictionary);
            return NoContent();
        }
    }
}
