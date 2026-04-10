using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IMedicationDatabaseService
{
    /// <summary>Gets a medication object by its ID</summary>
    /// <param name="id">The ID you're searching for</param>
    /// <returns>The medication object found</returns>
    /// <exception cref="MedicationNotFoundException">Thrown when the ID for the medication is not found</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<Medication> GetMedicationById(Guid id);

    /// <summary>Sets a new medication by its ID</summary>
    /// <param name="id">The ID you want to save the medication at</param>
    /// <param name="medication">The medication being saved</param>
    /// <returns>nothing</returns>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task SetMedicationById(Guid id, Medication medication);
}