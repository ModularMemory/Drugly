using Drugly.AvaloniaApp.Models;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface IAccountSessionService
{
    /// <summary>The account type of the current session.</summary>
    AccountType AccountType { get; }

    /// <summary>Stores the current account login session.</summary>
    /// <param name="session">The session to store.</param>
    void StoreSession(AccountSession session);

    /// <summary>Tries to set the current session token as the default authorization request header on the given <see cref="HttpClient"/>.</summary>
    /// <param name="httpClient">The client to authorize.</param>
    /// <returns><see langword="true"/> if the client was successfully authorized, otherwise <see langword="false"/>.</returns>
    bool TryAuthorizeClient(HttpClient httpClient);
}