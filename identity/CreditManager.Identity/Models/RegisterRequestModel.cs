namespace CreditManager.Identity.Models;

public class RegisterRequestModel
{
    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string PersonalNumber { get; set; } = null!;
    
    public DateTime BirthDate { get; set; }
    
    public string Password { get; set; } = null!;
}