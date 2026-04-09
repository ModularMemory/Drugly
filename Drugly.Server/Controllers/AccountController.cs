using Drugly.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    // /Account/Get
    [HttpGet(nameof(Get))]
    public async Task<IActionResult> Get(int id)
    {


        return Ok();
    }
}