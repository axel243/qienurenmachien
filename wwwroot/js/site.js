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
$(("textarea")).change(function () {
    TableToString();
});

// Whenever the input table get changed create new data string
$(("input")).change(function () {
    TableToString();
});


//Websocket
var connection = new signalR.HubConnectionBuilder().withUrl("http://qienurenmachien.tk:4999/chatHub").build();

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
        console.log(key, value);
        var cel = row.insertCell(index);
        cel.innerHTML = value;
        index++;
    };
   
    document.getElementById("OverViewTable").deleteRow(-1);
    console.log(encodedMsg);

});



//***********************activiteiten tabel*****************************
connection.on("ReceiveActivity", function (jsonObject) {

    console.log(jsonObject);
    var newActivity = jsonObject;
    var comment = newActivity.comment;
    var timestamp = newActivity.timestamp;

    var table = document.getElementById("ActivityTable");

    var row = table.insertRow(1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);

    cell1.innerHTML = comment;
    cell2.innerHTML = timestamp;

    table.deleteRow(-1);

});
//*****************************************************