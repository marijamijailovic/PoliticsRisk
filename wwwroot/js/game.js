"use strict";

var connection = new signalR.HubConnectionBuilder()
                        .withUrl("/gameHub")
                        .build();

document.getElementById("activePlayerButton").disabled = true;

connection.on("List of online player", function(player) {
    /*var li = document.createElement("li");
    li.innerHTML = player + "<hr/>"
    li.textContent = player
    li.id = player

    document.getElementById("listOfPlayer").appendChild(li)*/
    var player = player.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    console.log('Igrac ', player);
    var div = document.createElement("div");
    div.innerHTML = player + "<hr/>";
    document.getElementById("listOfPlayer").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("activePlayerButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("activePlayerButton").addEventListener("click", function(event) {
    var player = document.getElementById("playerInput").value;
    console.log('Alo ',player);
    
    connection.invoke("ShowAllOnlinePlayers", player).catch(function (err) {
            return console.error(err.toString());
        });
 
    event.preventDefault();
});