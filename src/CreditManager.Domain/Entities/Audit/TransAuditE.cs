namespace CreditManager.Domain.Entities.Audit;

public class TransAuditE
{
    public Guid Id { get; set; }
    
    public string FieldName { get; set; } = null!;
    
    public string? OldValue { get; set; }
    
    public string? NewValue { get; set; }
    
    public Guid TransAuditHId { get; set; }

    public TransAuditH TransAuditH { get; set; } = null!;
}