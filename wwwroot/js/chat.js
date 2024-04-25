"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

var currentUser = document.getElementById("userFullName").dataset.fullname;


connection.on("ReceiveMessage", function (senderName, message, timestamp, read, messageId) {
    var li = document.createElement("li");
    li.id = "message" + messageId;
    var sender = senderName === currentUser ? "You" : senderName;
    li.classList.add("p-3", "mb-2", "rounded");
    if (senderName === currentUser) {
        li.classList.add("sent-message", "text-white", "sent");
    } else {
        li.classList.add("bg-light", "received");
    }
    if (!read) {
        connection.invoke("ReadMessage", messageId);
    }

    var senderElement = document.createElement("strong");
    senderElement.textContent = sender;
    var messageElement = document.createElement("p");
    messageElement.textContent = message;
    var timestampElement = document.createElement("small");
    timestampElement.textContent = timestamp;

    li.appendChild(senderElement);
    li.appendChild(document.createElement("br"));
    li.appendChild(messageElement);
    li.appendChild(document.createElement("br"));
    li.appendChild(timestampElement);

    document.getElementById("messagesList").appendChild(li);
    setTimeout(scrollToBottom, 0);
});



connection.on("MessageRead", function (messageId) {
    var li = document.getElementById("message" + messageId);
    if (li) {
        li.textContent = li.textContent.replace("Unread", "Read");
    }
});



connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userFullName").dataset.fullname;
    var receiver = document.getElementById("chatContainer").dataset.receiverid;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, receiver, message).then(function () {
        document.getElementById("messageInput").value = ""; // Clear the input field after sending the message
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    setTimeout(scrollToBottom, 0);
});

document.getElementById("messageInput").addEventListener("keydown", function (event) {
    if (event.key === "Enter") {
        event.preventDefault();
        document.getElementById("sendButton").click();
    }
});


function scrollToBottom() {
    var messagesList = document.getElementById('messagesList');
    messagesList.scrollTop = messagesList.scrollHeight;
}

scrollToBottom();