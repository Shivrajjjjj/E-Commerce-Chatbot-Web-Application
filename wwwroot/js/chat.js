const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", function (user, message) {
    const chatBox = document.getElementById("chatBox");
    const newMessage = document.createElement("p");
    newMessage.textContent = `${user}: ${message}`;
    chatBox.appendChild(newMessage);
});

connection.start().catch(err => console.error(err.toString()));

function sendMessage() {
    const messageInput = document.getElementById("messageInput");
    const message = messageInput.value;
    connection.invoke("SendMessage", "User", message)
        .catch(err => console.error(err.toString()));
    messageInput.value = "";
}
