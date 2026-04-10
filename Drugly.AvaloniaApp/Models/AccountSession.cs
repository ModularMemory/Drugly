namespace Drugly.AvaloniaApp.Models;

/// <summary>Represents an account session.</summary>
public record AccountSession
{
    /// <summary>The session token used to make authenticated HTTP requests.</summary>
    public string SessionToken { get; }

    /// <summary>The type of the account associated with the session.</summary>
    public AccountType AccountType { get; }

    /// <summary>The time when the session expires.</summary>
    public DateTimeOffset Expiration { get; }

    public AccountSession(string sessionToken, AccountType accountType, DateTimeOffset expiration)
    {
        ArgumentNullException.ThrowIfNull(sessionToken);
        if (accountType == AccountType.Unknown)
            throw new ArgumentException($"Account type cannot be {AccountType.Unknown}.", nameof(accountType));

        SessionToken = sessionToken;
        AccountType = accountType;
        Expiration = expiration;
    }

    /// <summary>Checks if the session has expired.</summary>
    /// <returns><see langword="true"/> if the session is valid, otherwise <see langword="false"/>.</returns>
    public bool IsValid() => DateTimeOffset.UtcNow > Expiration;
}