using CreditManager.Domain.Enumerations;

namespace CreditManager.Domain.Entities.Identity;

public class User : AuditableEntity<Guid>
{
    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string PersonalNumber { get; set; } = null!;
    
    public DateOnly BirthDate { get; set; }
    
    public UserRole Role { get; set; }
    
    public string PasswordHash { get; set; } = null!;
}