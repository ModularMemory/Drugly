using System.ComponentModel.DataAnnotations;

namespace Drugly.Validation;

public static class NumberValidator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MinimumAttribute : ValidationAttribute
    {
        private readonly decimal _min;

        public MinimumAttribute(long min)
        {
            _min = min;
        }

        public MinimumAttribute(double min)
        {
            _min = (decimal)min;
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

            if (val < _min)
            {
                ErrorMessage = $"Value must be {_min} or greater.";
                return false;
            }

            ErrorMessage = null;
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MaximumAttribute : ValidationAttribute
    {
        private readonly decimal _max;

        public MaximumAttribute(long max)
        {
            _max = max;
        }

        public MaximumAttribute(double max)
        {
            _max = (decimal)max;
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

            if (val > _max)
            {
                ErrorMessage = $"Value must be {_max} or less.";
                return false;
            }

            ErrorMessage = null;
            return true;
        }
    }
}