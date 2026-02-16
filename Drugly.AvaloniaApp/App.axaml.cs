using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    private ILogger _logger = Logger.None;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Dispatcher.UIThread.UnhandledException += UIThread_OnUnhandledException;
        DisableAvaloniaDataAnnotationValidation();

        _serviceProvider = new ServiceCollection()
            .ConfigureServices(this)
            .ConfigureViews()
            .BuildServiceProvider();

#if DEBUG
        Trace.Listeners.Add(new TextWriterTraceListener(_serviceProvider.GetRequiredKeyedService<LoggerTextWriter>(LogEventLevel.Verbose)));
#endif
        DataTemplates.Add(_serviceProvider.GetRequiredService<ViewLocator>());
        _logger = _serviceProvider.GetRequiredService<ILogger>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Show the startup window first
            desktop.MainWindow = _serviceProvider.GetRequiredView<StartupWindow>();

            var loginService = _serviceProvider.GetRequiredService<ILoginService>();
            loginService.LoginSuccessful += (s, e) => Dispatcher.UIThread.Invoke(() =>
            {
                // Swap the old main window with a new instance of MainWindow
                // Replaces the startup window at login, or relaunches the MainWindow on re-login
                var oldWindow = desktop.MainWindow;

                var mainWindow = _serviceProvider.GetRequiredView<MainWindow>();
                desktop.MainWindow = mainWindow;

                mainWindow.Show();
                oldWindow.Close();
            });
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void UIThread_OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.Error(e.Exception, "Unhandled exception");

        var applicationLifetime = _serviceProvider?.GetService<IApplicationLifetime>();
        var dialogManager = _serviceProvider?.GetService<ISukiDialogManager>();
        if (applicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || dialogManager is null)
        {
            return;
        }

        dialogManager.CreateDialog()
            .OfType(NotificationType.Error)
            .WithTitle("Fatal Error")
            .WithGroupedContent(
                new TextBlock
                {
                    Text = $"An uncaught exception occurred. {nameof(Drugly)} can resume but may be unstable."
                },
                new TextBlock
                {
                    Text = e.Exception.ToString(),
                    FontSize = 12,
                    Foreground = Brushes.Red
                })
            .WithColoredYesNoResult("Resume", "Exit")
            .OnClosed(res =>
            {
                if (!res)
                {
                    desktop.Shutdown(e.Exception.HResult);
                }
            })
            .TryShow();

        e.Handled = true;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}