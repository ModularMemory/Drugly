using Drugly.AvaloniaApp.Models;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface ILoginService
{
    /// <summary>Invoked when a call is <see cref="TryLoginAsync"/> is successful.</summary>
    event EventHandler<AccountType>? LoginSuccessful;

    /// <summary>Invoked when a call is <see cref="TryLoginAsync"/> is not successful.</summary>
    event EventHandler<string>? LoginError;

    Task TryLoginAsync(string? authKey);
}