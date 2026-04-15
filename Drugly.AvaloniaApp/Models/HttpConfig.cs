using Microsoft.Extensions.Configuration;

namespace Drugly.AvaloniaApp.Models;

public sealed class HttpConfig
{
    public HttpConfig(IConfigurationRoot configuration)
    {
        configuration.GetSection(nameof(HttpConfig)).Bind(this);
    }

    public string? ServerHostname { get; set; }
}