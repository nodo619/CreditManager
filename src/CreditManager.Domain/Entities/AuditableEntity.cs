namespace CreditManager.Domain.Entities;

public class AuditableEntity<T> where T : struct, IEquatable<T>, IFormattable
{
    public T Id { get; set; }
    
    public Guid? CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}