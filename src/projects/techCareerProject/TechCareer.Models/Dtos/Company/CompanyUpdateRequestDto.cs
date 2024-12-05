namespace TechCareer.Models.Dtos.Company;

public class CompanyUpdateRequestDto
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; }
}
