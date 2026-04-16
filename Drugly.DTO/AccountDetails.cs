using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Drugly.DTO;

/// <summary>A container class for all the information relating to an account</summary>
public record AccountDetails
{
    [UsedImplicitly]
    public AccountDetails() { }

    [SetsRequiredMembers]
    public AccountDetails(Guid id, AccountType type, string firstName, string lastName, string email)
    {
        UserId = id;
        AccountType = type;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    /// <summary>
    /// The ID of the account
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The type of account this is
    /// </summary>
    public required AccountType AccountType { get; set; }

    /// <summary>
    /// The user's first name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// The user's last name
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// The user's email
    /// </summary>
    public required string Email { get; set; }
}