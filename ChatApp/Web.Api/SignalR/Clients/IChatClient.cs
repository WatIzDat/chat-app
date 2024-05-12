namespace Web.Api.SignalR.Clients;

public interface IChatClient
{
    Task ReceiveMessage(string username, string contents);
}
