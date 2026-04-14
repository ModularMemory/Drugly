using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

/// <summary>A service that manages the database of prescriptions</summary>
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
    /// <returns>returns A task that can be awaited</returns>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task SetPrescriptionById(Guid id, Prescription prescription);

    /// <summary>Gets a list of all the prescriptions associated with an account</summary>
    /// <param name="accountId">The account Id you want the prescriptions for</param>
    /// <returns>A list of prescriptions</returns>
    /// <exception cref="PrescriptionNotFoundException">thrown when prescriptions aren't found for that account</exception>
    /// <exception cref="IOException">Thrown when there's an error</exception>
    Task<List<Prescription>> GetAllPrescriptionsByAccountId(Guid accountId);
}