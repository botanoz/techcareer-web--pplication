using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Event : Entity<Guid>
{
    public bool IsDeleted;

    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public string ParticipationText { get; set; }
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public Event()
    {
        Title = string.Empty;
        Description = string.Empty;
        ImageUrl = string.Empty;
        ParticipationText = string.Empty;
    }

    public Event(
        string title,
        string description,
        string imageUrl,
        DateTime startDate,
        DateTime endDate,
        DateTime applicationDeadline,
        string participationText,
        int categoryId
    )
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        StartDate = startDate;
        EndDate = endDate;
        ApplicationDeadline = applicationDeadline;
        ParticipationText = participationText;
        CategoryId = categoryId;
    }

    public Event(
        Guid id,
        string title,
        string description,
        string imageUrl,
        DateTime startDate,
        DateTime endDate,
        DateTime applicationDeadline,
        string participationText,
        int categoryId
    ) : base(id)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        StartDate = startDate;
        EndDate = endDate;
        ApplicationDeadline = applicationDeadline;
        ParticipationText = participationText;
        CategoryId = categoryId;
    }
}
