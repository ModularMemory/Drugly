using System.Net.Http.Headers;
using Drugly.DTO;
using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

/// <summary>A service that manages the authentication of the user's sessions</summary>
public interface IAuthorizationService
{
    /// <summary>Creates a session when a user logs in, assigning them a token and storing the session in the dictionary</summary>
    /// <param name="accountDetails">The details for the account being given a session</param>
    /// <returns>An account session object to the user</returns>
    AccountSession CreateSession(AccountDetails accountDetails);

    /// <summary>Deletes the session for a safe log out</summary>
    /// <param name="session">The user's session to be deleted</param>
    /// <returns>A bool to represent success or failure</returns>
    bool DeleteSession(AccountSession session);

    /// <summary>Checks for a user's token in the dictionary to get their session and determines if they're authorized for this request</summary>
    /// <param name="headers">The header of the request that has the token</param>
    /// <param name="allowedType">The account type permitted to make this request</param>
    /// <returns>A bool determining if the request is authorized or not</returns>
    bool IsUserAuthorized(IHeaderDictionary headers, AccountType allowedType);

    /// <summary>An overload for IsUserAuthorized that allowed multiple permitted account types</summary>
    /// <param name="headers">The header of the request that has the token</param>
    /// <param name="allowedTypes">The account types permitted to make this request</param>
    /// <returns>A bool determining if the request is authorized or not</returns>
    bool IsUserAuthorized(IHeaderDictionary headers, ReadOnlySpan<AccountType> allowedTypes);
}