using System.Net.Http.Json;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

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
            throw new IOException();
        }

        using var res = await client.GetAsync($"/Account/GetById/{id}");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<AccountDetails>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching info for account {Id} in: {Code} - {Message}", id, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage, null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<AccountDetails> GetAccountByEmail(string email)
    {
        var client = _httpClientFactory.CreateClient(nameof(IAccountDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException();
        }

        using var req = new HttpRequestMessage(HttpMethod.Get, "/Account/GetByEmail/");
        req.Content = new StringContent(email);

        using var res = await client.SendAsync(req);
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<AccountDetails>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching info for account {Email} in: {Code} - {Message}", email, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage, null, res.StatusCode);
        }

        return resBody!.Data!;
    }
}