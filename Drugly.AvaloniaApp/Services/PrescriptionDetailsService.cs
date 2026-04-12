using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class PrescriptionDetailsService : IPrescriptionDetailsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public PrescriptionDetailsService(
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
}