using Microsoft.AspNetCore.SignalR;

namespace PRN222.Milktea.RazorPage.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderUpdate(int orderId, string status)
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate", orderId, status);
        }
    }
}
