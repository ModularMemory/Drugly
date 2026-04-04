using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.ViewModels.Pages;
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

    public static PrescriptionViewModel[] ExamplePrescriptions =>
    [
        new("Estrogen", "Mreowww", "https://www.reddit.com/media?url=https%3A%2F%2Fi.redd.it%2F2yp7s912k6m81.jpg"),
        new("Addherall", "Girl, you need to focus.", "https://f4.bcbits.com/img/a4229702017_10.jpg"),
    ];

    public static PrescriptionViewModel ExamplePrescription => ExamplePrescriptions[0];

    private static IServiceProvider ServiceProvider
        => field ??= new ServiceCollection()
            .ConfigureServices(new DesignApplication())
            .ConfigureViews()
            .BuildServiceProvider();
}