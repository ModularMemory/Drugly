using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class AccountDetailsService : IAccountDetailsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public AccountDetailsService(
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
}