using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IImageDatabaseService
{
    /// <summary>Gets an image from an id and a </summary>
    /// <param name="id">The ID of the image</param>
    /// <param name="contentType">out parameter to describe the content type, must be set before returning</param>
    /// <returns>Image Stream</returns>
    /// <exception cref="ImageNotFoundException">Thrown when the image can't be found</exception>
    /// <exception cref="IOException"></exception>
    Task<Stream> GetImageById(string id, out string contentType);

    /// <summary>Sets an image by its ID, with a content type</summary>
    /// <param name="id">The ID to save the image to</param>
    /// <param name="contentType">The type of content the image is</param>
    /// <param name="content">The Stream for the image</param>
    /// <returns>nothing</returns>
    /// <exception cref="IOException">Thrown when there's an error saving</exception>
    Task SetImageById(string id, string contentType, Stream content);
}