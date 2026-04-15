using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class ImageDatabaseService : IImageDatabaseService
{
    public Task<Stream> GetImageById(string id, out string contentType)
    {
        var path = Path.Combine("images", $"{id}.bin");

        if (!File.Exists(path))
        {
           throw new ImageNotFoundException();
        }


        contentType = "application/octet-stream";
        return Task.FromResult<Stream>(File.OpenRead(path));
    }

    public async Task SetImageById(string id, string contentType, Stream content)
    {
        var path = Path.Combine("images", $"{id}.bin");

        Directory.CreateDirectory("images");

        await using var fs = File.Create(path);
        await content.CopyToAsync(fs);
    }
}