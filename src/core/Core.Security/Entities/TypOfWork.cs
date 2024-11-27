using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class TypOfWork : Entity<int>
{
    public string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = null!;

    public TypOfWork()
    {
        Name = string.Empty;
    }

    public TypOfWork(string name)
    {
        Name = name;
    }

    public TypOfWork(int id, string name) : base(id)
    {
        Name = name;
    }
}
