namespace CreditManager.Domain.Entities.Credit;

public class CreditRequest : AuditableEntity<Guid>
{
    public Guid CustomerId { get; set; }
    
    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;
    
    DateTime RequestDate { get; set; }

    public CreditRequestStatus Status { get; set; }
    
    public string? Comments { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public Guid? ApprovedBy { get; set; }
}

public enum CreditRequestStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}