using Drugly.DTO;
using Drugly.Server.Data;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;


public class MedicationDatabaseService : IHostedService, IMedicationDatabaseService
{
    private readonly Dictionary<Guid, Medication> _medications = new();
    private readonly string _folderPath = "Medications";
    public Task<Medication> GetMedicationById(Guid id)
    {
        if (!_medications.TryGetValue(id, out var medication))
        {
            throw new MedicationNotFoundException();
        }

        return Task.FromResult(medication);
    }

    public Task<Medication[]> GetAllMedications()
    {
        var medications = _medications.Values.ToArray();

        if (medications.Length == 0)
        {
            throw new MedicationNotFoundException("Medications not found");
        }

        return Task.FromResult(medications);
    }

    public async Task SetMedicationById(Guid id, Medication medication)
    {
        _medications[id] = medication;

        var filePath = Path.Combine(_folderPath, $"{id}.json");

        await JsonWriteMedication.SaveMedication(medication, filePath);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        var files = Directory.GetFiles(_folderPath, "*.json");

        foreach (var file in files)
        {
            var medication = await JsonReadMedication.LoadMedication(file);

            if (medication is null)
                continue;

            _medications[medication.Id] = medication;
        }

        Console.WriteLine($"Loaded {_medications.Count} medications.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Medication service stopping.");
        return Task.CompletedTask;
    }
}