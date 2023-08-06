namespace GustoExpress.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task SendToUserMessage(string message, string receiver)
        {
            await Clients.Group(receiver).SendAsync("ReceiveMessage", message, receiver);
        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            await Groups.AddToGroupAsync(Context.ConnectionId, name);

        }
    }
}
