
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (Number, Message) {
    ClickFunc(Number, Message);
    var msg = Message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = Number + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);


});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var Number = document.getElementById("userInput").value;
    var Message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", Number, Message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});