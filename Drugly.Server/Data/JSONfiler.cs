namespace Drugly.Server.Models;
using Drugly.DTO;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
public static class JsonWritePrescription
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SavePrescription(Prescription prescription, string filePath)
    {
        var json = JsonSerializer.Serialize(prescription, _options);
        File.WriteAllText(filePath, json);
    }
}

public static class JsonReadPrescription
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static Prescription? LoadPrescription(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Prescription>(json, _options);
    }
}

public static class JsonWriteMedication
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SavePrescription(Medication medication, string filePath)
    {
        var json = JsonSerializer.Serialize(medication, _options);
        File.WriteAllText(filePath, json);
    }
}

public static class JsonReadMedication
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static Medication? LoadMedication(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Medication>(json, _options);
    }
}

public static class JsonWriteAccountDetails
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SavePrescription(AccountDetails accountdetails, string filePath)
    {
        var json = JsonSerializer.Serialize(accountdetails, _options);
        File.WriteAllText(filePath, json);
    }
}

public static class JsonReadAccountDetails
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static AccountDetails? LoadAccountDetails(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<AccountDetails>(json, _options);
    }
}