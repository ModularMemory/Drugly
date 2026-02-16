using CommunityToolkit.Mvvm.ComponentModel;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ViewModelBase ContentViewModel { get; set; }

    public MainViewModel(
        IServiceProvider serviceProvider,
        IAccountSessionService accountSessionService
    )
    {
        var accountType = accountSessionService.AccountType;
        ContentViewModel = accountType switch
        {
            AccountType.Patient => serviceProvider.GetRequiredService<PatientMainViewModel>(),
            AccountType.Doctor => serviceProvider.GetRequiredService<DoctorMainViewModel>(),
            AccountType.Pharmacist => serviceProvider.GetRequiredService<PharmacistMainViewModel>(),
            _ => null!
        };
    }
}