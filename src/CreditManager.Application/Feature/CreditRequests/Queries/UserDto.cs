namespace CreditManager.Application.Feature.CreditRequests.Queries;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PersonalNumber { get; set; }
} 