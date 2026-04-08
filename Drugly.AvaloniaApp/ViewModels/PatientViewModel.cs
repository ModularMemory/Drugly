namespace Drugly.AvaloniaApp.ViewModels;

public class PatientViewModel : ViewModelBase
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    public PatientViewModel(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}