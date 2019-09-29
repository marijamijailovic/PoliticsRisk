"use strict";

var connection = new signalR.HubConnectionBuilder()
                        .withUrl("/messageHub")
                        .build();


//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function(player, message) {
    var player = player.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    
    var div = document.createElement("div");
    div.innerHTML = player + ": <br /><p style='margin-left:10%'>" +  msg + "</p>";
    document.getElementById("messages").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function(event) {
    var player = document.getElementById("playerInput").value;
    var message = document.getElementById("messageInput").value;
    var groupElement = document.getElementById("group");
    var groupValue = groupElement.options[groupElement.selectedIndex].value;
   
    if (groupValue === "All") {
        connection.invoke("SendMessageToAll", player, message).catch(function (err) {
            return console.error(err.toString());
        });
    } else if (groupValue === "PrivateGroup") {
        connection.invoke("SendMessageToGroup", "PrivateGroup", player, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    
    event.preventDefault();
});