using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class StateMachineFactoryService : IStateMachineFactoryService
{
    private readonly IServiceProvider _serviceProvider;
    public StateMachineFactoryService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public PrescriptionStateMachine GetStateMachine(Prescription prescription)
    {
        ILogger<PrescriptionStateMachine> logger = _serviceProvider.GetRequiredService<ILogger<PrescriptionStateMachine>>();
        return new PrescriptionStateMachine(logger, prescription);
    }
}