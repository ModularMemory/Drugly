using System.Text.Json.Serialization;

namespace Drugly.DTO;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    Patient,
    Doctor,
    Pharmacist
}