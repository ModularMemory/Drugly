namespace Drugly.DTO;

/// <summary>Holds either an error message or a data object. Never both.</summary>
/// <typeparam name="T">The data type.</typeparam>
public class ApiResponse<T>
{
    /// <summary>The optional error message.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>The optional data.</summary>
    public T? Data { get; set; }
}

/// <summary>Helpful static methods related to <see cref="ApiResponse{T}"/>.</summary>
public static class ApiResponse
{
    /// <summary>Creates a <see cref="ApiResponse{T}"/> with an error message and no data.</summary>
    /// <param name="message">The error message to use.</param>
    /// <returns>The created error object.</returns>
    public static ApiResponse<object?> Error(string message) => new()
    {
        ErrorMessage = message,
        Data = null
    };
}