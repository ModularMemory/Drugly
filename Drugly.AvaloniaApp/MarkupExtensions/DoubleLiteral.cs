using Avalonia;
using Avalonia.Markup.Xaml;

namespace Drugly.AvaloniaApp.MarkupExtensions;

/// <summary>
/// Allows passing an immutable double literal to any source expecting a binding.
/// </summary>
public sealed class DoubleLiteral : MarkupExtension, IObservable<double>, IDisposable
{
    /// <summary>
    /// The value of the literal.
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Initialized the literal with the default value of 0.
    /// </summary>
    public DoubleLiteral() { }

    /// <summary>
    /// Initializes the literal with a specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value of the literal.</param>
    public DoubleLiteral(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Converts the <see cref="DoubleLiteral"/> into an Avalonia binding.
    /// </summary>
    /// <returns>The double literal as a binding object.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<double> observer)
    {
        observer.OnNext(Value);
        return this;
    }

    /// <summary>
    /// No-op to satisfy the return type of <see cref="Subscribe"/>.
    /// </summary>
    public void Dispose() { }
}