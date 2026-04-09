namespace Drugly.Server.Models;

public class AccountDetails
{
    public required string SessionToken { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required AccountType AccountType { get; set; }
}