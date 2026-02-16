using Drugly.DTO;

namespace Drugly.AvaloniaApp.Models;

public enum AccountType
{
    Unknown,
    Patient,
    Doctor,
    Pharmacist
}

public static class AccountTypeExtensions
{
    public static AccountType ToAccountType(this AccountTypeDto dto)
    {
        return dto switch
        {
            AccountTypeDto.Patient => AccountType.Patient,
            AccountTypeDto.Doctor => AccountType.Doctor,
            AccountTypeDto.Pharmacist => AccountType.Pharmacist,
            _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, null)
        };
    }

    public static AccountTypeDto ToAccountTypeDto(this AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Patient => AccountTypeDto.Patient,
            AccountType.Doctor => AccountTypeDto.Doctor,
            AccountType.Pharmacist => AccountTypeDto.Pharmacist,
            _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
        };
    }

    public static bool CanConvertToDto(this AccountType accountType)
        => accountType is AccountType.Patient or AccountType.Doctor or AccountType.Pharmacist;
}