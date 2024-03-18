const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.start()
    .then(() => console.log('Connection Started.'))
    .catch(err => console.error('Error while starting connection: ' + err));

var currentUser = document.getElementById('chatContainer').getAttribute('data-current-user');
var recipientId = document.getElementById('chatContainer').getAttribute('data-recipient-id');
var messageInput = document.getElementById('messageInput');

var messageContainer = document.getElementById('messageContainer');

document.getElementById('messageInput').addEventListener('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        document.getElementById('sendButton').click();
    }
});

document.getElementById('sendButton').addEventListener('click', function () {
    if (messageInput.value.trim() !== '') {
        const li = document.createElement("div");
        li.className = `message-bubble message-bubble-sent`;
        li.innerHTML = `<p><strong>${currentUser}:</strong> ${messageInput.value}</p><span class="small text-muted">${new Date().toLocaleString()}</span>`;
        document.getElementById("messageContainer").appendChild(li);

        connection.invoke('SendMessage', recipientId, messageInput.value)
            .catch(err => console.error(err.toString()));

        messageInput.value = '';
    }
});

connection.on("ReceiveMessage", (senderId, senderName, message, time) => {
    const encodedMessage = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const isCurrentUser = senderId === currentUser;
    const bubbleClass = isCurrentUser ? "message-bubble-sent" : "message-bubble-received";
    const li = document.createElement("div");
    li.className = `message-bubble ${bubbleClass} ${isCurrentUser ? "message-bubble-current-user" : ""}`;
    li.innerHTML = `<p><strong>${senderName}:</strong> ${encodedMessage}</p><span class="small text-muted">${new Date(time).toLocaleString()}</span>`;
    document.getElementById("messageContainer").appendChild(li);

    setTimeout(function () {
        messageContainer.scrollTop = messageContainer.scrollHeight;
    }, 0);
});
