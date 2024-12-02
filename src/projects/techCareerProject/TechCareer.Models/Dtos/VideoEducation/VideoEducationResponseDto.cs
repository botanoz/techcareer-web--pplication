namespace TechCareer.Models.Dtos.VideoEducation;

public class VideoEducationResponseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float TotalHour { get; set; }
    public bool IsCertified { get; set; }
    public int Level { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid InstructorId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string ProgrammingLanguage { get; set; } = string.Empty; 
}
