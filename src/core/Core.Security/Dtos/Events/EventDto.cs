namespace Core.Security.Dtos.Event;

public class EventDto
{
    public string Id { get; set; } = string.Empty;
    public string? Base64Image { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; }
    public DateTime Deadline { get; set; }
}