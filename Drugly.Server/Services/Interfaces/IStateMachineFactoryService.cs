using Drugly.Server.Models;

namespace Drugly.Server.Services.Interfaces;

/// <summary>A factory service that creates a state machine for the prescriptions</summary>
public interface IStateMachineFactoryService
{
    /// <summary>Generates a state machine for a given prescription</summary>
    /// <param name="prescription">The prescription that the state machine is being generated for</param>
    /// <returns>The state machine</returns>
    PrescriptionStateMachine GetStateMachine(Prescription prescription);
}