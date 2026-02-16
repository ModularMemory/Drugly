using System.Text.Json.Serialization;

namespace Drugly.DTO;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountTypeDto
{
    Patient,
    Doctor,
    Pharmacist
}