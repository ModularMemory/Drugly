using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMedicationListViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public AvaloniaList<PrescriptionViewModel> Medications { get; } = [];

    public DoctorMedicationListViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }

    [RelayCommand]
    private void PrescribeMedications(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationDetailsPageViewModel>();
        vm.Prescription = dataContext as PrescriptionViewModel;
        _pageRouter.PushPage(vm);
    }
}