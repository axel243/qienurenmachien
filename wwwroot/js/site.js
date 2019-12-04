// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Convert table data to string
function TableToString() {
    var data = {}

    var table = document.getElementById('TimeSheetTable'),
        rows = table.rows, rowcount = rows.length, r, c;
    for (r = 1; r < rowcount; r++) {
        var day = {};
        for (c = 0; c < 6; c++) {
            if (c == 0) {
                if (document.getElementById("" + r + c).value == "") {
                    day.projecthours = 0;
                }
                else {

                    day.projecthours = document.getElementById("" + r + c).value;
                }
            }
            else if (c == 1) {
                if (document.getElementById("" + r + c).value == "") {
                    day.overwork = 0;
                }
                else {

                    day.overwork = document.getElementById("" + r + c).value;
                }
            }
            else if (c == 2) {
                if (document.getElementById("" + r + c).value == "") {
                    day.sick = 0;
                }
                else {

                    day.sick = document.getElementById("" + r + c).value;
                }
            }
            else if (c == 3) {
                if (document.getElementById("" + r + c).value == "") {
                    day.absence = 0;
                }
                else {

                    day.absence = document.getElementById("" + r + c).value;
                }
            }
            else if (c == 4) {
                if (document.getElementById("" + r + c).value == "") {
                    day.training = 0;
                }
                else {

                    day.training = document.getElementById("" + r + c).value;
                }
            }
            else if (c == 5) {
                if (document.getElementById("" + r + c).value == "") {
                    day.other = 0;
                }
                else {

                    day.other = document.getElementById("" + r + c).value;
                }
            }
        }
        data[r] = day;
    }

    // Stringify object array

    console.log(JSON.stringify(data));
    let SheetID = document.getElementById("SheetID").innerHTML;
    let comment = document.getElementById("Comment").value;
    console.log(comment)
    connection.invoke("SendMessage", parseInt(SheetID), JSON.stringify(data), comment).catch(function (err) {
        return console.error(err.toString());
    });
    console.log("Updated database");
   
}


// Whenever the input table get changed create new data string
$(("input", "textarea")).change(function() {
    TableToString();
});

//Websocket
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44398/chatHub").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (jsonObject) {
    var encodedMsg = JSON.parse(jsonObject);
    console.log(encodedMsg.Comment)
    var comment = document.getElementById("Comment")
    comment.innerHTML = encodedMsg.Comment;
    var table = document.getElementById("OverViewTable");
    var row = table.insertRow(1);
    var index = 0;

    for (let [key, value] of Object.entries(encodedMsg)) {
        var cel = row.insertCell(index);
        cel.innerHTML = value;
        index++;
    };
   
    document.getElementById("OverViewTable").deleteRow(-1);
    console.log(encodedMsg);

});




$(document).ready(function () {
    new Chart(document.getElementById("line-chart"), {
        type: 'line',
        data: {
            labels: [1500, 1600, 1700, 1750, 1800, 1850, 1900, 1950, 1999, 2050],
            datasets: [{
                data: [86, 114, 106, 106, 107, 111, 133, 221, 783, 2478],
                label: "Africa",
                borderColor: "#3e95cd",
                fill: false
            }, {
                data: [282, 350, 411, 502, 635, 809, 947, 1402, 3700, 5267],
                label: "Asia",
                borderColor: "#8e5ea2",
                fill: false
            }, {
                data: [168, 170, 178, 190, 203, 276, 408, 547, 675, 734],
                label: "Europe",
                borderColor: "#3cba9f",
                fill: false
            }, {
                data: [40, 20, 10, 16, 24, 38, 74, 167, 508, 784],
                label: "Latin America",
                borderColor: "#e8c3b9",
                fill: false
            }, {
                data: [6, 3, 2, 2, 7, 26, 82, 172, 312, 433],
                label: "North America",
                borderColor: "#c45850",
                fill: false
            }
            ]
        },
        options: {
            title: {
                display: true,
                text: 'World population per region (in millions)'
            }
        }
    });
});