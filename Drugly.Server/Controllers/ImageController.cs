using Drugly.DTO;
using Drugly.Server.Models;
using Drugly.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Drugly.Server.Controllers;

public class ImageController : DruglyController
{
    private readonly IImageDatabaseService _databaseService;
    private readonly ILogger<ImageController>  _logger;

    public ImageController(
        IImageDatabaseService databaseService,
        ILogger<ImageController> logger
    )
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
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