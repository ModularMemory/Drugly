using System.Net.Http.Json;
using System.Text;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class AccountDetailsService : IAccountDetailsService
{
    private readonly IAccountSessionService _accountSessionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public AccountDetailsService(
        IAccountSessionService accountSessionService,
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _accountSessionService = accountSessionService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<AccountDetails> GetAccountById(Guid id)
    {
        var client = _httpClientFactory.CreateClient(nameof(IAccountDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync($"/Account/GetById/{id}");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<AccountDetails>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching info for account {Id}: {Code} - {Message}", id, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<AccountDetails> GetAccountByEmail(string email)
    {
        var client = _httpClientFactory.CreateClient(nameof(IAccountDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, "/Account/GetIdByEmail/");
        req.Content = new StringContent(email, Encoding.UTF8, "text/plain");

        using var res = await client.SendAsync(req);
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Guid>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching email for account {Email}: {Code} - {Message}", email, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return await GetAccountById(resBody!.Data);
    }

    public async Task<AccountDetails[]> GetPatients()
    {
        var client = _httpClientFactory.CreateClient(nameof(IAccountDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync("/Account/GetPatientAccounts");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<AccountDetails[]>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching patients: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }
}