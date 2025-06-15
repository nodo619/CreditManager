namespace CreditManager.Application.Messages;

public class CreditRequestMessage
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;
    
    public string Comments { get; set; } = null!;

    public int CreditType { get; set; }

    public DateTime RequestDate { get; set; }

    public TimeSpan Period { get; set; }
}