namespace Drugly.AvaloniaApp.Models;

/// <summary>Represents a prescription.</summary>
public class PrescriptionModel
{
    /// <summary>The name of the medication.</summary>
    public string Name { get; }

    /// <summary>The description of the medication.</summary>
    public string Description { get; }
    
    /// <summary>The dosage of the medication.</summary>
    public string Dosage { get; }

    /// <summary>A URI to the image of the medication.</summary>
    public string ImageUri { get; }
    
    public PrescriptionModel(string name, string description, string dosage, string imageUri)
    {
        Name = name;
        Description = description;
        Dosage = dosage;
        ImageUri = imageUri;
    }
}