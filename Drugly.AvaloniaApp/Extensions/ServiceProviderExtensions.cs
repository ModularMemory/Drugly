using Avalonia.Controls;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Extensions;

/// <summary>Provides extensions for <see cref="IServiceProvider"/>s.</summary>
public static class ServiceProviderExtensions
{
    extension(IServiceProvider serviceProvider)
    {
        /// <summary>Gets a view of type <typeparamref name="TView"/> from the <see cref="IServiceProvider"/>.</summary>
        /// <typeparam name="TView">The type of view to create.</typeparam>
        /// <returns>An instance of the <typeparamref name="TView"/>.</returns>
        /// <exception cref="KeyNotFoundException">There is no viewmodel associated with the view of type <typeparamref name="TView"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The <see cref="IServiceProvider"/> does not have a registered <see cref="IViewFactory"/>,
        /// no <typeparamref name="TView"/>s are registered in the associated <see cref="IServiceProvider"/>,
        /// or the viewmodel associated with the <typeparamref name="TView"/> is not registered in the <see cref="IServiceProvider"/>.
        /// </exception>
        public TView GetRequiredView<TView>() where TView : Control
        {
            return serviceProvider
                .GetRequiredService<IViewFactory>()
                .CreateView<TView>();
        }
    }
}