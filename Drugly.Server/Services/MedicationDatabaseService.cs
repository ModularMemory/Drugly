using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;


public class MedicationDatabaseService : IHostedService, IMedicationDatabaseService
{
    public Task<Medication> GetMedicationById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task SetMedicationById(Guid id, Medication medication)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}