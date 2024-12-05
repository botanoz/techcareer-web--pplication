namespace TechCareer.Models.Dtos.Job;

public class JobResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int TypeOfWork { get; set; }
    public int YearsOfExperience { get; set; }
    public int WorkPlace { get; set; }
    public DateTime StartDate { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Skills { get; set; }
    public int CompanyId { get; set; }
}
