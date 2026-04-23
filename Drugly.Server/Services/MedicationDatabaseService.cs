using Drugly.DTO;
using Drugly.Server.Data;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;

namespace Drugly.Server.Services;


public class MedicationDatabaseService : IHostedService, IMedicationDatabaseService
{
    private readonly ILogger<MedicationDatabaseService> _logger;
    private readonly Dictionary<Guid, Medication> _medications = new();
    private const string FOLDER_PATH = "Medications";

    public MedicationDatabaseService(
        ILogger<MedicationDatabaseService> logger
        )
    {
        _logger = logger;
    }

    /// <summary>
    /// searches for medication by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="MedicationNotFoundException"></exception>
    public Task<Medication> GetMedicationById(Guid id)
    {
        if (!_medications.TryGetValue(id, out var medication))
        {
            throw new MedicationNotFoundException();
        }

        return Task.FromResult(medication);
    }

    /// <summary>
    /// finds all medication
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MedicationNotFoundException"></exception>
    public Task<Medication[]> GetAllMedications()
    {
        var medications = _medications.Values.ToArray();

        if (medications.Length == 0)
        {
            throw new MedicationNotFoundException("Medications not found");
        }

        return Task.FromResult(medications);
    }

    /// <summary>
    /// sets medication id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="medication"></param>
    /// <returns></returns>
    public async Task SetMedicationById(Guid id, Medication medication)
    {
        _medications[id] = medication;

        var filePath = Path.Combine(FOLDER_PATH, $"{id}.json");

        await JsonWriteMedication.SaveMedication(medication, filePath);
    }

    /// <summary>
    /// loads medication data from the disk
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(FOLDER_PATH))
            Directory.CreateDirectory(FOLDER_PATH);

        var files = Directory.GetFiles(FOLDER_PATH, "*.json");

        foreach (var file in files)
        {
            var medication = await JsonReadMedication.LoadMedication(file);

            if (medication is null)
                continue;

            _medications[medication.Id] = medication;
        }

        _logger.LogInformation("Loaded {MedicationsCount} medications", _medications.Count);
    }

    /// <summary>
    /// stops async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}