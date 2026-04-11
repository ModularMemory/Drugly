using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicationController : DruglyController
{
    private readonly IMedicationDatabaseService _databaseService;
    private readonly ILogger<AccountController>  _logger;

    public MedicationController(
        IMedicationDatabaseService databaseService,
        ILogger<AccountController> logger
    )
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    [HttpGet(nameof(GetById))]
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

    [HttpPost(nameof(SetById))]
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