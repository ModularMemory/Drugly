using Drugly.AvaloniaApp.Models;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface IMedicationDetailsService
{
    MedicationModel GetMedication(Guid id);
}