using Drugly.DTO;

namespace Drugly.Server.Models;

/// <summary>A container class for all the information needed for a prescription</summary>
public sealed record Prescription
{
    /// <summary>
    /// The state of the prescription
    /// </summary>
    public required PrescriptionState State { get; set; }

    /// <summary>
    /// The ID of this prescription
    /// </summary>
    public required Guid PrescriptionId { get; set; }

    /// <summary>
    /// The ID of the medication associated with the prescription
    /// </summary>
    public required Guid MedicationId { get; set; }

    /// <summary>
    /// The ID of the patient this prescription is assigned to
    /// </summary>
    public required Guid PatientId { get; set; }

    /// <summary>
    /// Dosage instructions per day
    /// </summary>
    public required string DosagePerDay { get; set; }

    /// <summary>
    /// How many days between dosages
    /// </summary>
    public required ulong DaysBetweenDosage { get; set; }

    /// <summary>
    /// How many days this prescription lasts
    /// </summary>
    public required ulong DaysPrescribed { get; set; }

    /// <summary>
    /// Any additional notes specified by the doctor
    /// </summary>
    public required string AdditionalNotes { get; set; }
}