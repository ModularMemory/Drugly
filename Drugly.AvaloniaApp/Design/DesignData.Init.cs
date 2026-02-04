using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
    static DesignData() {
        try {
            throw new Exception("Example exception");
        }
        catch (Exception ex) {
            ExampleException = ex;
        }
    }

    private class DesignApplication : Application {
        public DesignApplication() {
            ApplicationLifetime = new ClassicDesktopStyleApplicationLifetime();
        }
    }

    private static partial IServiceProvider ServiceProvider =>
        field ??= new ServiceCollection()
            .ConfigureServices(new DesignApplication())
            .ConfigureViews()
            .BuildServiceProvider();
}