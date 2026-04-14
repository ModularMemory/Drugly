using Drugly.DTO;

namespace Drugly.Server.Models;

/// <summary>A state machine for a prescription</summary>
public class PrescriptionStateMachine : IAsyncDisposable, IDisposable
{
    /// <summary>The service for logging</summary>
    private readonly ILogger<PrescriptionStateMachine> _logger;

    /// <summary>The prescription being managed</summary>
    public Prescription prescription { get; }

    /// <summary>The constructor for the prescription state machine</summary>
    /// <param name="logger">The logger that gets created in the factory</param>
    /// <param name="p">The prescription being managed</param>
    public PrescriptionStateMachine(ILogger<PrescriptionStateMachine> logger, Prescription p)
    {
        _logger = logger;
        prescription = p;
    }

    /// <summary>A method to progress the state of the prescription</summary>
    /// <param name="newState">The "target" state, that's attempting to be set</param>
    /// <returns>A bool that determines whether or not the change was a success</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the current state is something undefined and doesn't know how to be handled</exception>
    public bool ProgressState(PrescriptionState newState)
    {
        if (newState is PrescriptionState.Cancelled && prescription.State is not  PrescriptionState.Cancelled)
        {
            prescription.State = PrescriptionState.Cancelled;
            return true;
        }

        switch (prescription.State)
        {
            case PrescriptionState.DoctorPrescription:
                if (newState == PrescriptionState.PharmacyProcessing)
                {
                    prescription.State = PrescriptionState.PharmacyProcessing;
                    return true;
                }
                break;

            case PrescriptionState.PharmacyProcessing:
                if (newState is PrescriptionState.Filled)
                {
                    prescription.State = PrescriptionState.Filled;
                    return true;
                }
                break;

            case PrescriptionState.Filled:
                if (newState is PrescriptionState.Billing)
                {
                    prescription.State = PrescriptionState.Billing;
                    return true;
                }
                break;

            case PrescriptionState.Billing:
                if (newState is PrescriptionState.PickedUp)
                {
                    prescription.State = PrescriptionState.PickedUp;
                    return true;
                }
                break;
            case PrescriptionState.PickedUp:
                if (newState is PrescriptionState.PharmacyProcessing)
                {
                    prescription.State = PrescriptionState.PharmacyProcessing;
                    return true;
                }
                break;

            case PrescriptionState.Cancelled:
            case PrescriptionState.Unknown:
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(PrescriptionState));
        }
        return false;
    }

    /// <summary>I don't know why this exists</summary>
    /// <returns>mreow</returns>
    /// <exception cref="NotImplementedException">mrrrp</exception>
    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>Or this tbh</summary>
    /// <exception cref="NotImplementedException">mrreow</exception>
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}