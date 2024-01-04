namespace Domain.Messages;

public interface IMessageRepository
{
    void Insert(Message message);

    void Update(Message message);

    Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
