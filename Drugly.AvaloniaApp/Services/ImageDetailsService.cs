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

    public async Task<Uri> UploadImage(byte[] bytes)
    {
        var client = _httpClientFactory.CreateClient(nameof(IImageDetailsService));
        if (!_accountSessionService.TryAuthorizeClient(client))
        {
            throw new IOException("Failed to authorize");
        }

        using var req = new HttpRequestMessage(HttpMethod.Put, "/Image/Upload");
        req.Content = new ByteArrayContent(bytes);
        req.Content.Headers.ContentLength = bytes.Length;

        using var res = await client.SendAsync(req);
        var resBody = await res.Content.ReadFromJsonAsync<ApiResponse<string>>();
        if (!res.IsSuccessStatusCode)
        {
            _logger.Error("Error while creating prescription: {Code} - {Message}", res.StatusCode, resBody?.ErrorMessage);
            throw new HttpRequestException(resBody?.ErrorMessage ?? res.StatusCode.ToString(), null, res.StatusCode);
        }

        if (!Uri.TryCreate(client.BaseAddress, new Uri(Path.Combine("Image/GetById", resBody!.Data!), UriKind.Relative), out var uri))
        {
            _logger.Warning("Failed to create image uri");
            throw new IOException("Failed to create image uri");
        }

        return uri!;
    }
}