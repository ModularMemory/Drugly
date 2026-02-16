using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages;
using Drugly.AvaloniaApp.ViewModels.Windows;
using Drugly.AvaloniaApp.Views.Pages;
using Drugly.AvaloniaApp.Views.Windows;
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
                .AddSingleton<IAccountSessionService, AccountSessionService>()
                // UI
                .AddSingleton<ISukiToastManager, SukiToastManager>()
                .AddSingleton<ISukiDialogManager, SukiDialogManager>()
                .AddTransient<SukiDialogHost>(provider => new SukiDialogHost
                {
                    Manager = provider.GetRequiredService<ISukiDialogManager>()
                });
        }

        public IServiceCollection ConfigureViews()
        {
            var builder = new ViewMapBuilder(serviceCollection)
                .AddView<StartupWindow, StartupWindowViewModel>()
                .AddView<PatientMainView, PatientMainViewModel>()
                .AddView<DoctorMainView, DoctorMainViewModel>()
                .AddView<PharmacistMainView, PharmacistMainViewModel>()
                .AddView<MainView, MainViewModel>()
                .AddView<MainWindow, MainWindowViewModel>();

            return serviceCollection
                .AddSingleton<ViewLocator>()
                .AddSingleton<IViewMap, ViewMap>(provider => builder.Build(provider));
        }
    }
}