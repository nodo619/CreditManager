using CreditManager.Domain.Entities.Credit;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;

public class CreditRequestDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public TimeSpan Period { get; set; }
    public CreditType CreditType { get; set; }
    public CreditRequestStatus Status { get; set; }
    public string? Comments { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
} 