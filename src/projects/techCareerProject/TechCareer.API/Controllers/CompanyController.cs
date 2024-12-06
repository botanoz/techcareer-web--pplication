using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Company;
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly LoggerServiceBase _logger;

        public CompanyController(ICompanyRepository companyRepository, LoggerServiceBase logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            _logger.Info("GetAllCompanies endpoint started.");
            try
            {
                var companies = await _companyRepository.GetListAsync();

                if (companies == null || companies.Count == 0)
                {
                    _logger.Warn("No companies found.");
                    return NotFound("No companies found.");
                }

                var companyDtos = companies.Select(company => new CompanyResponseDto
                {
                    Name = company.Name,
                    Location = company.Location,
                    Description = company.Description,
                    ImageUrl = company.ImageUrl
                }).ToList();

                _logger.Info($"Retrieved {companyDtos.Count} companies.");
                return Ok(companyDtos);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetAllCompanies: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            _logger.Info($"GetCompany endpoint started. ID: {id}");
            try
            {
                var company = await _companyRepository.GetAsync(c => c.Id == id);

                if (company == null)
                {
                    _logger.Warn($"Company with id {id} not found.");
                    return NotFound($"Company with id {id} not found.");
                }

                var companyDto = new CompanyResponseDto
                {
                    Name = company.Name,
                    Location = company.Location,
                    Description = company.Description,
                    ImageUrl = company.ImageUrl
                };

                _logger.Info($"Company with id {id} retrieved successfully.");
                return Ok(companyDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in GetCompany: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyAddRequestDto companyAddRequestDto)
        {
            _logger.Info("CreateCompany endpoint started.");
            if (companyAddRequestDto == null)
            {
                _logger.Warn("Company data is required but null was provided.");
                return BadRequest("Company data is required.");
            }

            try
            {
                var company = new Company
                {
                    Name = companyAddRequestDto.Name,
                    Location = companyAddRequestDto.Location,
                    Description = companyAddRequestDto.Description,
                    ImageUrl = companyAddRequestDto.ImageUrl
                };

                var createdCompany = await _companyRepository.AddAsync(company);

                _logger.Info($"Company created successfully. ID: {createdCompany.Id}");
                return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, new CompanyResponseDto
                {
                    Name = company.Name,
                    Location = company.Location,
                    Description = company.Description,
                    ImageUrl = company.ImageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in CreateCompany: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyUpdateRequestDto companyUpdateRequestDto)
        {
            _logger.Info($"UpdateCompany endpoint started. ID: {id}");
            if (companyUpdateRequestDto == null)
            {
                _logger.Warn("Company update data is required but null was provided.");
                return BadRequest("Company data is required.");
            }

            try
            {
                var existingCompany = await _companyRepository.GetAsync(c => c.Id == id);
                if (existingCompany == null)
                {
                    _logger.Warn($"Company with id {id} not found.");
                    return NotFound($"Company with id {id} not found.");
                }

                existingCompany.Name = companyUpdateRequestDto.Name ?? existingCompany.Name;
                existingCompany.Location = companyUpdateRequestDto.Location ?? existingCompany.Location;
                existingCompany.Description = companyUpdateRequestDto.Description ?? existingCompany.Description;
                existingCompany.ImageUrl = companyUpdateRequestDto.ImageUrl ?? existingCompany.ImageUrl;

                var updatedCompany = await _companyRepository.UpdateAsync(existingCompany);

                _logger.Info($"Company with id {id} updated successfully.");
                return Ok(new CompanyResponseDto
                {
                    Name = updatedCompany.Name,
                    Location = updatedCompany.Location,
                    Description = updatedCompany.Description,
                    ImageUrl = updatedCompany.ImageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in UpdateCompany: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            _logger.Info($"DeleteCompany endpoint started. ID: {id}");
            try
            {
                var company = await _companyRepository.GetAsync(c => c.Id == id);
                if (company == null)
                {
                    _logger.Warn($"Company with id {id} not found.");
                    return NotFound($"Company with id {id} not found.");
                }

                var companyDto = new CompanyResponseDto
                {
                    Name = company.Name,
                    Location = company.Location,
                    Description = company.Description,
                    ImageUrl = company.ImageUrl
                };

                await _companyRepository.DeleteAsync(company);

                _logger.Info($"Company with id {id} deleted successfully.");
                return Ok(new
                {
                    Message = "Company has been deleted successfully.",
                    DeletedCompany = companyDto
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred in DeleteCompany: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
