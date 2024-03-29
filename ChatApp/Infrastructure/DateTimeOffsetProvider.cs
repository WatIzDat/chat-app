using SharedKernel;

namespace Infrastructure;

public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset UtcNow { get; } = DateTimeOffset.UtcNow;
}
