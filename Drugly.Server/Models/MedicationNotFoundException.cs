namespace Drugly.Server.Models;

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