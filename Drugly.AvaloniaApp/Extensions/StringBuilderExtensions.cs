using System.Text;

namespace Drugly.AvaloniaApp.Extensions;

public static class StringBuilderExtensions
{
    extension(StringBuilder sb)
    {
        /// <summary>
        /// Removes all trailing occurrences of a character.
        /// </summary>
        /// <param name="character">The character to look for and remove.</param>
        /// <returns>The <see cref="StringBuilder"/> instance.</returns>
        public StringBuilder TrimEnd(char character) => sb.TrimEnd([character]);

        /// <summary>
        /// Removes all trailing occurrences of a set of characters.
        /// </summary>
        /// <param name="chars">The set of characters to look for and remove.</param>
        /// <returns>The <see cref="StringBuilder"/> instance.</returns>
        public StringBuilder TrimEnd(ReadOnlySpan<char> chars)
        {
            var end = sb.Length - 1;
            while (end > 0 && chars.Contains(sb[end]))
            {
                end--;
            }

            if (end < 0)
            {
                end = 0;
            }

            return sb.Remove(end, sb.Length - end);
        }
    }
}