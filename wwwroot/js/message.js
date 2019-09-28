"use strict";

var connection = new signalR.HubConnectionBuilder()
                        .withUrl("/messageHub")
                        .build();


//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function(message) {
    console.log('TEST PRIMIO PORUKU ', message);
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.innerHTML = msg + "<hr/>";
    document.getElementById("messages").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function(event) {
    var message = document.getElementById("messageInput").value;
    console.log('TEST ', message);
    var groupElement = document.getElementById("group");
    var groupValue = groupElement.options[groupElement.selectedIndex].value;
    
    if (groupValue === "All" || groupValue === "Myself") {
        var method = groupValue === "All" ? "SendMessageToAll" : "SendMessageToCaller";
        console.log('TEST All ili JA ', message);
        connection.invoke(method, message).catch(function (err) {
            return console.error(err.toString());
        });
    } else if (groupValue === "PrivateGroup") {
        connection.invoke("SendMessageToGroup", "PrivateGroup", message).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        connection.invoke("SendMessageToUser", groupValue, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    
    event.preventDefault();
});