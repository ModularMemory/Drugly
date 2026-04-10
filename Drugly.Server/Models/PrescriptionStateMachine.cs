using Drugly.DTO;

namespace Drugly.Server.Models;
public class PrescriptionStateMachine : IAsyncDisposable, IDisposable
{
    public PrescriptionStateMachine(PrescriptionState prescriptionState, Guid id)
    {
        PrescriptionState  = prescriptionState;
        Id = id;
    }

    private PrescriptionState PrescriptionState { get; set; }

    private Guid Id { get; set; }

    public PrescriptionState GetPrescriptionState()
    {
        return PrescriptionState;
    }

    public void ProgressState()
    {
        switch(PrescriptionState)
        {
            // didn't add "unknown", don't know what you want to do with it
            case PrescriptionState.DoctorPrescription:
                PrescriptionState = PrescriptionState.PharmacyProcessing;
                break;
            case PrescriptionState.PharmacyProcessing:
                PrescriptionState = PrescriptionState.ReadyForPickup;
                break;
            case PrescriptionState.ReadyForPickup:
                PrescriptionState = PrescriptionState.Billing;
                break;
            case PrescriptionState.Billing:
                PrescriptionState = PrescriptionState.PickedUp;
                break;
            case PrescriptionState.PickedUp:
                PrescriptionState = PrescriptionState.PharmacyProcessing;
                break;
            case PrescriptionState.Cancelled:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(PrescriptionState));
        }
    }

    public void CancelPrescription()
    {
        PrescriptionState = PrescriptionState.Cancelled;
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