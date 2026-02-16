namespace Drugly.AvaloniaApp.Models;

public record AccountSession
{
    public string SessionToken { get; }
    public AccountType AccountType { get; }
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