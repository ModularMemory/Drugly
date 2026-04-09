namespace Drugly.AvaloniaApp.Models;

public record PatientModel
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    public PatientModel(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}