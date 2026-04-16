using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

/// <summary>A controller that handles route requests for medications</summary>
public class MedicationController : DruglyController
{
    private readonly IMedicationDatabaseService _databaseService;
    private readonly ILogger<MedicationController>  _logger;

    public MedicationController(
        IMedicationDatabaseService databaseService,
        ILogger<MedicationController> logger
    )
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        ApiResponse<Medication> response = new ApiResponse<Medication>();

        try
        {
            response.Data = await _databaseService.GetMedicationById(id);
            Response.Headers.ContentType = "application/json";
        }
        catch (MedicationNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find medication {id}", id);
            return NotFound(ApiResponse.Error("Medication not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch medication {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Medication {id} successfully retrieved", id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedications()
    {
        ApiResponse<Medication[]> response = new ApiResponse<Medication[]>();
        try
        {
            response.Data = await _databaseService.GetAllMedications();
        }
        catch (MedicationNotFoundException ex)
        {
            _logger.LogError(ex, "No medications found");
            return NotFound(ApiResponse.Error("No medications found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching medications");
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }

        _logger.LogInformation("{count} Medications successfully retrieved", response.Data.Length);
        return Ok(response);
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> SetById(Guid id, [FromBody] Medication medication)
    {
        try
        {
            await _databaseService.SetMedicationById(id, medication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set medication {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Medication {id} successfully set", id);
        return Ok();
    }
}