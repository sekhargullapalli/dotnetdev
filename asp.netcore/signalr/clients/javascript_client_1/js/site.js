"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/generichub").build();

var connectionSet = false;
connection.start().then(function () {
    connectionSet=true;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("mainpanel").addEventListener("mousemove", function (event) {
    var message =`X: ${event.clientX}, Mouse Y: ${event.clientY}`;
    if(connectionSet){
        var user = "browserClient";    
    connection.invoke("SendMessage", user, message,true)
               .catch(function (err) {
                    return console.error(err.toString());
                });
    document.getElementById("coordinates").innerHTML=message;
    event.preventDefault();
    }
});