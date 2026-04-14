using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Drugly.AvaloniaApp.Extensions;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Drugly.AvaloniaApp.ViewModels.Pages;
using Drugly.AvaloniaApp.ViewModels.Pages.Doctor;
using Drugly.AvaloniaApp.ViewModels.Pages.Patient;
using Drugly.AvaloniaApp.ViewModels.Windows;
using Drugly.AvaloniaApp.Views;
using Drugly.AvaloniaApp.Views.Pages;
using Drugly.AvaloniaApp.Views.Pages.Doctor;
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
using PatientPrescriptionDetailsViewModel = Drugly.AvaloniaApp.ViewModels.Pages.Patient.PatientPrescriptionDetailsViewModel;

namespace Drugly.AvaloniaApp;

/// <summary>Provides <see cref="IServiceProvider"/> extensions for initializing <see cref="Drugly"/>.</summary>
public static class Startup
{
    extension(IServiceCollection serviceCollection)
    {
        /// <summary>Registers and configures the services required by <see cref="Drugly"/> in the given <see cref="IServiceCollection"/>.</summary>
        /// <param name="application">The current application instance.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public IServiceCollection ConfigureServices(Application application)
        {
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            serviceCollection.AddSingleton<Application>(application);
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
                }).Services
                .AddHttpClient(nameof(IAccountDetailsService), client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                }).Services
                .AddHttpClient(nameof(IMedicationDetailsService), client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
                }).Services
                .AddHttpClient(nameof(IPrescriptionDetailsService), client =>
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
                .AddSingleton<IAccountDetailsService, AccountDetailsService>()
                .AddSingleton<IMedicationDetailsService, MedicationDetailsService>()
                .AddSingleton<IPrescriptionDetailsService, PrescriptionDetailsService>()
                .AddSingleton<IAccountSessionService, AccountSessionService>()
                // UI
                .AddSingleton<IPageRouter, PageRouter>()
                .AddSingleton<IFontSizeService, FontSizeService>()
                .AddSingleton<ISukiToastManager, SukiToastManager>()
                .AddSingleton<ISukiDialogManager, SukiDialogManager>()
                .AddTransient<SukiDialogHost>(provider => new SukiDialogHost
                {
                    Manager = provider.GetRequiredService<ISukiDialogManager>()
                });

            return serviceCollection;
        }

        /// <summary>Registers and configures the views and view models required by <see cref="Drugly"/> in the given <see cref="IServiceCollection"/>.</summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public IServiceCollection ConfigureViews()
        {
            var builder = new ViewFactoryBuilder(serviceCollection)
                .AddView<StartupWindow, StartupWindowViewModel>()
                .AddView<PatientMainView, PatientMainViewModel>()
                .AddView<PatientPrescriptionView, PatientPrescriptionDetailsViewModel>()
                .AddView<DoctorMainView, DoctorMainViewModel>()
                .AddView<DoctorPrescribeModalView, DoctorPrescribeModalViewModel>()
                .AddView<DoctorMedicationListView, DoctorMedicationListViewModel>()
                .AddView<DoctorPatientListView, DoctorPatientListViewModel>()
                .AddView<DoctorMedicationDetailsPageView, DoctorMedicationDetailsPageViewModel>()
                .AddView<SettingsView, SettingsViewModel>()
                .AddView<MainView, MainViewModel>()
                .AddView<MainWindow, MainWindowViewModel>();

            return serviceCollection
                .AddSingleton<ViewLocator>()
                .AddSingleton<IViewFactory, ViewFactory>(provider => builder.Build(provider));
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