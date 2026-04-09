using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorMedicationListViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public AvaloniaList<MedicationModel> Medications { get; } = [];

    public DoctorMedicationListViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;

        Medications.AddRange(Drugly.AvaloniaApp.Design.DesignData.ExampleMedications);
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }

    [RelayCommand]
    private void PrescribeMedication(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<DoctorMedicationDetailsPageViewModel>();
        vm.Medication = dataContext as MedicationModel;
        _pageRouter.PushPage(vm);
    }
}