using System.Text.Json;
using System.Text.Json.Serialization;
using Drugly.DTO;

namespace Drugly.Server.Data;

public static class JsonWritePrescription
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SavePrescription(Prescription prescription, string filePath)
    {
        var directoryName = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

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
        var directoryName = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

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
        var directoryName = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

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

public static class JsonWriteAccountDatabaseEntry
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static void SaveAccount(AccountCredentials entry, string filePath)
    {
        var directoryName = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        var json = JsonSerializer.Serialize(entry, _options);
        File.WriteAllText(filePath, json);
    }
}

public static class JsonReadAccountDatabaseEntry
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static AccountCredentials? LoadAccount(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<AccountCredentials>(json, _options);
    }
}