using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IMedicationDatabaseService
{
    Task<Medication> GetMedicationById(Guid id);
    Task<bool> SetMedicationById(Guid id, Medication medication);
}