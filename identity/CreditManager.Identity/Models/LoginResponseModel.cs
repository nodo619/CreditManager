namespace CreditManager.Identity.Models;

public record LoginResponseModel
{
    public string AccessToken { get; set; }
    public string UserName { get; set; }
    public int ExpiresIn { get; set; }
}