namespace Drugly.Server.Services.Interfaces;

public interface IImageDatabaseService
{
    Task<Stream?> GetImageById(string id, out string contentType);
    Task<bool> SetImageById(string id, string contentType, Stream content);
}