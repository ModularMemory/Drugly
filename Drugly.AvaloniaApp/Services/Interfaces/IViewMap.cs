using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface IViewMap
{
    /// <summary>Creates a specified view with its associated viewmodel as the data context.</summary>
    /// <typeparam name="TView">The type of the view to create.</typeparam>
    /// <returns>The created view.</returns>
    TView CreateView<TView>() where TView : Control;

    /// <summary>Creates a view based on its associated viewmodel, with the viewmodel as the data context.</summary>
    /// <typeparam name="TViewModel">The type of the viewmodel to create a view from.</typeparam>
    /// <returns>The created view.</returns>
    Control CreateViewFromVm<TViewModel>() where TViewModel : ViewModelBase;

    /// <summary>Creates a view based on its associated viewmodel.</summary>
    /// <param name="vmType">The type of the viewmodel to create a view from.</param>
    /// <returns>The created view.</returns>
    Control CreateViewFromVmNoDataContext(Type vmType);
}