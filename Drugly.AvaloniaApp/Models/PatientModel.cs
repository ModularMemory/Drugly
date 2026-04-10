namespace Drugly.AvaloniaApp.Models;

/// <summary>Represents a patient.</summary>
public record PatientModel
{
    /// <summary>The first name of the patient.</summary>
    public string FirstName { get; }

    /// <summary>The last name of the patient.</summary>
    public string LastName { get; }

    /// <summary>The email of the patient.</summary>
    public string Email { get; }

    public PatientModel(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}