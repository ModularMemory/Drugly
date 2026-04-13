using Drugly.AvaloniaApp.Models;
using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public class AccountController : DruglyController
{
    private readonly IAccountDatabaseService _databaseService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<AccountController>  _logger;

    public AccountController(
        IAccountDatabaseService databaseService,
        ILogger<AccountController> logger,
        IAuthorizationService authService
    )
    {
        _databaseService = databaseService;
        _logger = logger;
        _authorizationService = authService;
    }

    // /Account/GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) // TODO: add auth
    {
        ApiResponse<AccountDetails> response = new ApiResponse<AccountDetails>();

        try
        {
            response.Data = await _databaseService.GetAccountById(id);
            Response.Headers.ContentType = "application/json";
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find account {id}", id);
            return NotFound(ApiResponse.Error("Account not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch account {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Account {id} successfully retrieved", id);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetIdByEmail([FromBody] string email) // TODO: add auth
    {
        ApiResponse<Guid> response = new ApiResponse<Guid>();

        try
        {
            response.Data = await _databaseService.GetIdByEmail(email);
            Response.Headers.ContentType = "application/json";
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find Id associated with email: {email}", email);
            return NotFound(ApiResponse.Error("Id not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch Id associated with email: {email}", email);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Id associated with {email} successfully retrieved", email);
        return Ok(response);
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> SetById(Guid id, [FromBody] AccountDetails detailsDto)
    {
        try
        {
            await _databaseService.SetAccountById(id, detailsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set account {id} details", id);
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Account {id} details successfully set", id);
        return Ok();
    }

    [HttpGet("{???}")] // idk what information I'm getting for this
    public async Task<IActionResult> Login()
    {
        ApiResponse<AccountSession> response = new ApiResponse<AccountSession>();
        try
        {
            _authorizationService.Authorize();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to authorize ");
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Login successful");
        return Ok(response);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPatientAccounts() // TODO: add auth
    {
        ApiResponse<List<AccountDetails>> response = new ApiResponse<List<AccountDetails>>();
        try
        {
            _databaseService.GetAllPatientAccounts();
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find any patient accounts");
            return NotFound(ApiResponse.Error("Accounts not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch patient accounts");
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Patient account found successfully");
        return Ok(response);
    }
}