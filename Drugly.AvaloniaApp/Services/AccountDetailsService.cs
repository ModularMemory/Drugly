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

    public AccountDetails GetAccountById(Guid id)
    {
        throw new NotImplementedException();
    }

    public AccountDetails GetAccountByEmail(string email)
    {
        throw new NotImplementedException();
    }
}