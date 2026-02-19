using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Drugly.Validation;

public static class ValidatorCache<TValidator> where TValidator : ValidationAttribute, new()
{
    private static readonly TValidator Validator = new();

    /// <summary>
    /// Determines if a <paramref name="value"/> is valid according to the <typeparamref name="TValidator"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="message">An error message associated with the failed validation.</param>
    /// <returns><see langword="true"/> if the value is valid, otherwise <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsValid(object? value, [NotNullWhen(false)] out string? message)
    {
        var res = Validator.IsValid(value);

        if (!res)
        {
            // Ideally this would be logged instead of asserted
            // but because this is a static class and our projects don't share a common ILogger, we can't
            Debug.Assert(Validator.ErrorMessage != null, $"{typeof(TValidator).Name} error message was null for {value}.");
            message = Validator.ErrorMessage ?? "Field is not valid.";
        }
        else
        {
            message = null;
        }

        return res;
    }
}