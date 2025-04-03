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