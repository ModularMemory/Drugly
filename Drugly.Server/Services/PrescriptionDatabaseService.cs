using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class PrescriptionDatabaseService : IHostedService, IPrescriptionDatabaseService
{
    public Task<Prescription> GetPrescriptionById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task SetPrescriptionById(Guid id, Prescription prescription)
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