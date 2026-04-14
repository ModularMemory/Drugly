using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

/// <summary>A controller that handles routes for managing images</summary>
public class ImageController : DruglyController
{
    private readonly IImageDatabaseService _databaseService;
    private readonly ILogger<ImageController>  _logger;
    private readonly IAuthorizationService _authorizationService;
    public ImageController(
        IImageDatabaseService databaseService,
        ILogger<ImageController> logger,
        IAuthorizationService authorizationService
    )
    {
        _databaseService = databaseService;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, [AccountType.Doctor, AccountType.Patient]))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        Stream response;
        try
        {
            response = await _databaseService.GetImageById(id, out var contentType);
            Response.ContentType= contentType;
        }
        catch (ImageNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to find image {id}", id);
            return NotFound(ApiResponse.Error("Image not found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch image {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Image {id} retrieved successfully", id);
        return Ok(response);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> SetById(string id, [FromBody] Stream content)
    {
        if (!_authorizationService.IsUserAuthorized(Request.Headers, AccountType.Doctor))
        {
            _logger.LogInformation("User is not authorized");
            return Forbid(ApiResponse.Error("User is not authorized"));
        }

        string contentType = Request.ContentType ?? "image/bmp";
        try
        {
            await _databaseService.SetImageById(id, contentType, content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set image {id}", id);
            return InternalServerError(ApiResponse.Error("Internal server error"));
        }
        _logger.LogInformation("Image {id} set successfully", id);
        return Ok();
    }
}