using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Drugly.AvaloniaApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace Drugly.AvaloniaApp;

public static class Startup {
    extension(IServiceCollection serviceCollection) {
        public IServiceCollection ConfigureServices(Application application) {
            if (application.ApplicationLifetime is { } lifetime) {
                // ReSharper disable once RedundantTypeArgumentsOfMethod
                serviceCollection.AddSingleton<IApplicationLifetime>(lifetime);
            }

            return serviceCollection
                // Logging
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
                // UI
                .AddSingleton<ISukiToastManager, SukiToastManager>()
                .AddSingleton<ISukiDialogManager, SukiDialogManager>();
        }

        public IServiceCollection ConfigureViews() {
            var builder = new ViewMapBuilder(serviceCollection)
                .AddView<MainView, MainViewModel>()
                .AddView<MainWindow, MainWindowViewModel>();

            return serviceCollection
                .AddSingleton<ViewLocator>()
                .AddSingleton<IViewMap, ViewMap>(provider => builder.Build(provider));
        }
    }
}