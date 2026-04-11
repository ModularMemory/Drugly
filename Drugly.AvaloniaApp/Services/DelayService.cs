namespace Drugly.AvaloniaApp.Services;

/// <summary>Provides static methods related to delaying.</summary>
public static class DelayService
{
    /// <summary>Fake delay feels a lot better than instant feedback when performing local validation.</summary>
    public static Task FakeDelay(int millis = 200) => Task.Delay(millis);
}