using System.Diagnostics;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Drugly.Validation;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class LoginService : ILoginService
{
    private readonly IAccountSessionService _accountSessionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    public event EventHandler<AccountType>? LoginSuccessful;
    public event EventHandler<string>? LoginError;

    public LoginService(
        IAccountSessionService accountSessionService,
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _accountSessionService = accountSessionService;
        _httpClientFactory = httpClientFactory;
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
            await DelayService.FakeDelay();
            OnLoginError($"Bad or invalid login information: {message}");
            return;
        }

        Debug.Assert(authKey != null);
        var session = GetAccountSession(authKey);
        if (session is null)
        {
            OnLoginError("Failed to fetch account data");
            return;
        }

        _accountSessionService.StoreSession(session);
        OnLoginSuccessful(session.AccountType);
    }

    private AccountSession? GetAccountSession(string authKey)
    {
        var client = _httpClientFactory.CreateClient(nameof(ILoginService));

        // TODO: API request here
        // var accountType = AccountType.Patient;
        var accountType = Random.Shared.GetItems([AccountType.Patient, AccountType.Doctor], 1)[0];
        var sessionToken = authKey;
        var expiration = DateTimeOffset.Now.AddDays(1);

        return new AccountSession(sessionToken, accountType, expiration);
    }
}