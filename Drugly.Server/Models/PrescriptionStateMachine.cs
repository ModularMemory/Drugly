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
            case PrescriptionState.Unconfirmed:
                PrescriptionState = PrescriptionState.Processing;
                break;
            case PrescriptionState.Processing:
                PrescriptionState = PrescriptionState.Ready;
                break;
            case PrescriptionState.Ready:
                PrescriptionState = PrescriptionState.Billing;
                break;
            case PrescriptionState.Billing:
                PrescriptionState = PrescriptionState.PickedUp;
                break;
            case PrescriptionState.PickedUp:
                PrescriptionState = PrescriptionState.Processing;
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