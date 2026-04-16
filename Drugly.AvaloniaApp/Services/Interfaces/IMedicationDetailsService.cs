using Drugly.DTO;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides functionality related to <see cref="Medication"/>s.</summary>
public interface IMedicationDetailsService
{
    /// <summary>Gets a medication by a given id.</summary>
    /// <param name="id">The id of the medication.</param>
    /// <returns>The found medication.</returns>
    /// <exception cref="IOException">The server could not be reached.</exception>
    /// <exception cref="HttpRequestException">The request was denied.</exception>
    Task<Medication> GetMedication(Guid id);
}