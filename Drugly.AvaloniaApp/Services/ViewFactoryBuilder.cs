using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Services;

public sealed class ViewFactoryBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly Dictionary<Type, Type> _vmToViewMap = [];

    public ViewFactoryBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public ViewFactoryBuilder AddView<TView, TViewModel>()
        where TView : Control
        where TViewModel : ViewModelBase
    {
        _vmToViewMap.Add(typeof(TViewModel), typeof(TView));

        _serviceCollection
            .AddTransient<TView>()
            .AddTransient<TViewModel>();

        return this;
    }

    public ViewFactory Build(IServiceProvider serviceProvider)
    {
        return new ViewFactory(serviceProvider, _vmToViewMap);
    }
}