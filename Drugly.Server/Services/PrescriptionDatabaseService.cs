using Drugly.DTO;
using Drugly.Server.Data;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;

public class PrescriptionDatabaseService : IHostedService, IPrescriptionDatabaseService
{
    private readonly Dictionary<Guid, Prescription> _prescriptions = new();
    private readonly string _folderPath = "Prescriptions";

    /// <summary>
    /// searches for prescription by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="PrescriptionNotFoundException"></exception>
    public Task<Prescription> GetPrescriptionById(Guid id)
    {
        if (!_prescriptions.TryGetValue(id, out var prescription))
        {
            throw new PrescriptionNotFoundException();
        }

        return Task.FromResult(prescription);
    }

    /// <summary>
    /// sets prescription by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="prescription"></param>
    /// <returns></returns>
    public async Task SetPrescriptionById(Guid id, Prescription prescription)
    {
        _prescriptions[id] = prescription;

        var filePath = Path.Combine(_folderPath, $"{id}.json");

        await JsonWritePrescription.SavePrescription(prescription, filePath);
    }

    /// <summary>
    /// gets all prescriptions assigned to account id
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    /// <exception cref="PrescriptionNotFoundException"></exception>
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

    /// <summary>
    /// begins async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

        var files = Directory.GetFiles(_folderPath, "*.json");

        foreach (var file in files)
        {
            var prescription = await JsonReadPrescription.LoadPrescription(file);

            if (prescription != null)
            {
                _prescriptions[prescription.PrescriptionId] = prescription;
            }
        }

        Console.WriteLine($"Loaded {_prescriptions.Count} prescriptions.");
    }

    /// <summary>
    /// stops async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Prescription service stopping.");
        return Task.CompletedTask;
    }
}