namespace Domain.Messages;

public interface IMessageNotifications
{
    Task SendMessageAsync(string username, string contents);
}
