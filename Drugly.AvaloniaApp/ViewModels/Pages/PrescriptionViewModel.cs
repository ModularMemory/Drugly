namespace Drugly.AvaloniaApp.ViewModels.Pages;

public class PrescriptionViewModel : ViewModelBase
{
    public string Name { get; }
    public string Description { get; }
    public string ImageUri { get; }

    public PrescriptionViewModel(string name, string description, string imageUri)
    {
        Name = name;
        Description = description;
        ImageUri = imageUri;
    }
}