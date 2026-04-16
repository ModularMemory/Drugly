namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to authorized images.</summary>
public interface IImageDetailsService
{
    /// <summary>Uploads a given image to the server.</summary>
    /// <param name="stream">The image stream.</param>
    /// <returns>A task that can be awaited to complete the upload operation.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<Uri> UploadImage(Stream stream);
}