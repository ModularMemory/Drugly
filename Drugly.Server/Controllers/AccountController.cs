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
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<AccountDetails> response = new ApiResponse<AccountDetails>();

        try
        {
            AccountDatabaseEntry entry = await _databaseService.GetAccountById(id);
            response.Data = entry.AccountDetails;
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
    public async Task<IActionResult> GetIdByEmail([FromBody] string email)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }
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
    public async Task<IActionResult> SetById(Guid id, [FromBody] AccountDetails details, LoginRequest login)
    {
        AccountDatabaseEntry entry = new AccountDatabaseEntry(login.Password, details);
        try
        {
            await _databaseService.SetAccountById(id, login.Email, entry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set account {id} details", id);
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Account {id} details successfully set", id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        ApiResponse<AccountSession> response = new ApiResponse<AccountSession>();
        Guid id;
        AccountDatabaseEntry entry;
        // get the ID from the email
        try
        {
            id = await _databaseService.GetIdByEmail(loginRequest.Email);
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Id associated with email {email} not found", loginRequest.Email);
            return NotFound(ApiResponse.Error("Account not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch id {email}", loginRequest.Email);
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }

        // Get the database entry from the ID
        try
        {
            entry = await _databaseService.GetAccountById(id);
        }
        catch (AccountNotFoundException ex)
        {
            _logger.LogError(ex, "Entry associated with Id {id} not found", id);
            return NotFound(ApiResponse.Error("Account not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch account {id}", id);
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }

        // check for correct password for given email
        if (entry.Password != loginRequest.Password)
        {
            _logger.LogInformation("Invalid password");
            return Forbid(ApiResponse.Error("Invalid email or password"));
        }

        try
        {
            response.Data = _authorizationService.CreateSession(entry.AccountDetails);
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
    public async Task<IActionResult> GetPatientAccounts()
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, AccountType.Doctor))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<List<AccountDetails>> response = new ApiResponse<List<AccountDetails>>();
        try
        {
            await _databaseService.GetAllPatientAccounts();
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