using Microsoft.AspNetCore.Identity;

namespace CreditManager.Identity.Data;

public class CreditManagerUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}