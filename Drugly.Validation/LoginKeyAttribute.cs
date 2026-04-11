using System.ComponentModel.DataAnnotations;
using Drugly.Validation.Common;

namespace Drugly.Validation;

/// <summary>Validates if a variable follows the format of login keys.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class LoginKeyAttribute : ValidationAttribute
{
    private const int MIN_LENGTH = 2;

    public override bool IsValid(object? value)
    {
        if (!LengthHelper.TryGetLength(value, out var length) || length < MIN_LENGTH)
        {
            ErrorMessage = $"Key must contain at least {MIN_LENGTH} characters.";
            return false;
        }

        ErrorMessage = null;
        return true;
    }
}