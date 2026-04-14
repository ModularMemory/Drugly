namespace Drugly.AvaloniaApp.Services.Interfaces;

/// <summary>Provides the ability to change the application font size.</summary>
public interface IFontSizeService
{
    /// <summary>The application font size.</summary>
    int FontSize { get; set; }
}