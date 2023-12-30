namespace SharedKernel;

public interface IDateTimeOffsetProvider
{
    public DateTimeOffset UtcNow { get; }
}
