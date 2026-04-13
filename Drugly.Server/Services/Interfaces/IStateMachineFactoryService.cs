using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

public interface IStateMachineFactoryService
{
    PrescriptionStateMachine GetStateMachine(Prescription prescription);
}