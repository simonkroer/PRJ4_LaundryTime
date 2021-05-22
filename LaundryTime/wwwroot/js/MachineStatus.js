"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/machineHub").build();

connection.on("Status Changed", function() {
    
    let machines = Array.from(document.getElementsByClassName("machinestatus"));

    machines.forEach(toggleImage);
});

function toggleImage(machine) {

    if (machine.src.equalTo("~/Images/redwasher.png")) {
        machine.src = "~/Images/greenwasher.png";
    } else {
        machine.src = "~/Images/redwasher.png";
    }
}

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});