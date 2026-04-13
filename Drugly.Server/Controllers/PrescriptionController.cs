using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public class PrescriptionController : DruglyController
{
    private readonly IPrescriptionDatabaseService _databaseService;
    private readonly IStateMachineFactoryService _stateMachineFactoryService;
    private readonly ILogger<PrescriptionController> _logger;

    public PrescriptionController(
        IPrescriptionDatabaseService databaseService,
        IStateMachineFactoryService stateMachineFactoryService,
        ILogger<PrescriptionController> logger
    )
    {
        _databaseService = databaseService;
        _stateMachineFactoryService = stateMachineFactoryService;
        _logger = logger;
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) // TODO: add auth
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByAccountId(Guid id) // TODO: add auth
    {
        ApiResponse<List<Prescription>> response = new ApiResponse<List<Prescription>>();

        try
        {
            response.Data = await _databaseService.GetAllPrescriptionsByAccountId(id);
            Response.Headers.ContentType = "application/json";
        }
        catch (PrescriptionNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find prescriptions associated with account {id}", id);
            return NotFound(ApiResponse.Error("Not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch prescriptions associated with account {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }

        _logger.LogInformation("Prescriptions associated with account {id} successfully retrieved", id);
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

    [HttpPut("{state:PrescriptionState}")]
    public async Task<IActionResult> AdvanceState(PrescriptionState state, [FromBody] Prescription prescription)
    {
        PrescriptionStateMachine prescriptionStateMachine = _stateMachineFactoryService.GetStateMachine(prescription);
        try
        {
            prescriptionStateMachine.ProgressState(state);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Desired State  {state} Not Recognized", state);
            return InternalServerError(ApiResponse.Error("Invalid state")); // probably not the right error
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed To Progress State");
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Prescription State Progressed to {state}", state);
        return Ok();
    }
}