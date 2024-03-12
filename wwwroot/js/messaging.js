var connection = new signalR.HubConnectionBuilder().withUrl("/messagesHub").build();
var currentUserId = "@User.Identity.Name";
var currentUserName = "@User.Identity.GetUserName()";
var receiverId, receiverName;

connection.on("ReceiveMessage", function (sender, message, sentAt) {
    var messageHtml = '<li class="message received">' +
        '<span class="sender">' + sender + ':</span> ' +
        '<span class="message-text">' + message + '</span> ' +
        '<span class="message-time">' + formatDateTime(sentAt) + '</span>' +
        '</li>';
    $("#messageList").append(messageHtml);
});

connection.start().then(function () {
    // Connection started successfully
    var urlParams = new URLSearchParams(window.location.search);
    receiverId = urlParams.get('receiverId');
    receiverName = urlParams.get('receiverName');
    $("#receiverName").text(receiverName);

    // Load conversation list
    loadConversationList();
}).catch(function (err) {
    console.error(err.toString());
});

function sendMessage() {
    var message = $("#messageInput").val();
    if (message && message.trim() !== "") {
        var currentDateTime = new Date();
        var senderMessage = '<li class="message sent">' +
            '<span class="sender">You:</span> ' +
            '<span class="message-text">' + message + '</span> ' +
            '<span class="message-time">' + formatDateTime(currentDateTime) + '</span>' +
            '</li>';
        $("#messageList").append(senderMessage);
        connection.invoke("SendMessage", receiverId, message).catch(function (err) {
            console.error(err.toString());
        });
        $("#messageInput").val("");
    }
}

function formatDateTime(dateTime) {
    var options = { hour: 'numeric', minute: 'numeric', hour12: true };
    return dateTime.toLocaleString(undefined, options);
}

function loadConversationList() {
    // Fetch conversation list from server and render in the sidebar
    // You'll need to implement this functionality on the server-side
}

$(document).ready(function () {
    $("#sendButton").click(sendMessage);
});
