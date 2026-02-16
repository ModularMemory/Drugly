using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.ViewModels;
using Serilog;
using SukiUI.Controls;

namespace Drugly.AvaloniaApp;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
public sealed class ViewLocator : IDataTemplate
{
    private readonly IViewMap _viewMap;
    private readonly ILogger _logger;

    public ViewLocator(
        IViewMap viewMap,
        ILogger logger
    )
    {
        _viewMap = viewMap;
        _logger = logger;
    }

    public Control? Build(object? param)
    {
        if (param is null)
        {
            var stack = new StackTrace();
            _logger.Warning("Tried to locate view for null. Stack trace: {Stack}", stack);
            return null;
        }

        var type = param.GetType();
        try
        {
            return _viewMap.CreateViewFromVmNoDataContext(type);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to find view for type {Type}", type);

            return new GroupBox
            {
                Header = new TextBlock
                {
                    Text = $"View Not Found For {type.FullName}"
                },
                Content = new TextBlock
                {
                    Text = ex.ToString(),
                    FontSize = 12,
                    Foreground = Brushes.Red
                }
            };
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}