using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public class AccountController : DruglyController
{
    private readonly IAccountDatabaseService _databaseService;
    private readonly ILogger<AccountController>  _logger;

    public AccountController(
        IAccountDatabaseService databaseService,
        ILogger<AccountController> logger
    )
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    // /Account/GetById
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
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
    public async Task<IActionResult> GetIdByEmail([FromBody] string email)
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
}