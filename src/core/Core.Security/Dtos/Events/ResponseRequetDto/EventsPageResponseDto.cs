using Core.Security.Dtos.Event;

namespace Core.Security.Dtos.Events.ResponseRequetDto;

public class EventsPageResponseDto
{
    public string PageTitle { get; set; } = string.Empty;
    public List<EventDto> ContinueEvents { get; set; } = new List<EventDto>();
    public List<EventDto> PastEvents { get; set; } = new List<EventDto>();
    public EventInformationCardDto? EventInformationCard { get; set; }
    public string? WhoCanParticipateHtmlContent { get; set; }
    public string? WhatAwaitsYouTitle { get; set; }
    public List<WhatAwaitsYouCardDto>? WhatAwaitsYouCard { get; set; }
    public string? TalentSuccessStoriesTitle { get; set; }
    public List<TalentSuccessStoriesCardDto>? TalentSuccessStoriesCard { get; set; }
    public string AboutEvents { get; set; } = string.Empty;
}
