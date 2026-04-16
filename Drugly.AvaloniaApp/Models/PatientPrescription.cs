using Drugly.DTO;

namespace Drugly.AvaloniaApp.Models;

/// <summary>Combines a Prescription and medication.</summary>
public class PatientPrescription
{
    public PatientPrescription(Prescription prescription, Medication medication)
    {
        Medication = medication;
        Prescription = prescription;
    }

    /// <summary>The prescription.</summary>
    public Prescription Prescription { get; set; }

    /// <summary>The medication.</summary>
    public Medication Medication { get; set; }
}