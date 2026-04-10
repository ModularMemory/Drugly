using Drugly.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public abstract class DruglyController : ControllerBase
{
    public IActionResult InternalServerError(object? value)
    {
        return  StatusCode(StatusCodes.Status500InternalServerError, value);
    }

    public IActionResult NotFound(object? value)
    {
        return StatusCode(StatusCodes.Status404NotFound, value);
    }
}