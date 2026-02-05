using Avalonia.Controls;
using Drugly.AvaloniaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Drugly.AvaloniaApp.Extensions;

public static class ServiceProviderExtensions
{
    extension(IServiceProvider serviceProvider)
    {
        public TView GetRequiredView<TView>() where TView : Control
        {
            return serviceProvider
                .GetRequiredService<IViewMap>()
                .CreateView<TView>();
        }
    }
}