using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Drugly.AvaloniaApp.Services.Interfaces;

namespace Drugly.AvaloniaApp.Services;

/// <inheritdoc />
public sealed class FontSizeService : IFontSizeService
{
    private readonly Application _application;
    private readonly Style _fontStyle;

    public int FontSize
    {
        get;
        set
        {
            field = value;

            _fontStyle.Setters.Clear();
            _fontStyle.Children.Clear();
            _fontStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, (double)value));
            AddChild(_fontStyle, "Body", FontSize);
            AddChild(_fontStyle, "H1", FontSize * 2);
            AddChild(_fontStyle, "H2", FontSize * 1.5);
            AddChild(_fontStyle, "H3", FontSize * 1.17);
            AddChild(_fontStyle, "H4", FontSize * 1);
            AddChild(_fontStyle, "H5", FontSize * 0.83);
            AddChild(_fontStyle, "H6", FontSize * 0.67);

            _application.Styles.Remove(_fontStyle);
            _application.Styles.Add(_fontStyle);

            static void AddChild(Style fontStyle, string selector, double fontSize)
            {
                fontStyle.Add(new Style(x => x.Nesting().Class(selector))
                {
                    Setters = { new Setter(TextBlock.FontSizeProperty, (double)(int)fontSize) }
                });
            }
        }
    }

    public FontSizeService(
        Application application
    )
    {
        _application = application;
        _fontStyle = new Style(x => x.Is<TextBlock>());
        FontSize = 14;
    }
}