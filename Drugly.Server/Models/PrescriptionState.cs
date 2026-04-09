namespace Drugly.Server.Models;

public enum PrescriptionState
{
    Unconfirmed,
    Processing,
    Ready,
    Billing,
    PickedUp,
    Cancelled
}