namespace CreditManager.Identity.Models;

public record LoginRequestModel
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}