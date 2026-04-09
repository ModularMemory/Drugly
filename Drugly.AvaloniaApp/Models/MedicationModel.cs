namespace Drugly.AvaloniaApp.Models;

public record MedicationModel
{
    public string Name { get; }
    public string Description { get; }
    public string ImageUri { get; }

    public MedicationModel(string name, string description, string imageUri)
    {
        Name = name;
        Description = description;
        ImageUri = imageUri;
    }
}