using Drugly.DTO;

namespace Drugly.AvaloniaApp.Models;

public class PatientPrescription (Prescription prescription, Medication medication)
{
    public Medication Medication { get; set; } = medication;
    public Prescription Prescription { get; set; } = prescription;
}