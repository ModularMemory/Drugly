using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Services;

/// <summary>Provides functionality for preparing and creating <see cref="ViewFactory"/>s.</summary>
public sealed class ViewFactoryBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly Dictionary<Type, Type> _vmToViewMap = [];

    /// <summary>Initializes a new instance of the <see cref="ViewFactoryBuilder"/> class.</summary>
    /// <param name="serviceCollection">The associated service provider.</param>
    public ViewFactoryBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    /// <summary>Adds a view to the factory.</summary>
    /// <typeparam name="TView">The type of the view to add.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model to add.</typeparam>
    /// <returns>The <see cref="ViewFactoryBuilder"/> instance.</returns>
    /// <exception cref="ArgumentException">The <typeparamref name="TView"/> has already been registered.</exception>
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

    /// <summary>Builds and returns the factory.</summary>
    /// <param name="serviceProvider">The associated service provider.</param>
    /// <returns>The compiled <see cref="ViewFactory"/>.</returns>
    public ViewFactory Build(IServiceProvider serviceProvider)
    {
        return new ViewFactory(serviceProvider, _vmToViewMap);
    }
}