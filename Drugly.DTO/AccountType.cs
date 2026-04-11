using System.Text.Json.Serialization;

namespace Drugly.DTO;

/// <summary>The type of an account.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    /// <summary>Default value. The account is something else.</summary>
    Unknown,
    /// <summary>The account is a patient.</summary>
    Patient,
    /// <summary>The account is a doctor.</summary>
    Doctor,
}