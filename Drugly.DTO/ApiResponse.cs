namespace Drugly.DTO;

public class ApiResponse<T>
{
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }
}

public static class ApiResponse {
    public static ApiResponse<object?> Error(string message) => new() {
        ErrorMessage = message,
        Data = null
    };
}