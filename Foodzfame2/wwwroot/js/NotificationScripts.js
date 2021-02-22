"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();


connection.on("ReceiveMessage", function (client,message) {
    $("#Notification").empty();
    var newElement = '<i style="color:#FFD700;text-shadow: 3px 3px grey;" class="fa fa-bell notificationIcon" aria-hidden="true"></i><span class="Notificationmsg">'+message+'</span>';
    $("#Notification").append(newElement);
    if (client != connection.connection.connectionId) {
        $("#Notification").slideDown("slow").delay(5000).slideUp();
    }
    
});

connection.start().then(function () {
    console.debug("SignalR client started");
}).catch(function (err) {
    return console.error(err.toString());
});
