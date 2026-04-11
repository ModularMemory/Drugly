using System.ComponentModel.DataAnnotations;

namespace Drugly.Validation;

/// <summary>Specifies the minimum value of a data field.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinimumAttribute : ValidationAttribute
{
    /// <summary>The inclusive minimum value.</summary>
    public decimal Min { get; }

    public MinimumAttribute(long min)
    {
        Min = min;
    }

    public MinimumAttribute(double min)
    {
        Min = (decimal)min;
    }

    public override bool IsValid(object? value)
    {
        decimal val;
        try
        {
            val = Convert.ToDecimal(value);
        }
        catch
        {
            ErrorMessage = "Value must be a number.";
            return false;
        }

        if (val < Min)
        {
            ErrorMessage = $"Value must be {Min} or greater.";
            return false;
        }

        ErrorMessage = null;
        return true;
    }
}