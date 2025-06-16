namespace CreditManager.Application.Messages;

public class CreditRequestSentMessage
{
    public Guid Id { get; set; }

    public Guid CreditRequestId { get; set; }

    public DateTime SendTime { get; set; }
}