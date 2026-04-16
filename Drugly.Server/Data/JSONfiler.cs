using System.Text.Json;
using System.Text.Json.Serialization;
using Drugly.DTO;

namespace Drugly.Server.Data;

/// <summary>
/// General JSON Writer to prevent repeating code
/// </summary>
public static class JsonWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task SaveAsync<T>(T data, string filePath)
    {
        var json = JsonSerializer.Serialize(data, Options);
        await File.WriteAllTextAsync(filePath, json);
    }
}

/// <summary>
/// General JSON Reader to prevent repeating code
/// </summary>
public static class JsonReader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<T?> LoadAsync<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json, Options);
    }
}

/// <summary>
/// Json Writer for Prescription
/// </summary>
public static class JsonWritePrescription
{
    public static Task SavePrescription(Prescription prescription, string filePath) =>
        JsonWriter.SaveAsync(prescription, filePath);
}


/// <summary>
/// Json Reader for Prescription
/// </summary>
public static class JsonReadPrescription
{
    public static Task<Prescription?> LoadPrescription(string filePath) =>
        JsonReader.LoadAsync<Prescription>(filePath);
}


/// <summary>
/// Json Writer for Medication
/// </summary>
public static class JsonWriteMedication
{
    public static Task SaveMedication(Medication medication, string filePath) =>
        JsonWriter.SaveAsync(medication, filePath);
}


/// <summary>
/// Json Reader for Medication
/// </summary>
public static class JsonReadMedication
{
    public static Task<Medication?> LoadMedication(string filePath) =>
        JsonReader.LoadAsync<Medication>(filePath);
}

public static class JsonWriteAccountDatabaseEntry
{
    public static Task SaveAccount(AccountCredentials entry, string filePath) =>
        JsonWriter.SaveAsync(entry, filePath);
}

/// <summary>
/// Json Reader for Account Database
/// </summary>
public static class JsonReadAccountDatabaseEntry
{
    public static Task<AccountCredentials?> LoadAccount(string filePath) =>
        JsonReader.LoadAsync<AccountCredentials>(filePath);
}