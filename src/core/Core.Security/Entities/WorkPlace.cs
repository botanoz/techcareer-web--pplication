using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class WorkPlace : Entity<int>
{
    public string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = null!;

    public WorkPlace()
    {
        Name = string.Empty;
    }

    public WorkPlace(string name)
    {
        Name = name;
    }

    public WorkPlace(int id, string name) : base(id)
    {
        Name = name;
    }
}
