using Drugly.AvaloniaApp.ViewModels;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface IPageRouter
{
    event EventHandler<ViewModelBase?>? PageNavigate;

    ViewModelBase? RootPage { get; set; }

    void ResetPageHistory();
    void PushPage(ViewModelBase? viewModel);
    void PopPage();
}