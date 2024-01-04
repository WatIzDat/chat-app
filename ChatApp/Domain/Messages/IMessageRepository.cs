namespace Domain.Messages;

public interface IMessageRepository
{
    void Insert(Message message);
}
