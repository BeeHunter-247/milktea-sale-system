using Microsoft.AspNetCore.SignalR;
using PRN222.Milktea.Service.BusinessModels;

namespace PRN222.Milktea.RazorPage.Hubs
{
    public class CartHub : Hub
    {
        // Phương thức này sẽ được gọi từ client để thông báo cập nhật giỏ hàng
        public async Task UpdateCart(CartViewModel cart)
        {
            // Gửi giỏ hàng mới đến tất cả các client đang kết nối
            await Clients.All.SendAsync("ReceiveCart", cart);
        }
    }
}
