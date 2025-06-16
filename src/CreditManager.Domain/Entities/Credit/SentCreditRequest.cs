
namespace CreditManager.Domain.Entities.Credit;

public class SentCreditRequest : AuditableEntity<Guid>
{
    public Guid CreditRequestId { get; set; }

    public CreditRequest? CreditRequest { get; set; }

    public DateTime SendTime { get; set; }
}