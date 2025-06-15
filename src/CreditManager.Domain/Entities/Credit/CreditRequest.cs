namespace CreditManager.Domain.Entities.Credit;

public class CreditRequest : AuditableEntity<Guid>
{
    public Guid CustomerId { get; set; }
    
    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;
    
    public DateTime RequestDate { get; set; }

    public int PeriodYears { get; set; }
    
    public int PeriodMonths { get; set; }
    
    public int PeriodDays { get; set; }

    public CreditType CreditType { get; set; }

    public CreditRequestStatus Status { get; set; }
    
    public string? Comments { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public Guid? ApprovedBy { get; set; }
}

public enum CreditRequestStatus
{
    Pending = 1,
    Sent = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5
}

public enum CreditType
{
    QuickCredit = 1,
    VehicleCredit = 2,
    Installment = 3
}