using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Design;
using Drugly.AvaloniaApp.Models;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Doctor;

public partial class DoctorPatientListViewModel : ViewModelBase
{
    private readonly IPageRouter _pageRouter;
    private readonly IServiceProvider _serviceProvider;

    public AvaloniaList<PatientModel> Patients { get; } = [];

    public DoctorPatientListViewModel(
        IPageRouter pageRouter,
        IServiceProvider serviceProvider
    )
    {
        _pageRouter = pageRouter;
        _serviceProvider = serviceProvider;

        Patients.AddRange(DesignData.ExamplePatients);
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }

    [RelayCommand]
    private async Task ViewPatient(object? dataContext)
    {
        var vm = _serviceProvider.GetRequiredService<PatientDetailsPageViewModel>();
        vm.Patient = dataContext as PatientModel;
        _pageRouter.PushPage(vm);
    }
}