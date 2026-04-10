using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IPrescriptionDatabaseService
{
    /// <summary>Gets a prescription by its ID</summary>
    /// <param name="id">The ID being searched for</param>
    /// <returns>The prescription found</returns>
    /// <exception cref="PrescriptionNotFoundException">Thrown when the prescription is not found</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<Prescription> GetPrescriptionById(Guid id);

    /// <summary>Set a new prescription at a given ID</summary>
    /// <param name="id">The ID where you want to save the prescription</param>
    /// <param name="prescription">The prescription being saved</param>
    /// <returns>nothing</returns>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task SetPrescriptionById(Guid id, Prescription prescription);
}