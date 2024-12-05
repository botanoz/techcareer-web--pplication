namespace TechCareer.Models.Dtos.Instructor;

public class InstructorUpdateRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
}
