using System.Reflection;

namespace Drugly.AvaloniaApp.Extensions;

public static class AssemblyExtensions
{
    extension(Assembly assembly)
    {
        public Version Version
            => assembly.GetName().Version ?? throw new InvalidOperationException($"Assembly '{assembly}' doesnt not have a version.");
    }
}