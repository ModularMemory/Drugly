using System.Diagnostics;
using System.Reflection;
using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

[ApiController]
[Route("[controller]")]
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
    [HttpGet(nameof(GetById))]
    public async Task<IActionResult> GetById(Guid id)
    {
        ApiResponse<AccountDetails> response = new ApiResponse<AccountDetails>();

        try
        {
            response.Data = await _databaseService.GetAccountById(id);
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
}