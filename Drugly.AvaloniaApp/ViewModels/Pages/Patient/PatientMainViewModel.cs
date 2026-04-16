using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Patient;

public partial class PatientMainViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public AvaloniaList<Prescription> Prescriptions { get; } = [];
    public PatientMainViewModel(
        IPageRouter pageRouter, 
        IServiceProvider serviceProvider
        )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private void ViewPrescription()
    {
        var vm = _serviceProvider.GetRequiredService<PatientPrescriptionDetailsViewModel>();
        _pageRouter.PushPage(vm);
    }
    
    
}