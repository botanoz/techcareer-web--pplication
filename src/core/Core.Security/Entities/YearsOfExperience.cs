using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class YearsOfExperience : Entity<int>
{
    public string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = null!;

    public YearsOfExperience()
    {
        Name = string.Empty;
    }

    public YearsOfExperience(string name)
    {
        Name = name;
    }

    public YearsOfExperience(int id, string name) : base(id)
    {
        Name = name;
    }
}
