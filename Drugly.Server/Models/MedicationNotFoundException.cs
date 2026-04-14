namespace Drugly.Server.Models;

/// <summary>An exception class for when a medication is not hound in the database</summary>
public class MedicationNotFoundException : Exception
{
    public string MedicationMessage;

    public MedicationNotFoundException()
    {
        MedicationMessage = "Medication not found";
    }

    public MedicationNotFoundException(string message)
    {
        MedicationMessage = message;
    }
}