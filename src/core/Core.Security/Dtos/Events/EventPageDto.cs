namespace Core.Security.Dtos.Event;

public class EventPageDto
{
    public List<EventDto> Events { get; set; }
    public List<EventDto> PastEvents { get; set; }
    public string AboutEvents { get; set; }
}
