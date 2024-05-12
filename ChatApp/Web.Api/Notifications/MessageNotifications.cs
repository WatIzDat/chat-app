using Domain.Messages;
using Microsoft.AspNetCore.SignalR;
using Web.Api.SignalR.Clients;
using Web.Api.SignalR.Hubs;

namespace Web.Api.Notifications;

public sealed class MessageNotifications(IHubContext<ChatHub, IChatClient> hubContext) : IMessageNotifications
{
    private readonly IHubContext<ChatHub, IChatClient> hubContext = hubContext;

    public async Task SendMessageAsync(string username, string contents)
    {
        await hubContext.Clients.All.ReceiveMessage(username, contents);
    }
}
