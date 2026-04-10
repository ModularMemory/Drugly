using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;

namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Allows the creation of views and associated viewmodels.</summary>
public interface IViewFactory
{
    /// <summary>Creates a specified view with its associated viewmodel as the data context.</summary>
    /// <typeparam name="TView">The type of the view to create.</typeparam>
    /// <returns>The created view.</returns>
    /// <exception cref="KeyNotFoundException">There is no view associated with the view of type <typeparam name="TView"/>.</exception>
    /// <exception cref="InvalidOperationException">There is no registered view of type <typeparam name="TView"/>, or the associated viewmodel.</exception>
    TView CreateView<TView>() where TView : Control;

    /// <summary>Creates a view based on its associated viewmodel, with the viewmodel as the data context.</summary>
    /// <typeparam name="TViewModel">The type of the viewmodel to create a view from.</typeparam>
    /// <returns>The created view.</returns>
    /// <exception cref="ArgumentException"><typeparam name="TViewModel"/> does not inherit from type <see cref="ViewModelBase"/>.</exception>
    /// <exception cref="KeyNotFoundException">There is no view associated with type <typeparam name="TViewModel"/>.</exception>
    /// <exception cref="InvalidOperationException">There is no registered viewmodel of type <typeparam name="TViewModel"/>, or the associated view.</exception>
    Control CreateViewFromVm<TViewModel>() where TViewModel : ViewModelBase;

    /// <summary>Creates a view based on its associated viewmodel.</summary>
    /// <param name="vmType">The type of the viewmodel to create a view from.</param>
    /// <returns>The created view.</returns>
    /// <exception cref="ArgumentException"><paramref name="vmType"/> does not inherit from type <see cref="ViewModelBase"/>.</exception>
    /// <exception cref="KeyNotFoundException">There is no view associated with type <paramref name="vmType"/>.</exception>
    /// <exception cref="InvalidOperationException">There is no registered view associated of type <paramref name="vmType"/>.</exception>
    Control CreateViewFromVmNoDataContext(Type vmType);
}