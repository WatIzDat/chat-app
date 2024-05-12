using Microsoft.AspNetCore.SignalR;
using Web.Api.SignalR.Clients;

namespace Web.Api.SignalR.Hubs;

public sealed class ChatHub : Hub<IChatClient>
{
    //public override async Task OnConnectedAsync()
    //{
    //    //await Clients.All.ReceiveMessage($"{Context.ConnectionId}");
    //}

    //public async Task SendMessage(string contents)
    //{
    //    //await Clients.All.ReceiveMessage(contents);
    //}
}
