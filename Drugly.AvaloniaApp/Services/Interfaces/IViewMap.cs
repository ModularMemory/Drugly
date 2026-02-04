using Avalonia.Controls;
using Drugly.AvaloniaApp.ViewModels;

namespace Drugly.AvaloniaApp.Services.Interfaces;

public interface IViewMap
{
    TView CreateView<TView>() where TView : Control;
    Control CreateViewFromVm<TViewModel>() where TViewModel : ViewModelBase;
    Control CreateViewFromVmNoDataContext(Type vmType);
}