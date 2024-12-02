using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Instructor : Entity<Guid>
{
    public bool IsDeleted;

    public string Name { get; set; }
    public string About { get; set; }

    public virtual ICollection<VideoEducation> VideoEducations { get; set; } = null!;

    public Instructor()
    {
        Name = string.Empty;
        About = string.Empty;
    }

    public Instructor(string name, string about)
    {
        Name = name;
        About = about;
    }

    public Instructor(Guid id, string name, string about) : base(id)
    {
        Name = name;
        About = about;
    }
}
