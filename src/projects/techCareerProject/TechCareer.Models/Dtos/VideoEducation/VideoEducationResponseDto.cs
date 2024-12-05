namespace TechCareer.Models.Dtos.VideoEducation;

public class VideoEducationResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public float TotalHour { get; set; }
    public bool IsCertified { get; set; }
    public int Level { get; set; }
    public string ImageUrl { get; set; }
    public Guid InstructorId { get; set; }
    public string ProgrammingLanguage { get; set; }
}
