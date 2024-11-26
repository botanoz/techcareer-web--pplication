using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Category : Entity<int>
{
    public string Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = null!;

    public Category()
    {
        Name = string.Empty;
    }

    public Category(string name)
    {
        Name = name;
    }

    public Category(int id, string name) : base(id)
    {
        Name = name;
    }
}
