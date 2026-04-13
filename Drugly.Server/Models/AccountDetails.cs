using Drugly.DTO;

namespace Drugly.Server.Models;

public class AccountDetails
{
    public required Guid UserId { get; set; }
    public required AccountType AccountType { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}