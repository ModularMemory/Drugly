using System.Net.Http.Json;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class ImageDetailsService : IImageDetailsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAccountSessionService _accountSessionService;
    private readonly ILogger _logger;

    public ImageDetailsService(
        IHttpClientFactory httpClientFactory,
        IAccountSessionService accountSessionService,
        ILogger logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _accountSessionService = accountSessionService;
        _logger = logger;
    }

    public async Task<Uri> UploadImage(Stream stream)
    {
        var client = _httpClientFactory.CreateClient(nameof(IImageDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException();
        }

        using var res = await client.PostAsync("/Image/Upload", new StreamContent(stream));
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while creating prescription: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage, null, res.StatusCode);
        }

        if (!Uri.TryCreate(client.BaseAddress, new Uri(Path.Combine("Image/GetById", resBody!.Data!)), out var uri))
        {
            _logger.Warning("Failed to create image uri");
            throw new IOException("Failed to create image uri");
        }

        return uri!;
    }
}