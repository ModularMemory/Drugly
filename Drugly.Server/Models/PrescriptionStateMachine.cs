using System.Diagnostics;

namespace Drugly.Server.Models;

public class PrescriptionStateMachine : IAsyncDisposable, IDisposable
{
    public PrescriptionStateMachine(PrescriptionState prescriptionState, int id)
    {
        _prescriptionState  = prescriptionState;
        _id = id;
    }

    private PrescriptionState _prescriptionState;

    private int _id;

    public PrescriptionState GetPrescriptionState()
    {
        return _prescriptionState;
    }

    int GetId()
    {
        return _id;
    }

    public void ProgressState()
    {
        switch(_prescriptionState)
        {
            case PrescriptionState.Unconfirmed:
                _prescriptionState = PrescriptionState.Processing;
                break;
            case PrescriptionState.Processing:
                _prescriptionState = PrescriptionState.Ready;
                break;
            case PrescriptionState.Ready:
                _prescriptionState = PrescriptionState.Billing;
                break;
            case PrescriptionState.Billing:
                _prescriptionState = PrescriptionState.PickedUp;
                break;
            case PrescriptionState.PickedUp:
                _prescriptionState = PrescriptionState.Processing;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void CancelPrescription()
    {
        _prescriptionState = PrescriptionState.Cancelled;
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