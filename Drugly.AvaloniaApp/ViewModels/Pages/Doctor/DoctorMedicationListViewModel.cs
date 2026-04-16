using Avalonia.Collections;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

/// <summary>VM for the medication list page for doctors.</summary>
public partial class DoctorMedicationListViewModel : ViewModelBase, IPageViewModel
{
    private readonly IMedicationDetailsService _medicationDetailsService;
    private readonly IPageRouter _pageRouter;
    private readonly ISukiDialogManager _dialogManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public string? PageTitle => "Choose a Medication to Prescribe";

    public AvaloniaList<Medication> Medications { get; } = [];

    public DoctorMedicationListViewModel(
        IMedicationDetailsService medicationDetailsService,
        IPageRouter pageRouter,
        ISukiDialogManager dialogManager,
        IServiceProvider serviceProvider,
        ILogger logger
    )
    {
        _medicationDetailsService = medicationDetailsService;
        _pageRouter = pageRouter;
        _dialogManager = dialogManager;
        _serviceProvider = serviceProvider;
        _logger = logger;

        Dispatcher.UIThread.InvokeAsync(LoadMedications);
    }

    private async Task LoadMedications()
    {
        try
        {
            var medications = await _medicationDetailsService.GetAllMedications();
            Medications.AddRange(medications);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while loading medications");

            await _dialogManager.CreateDialog()
                .OfType(NotificationType.Error)
                .WithTitle("Error while loading medications")
                .WithContent(ex.Message)
                .WithOkResult("Ok")
                .Dismiss().ByClickingBackground()
                .TryShowAsync();
        }
    }

    /// <summary>Requests a page navigation to the medication details page.</summary>
    [RelayCommand]
    private void ViewMedication(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationDetailsPageViewModel>();
        vm.Medication = dataContext as Medication;
        _pageRouter.PushPage(vm);
    }
}