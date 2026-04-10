using Drugly.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    // /Account/Get
    [HttpGet(nameof(GetAccount))]
    public async Task<IActionResult> GetAccount(int id)
    {


        return Ok();
    }
}