using Drugly.AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
    private static Exception ExampleException { get; }
    private static partial IServiceProvider ServiceProvider { get; }

    public static MainWindowViewModel MainWindowViewModel
        => field ??= ServiceProvider.GetRequiredService<MainWindowViewModel>();

    public static MainViewModel MainViewModel
    {
        get
        {
            if (field != null)
            {
                return field;
            }

            field = ServiceProvider.GetRequiredService<MainViewModel>();

            return field;
        }
    }
}