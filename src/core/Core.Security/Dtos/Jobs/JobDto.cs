namespace Core.Security.Dtos.Jobs;

public class JobDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public string? WorkingStyle { get; set; }
    public int Experience { get; set; }
    public string Location { get; set; }=string.Empty;
    public string? WorkType { get; set; }
    public string JobDescription { get; set; }= string.Empty;
    public string? CompanyDescription { get; set; }
}
