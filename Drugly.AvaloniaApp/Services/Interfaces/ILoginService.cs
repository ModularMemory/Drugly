using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface ILoginService
{
    event EventHandler<AccountType>? LoginSuccessful;
    event EventHandler<string>? LoginError;
    Task TryLoginAsync(string? authToken);
}