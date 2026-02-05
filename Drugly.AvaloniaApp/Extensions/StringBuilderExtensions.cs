using System.Text;

namespace Drugly.AvaloniaApp.Extensions;

public static class StringBuilderExtensions
{
    extension(StringBuilder sb)
    {
        public StringBuilder TrimEnd(char character) => sb.TrimEnd([character]);

        public StringBuilder TrimEnd(ReadOnlySpan<char> chars)
        {
            var end = sb.Length - 1;
            while (end > 0 && chars.Contains(sb[end]))
            {
                end--;
            }

            return sb.Remove(end, sb.Length - end);
        }
    }
}