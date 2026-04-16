namespace Drugly.Server.Models;
using Drugly.DTO;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


/// <summary> General JSON Writer to prevent repetition 
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

/// <summary> General JSON Reader to prevent repetition 
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

/// <summary> Json Writer for Prescription data.  Writes Prescription Data to file
public static class JsonWritePrescription
{
    public static Task SavePrescription(Prescription prescription, string filePath) =>
        JsonWriter.SaveAsync(prescription, filePath);
}


/// <summary> Json Reader for Prescription data.  Reads Prescription Data from file
public static class JsonReadPrescription
{
    public static Task<Prescription?> LoadPrescription(string filePath) =>
         JsonReader.LoadAsync<Prescription>(filePath);
}

/// <summary> Json Writer for Medication data.  Writes Medication Data to file
public static class JsonWriteMedication
{
    public static Task SaveMedication(Medication medication, string filePath) =>
        JsonWriter.SaveAsync(medication, filePath);
}

/// <summary> Json Reader for Medication data.  Reads Medication Data from file
public static class JsonReadMedication
{
public static Task<Medication?> LoadMedication(string filePath) =>
         JsonReader.LoadAsync<Medication>(filePath);
}

/// <summary> Json Writer for Account Details.  Writes Account Details to JSON file
public static class JsonWriteAccountDetails
{
    public static Task SaveAccountDetails(AccountDetails details, string filePath) =>
        JsonWriter.SaveAsync(details, filePath);
}

/// <summary> Json Reader for Account Details.  Reads Account Data from file
public static class JsonReadAccountDetails
{
    public static Task<AccountDetails?> LoadAccountDetails(string filePath) =>
        JsonReader.LoadAsync<AccountDetails>(filePath);
}
/// <summary> Json Writer for Account Entry Database.  Writes Accounts to File
public static class JsonWriteAccountDatabaseEntry
{
    public static Task SaveAccount(AccountDatabaseEntry entry, string filePath) =>
            JsonWriter.SaveAsync(entry, filePath);
}

/// <summary> Json Reader for Accout Entry Database.  Reads Accounts from Json File
public static class JsonReadAccountDatabaseEntry
{
    public static Task<AccountDatabaseEntry?> LoadAccount(string filePath) =>
          JsonReader.LoadAsync<AccountDatabaseEntry>(filePath);
}