using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class PrescriptionDetailsService : IPrescriptionDetailsService
{
    private readonly IAccountSessionService _accountSessionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public PrescriptionDetailsService(
        IAccountSessionService accountSessionService,
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _accountSessionService = accountSessionService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
}