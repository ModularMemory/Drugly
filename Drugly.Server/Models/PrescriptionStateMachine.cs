using Drugly.DTO;

namespace Drugly.Server.Models;

public class PrescriptionStateMachine : IAsyncDisposable, IDisposable
{
    private readonly ILogger<PrescriptionStateMachine> _logger;
    public Prescription prescription { get; }

    public PrescriptionStateMachine(ILogger<PrescriptionStateMachine> logger, Prescription p)
    {
        _logger = logger;
        prescription = p;
    }



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
                if (newState is PrescriptionState.ReadyForPickup)
                {
                    prescription.State = PrescriptionState.ReadyForPickup;
                    return true;
                }
                break;

            case PrescriptionState.ReadyForPickup:
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

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}