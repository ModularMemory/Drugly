using System.Net.Http.Json;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
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
    public event EventHandler? Logout;

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

    private void OnLogout()
    {
        _logger.Information("Successfully logged out");
        Logout?.Invoke(this, EventArgs.Empty);
    }

    public async Task TryLoginAsync(string email, string password)
    {
        var response = await GetAccountSession(email, password);
        if (response?.Data is null)
        {
            OnLoginError($"Failed to fetch account data: {response?.ErrorMessage ?? "Unknown error"}");
            return;
        }

        var session = response.Data;
        _accountSessionService.StoreSession(session);
        OnLoginSuccessful(session.AccountType);
    }

    private async Task<ApiResponse<AccountSession>?> GetAccountSession(string email, string password)
    {
        var client = _httpClientFactory.CreateClient(nameof(ILoginService));

        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        using var res = await client.PostAsync("/Account/Login", JsonContent.Create(request));
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<AccountSession>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while logging in: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
        }

        return resBody;
    }

    public async Task LogoutAsync()
    {
        var client = _httpClientFactory.CreateClient(nameof(ILoginService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            _logger.Warning("Failed to authorize {MethodName} client", nameof(LogoutAsync));
        }

        try
        {
            using var res = await client.DeleteAsync("/Account/Logout");
            if (!res.IsSuccessStatusCode)
            {
                var resBody = await res.Content.ReadAsStringAsync();
                _logger.Error("Error while logging out: {Code} - {Message}", res.StatusCode, resBody);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to log out");
        }

        _accountSessionService.ClearSession();
        OnLogout();
    }
}