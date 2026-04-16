namespace Drugly.AvaloniaApp.ViewModels.Pages;

/// <summary>Contract for all VM pages.</summary>
public interface IPageViewModel
{
    /// <summary>The title of the page.</summary>
    string? PageTitle { get; }
}