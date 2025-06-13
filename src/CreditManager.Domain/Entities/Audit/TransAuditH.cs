namespace CreditManager.Domain.Entities.Audit;

public class TransAuditH
{
    public Guid Id { get; set; }

    public string TableName { get; set; } = null!;
    
    public int TranType { get; set; }
    
    public string Username { get; set; } = null!;
    
    public string? OsUser { get; set; }
    
    public string? Machine { get; set; }
    
    public DateTime TransDate { get; set; }

    public string PrimKey { get; set; } = null!;
    
    public ICollection<TransAuditE> TransAuditEs { get; set; } = new List<TransAuditE>();
}