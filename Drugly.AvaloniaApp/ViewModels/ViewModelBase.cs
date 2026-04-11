using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Drugly.AvaloniaApp.ViewModels;

/// <summary>
/// The base class for all view models. Provides <see cref="INotifyPropertyChangedAttribute"/> and <see cref="INotifyDataErrorInfo"/> functionality.
/// </summary>
public abstract class ViewModelBase : ObservableValidator { }