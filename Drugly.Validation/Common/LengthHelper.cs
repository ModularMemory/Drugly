using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Drugly.Validation.Common;

internal static class LengthHelper
{
    /// <summary>
    /// Tries to get the length from an <see cref="object"/> of unknown type.
    /// </summary>
    /// <returns><see langword="true"/> if a length could be found, otherwise <see langword="false"/>.</returns>
    [RequiresUnreferencedCode("Uses reflection to get Count/Length properties for objects that are not strings and do not implement ICollection")]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
    public static bool TryGetLength(object? value, out int length)
    {
        if (value is string str)
        {
            length = str.Length;
            return true;
        }

        if (value is ICollection col)
        {
            length = col.Count;
            return true;
        }

        foreach (var propName in (IEnumerable<string>)["Count", "Length"])
        {
            var property = value?.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (property is { CanRead: true } && property.PropertyType == typeof(int))
            {
                length = (int)property.GetValue(value)!;
                return true;
            }
        }

        length = 0;
        return false;
    }
}