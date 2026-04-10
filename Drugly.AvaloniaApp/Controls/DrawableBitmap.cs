using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace Drugly.AvaloniaApp.Controls;

/// <summary>
/// A <see cref="Control"/> which displays a mouse-interactive <see cref="Bitmap"/>.
/// </summary>
public sealed partial class DrawableBitmap : Control
{
    /// <summary>
    /// Defines the <see cref="Background"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        Border.BackgroundProperty.AddOwner<DrawableBitmap>();

    /// <summary>
    /// Gets or Sets Bitmap background brush.
    /// </summary>
    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Foreground"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        TextElement.ForegroundProperty.AddOwner<DrawableBitmap>();

    /// <summary>
    /// Gets or Sets Bitmap background brush.
    /// </summary>
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="Foreground"/> property.
    /// </summary>
    public static readonly StyledProperty<double> StrokeThicknessProperty =
        Shape.StrokeThicknessProperty.AddOwner<DrawableBitmap>();

    /// <summary>
    /// Gets or Sets Bitmap background brush.
    /// </summary>
    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    /// <summary>
    /// Completely resets the bitmap to the original colour.
    /// </summary>
    [RelayCommand]
    private void Clear()
    {
        InitBitmap();
        _points.Clear();
        InvalidateVisual();
    }

    /// <summary>
    /// Saves the bitmap to a given<see cref="Stream"/>. See <see cref="Bitmap.Save(Stream, int?)"/>.
    /// </summary>
    /// <param name="stream">The stream to save to.</param>
    /// <returns><see langword="true"/> if the bitmap could be saved, otherwise <see langword="false"/>.</returns>
    public bool Save(Stream stream)
    {
        if (_bitmap is null)
        {
            return false;
        }

        _bitmap.Save(stream);
        return true;
    }

    // ---------------- //

    private readonly List<Point> _points = [];
    private RenderTargetBitmap? _bitmap;
    private bool _isDrawing;

    public DrawableBitmap()
    {
        PointerPressed += OnPointerPressed;
        PointerMoved += OnPointerMoved;
        PointerReleased += OnPointerReleased;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Only allow left mouse button
        if (e.Pointer.Type is PointerType.Mouse && !e.Properties.IsLeftButtonPressed)
        {
            return;
        }

        _isDrawing = true;

        // Add the starting point
        _points.Add(e.GetPosition(this));
        InvalidateVisual();
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isDrawing)
        {
            _points.AddRange(e.GetIntermediatePoints(this).Select(x => x.Position));
            InvalidateVisual();
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        // Only allow left mouse button
        if (e.InitialPressMouseButton is not MouseButton.Left)
        {
            return;
        }

        _isDrawing = false;

        // Clear points to start a new line
        _points.Clear();
        InvalidateVisual();
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        InitBitmap();

        base.OnAttachedToLogicalTree(e);
    }

    private void InitBitmap()
    {
        var width = Width;
        width = width > 0 ? width : 512;
        var height = Height;
        height = height > 0 ? height : 256;

        _bitmap?.Dispose();
        _bitmap = new RenderTargetBitmap(new PixelSize((int)width, (int)height), new Vector(96, 96));
        using var ctx = _bitmap.CreateDrawingContext();
        ctx.FillRectangle(Background ?? Brushes.White, new Rect(_bitmap.PixelSize.ToSize(1)));
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        _bitmap?.Dispose();
        _bitmap = null;
    }

    public override void Render(DrawingContext context)
    {
        if (_bitmap is null)
            return;

        base.Render(context);

        var renderSize = new Rect(Bounds.Size);
        context.FillRectangle(Background ?? Brushes.White, renderSize);

        DrawPoints(_bitmap);
        context.DrawImage(_bitmap, new Rect(_bitmap.Size), renderSize);
        return;

        void DrawPoints(RenderTargetBitmap bitmap)
        {
            // Avoid crash when only 1 point
            if (_points.Count < 2)
            {
                return;
            }

            using var ctx = bitmap.CreateDrawingContext(false);
            var pen = new Pen(Foreground ?? Brushes.Black, thickness: StrokeThickness);

            for (var i = 1; i < _points.Count; i++)
            {
                ctx.DrawLine(pen, _points[i - 1], _points[i]);
            }

            // Do not remove the last point to ensure a continuous line
            _points.RemoveRange(0, _points.Count - 1);
        }
    }
}