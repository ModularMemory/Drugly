namespace Drugly.Server.Models;

public class PrescriptionNotFoundException
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