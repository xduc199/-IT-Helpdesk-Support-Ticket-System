// State Machine cho Ticket
export const getTicketActions = (status) => {
    switch (status) {
        case 'Open': return { canDelete: true, canAssign: true, canChangeStatus: true };
        case 'InProgress': return { canDelete: false, canAssign: false, canChangeStatus: true };
        case 'Resolved': 
        case 'Closed': return { canDelete: false, canAssign: false, canChangeStatus: false };
        default: return { canDelete: false, canAssign: false, canChangeStatus: false };
    }
};

// Hàm render ticket mẫu (Ví dụ cho trang Client)
export const renderTicketList = (tickets, containerId) => {
    const container = document.getElementById(containerId);
    container.innerHTML = tickets.map(t => `
        <div class="border p-4 mb-2 rounded ${t.status === 'Closed' ? 'bg-gray-100' : ''}">
            <h3>${t.title}</h3>
            <p>Status: ${t.status}</p>
            ${getTicketActions(t.status).canDelete ? `<button onclick="deleteTicket('${t.id}')">Xóa</button>` : ''}
        </div>
    `).join('');
};

export const parseTickets = (res) => {
    return res.items ||
           res.Items ||
           res.data ||
           res.Data ||
           (Array.isArray(res) ? res : []);
};

export const ticketStatusColor = (status) => {
    switch (status) {
        case "New": return "blue";
        case "In Progress": return "yellow";
        case "Resolved": return "green";
        default: return "gray";
    }
};
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>