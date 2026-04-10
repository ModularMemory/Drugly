namespace Drugly.Server.Models;

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