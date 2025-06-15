namespace CreditManager.Identity.Models;

public class RegisterResponseModel
{
    public Guid? UserId { get; set; }

    public string? FailureMessage { get; set; }
}