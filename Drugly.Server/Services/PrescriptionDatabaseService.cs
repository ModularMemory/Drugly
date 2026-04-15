using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class PrescriptionDatabaseService : IHostedService, IPrescriptionDatabaseService
{
    private readonly Dictionary<Guid, Prescription> _prescriptions = new();
    private readonly string _folderPath = "Prescriptions";
    public Task<Prescription> GetPrescriptionById(Guid id)
    {
        if (!_prescriptions.TryGetValue(id, out var prescription))
        {
            throw new PrescriptionNotFoundException();
        }

        return Task.FromResult(prescription);
    }

    public Task SetPrescriptionById(Guid id, Prescription prescription)
    {
        _prescriptions[id] = prescription;

        var filePath = Path.Combine(_folderPath, $"{id}.json");
        JsonWritePrescription.SavePrescription(prescription, filePath);

        return Task.CompletedTask;
    }

    public Task<List<Prescription>> GetAllPrescriptionsByAccountId(Guid accountId)
    {
        var result = _prescriptions.Values
          .Where(p => p.PatientId == accountId)
          .ToList();
        if (result.Count == 0)
        {
            throw new PrescriptionNotFoundException();
        }
        return Task.FromResult(result);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        var files = Directory.GetFiles(_folderPath, "*.json");

        foreach (var file in files)
        {
            var prescription = JsonReadPrescription.LoadPrescription(file);

            if (prescription != null)
            {
                _prescriptions[prescription.PrescriptionId] = prescription;
            }
        }

        Console.WriteLine($"Loaded {_prescriptions.Count} prescriptions.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Prescription service stopping.");
        return Task.CompletedTask;
    }
}