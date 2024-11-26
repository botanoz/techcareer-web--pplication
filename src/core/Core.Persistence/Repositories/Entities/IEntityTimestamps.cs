namespace Core.Persistence.Repositories.Entities;

public interface IEntityTimestamps
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

}
