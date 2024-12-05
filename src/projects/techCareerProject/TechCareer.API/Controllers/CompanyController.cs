using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Company;

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

            var companyDtos = companies.Select(company => new CompanyResponseDto
            {
                Name = company.Name,
                Location = company.Location,
                Description = company.Description,
                ImageUrl = company.ImageUrl
            }).ToList();

            return Ok(companyDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRepository.GetAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound($"Company with id {id} not found.");
            }

            var companyDto = new CompanyResponseDto
            {
                Name = company.Name,
                Location = company.Location,
                Description = company.Description,
                ImageUrl = company.ImageUrl
            };

            return Ok(companyDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyAddRequestDto companyAddRequestDto)
        {
            if (companyAddRequestDto == null)
            {
                return BadRequest("Company data is required.");
            }

            
            var company = new Company
            {
                Name = companyAddRequestDto.Name,
                Location = companyAddRequestDto.Location,
                Description= companyAddRequestDto.Description,
                ImageUrl = companyAddRequestDto.ImageUrl
               
            };

            var createdCompany = await _companyRepository.AddAsync(company);

            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyUpdateRequestDto companyUpdateRequestDto)
        {
            if (companyUpdateRequestDto == null)
            {
                return BadRequest("Company data is required.");
            }

            var existingCompany = await _companyRepository.GetAsync(c => c.Id == id);
            if (existingCompany == null)
            {
                return NotFound($"Company with id {id} not found.");
            }

            
            existingCompany.Name = companyUpdateRequestDto.Name ?? existingCompany.Name;
            existingCompany.Location = companyUpdateRequestDto.Location ?? existingCompany.Location;
            existingCompany.Description = companyUpdateRequestDto.Description ?? existingCompany.Description;
            existingCompany.ImageUrl = companyUpdateRequestDto.ImageUrl ?? existingCompany.ImageUrl;


            var updatedCompany = await _companyRepository.UpdateAsync(existingCompany);

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

            var companyDto = new CompanyResponseDto
            {
                Name = company.Name,
                Location = company.Location,
                Description = company.Description,
                ImageUrl= company.ImageUrl,
            };

            await _companyRepository.DeleteAsync(company);

            return Ok(new
            {
                Message = "Company has been deleted successfully.",
                DeletedCompany = companyDto
            });
        }

    }
}
