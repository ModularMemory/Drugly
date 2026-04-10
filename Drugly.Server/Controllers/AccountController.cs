using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
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


        return Ok();
    }
}