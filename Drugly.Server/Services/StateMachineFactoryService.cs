using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class StateMachineFactoryService : IStateMachineFactoryService
{
    public PrescriptionStateMachine GetStateMachine(IServiceProvider serviceProvider, Prescription prescription)
    {
        ILogger<PrescriptionStateMachine> logger = serviceProvider.GetService<ILogger<PrescriptionStateMachine>>();
        return new PrescriptionStateMachine(logger, prescription);
    }
}