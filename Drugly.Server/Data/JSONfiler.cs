namespace Drugly.Server.Models;
using Drugly.DTO;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
public static class JsonWrite
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

public static class JsonRead
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