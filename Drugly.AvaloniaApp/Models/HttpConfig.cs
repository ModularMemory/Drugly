using Microsoft.Extensions.Configuration;

namespace Drugly.AvaloniaApp.Models;

/// <summary>The configuration of the HTTP services.</summary>
public sealed class HttpConfig
{
    public HttpConfig(IConfigurationRoot configuration)
    {
        configuration.GetSection(nameof(HttpConfig)).Bind(this);
    }

    /// <summary>The hostname of the server.</summary>
    public string? ServerHostname { get; set; }
}