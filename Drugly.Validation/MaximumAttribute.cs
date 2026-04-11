using System.ComponentModel.DataAnnotations;

namespace Drugly.Validation;

/// <summary>Specifies the maximum value of a data field.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MaximumAttribute : ValidationAttribute
{
    /// <summary>The inclusive maximum value.</summary>
    public decimal Max { get; }

    public MaximumAttribute(long max)
    {
        Max = max;
    }

    public MaximumAttribute(double max)
    {
        Max = (decimal)max;
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

        if (val > Max)
        {
            ErrorMessage = $"Value must be {Max} or less.";
            return false;
        }

        ErrorMessage = null;
        return true;
    }
}