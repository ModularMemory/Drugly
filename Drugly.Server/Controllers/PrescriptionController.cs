using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public class PrescriptionController : DruglyController
{
    private readonly IPrescriptionDatabaseService _databaseService;
    private readonly ILogger<PrescriptionController>  _logger;

    public PrescriptionController(
        IPrescriptionDatabaseService databaseService,
        ILogger<PrescriptionController> logger
    )
    {
        _databaseService = databaseService;
        _logger = logger;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        ApiResponse<Prescription> response = new ApiResponse<Prescription>();

        try
        {
            response.Data = await _databaseService.GetPrescriptionById(id);
            Response.Headers.ContentType = "application/json";
        }
        catch (PrescriptionNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find prescription {id}", id);
            return NotFound(ApiResponse.Error("Not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch prescription {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Prescription {id} successfully retrieved", id);
        return Ok(response);
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> SetById(Guid id, [FromBody] Prescription prescription)
    {
        try
        {
            await _databaseService.SetPrescriptionById(id, prescription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set prescription {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Prescription {id} successfully set", id);
        return Ok();
    }
}