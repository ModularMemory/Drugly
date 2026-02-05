using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Drugly.AvaloniaApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace Drugly.AvaloniaApp;

public static class Startup
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection ConfigureServices(Application application)
        {
            if (application.ApplicationLifetime is { } lifetime)
            {
                // ReSharper disable once RedundantTypeArgumentsOfMethod
                serviceCollection.AddSingleton<IApplicationLifetime>(lifetime);
            }

            return serviceCollection
                // Logging
                .AddKeyedTransient<LoggerTextWriter>(LogEventLevel.Verbose)
                .AddSingleton<ILogger, Logger>(provider =>
                    new LoggerConfiguration()
#if DEBUG
                        .MinimumLevel.Verbose()
#else
                        .MinimumLevel.Debug()
#endif
                        .WriteTo.Console()
                        .WriteTo.File($"{nameof(Drugly)}_Log.txt")
                        .CreateLogger())
                // Misc
                .AddSingleton<ILoginService, LoginService>()
                // UI
                .AddSingleton<ISukiToastManager, SukiToastManager>()
                .AddSingleton<ISukiDialogManager, SukiDialogManager>()
                .AddSingleton<SukiDialogHost>(provider => new SukiDialogHost
                {
                    Manager = provider.GetRequiredService<ISukiDialogManager>()
                });
        }

        public IServiceCollection ConfigureViews()
        {
            var builder = new ViewMapBuilder(serviceCollection)
                .AddView<StartupWindow, StartupWindowViewModel>()
                .AddView<MainView, MainViewModel>()
                .AddView<MainWindow, MainWindowViewModel>();

            return serviceCollection
                .AddSingleton<ViewLocator>()
                .AddSingleton<IViewMap, ViewMap>(provider => builder.Build(provider));
        }
    }
}