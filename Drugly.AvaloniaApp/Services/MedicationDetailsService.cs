using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class MedicationDetailsService : IMedicationDetailsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public MedicationDetailsService(
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public MedicationModel GetMedication(Guid id)
    {
        var client = _httpClientFactory.CreateClient(nameof(IMedicationDetailsService));

        throw new NotImplementedException();
    }
}