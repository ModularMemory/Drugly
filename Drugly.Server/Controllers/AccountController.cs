using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

/// <summary>Controller class to handle HTTP requests relating to account management</summary>
public class AccountController : DruglyController
{
    /// <summary>The database service object</summary>
    private readonly IAccountDatabaseService _databaseService;

    /// <summary>The authorization service object</summary>
    private readonly IAuthorizationService _authorizationService;

    /// <summary>The logger object</summary>
    private readonly ILogger<AccountController>  _logger;

    /// <summary>The constructor for the Account controller for including dependencies</summary>
    /// <param name="databaseService">The database service</param>
    /// <param name="logger">The logger</param>
    /// <param name="authService">The authentication service</param>
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

    /// <summary>A route to get an account's details by the ID of the account</summary>
    /// <param name="id">The ID of the account being fetched</param>
    /// <returns>The response object with the account details in the body</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<AccountDetails> response = new ApiResponse<AccountDetails>();
        Response.Headers.ContentType = "application/json";

        try
        {
            AccountCredentials entry = await _databaseService.GetAccountById(id);
            response.Data = entry.AccountDetails;
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

    /// <summary>A route to get the ID of an account by the account's associated email</summary>
    /// <param name="email">The email being searched for</param>
    /// <returns>A response object that contains the ID in the body</returns>
    [HttpGet]
    public async Task<IActionResult> GetIdByEmail([FromBody] string email)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }
        ApiResponse<Guid> response = new ApiResponse<Guid>();
        Response.Headers.ContentType = "application/json";

        try
        {
            response.Data = await _databaseService.GetIdByEmail(email);
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

    /// <summary>A route to set new account details for a given user ID</summary>
    /// <param name="id">The user ID being set</param>
    /// <param name="details">The new details</param>
    /// <param name="login">The login information for the account</param>
    /// <returns>An Ok response if successful</returns>
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> SetById(Guid id, [FromBody] AccountCredentials accountCredentials)
    {
        try
        {
            await _databaseService.SetAccountById(id, accountCredentials.AccountDetails.Email, accountCredentials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set account {id} details", id);
            return InternalServerError(ApiResponse.Error("Internal Server Error"));
        }
        _logger.LogInformation("Account {id} details successfully set", id);
        return Ok();
    }

    /// <summary>A route for loggin in</summary>
    /// <param name="loginRequest">An object that contains the login information</param>
    /// <returns>A response object with the assigned session in the body</returns>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        ApiResponse<AccountSession> response = new ApiResponse<AccountSession>();
        Response.Headers.ContentType = "application/json";
        Guid id;
        AccountCredentials entry;
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

    [HttpDelete]
    public async Task<IActionResult> Logout()
    {

        if (!_authorizationService.DeleteSession(Request.Headers))
        {
            _logger.LogError("Logout failed");
            return BadRequest(ApiResponse.Error("Internal Server Error"));
        }

        _logger.LogInformation("Logout successful");
        return Ok();
    }

    /// <summary>A route that fetches all patient accounts in the database</summary>
    /// <returns>A list of patient's account details</returns>
    [HttpGet]
    public async Task<IActionResult> GetPatientAccounts()
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, AccountType.Doctor))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        ApiResponse<AccountDetails[]> response = new ApiResponse<AccountDetails[]>();
        Response.Headers.ContentType = "application/json";

        try
        {
            response.Data = await _databaseService.GetAllPatientAccounts();
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