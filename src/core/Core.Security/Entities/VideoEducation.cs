using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class VideoEducation : Entity<int>
{
    public bool IsDeleted;

    public string Title { get; set; }
    public string Description { get; set; }
    public float TotalHour { get; set; }
    public bool IsCertified { get; set; }
    public int Level { get; set; }
    public string ImageUrl { get; set; }
    public Guid InstructorId { get; set; }
    public string ProgrammingLanguage { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;

    public VideoEducation()
    {
        Title = string.Empty;
        Description = string.Empty;
        ImageUrl = string.Empty;
        ProgrammingLanguage = string.Empty;
    }

    public VideoEducation(
        string title,
        string description,
        float totalHour,
        bool isCertified,
        int level,
        string imageUrl,
        Guid instructorId,
        string programmingLanguage
    )
    {
        Title = title;
        Description = description;
        TotalHour = totalHour;
        IsCertified = isCertified;
        Level = level;
        ImageUrl = imageUrl;
        InstructorId = instructorId;
        ProgrammingLanguage = programmingLanguage;
    }

    public VideoEducation(
        int id,
        string title,
        string description,
        float totalHour,
        bool isCertified,
        int level,
        string imageUrl,
        Guid instructorId,
        string programmingLanguage
    ) : base(id)
    {
        Title = title;
        Description = description;
        TotalHour = totalHour;
        IsCertified = isCertified;
        Level = level;
        ImageUrl = imageUrl;
        InstructorId = instructorId;
        ProgrammingLanguage = programmingLanguage;
    }
}
