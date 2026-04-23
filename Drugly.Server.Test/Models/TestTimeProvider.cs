namespace Drugly.Server.Test.Models;

public class TestTimeProvider : TimeProvider
{
    private DateTimeOffset _time = DateTimeOffset.Now;

    public void AddHours(int hours)
    {
        _time = _time.AddHours(hours);
    }

    public override DateTimeOffset GetUtcNow()
    {
        return _time;
    }
}