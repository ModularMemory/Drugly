namespace Drugly.AvaloniaApp.Models;

/// <summary>Represents a medication.</summary>
public record MedicationModel
{
    /// <summary>The name of the medication.</summary>
    public string Name { get; }

    /// <summary>The description of the medication.</summary>
    public string Description { get; }

    /// <summary>A URI to the image of the medication.</summary>
    public string ImageUri { get; }

    public MedicationModel(string name, string description, string imageUri)
    {
        Name = name;
        Description = description;
        ImageUri = imageUri;
    }
}