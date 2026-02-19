using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels.Pages;
using Drugly.AvaloniaApp.ViewModels.Windows;
using Drugly.AvaloniaApp.Views.Pages;
using Drugly.AvaloniaApp.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
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

            serviceCollection
                // HTTP
                .ConfigureHttpClientDefaults(builder =>
                {
                    builder
                        .ConfigureHttpClient(ConfigureHttpClient)
                        .ConfigurePrimaryHttpMessageHandler(ConfigureHttpMessageHandler)
                        .AddResilienceHandler("Retry", ConfigureHttpRetryPolicy);
                })
                .AddHttpClient(nameof(ILoginService), client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                });

            serviceCollection
                // Logging
                .AddKeyedTransient<LoggerTextWriter>(LogEventLevel.Verbose)
                .AddSingleton<ILogger, Logger>(_ =>
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

            return serviceCollection;
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

        private static void ConfigureHttpClient(HttpClient client)
        {
            var assemblyVersion = typeof(App).Assembly.Version;

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"{nameof(Drugly)}/{assemblyVersion} ({Environment.OSVersion.Platform}; {Environment.OSVersion.Version})");
        }

        [SuppressMessage("Performance", "CA1859")]
        private static HttpMessageHandler ConfigureHttpMessageHandler()
        {
#if DEBUG
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
#else
            return new HttpClientHandler();
#endif
        }

        private static void ConfigureHttpRetryPolicy(ResiliencePipelineBuilder<HttpResponseMessage> builder)
        {
            builder
                .AddRetry(new HttpRetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    MaxRetryAttempts = 6
                })
                .AddTimeout(TimeSpan.FromSeconds(15));
        }
    }
}