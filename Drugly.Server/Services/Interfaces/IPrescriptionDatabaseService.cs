using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IPrescriptionDatabaseService
{
    Task<Prescription> GetPrescriptionById(Guid id);
    Task<bool> SetPrescriptionById(Guid id, Prescription prescription);
}