using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Patient;

public partial class PatientMainViewModel : ViewModelBase, IPageViewModel
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountDetailsService _accountDetailsService;
    private readonly IAccountSessionService _accountSessionService;

    public string? PageTitle => "Hello John!";

    public AvaloniaList<PatientPrescription> Prescriptions { get; } = [];

    public PatientMainViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider,
        IAccountDetailsService accountDetailsService,
        IAccountSessionService accountSessionService
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;
        _accountDetailsService = accountDetailsService;
        _accountSessionService = accountSessionService;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (_accountSessionService.AccountType == AccountType.Patient)
            {
                _accountDetailsService.GetAccountById(_accountSessionService.AccountId);
            }
        });
}

    [RelayCommand]
    private void ViewPrescription()
    {
        var vm = _serviceProvider.GetRequiredService<PatientPrescriptionDetailsViewModel>();
        _pageRouter.PushPage(vm);
    }
    
    
}