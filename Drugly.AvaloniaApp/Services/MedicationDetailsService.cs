using System.Net.Http.Json;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class MedicationDetailsService : IMedicationDetailsService
{
    private readonly IAccountSessionService _accountSessionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public MedicationDetailsService(
        IAccountSessionService accountSessionService,
        IHttpClientFactory httpClientFactory,
        ILogger logger
    )
    {
        _accountSessionService = accountSessionService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Medication> GetMedication(Guid id)
    {
        var client = _httpClientFactory.CreateClient(nameof(IMedicationDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync($"/Medication/GetById/{id}");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Medication>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching info for medication {Id}: {Code} - {Message}", id, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<Medication[]> GetAllMedications()
    {
        var client = _httpClientFactory.CreateClient(nameof(IMedicationDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync("/Medication/GetAll");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Medication[]>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching all medications: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }
}