namespace Drugly.Server.Models;

/// <summary>An exception class for when an image is not hound in the database</summary>
public class ImageNotFoundException : Exception
{
    public string ImageMessage;

    public ImageNotFoundException()
    {
        ImageMessage = "Image not found";
    }

    public ImageNotFoundException(string message)
    {
        ImageMessage = message;
    }
}