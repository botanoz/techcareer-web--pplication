namespace TechCareer.Models.Dtos.Instructor;

public class InstructorDeleteRequestDto
{
    public Guid Id { get; set; }
    public bool Permanent { get; set; } = false;
}
