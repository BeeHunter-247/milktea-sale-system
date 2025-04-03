// MilkTea.RazorPage/wwwroot/js/signalr-client.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/orderHub")
    .build();

connection.on("ReceiveOrderUpdate", (orderId, status) => {
    const row = document.querySelector(`tr[data-order-id='${orderId}']`);
    if (row) {
        row.querySelector(".status").textContent = status;
    }
});

connection.start().catch(err => console.error(err));


// Kết nối với CartHub
const cartConnection = new signalR.HubConnectionBuilder()
    .withUrl("/cartHub")
    .build();

// Lắng nghe sự kiện 'ReceiveCartUpdate' từ server (cập nhật giỏ hàng)
cartConnection.on("ReceiveCartUpdate", (cart) => {
    updateCartUI(cart);  // Gọi hàm cập nhật giỏ hàng UI
});

// Khởi tạo kết nối CartHub
cartConnection.start()
    .catch(err => console.error("Error starting CartHub connection: " + err));

// Hàm cập nhật giỏ hàng trên UI
function updateCartUI(cart) {
    const tableBody = document.querySelector("table tbody");
    tableBody.innerHTML = ''; // Xóa các dòng hiện tại

    cart.Items.forEach(item => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${item.ProductName}</td>
            <td>${item.UnitPrice}</td>
            <td>
                <input type="number" id="quantity_${item.ProductId}" value="${item.Quantity}" min="1" class="form-control quantity-input" style="width: 80px;" data-product-id="${item.ProductId}" />
            </td>
            <td>${item.Quantity * item.UnitPrice}</td>
            <td>
                <form method="post" asp-page-handler="RemoveItem" class="d-inline">
                    <input type="hidden" name="productId" value="${item.ProductId}" />
                    <button type="submit" class="btn btn-danger btn-sm">Remove</button>
                </form>
            </td>
        `;
        tableBody.appendChild(row);
    });

    // Cập nhật tổng giá trị
    document.querySelector("h4").innerText = `Total: ${cart.TotalAmount}`;
}
