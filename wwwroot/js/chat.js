const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.start()
    .then(() => console.log('Connection Started.'))
    .catch(err => console.error('Error while starting connection: ' + err));


var currentUser = document.getElementById('chatContainer').getAttribute('data-current-user');
var currentUserName = document.getElementById('chatContainer').getAttribute('data-current-user-name');
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
        li.innerHTML = `<p><strong>${currentUserName}:</strong> ${messageInput.value}</p><span class="small text-muted">${new Date().toLocaleString()}</span>`;
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

connection.on("UpdateChatGroups", function (userGroups) {
    console.log("Updating Chat Groups");
    console.log("Received chat groups: ", userGroups);
    var chatGroupsContainer = document.getElementById("chatGroupsContainer");
    chatGroupsContainer.innerHTML = "";

    userGroups.forEach(function (group) {
        var recentMessage = group.Messages.sort(function (a, b) {
            return new Date(b.SentAt) - new Date(a.SentAt);
        })[0];

        var groupElement = document.createElement("p");
        groupElement.textContent = group.Name;

        var messageElement = document.createElement("p");
        messageElement.textContent = "Last message: " + recentMessage.Content;

        chatGroupsContainer.appendChild(groupElement);
        chatGroupsContainer.appendChild(messageElement);
    });
});

connection.on("UpdateChatGroup", function () {
    // Call a function to update the chat group list
    updateChatGroup();
});

connection.on("UpdateChatGroups", function (userGroups) {
    console.log("Updating Chat Groups");
    console.log("Received chat groups: ", userGroups);
    var chatGroupsContainer = document.getElementById("chatGroupsContainer");
    chatGroupsContainer.innerHTML = "";

    userGroups.forEach(function (group) {
        var recentMessage = group.Messages.sort(function (a, b) {
            return new Date(b.SentAt) - new Date(a.SentAt);
        })[0];

        var groupElement = document.createElement("p");
        groupElement.textContent = group.Name;
        groupElement.className = "chat-group"; // Add this line to set the class name

        var messageElement = document.createElement("p");
        messageElement.textContent = "Last message: " + recentMessage.Content;

        // Add a click event listener to the group element
        groupElement.addEventListener("click", function () {
            connection.invoke("JoinGroup", group.Name) // Invoke the JoinGroup method with the group name
                .catch(err => console.error(err.toString()));
        });

        chatGroupsContainer.appendChild(groupElement);
        chatGroupsContainer.appendChild(messageElement);
    });
});

connection.on("ReceiveChatHistory", function (chatHistory) {
    // Clear the message container
    messageContainer.innerHTML = "";

    // Add the chat history to the message container
    chatHistory.forEach(function (message) {
        var senderName = message.SenderId;
        var bubbleClass = message.SenderId === currentUser ? "message-bubble-sent" : "message-bubble-received";
        var li = document.createElement("div");
        li.className = `message-bubble ${bubbleClass}`;
        li.innerHTML = `<p><strong>${senderName}:</strong> ${message.Content}</p><span class="small text-muted">${new Date(message.SentAt).toLocaleString()}</span>`;
        messageContainer.appendChild(li);
    });

    // Scroll to the bottom of the message container
    messageContainer.scrollTop = messageContainer.scrollHeight;
});

userGroups.forEach(function (group) {
    var recentMessage = group.Messages.sort(function (a, b) {
        return new Date(b.SentAt) - new Date(a.SentAt);
    })[0];

    var groupElement = document.createElement("div");
    groupElement.textContent = group.Name;
    groupElement.className = "chat-group"; // Add this line to set the class name

    // Add a click event listener to the group element
    groupElement.addEventListener("click", function () {
        connection.invoke("JoinGroup", group.Name) // Invoke the JoinGroup method with the group name
            .catch(err => console.error(err.toString()));
    });

    chatGroupsContainer.appendChild(groupElement);
});


var groupElement = document.createElement("div");
groupElement.textContent = group.Name;
groupElement.className = "chat-group";  // Add this line