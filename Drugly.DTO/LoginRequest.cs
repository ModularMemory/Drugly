namespace Drugly.DTO;

/// <summary>A login request object.</summary>
public sealed class LoginRequest
{
    /// <summary>The email.</summary>
    public required string Email { get; set; }

    /// <summary>The password.</summary>
    public required string Password { get; set; }
}