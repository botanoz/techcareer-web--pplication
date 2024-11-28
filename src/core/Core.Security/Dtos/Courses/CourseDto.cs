namespace Core.Security.Dtos.Courses;

public class CourseDto
{
    public string Id { get; set; } = string.Empty;
    public string? Base64Image { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; }
    public string Level { get; set; }
    public int TotalHour { get; set; }
    public bool IsHaveCerficate { get; set; }
}
