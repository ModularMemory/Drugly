using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Drugly.Validation;

/// <summary>Stores cached <see cref="ValidationAttribute"/> instances for manually validating values.</summary>
/// <typeparam name="TValidator">The <see cref="ValidationAttribute"/> to validate with.</typeparam>
public static class ValidatorCache<TValidator> where TValidator : ValidationAttribute, new()
{
    private static readonly TValidator Validator = new();

    /// <summary>Determines if a <paramref name="value"/> is valid according to the <typeparamref name="TValidator"/>.</summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="message">An error message associated with the failed validation.</param>
    /// <returns><see langword="true"/> if the value is valid, otherwise <see langword="false"/>.</returns>
    public static bool IsValid(object? value, [NotNullWhen(false)] out string? message)
        => IsValidNoCache(Validator, value, out message);

    /// <summary>Determines if a <paramref name="value"/> is valid according to the <paramref name="validator"/>.</summary>
    /// <param name="validator">The validator to validate with.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="message">An error message associated with the failed validation.</param>
    /// <returns><see langword="true"/> if the value is valid, otherwise <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsValidNoCache(TValidator validator, object? value, [NotNullWhen(false)] out string? message)
    {
        ArgumentNullException.ThrowIfNull(validator);

        var res = validator.IsValid(value);

        if (!res)
        {
            // Ideally this would be logged instead of asserted
            // but because this is a static class and our projects don't share a common ILogger, we can't
            Debug.Assert(validator.ErrorMessage != null, $"{validator.GetType().Name} error message was null for {value}.");
            message = validator.ErrorMessage ?? "Field is not valid.";
        }
        else
        {
            message = null;
        }

        return res;
    }
}