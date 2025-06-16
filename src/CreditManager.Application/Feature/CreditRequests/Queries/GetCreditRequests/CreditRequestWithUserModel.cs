using CreditManager.Domain.Entities.Credit;
using CreditManager.Domain.Entities.Identity;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;

public class CreditRequestWithUserModel
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerUsername { get; set; }

    public string? CustomerPersonalNumber { get; set; }

    public User? Customer { get; set; }

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