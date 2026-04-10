namespace Drugly.Server.Models;

public class PrescriptionNotFoundException : Exception
{
    public string PrescriptionMessage;
    public PrescriptionNotFoundException()
    {
        PrescriptionMessage = "Prescription not found";
    }

    public PrescriptionNotFoundException(string message) : base(message)
    {
        PrescriptionMessage = message;
    }
}