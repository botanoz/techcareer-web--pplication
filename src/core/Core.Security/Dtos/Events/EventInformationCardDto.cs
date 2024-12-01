namespace Core.Security.Dtos.Events
{
    public class EventInformationCardDto
    {
        public string LeftBase64Image { get; set; } = string.Empty;
        public string TopBase64Image { get; set; } = string.Empty;
        public string RightBase64Image { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;

    }
}
