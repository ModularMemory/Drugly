using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Drugly.AvaloniaApp.Converters;

public static class NumberConverters {
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

    private class ConverterImpl<TResult>(
        Func<double, double, TResult> convert,
        Func<double, double, TResult>? convertBack = null
    ) : IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (TryCastToDouble(value, out var val) && TryCastToDouble(parameter, out var param)) {
                return convert(val, param);
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (convertBack is null) {
                return AvaloniaProperty.UnsetValue;
            }

            if (TryCastToDouble(value, out var val) && TryCastToDouble(parameter, out var param)) {
                return convertBack(val, param);
            }

            return AvaloniaProperty.UnsetValue;
        }

        private static bool TryCastToDouble(object? value, out double d) {
            if (value is double val || (value is string s && double.TryParse(s, out val))) {
                d = val;
                return true;
            }

            try {
                d = System.Convert.ToDouble(value);
                return true;
            }
            catch {
                d = 0;
                return false;
            }
        }
    }
}