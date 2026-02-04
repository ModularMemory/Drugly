using System.Text.Json.Serialization;

namespace Drugly.DTO;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PrescriptionState
{
    /// <summary>Default state - doctor hasn't finalized the order yet.</summary>
    DoctorPrescription,
    /// <summary>Another doctor must sign off to continue.</summary>
    SecondOpinionNeeded,
    /// <summary>Pharmacy is processing the order.</summary>
    PharmacyProcessing,
    /// <summary>The pharmacy is ready for the client to pick up their prescription.</summary>
    ReadyForPickup,
    /// <summary>The client must pay.</summary>
    Billing,
    /// <summary>The client has picked up their prescription.</summary>
    PickedUp
}