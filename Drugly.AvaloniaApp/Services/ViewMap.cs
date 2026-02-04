using System.Collections.Frozen;
using Avalonia.Controls;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Services;

public class ViewMap : IViewMap
{
    private readonly IServiceProvider _serviceProvider;
    private readonly FrozenDictionary<Type, Type> _vmToViewMap;
    private readonly FrozenDictionary<Type, Type> _viewToVmMap;

    public ViewMap(IServiceProvider serviceProvider, Dictionary<Type, Type> vmToViewMap)
    {
        _serviceProvider = serviceProvider;
        _vmToViewMap = vmToViewMap.ToFrozenDictionary();
        _viewToVmMap = _vmToViewMap.ToFrozenDictionary(x => x.Value, x => x.Key);
    }

    public TView CreateView<TView>() where TView : Control
    {
        var view = _serviceProvider.GetRequiredService<TView>();

        view.DataContext = _serviceProvider.GetRequiredService(_viewToVmMap[typeof(TView)]);

        return view;
    }

    public Control CreateViewFromVm<TViewModel>() where TViewModel : ViewModelBase
    {
        var vmType = typeof(TViewModel);

        var view = CreateViewFromVmNoDataContext(vmType);
        view.DataContext = _serviceProvider.GetRequiredService(vmType);

        return view;
    }

    public Control CreateViewFromVmNoDataContext(Type vmType)
    {
        if (!vmType.IsAssignableTo(typeof(ViewModelBase)))
        {
            throw new ArgumentException($"{vmType.FullName} is not assignable to {typeof(ViewModelBase).FullName}.", nameof(vmType));
        }

        var view = (Control)_serviceProvider.GetRequiredService(_vmToViewMap[vmType]);

        return view;
    }
}