using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Serilog;

namespace Drugly.AvaloniaApp.Services;

public sealed class PageRouter : IPageRouter
{
    private readonly ILogger _logger;
    private readonly List<ViewModelBase> _viewModels = [];

    public PageRouter(
        ILogger logger
    )
    {
        _logger = logger;
    }

    public event EventHandler<ViewModelBase?>? PageNavigate;

    public ViewModelBase? RootPage
    {
        get;
        set
        {
            field = value;
            if (_viewModels.Count == 0)
            {
                OnPageNavigate(value);
            }
        }
    }

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
}