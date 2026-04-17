namespace Drugly.Server.Test.Models;

public class TestTimeProvider : TimeProvider
{
    private DateTimeOffset _time = DateTimeOffset.Now;

    public void IncreaseUtc(int hours)
    {
        _time += TimeSpan.FromHours(hours);
    }

    public override DateTimeOffset GetUtcNow()
    {
        return _time;
    }
}