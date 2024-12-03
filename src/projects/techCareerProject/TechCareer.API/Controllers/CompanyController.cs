using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyRepository.GetListAsync();
            if (companies == null || companies.Count == 0)
            {
                return NotFound("No companies found.");
            }
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRepository.GetAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound($"Company with id {id} not found.");
            }
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest("Company data is required.");
            }

            var createdCompany = await _companyRepository.AddAsync(company);
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company company)
        {
            if (company == null || company.Id != id)
            {
                return BadRequest("Company data is invalid.");
            }

            var existingCompany = await _companyRepository.GetAsync(c => c.Id == id);
            if (existingCompany == null)
            {
                return NotFound($"Company with id {id} not found.");
            }

            var updatedCompany = await _companyRepository.UpdateAsync(company);
            return Ok(updatedCompany);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _companyRepository.GetAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound($"Company with id {id} not found.");
            }

            await _companyRepository.DeleteAsync(company);
            return NoContent();
        }
    }
}
