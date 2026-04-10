using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Drugly.AvaloniaApp.Converters;

/// <summary>
/// An assortment of double-based <see cref="IValueConverter"/>s.
/// </summary>
internal static class NumberConverters
{
    // Arithmetic
    public static readonly IValueConverter Add = new ConverterImpl<double>((a, b) => a + b, (a, b) => a - b);
    public static readonly IValueConverter Subtract = new ConverterImpl<double>((a, b) => a - b, (a, b) => a + b);
    public static readonly IValueConverter Multiply = new ConverterImpl<double>((a, b) => a * b, (a, b) => a / b);
    public static readonly IValueConverter Divide = new ConverterImpl<double>((a, b) => a / b, (a, b) => a * b);
    public static readonly IValueConverter Modulo = new ConverterImpl<double>((a, b) => a % b);

    // Greater than/less than
    public static readonly IValueConverter GreaterThan = new ConverterImpl<bool>((a, b) => a > b);
    public static readonly IValueConverter GreaterThanOrEqual = new ConverterImpl<bool>((a, b) => a >= b);
    public static readonly IValueConverter LessThan = new ConverterImpl<bool>((a, b) => a < b);
    public static readonly IValueConverter LessThanOrEqual = new ConverterImpl<bool>((a, b) => a <= b);

    // Min/max
    public static readonly IValueConverter Min = new ConverterImpl<double>(Math.Min);
    public static readonly IValueConverter Max = new ConverterImpl<double>(Math.Max);
    public static readonly IMultiValueConverter MultiMin = new MultiConverterImpl<double>(double.MaxValue, Math.Min);
    public static readonly IMultiValueConverter MultiMax = new MultiConverterImpl<double>(double.MinValue, Math.Max);

    private class ConverterImpl<TResult>(
        Func<double, double, TResult> convert,
        Func<double, double, TResult>? convertBack = null
    ) : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (TryCastToDouble(value, out var val) && TryCastToDouble(parameter, out var param))
            {
                return convert(val, param);
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (convertBack is null)
            {
                return AvaloniaProperty.UnsetValue;
            }

            if (TryCastToDouble(value, out var val) && TryCastToDouble(parameter, out var param))
            {
                return convertBack(val, param);
            }

            return AvaloniaProperty.UnsetValue;
        }
    }

    private class MultiConverterImpl<TResult>(
        TResult initialValue,
        Func<TResult, double, TResult> convert
    ) : IMultiValueConverter
        where TResult : struct
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            var accumulator = initialValue;
            foreach (var value in values)
            {
                if (!TryCastToDouble(value, out var val))
                {
                    continue;
                }

                accumulator = convert(accumulator, val);
            }

            if (accumulator.Equals(initialValue))
            {
                return BindingOperations.DoNothing;
            }

            return accumulator;
        }
    }

    private static bool TryCastToDouble(object? value, out double d)
    {
        if (value is double val || (value is string s && double.TryParse(s, out val)))
        {
            d = val;
            return true;
        }

        try
        {
            d = Convert.ToDouble(value);
            return true;
        }
        catch
        {
            d = 0;
            return false;
        }
    }
}