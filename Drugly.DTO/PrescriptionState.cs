using System.Text.Json.Serialization;

namespace Drugly.DTO;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PrescriptionState
{
    /// <summary>Default state - doctor hasn't finalized the order yet.</summary>
    DoctorPrescription,

    /// <summary>Pharmacy is processing the order.</summary>
    PharmacyProcessing,

    /// <summary>The pharmacy is ready for the client to pick up their prescription.</summary>
    ReadyForPickup,

    /// <summary>The client must pay.</summary>
    Billing,

    /// <summary>The client has picked up their prescription.</summary>
    PickedUp,

    /// <summary>The prescription was deleted or canceled.</summary>
    Canceled
}