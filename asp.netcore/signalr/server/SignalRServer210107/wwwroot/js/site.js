"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/generichub").build();
var maxlogs = 25;
var maxmessages = 25;

connection.on("pipelineMessage", function (stamp, method, path, addr, status) {      
    try {
        var lst = document.getElementById("requestsList");
        var d = document.createElement("div");        
        d.className = "msgContainer";
        processMessageSection(stamp, "consoleTextContainer", d);
        processMessageSection(addr, "consoleTextContainer   purple darken-2", d);
        processMessageSection(method, "methodContainer", d);
        processMessageSection(path, "consoleTextContainer", d);       
        processMessageSection(status, "consoleTextContainer yellow black-text", d);       

        lst.prepend(d);
        if (lst.childElementCount > maxlogs) {
            lst.removeChild(lst.childNodes[maxlogs]);
        }
    }
    catch (err) {
        console.log(err.message)
    }
    
});
connection.on("payloadLogMessage", function (stamp, user, payload) {
    try {
        var lst = document.getElementById("messagesList");
        var d = document.createElement("div");

        d.className = "msgContainer";
        processMessageSection(stamp, "consoleMessageContainer", d);
        processMessageSection(user, "consoleMessageContainer   purple darken-2", d);
        processMessageSection(payload, "consoleMessageContainer", d);

        lst.prepend(d);
        if (lst.childElementCount > maxmessages) {
            lst.removeChild(lst.childNodes[maxlogs]);
        }
    }
    catch (err) {
        console.log(err.message)
    }

});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

function processMessageSection(msg, classname, parent) {
    var m = msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var sp = document.createElement("span")
    sp.className = classname;
    sp.innerHTML = m;
    parent.appendChild(sp);    
}

