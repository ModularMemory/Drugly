using System.ComponentModel;
using Drugly.AvaloniaApp.ViewModels;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Allows individual pages to forward navigation requests to a containing view.</summary>
public interface IPageRouter : INotifyPropertyChanged
{
    /// <summary>Invoked when a page navigation is requested.</summary>
    event EventHandler<ViewModelBase?>? PageNavigate;

    /// <summary>The page to be returned when a navigation is requested with an empty page history.</summary>
    /// <remarks>Immediately requests a navigation on set if the history is empty.</remarks>
    ViewModelBase? RootPage { get; set; }

    /// <summary><see langword="true"/> if the history is empty, otherwise <see langword="false"/>.</summary>
    bool HistoryEmpty { get; }

    /// <summary>Clears the page history.</summary>
    void ResetPageHistory();

    /// <summary>Pushes a new page into the history and navigates to it.</summary>
    void PushPage(ViewModelBase? viewModel);

    /// <summary>Removes most recent page from the page history, then navigates to the last page in the page history.</summary>
    /// <remarks>Navigates to the <see cref="RootPage"/> if the history empty before or as a result of calling this operation.</remarks>
    void PopPage();

    /// <summary>Hides and restores the current page.</summary>
    void ReshowPage();
}