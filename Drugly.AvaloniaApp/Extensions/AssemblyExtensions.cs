using System.Reflection;

namespace Drugly.AvaloniaApp.Extensions;

/// <summary>Provides extensions for <see cref="Assembly"/>s.</summary>
public static class AssemblyExtensions
{
    extension(Assembly assembly)
    {
        /// <summary>Gets the <see cref="Version"/> of an <see cref="assembly"/>.</summary>
        /// <exception cref="InvalidOperationException">The <see cref="assembly"/> does not have a version.</exception>
        public Version Version
            => assembly.GetName().Version ?? throw new InvalidOperationException($"Assembly '{assembly}' does not have a version.");
    }
}