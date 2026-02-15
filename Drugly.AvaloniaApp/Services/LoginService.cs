using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Drugly.Validation;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class LoginService : ILoginService
{
    private readonly ILogger _logger;
    public event EventHandler<AccountType>? LoginSuccessful;
    public event EventHandler<string>? LoginError;

    public LoginService(
        ILogger logger
    )
    {
        _logger = logger;
    }

    private void OnLoginSuccessful(AccountType e)
    {
        _logger.Information("Successfully logged in as {AccountType}", e);
        LoginSuccessful?.Invoke(this, e);
    }

    private void OnLoginError(string e)
    {
        _logger.Error("Failed to log in: {ErrorMessage}", e);
        LoginError?.Invoke(this, e);
    }

    public async Task TryLoginAsync(string? authToken)
    {
        // Locally ensure it's valid before trying an API request
        if (!ValidatorCache<LoginKeyAttribute>.IsValid(authToken, out var message))
        {
            await FakeDelay();
            OnLoginError($"Bad or invalid login information: {message}");
            return;
        }

        OnLoginSuccessful((AccountType)Random.Shared.Next(0, 3));
    }

    /// <summary>
    /// Fake delay feels a lot better than instant feedback when we can validate an issue locally.
    /// </summary>
    private static Task FakeDelay()
        => Task.Delay(100);
}