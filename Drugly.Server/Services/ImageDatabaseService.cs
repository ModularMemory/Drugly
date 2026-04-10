using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class ImageDatabaseService : IImageDatabaseService
{
    public Task<Stream> GetImageById(string id, out string contentType)
    {
        throw new NotImplementedException();
    }

    public Task SetImageById(string id, string contentType, Stream content)
    {
        throw new NotImplementedException();
    }
}