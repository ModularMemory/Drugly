using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Services;

public sealed class ViewMapBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly Dictionary<Type, Type> _vmToViewMap = [];

    public ViewMapBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public ViewMapBuilder AddView<TView, TViewModel>()
        where TView : Control
        where TViewModel : ViewModelBase
    {
        _vmToViewMap.Add(typeof(TViewModel), typeof(TView));

        _serviceCollection
            .AddTransient<TView>()
            .AddTransient<TViewModel>();

        return this;
    }

    public ViewMap Build(IServiceProvider serviceProvider)
    {
        return new ViewMap(serviceProvider, _vmToViewMap);
    }
}