using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to <see cref="Prescription"/>s.</summary>
public interface IPrescriptionDetailsService
{
    /// <summary>Gets a prescription by a given id.</summary>
    /// <param name="id">The id of the prescription.</param>
    /// <returns>The found prescription.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<Prescription> GetPrescription(Guid id);

    /// <summary>Gets all prescriptions for a given account id.</summary>
    /// <param name="id">The id of the account.</param>
    /// <returns>The found prescriptions.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<List<Prescription>> GetPrescriptionsByAccountId(Guid id);

    /// <summary>Creates a new prescription entry on the server.</summary>
    /// <param name="prescription">The prescription to create.</param>
    /// <returns>The created prescription entry.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<Prescription> CreatePrescription(Prescription prescription);

    /// <summary>Asks the server to advance the <see cref="Prescription.State"/> of a <paramref name="prescription"/>.</summary>
    /// <param name="prescription">The prescription to update.</param>
    /// <param name="newState">The new <see cref="PrescriptionState"/>.</param>
    /// <returns>The updated prescription entry.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<Prescription> AdvanceState(Prescription prescription, PrescriptionState newState);
}