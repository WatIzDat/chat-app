using Domain.Messages;

namespace Application.UnitTests.Messages;

public abstract class BaseMessageTest<T>
{
    protected virtual Message CreateDefaultMessage()
    {
        return Message.Create(
            Guid.Empty,
            Guid.Empty,
            "test",
            DateTimeOffset.MinValue).Value;
    }

    protected virtual void ConfigureMocks(T command, Action? overrides = null)
    {
        overrides?.Invoke();
    }

    protected virtual void ConfigureMocks(Message message, T command, Action? overrides = null)
    {
        overrides?.Invoke();
    }
}
