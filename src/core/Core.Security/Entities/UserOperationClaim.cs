using Core.Persistence.Repositories.Entities;

namespace Core.Security.Entities;

public class UserOperationClaim : Entity<int>
{
    public bool IsDeleted;

    public int UserId { get; set; }
    public int UserOperationClaimId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual OperationClaim OperationClaim { get; set; } = null!;
    public int OperationClaimId { get; set; }

    public UserOperationClaim(int userId, int operationClaimId)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }

    public UserOperationClaim(int id, int userId, int operationClaimId)
        : base(id)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
}
