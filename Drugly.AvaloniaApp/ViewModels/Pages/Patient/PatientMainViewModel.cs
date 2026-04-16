using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Patient;

/// <summary>Vm for the main page for patients.</summary>
public partial class PatientMainViewModel : ViewModelBase, IPageViewModel
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountDetailsService _accountDetailsService;
    private readonly IAccountSessionService _accountSessionService;
    private readonly IPrescriptionDetailsService _prescriptionDetailsService;
    private readonly IMedicationDetailsService _medicationDetailsService;

    public string? PageTitle => "Hello, John!";

    public AccountDetails? Account { get; set; }

    public AvaloniaList<PatientPrescription> Prescriptions { get; } = [];

    public PatientMainViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider,
        IAccountDetailsService accountDetailsService,
        IAccountSessionService accountSessionService,
        IPrescriptionDetailsService prescriptionDetailsService,
        IMedicationDetailsService medicationDetailsService
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;
        _accountDetailsService = accountDetailsService;
        _accountSessionService = accountSessionService;
        _prescriptionDetailsService = prescriptionDetailsService;
        _medicationDetailsService = medicationDetailsService;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (_accountSessionService.AccountType == AccountType.Patient)
            {
                Account = await _accountDetailsService.GetAccountById(_accountSessionService.AccountId);
                var prescriptions = await _prescriptionDetailsService.GetPrescriptionsByAccountId(Account.UserId);
                List<Medication> medications = [];

                foreach (var prescription in prescriptions)
                {
                    medications.Add(await _medicationDetailsService.GetMedication(prescription.MedicationId));
                }

                Prescriptions.AddRange(medications.Select((m, i) => new PatientPrescription(prescriptions[i], m)));
            }
        });
    }

    [RelayCommand]
    private void ViewPrescription(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<PatientPrescriptionDetailsViewModel>();
        vm.Patient = Account;
        vm.Prescription = dataContext as PatientPrescription;
        _pageRouter.PushPage(vm);
    }
}