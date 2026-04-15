using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to obtaining account sessions from the server.</summary>
public interface ILoginService
{
    /// <summary>Invoked when a call to <see cref="TryLoginAsync"/> is successful.</summary>
    event EventHandler<AccountType>? LoginSuccessful;

    /// <summary>Invoked when a call to <see cref="TryLoginAsync"/> is not successful.</summary>
    event EventHandler<string>? LoginError;

    /// <summary>Invoked when a call to <see cref="LogoutAsync"/> is made.</summary>
    event EventHandler? Logout;

    /// <summary>Attempts to log in with the server using a given <paramref name="email"/> and <paramref name="password"/>.</summary>
    /// <param name="email">The email.</param>
    /// <param name="password">The password.</param>
    /// <returns>A task that can be awaited to complete the login operation.</returns>
    Task TryLoginAsync(string email, string password);

    /// <summary>Attempts to clear the current login session.</summary>
    /// <returns>A task that can be awaited to complete the logout operation.</returns>
    Task LogoutAsync();
}