using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Dictionary : Entity<int>
{
    public string Title { get; set; }
    public string Description { get; set; }

    public Dictionary()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public Dictionary(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public Dictionary(int id, string title, string description) : base(id)
    {
        Title = title;
        Description = description;
    }
}
