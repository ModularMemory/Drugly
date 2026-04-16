using System.Text.Json;
using System.Text.Json.Serialization;
using Drugly.DTO;

namespace Drugly.Server.Data;

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

public static class JsonWritePrescription
{
    public static Task SavePrescription(Prescription prescription, string filePath) =>
        JsonWriter.SaveAsync(prescription, filePath);
}

public static class JsonReadPrescription
{
    public static Task<Prescription?> LoadPrescription(string filePath) =>
        JsonReader.LoadAsync<Prescription>(filePath);
}

public static class JsonWriteMedication
{
    public static Task SaveMedication(Medication medication, string filePath) =>
        JsonWriter.SaveAsync(medication, filePath);
}

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

public static class JsonReadAccountDatabaseEntry
{
    public static Task<AccountCredentials?> LoadAccount(string filePath) =>
        JsonReader.LoadAsync<AccountCredentials>(filePath);
}