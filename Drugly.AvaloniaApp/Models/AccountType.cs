using Drugly.DTO;

namespace Drugly.AvaloniaApp.Models;

/// <summary>The type of an account.</summary>
public enum AccountType
{
    /// <summary>Default value. The account is something else.</summary>
    Unknown,
    /// <summary>The account is a patient.</summary>
    Patient,
    /// <summary>The account is a doctor.</summary>
    Doctor,
}

/// <summary>Provides extensions methods for converting to and form <see cref="AccountType"/> and <see cref="AccountTypeDto"/>.</summary>
public static class AccountTypeExtensions
{
    public static AccountType ToAccountType(this AccountTypeDto dto)
    {
        return dto switch
        {
            AccountTypeDto.Patient => AccountType.Patient,
            AccountTypeDto.Doctor => AccountType.Doctor,
            _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, null)
        };
    }

    public static AccountTypeDto ToAccountTypeDto(this AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Patient => AccountTypeDto.Patient,
            AccountType.Doctor => AccountTypeDto.Doctor,
            _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
        };
    }

    public static bool CanConvertToDto(this AccountType accountType)
        => accountType is AccountType.Patient or AccountType.Doctor;
}