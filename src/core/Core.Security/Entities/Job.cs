using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Job : Entity<int>
{
    public string Title { get; set; }
    public int TypeOfWork { get; set; }
    public int YearsOfExperience { get; set; }
    public int WorkPlace { get; set; }
    public DateTime StartDate { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public string Skills { get; set; }
    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;
    public virtual TypOfWork TypeOfWorkNavigation { get; set; } = null!;
    public virtual WorkPlace WorkPlaceNavigation { get; set; } = null!;
    public virtual YearsOfExperience YearsOfExperienceNavigation { get; set; } = null!;

    public Job()
    {
        Title = string.Empty;
        Content = string.Empty;
        Description = string.Empty;
        Skills = string.Empty;
    }

    public Job(
        string title,
        int typeOfWork,
        int yearsOfExperience,
        int workPlace,
        DateTime startDate,
        string content,
        string description,
        string skills,
        int companyId
    )
    {
        Title = title;
        TypeOfWork = typeOfWork;
        YearsOfExperience = yearsOfExperience;
        WorkPlace = workPlace;
        StartDate = startDate;
        Content = content;
        Description = description;
        Skills = skills;
        CompanyId = companyId;
    }

    public Job(
        int id,
        string title,
        int typeOfWork,
        int yearsOfExperience,
        int workPlace,
        DateTime startDate,
        string content,
        string description,
        string skills,
        int companyId
    ) : base(id)
    {
        Title = title;
        TypeOfWork = typeOfWork;
        YearsOfExperience = yearsOfExperience;
        WorkPlace = workPlace;
        StartDate = startDate;
        Content = content;
        Description = description;
        Skills = skills;
        CompanyId = companyId;
    }
}
