"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();


connection.on("ReceiveMessage", function (message) {
    $("#Notification").empty();
    var newElement = '<i style="color:darkgreen;" class="fa fa-bell notificationIcon" aria-hidden="true"></i><span class="Notificationmsg">'+message+'</span>';
    $("#Notification").append(newElement);
    $("#Notification").slideDown("slow").delay(5000).slideUp();
    
});

connection.start().then(function () {
    console.debug("SignalR client started");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = "test";
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});