const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ticketHub")
    .build();

connection.on("TicketUpdated", (id, status) => {
    console.log("Ticket updated:", id, status);

    // gọi lại API hoặc update UI
    loadTickets();
});

connection.start();
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>