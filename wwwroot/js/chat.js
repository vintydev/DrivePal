"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

var currentUser = document.getElementById("userFullName").dataset.fullname;


connection.on("ReceiveMessage", function (senderName, message, timestamp, read, messageId) {
    var li = document.createElement("li");
    li.id = "message" + messageId;
    /*li.textContent = senderName === currentUser ? `You sent: ${message} at ${timestamp}` : `${senderName} sent: ${message} at ${timestamp}`;*/
    li.textContent = `${senderName} sent: ${message} at ${timestamp}`;
    if (!read) {
        connection.invoke("ReadMessage", messageId);
    }
    document.getElementById("messagesList").appendChild(li);
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
    scrollToBottom();
});

function scrollToBottom() {
    var messagesList = document.getElementById('messagesList');
    messagesList.scrollTop = messagesList.scrollHeight;
}

scrollToBottom();