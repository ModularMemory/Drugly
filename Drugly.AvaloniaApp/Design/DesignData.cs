using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
    private class DesignApplication : Application
    {
        public DesignApplication()
        {
            ApplicationLifetime = new ClassicDesktopStyleApplicationLifetime();
        }
    }

    static DesignData()
    {
        try
        {
            throw new Exception("Example exception");
        }
        catch (Exception ex)
        {
            ExampleException = ex;
        }
    }

    private static Exception ExampleException { get; }

    private static IServiceProvider ServiceProvider
        => field ??= new ServiceCollection()
            .ConfigureServices(new DesignApplication())
            .ConfigureViews()
            .BuildServiceProvider();
}