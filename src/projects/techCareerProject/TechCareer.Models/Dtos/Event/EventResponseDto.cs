namespace TechCareer.Models.Dtos.Event;

public class EventResponseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public string ParticipationText { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
