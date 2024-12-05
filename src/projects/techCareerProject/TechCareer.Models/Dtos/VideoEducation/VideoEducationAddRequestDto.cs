using System.Globalization;

namespace TechCareer.Models.Dtos.VideoEducation;

public class VideoEducationAddRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int TotalHour { get; set; }

}
