using System.Net.Http.Json;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
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

    public async Task<Prescription> GetPrescription(Guid id)
    {
        var client = _httpClientFactory.CreateClient(nameof(IPrescriptionDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync($"/Prescription/GetById/{id}");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Prescription>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching info for prescription {Id}: {Code} - {Message}", id, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<List<Prescription>> GetPrescriptionsByAccountId(Guid id)
    {
        var client = _httpClientFactory.CreateClient(nameof(IPrescriptionDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.GetAsync($"/Prescription/GetByAccountId/{id}");
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<List<Prescription>>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while fetching prescriptions for account {Id}: {Code} - {Message}", id, res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<Prescription> CreatePrescription(Prescription prescription)
    {
        var client = _httpClientFactory.CreateClient(nameof(IPrescriptionDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.PostAsync("/Prescription/AddPrescription/", JsonContent.Create(prescription));
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Prescription>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while creating prescription: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }

    public async Task<Prescription> AdvanceState(Prescription prescription, PrescriptionState newState)
    {
        var client = _httpClientFactory.CreateClient(nameof(IPrescriptionDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var res = await client.PutAsync($"/Prescription/AdvanceState/{(int)newState}", JsonContent.Create(prescription));
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<Prescription>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while advancing prescription state: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        return resBody!.Data!;
    }
}