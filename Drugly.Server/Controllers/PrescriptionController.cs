using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

/// <summary>A controller that handles route requests for prescriptions</summary>
public class PrescriptionController : DruglyController
{
    private readonly IPrescriptionDatabaseService _prescriptionDatabaseService;
    private readonly IAccountDatabaseService _accountDatabaseService;
    private readonly IStateMachineFactoryService _stateMachineFactoryService;
    private readonly ILogger<PrescriptionController> _logger;
    private readonly IAuthorizationService _authorizationService;

    public PrescriptionController(
        IPrescriptionDatabaseService prescriptionDatabaseService,
        IAccountDatabaseService accountDatabaseService,
        IStateMachineFactoryService stateMachineFactoryService,
        ILogger<PrescriptionController> logger,
        IAuthorizationService authorizationService
    )
    {
        _prescriptionDatabaseService = prescriptionDatabaseService;
        _accountDatabaseService = accountDatabaseService;
        _stateMachineFactoryService = stateMachineFactoryService;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<Prescription> response = new ApiResponse<Prescription>();
        Response.Headers.ContentType = "application/json";

        try
        {
            response.Data = await _prescriptionDatabaseService.GetPrescriptionById(id);
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
    public async Task<IActionResult> GetByAccountId(Guid id)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<List<Prescription>> response = new ApiResponse<List<Prescription>>();
        Response.Headers.ContentType = "application/json";

        try
        {
            response.Data = await _prescriptionDatabaseService.GetAllPrescriptionsByAccountId(id);
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

    [HttpPut("{stateInt:int}")]
    public async Task<IActionResult> AdvanceState(int stateInt, [FromBody] Prescription prescription)
    {
        ApiResponse<Prescription> response = new ApiResponse<Prescription>();
        Response.Headers.ContentType = "application/json";

        var state = (PrescriptionState)stateInt;
        if (!Enum.IsDefined(state) || state is PrescriptionState.Unknown)
        {
            return BadRequest(ApiResponse.Error("Invalid state"));
        }

        PrescriptionStateMachine prescriptionStateMachine = _stateMachineFactoryService.GetStateMachine(prescription);
        try
        {
            prescriptionStateMachine.ProgressState(state);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Desired State {state} Not Recognized", state);
            return BadRequest(ApiResponse.Error("Invalid state"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed To Progress State");
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }

        try
        {
            await _prescriptionDatabaseService.SetPrescriptionById(prescription.PrescriptionId, prescription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update prescription {id}", prescription.PrescriptionId);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }

        response.Data = prescription;
        _logger.LogInformation("Prescription State Progressed to {state}", state);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] Prescription prescription)
    {
        ApiResponse<Prescription> response = new ApiResponse<Prescription>();
        Response.Headers.ContentType = "application/json";

        Guid prescriptionId = Guid.NewGuid();
        prescription.PrescriptionId = prescriptionId;
        AccountCredentials prescribedUser;
        try
        {
             prescribedUser = await _accountDatabaseService.GetAccountById(prescription.PatientId);
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find account {id} associated with prescription {id}", prescription.PatientId, prescriptionId);
            return NotFound(ApiResponse.Error("Not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "failed to fetch account {id}", prescription.PatientId);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        if (prescribedUser.AccountDetails.AccountType is not AccountType.Patient)
        {
            _logger.LogError("Cannot prescribe to a non-patient account");
            return BadRequest(ApiResponse.Error("Cannot prescribe to a non-patient account"));
        }
        try
        {
            await _prescriptionDatabaseService.SetPrescriptionById(prescriptionId, prescription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set new prescription {id}", prescriptionId);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        response.Data = prescription;
        _logger.LogInformation("New Prescription {id} Set", prescriptionId);
        return Ok(response);
    }
}
