using Drugly.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

/// <summary>An abstract controller for defining status codes</summary>
[ApiController]
[Route("[controller]/[action]")]
public abstract class DruglyController : ControllerBase
{
    public IActionResult InternalServerError(object? value)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, value);
    }

    public IActionResult Forbid(object? value)
    {
        return StatusCode(StatusCodes.Status403Forbidden, value);
    }
}