namespace Drugly.Server.Models;

/// <summary>An exception class for when a prescription is not hound in the database</summary>
public class PrescriptionNotFoundException : Exception
{
    public string PrescriptionMessage;

    public PrescriptionNotFoundException()
    {
        PrescriptionMessage = "Prescription not found";
    }

    public PrescriptionNotFoundException(string message)
    {
        PrescriptionMessage = message;
    }
}