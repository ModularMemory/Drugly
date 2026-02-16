using Drugly.AvaloniaApp.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Design;

public static partial class DesignData
{
    public static StartupWindowViewModel StartupWindowViewModel
        => field ??= ServiceProvider.GetRequiredService<StartupWindowViewModel>();

    public static MainWindowViewModel MainWindowViewModel
        => field ??= ServiceProvider.GetRequiredService<MainWindowViewModel>();
}