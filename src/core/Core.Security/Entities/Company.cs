using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class Company : Entity<int>
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = null!;

    public Company()
    {
        Name = string.Empty;
        Location = string.Empty;
        Description = string.Empty;
        ImageUrl = null;
    }

    public Company(string name, string location, string? imageUrl, string description)
    {
        Name = name;
        Location = location;
        ImageUrl = imageUrl;
        Description = description;
    }

    public Company(int id, string name, string location, string? imageUrl, string description) : base(id)
    {
        Name = name;
        Location = location;
        ImageUrl = imageUrl;
        Description = description;
    }
}
