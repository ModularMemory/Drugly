using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Drugly.Validation;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class LoginService : ILoginService
{
    private readonly IAccountSessionService _accountSessionService;
    private readonly ILogger _logger;
    public event EventHandler<AccountType>? LoginSuccessful;
    public event EventHandler<string>? LoginError;

    public LoginService(
        IAccountSessionService accountSessionService,
        ILogger logger
    )
    {
        _accountSessionService = accountSessionService;
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

    public async Task TryLoginAsync(string? authKey)
    {
        // Locally ensure it's valid before trying an API request
        if (!ValidatorCache<LoginKeyAttribute>.IsValid(authKey, out var message))
        {
            await FakeDelay();
            OnLoginError($"Bad or invalid login information: {message}");
            return;
        }

        // TODO: API request here
        // var accountTypeDto = AccountTypeDto.Patient;
        var accountTypeDto = Random.Shared.GetItems([AccountTypeDto.Patient, AccountTypeDto.Doctor, AccountTypeDto.Pharmacist], 1)[0];
        var sessionToken = authKey!;
        var accountType = accountTypeDto.ToAccountType();
        var expiration = DateTimeOffset.Now.AddDays(1);

        var session = new AccountSession(sessionToken, accountType, expiration);
        _accountSessionService.StoreSession(session);
        OnLoginSuccessful(accountType);
    }

    /// <summary>
    /// Fake delay feels a lot better than instant feedback when we can validate an issue locally.
    /// </summary>
    private static Task FakeDelay()
        => Task.Delay(100);
}