using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.DTO;
using Humanizer;
using Serilog;
using SukiUI.Dialogs;

namespace Drugly.AvaloniaApp.ViewModels.Pages.Patient;

public partial class PatientPrescriptionDetailsViewModel : ViewModelBase, IPageViewModel
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly IPageRouter _pageRouter;
    private readonly ILogger _logger;
    
    public string? PageTitle => "Hello, John!";

    [ObservableProperty]
    public partial AccountDetails? Patient { get; set; }

    [ObservableProperty]
    public partial Prescription? Prescription { get; set; }

    [ObservableProperty]
    public partial int StepIndex { get; set; }

    public IEnumerable<string> Steps { get; }

    public PatientPrescriptionDetailsViewModel(
        ISukiDialogManager dialogManager,
        IPageRouter pageRouter,
        ILogger logger
    )
    {
        _dialogManager = dialogManager;
        _pageRouter = pageRouter;
        _logger = logger;

        Steps = Enum.GetValues<PrescriptionState>()
            .Where(x => x is not PrescriptionState.Unknown and not PrescriptionState.Cancelled)
            .Select(x => x.Humanize(LetterCasing.Title));
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _pageRouter.PopPage();
    }

    [RelayCommand]
    private void SubmitBillingInfo()
    {
        // Do something here idk how this is handled :3c
        _pageRouter.PopPage();
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardHolder { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardNumber { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardExpiry { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    public partial string? CardCCV { get; set; }
}