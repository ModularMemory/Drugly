using Drugly.AvaloniaApp.Models;
using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to obtaining account sessions from the server.</summary>
public interface ILoginService
{
    /// <summary>Invoked when a call is <see cref="TryLoginAsync"/> is successful.</summary>
    event EventHandler<AccountType>? LoginSuccessful;

    /// <summary>Invoked when a call is <see cref="TryLoginAsync"/> is not successful.</summary>
    event EventHandler<string>? LoginError;

    /// <summary>Attempts to log in with the server using a given <paramref name="authKey"/>.</summary>
    /// <param name="authKey">The authentication key.</param>
    /// <returns>A task that can be awaited to complete the login operation.</returns>
    Task TryLoginAsync(string? authKey);
}