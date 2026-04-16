using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc cref="IPageRouter" />
public sealed partial class PageRouter : ObservableObject, IPageRouter
{
    private readonly ILogger _logger;
    private readonly ObservableCollection<ViewModelBase> _viewModels = [];

    public PageRouter(
        ILogger logger
    )
    {
        _logger = logger;

        HistoryEmpty = true;
        _viewModels.CollectionChanged += (_, _) => HistoryEmpty = _viewModels.Count == 0;
    }

    public event EventHandler<ViewModelBase?>? PageNavigate;

    [ObservableProperty]
    public partial ViewModelBase? RootPage { get; set; }

    partial void OnRootPageChanged(ViewModelBase? value)
    {
        if (_viewModels.Count == 0)
        {
            OnPageNavigate(value);
        }
    }

    [ObservableProperty]
    public partial bool HistoryEmpty { get; private set; }

    private void OnPageNavigate(ViewModelBase? e)
    {
        _logger.Information("Navigating to page {VmName}", e?.GetType().Name ?? "null");
        PageNavigate?.Invoke(this, e);
    }

    public void ResetPageHistory()
    {
        _viewModels.Clear();
    }

    public void PushPage(ViewModelBase? viewModel)
    {
        if (viewModel is not null)
        {
            _viewModels.Add(viewModel);
        }

        OnPageNavigate(viewModel);
    }

    public void PopPage()
    {
        if (_viewModels.Count == 0)
        {
            OnPageNavigate(RootPage);
            return;
        }

        _viewModels.RemoveAt(_viewModels.Count - 1);

        var vm = _viewModels.Count == 0
            ? RootPage
            : _viewModels[^1];

        OnPageNavigate(vm);
    }

    public void ReshowPage()
    {
        var currentPage = _viewModels.Count == 0
            ? RootPage
            : _viewModels[^1];

        OnPageNavigate(null);
        OnPageNavigate(currentPage);
    }
}