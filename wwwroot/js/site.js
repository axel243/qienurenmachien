// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Convert table data to string
function TableToString() {
    var data = []

    var table = document.getElementById('TimeSheetTable'),
        rows = table.rows, rowcount = rows.length, r, c;
    for (r = 1; r < rowcount; r++) {
        var day = {};
        day[r] = {}
        for (c = 0; c < 6; c++) {
            if (c == 0) {
                day[r].projecthours = document.getElementById("" + r + c).value;
            }
            else if (c == 1) {
                day[r].overwork = document.getElementById("" + r + c).value;
            }
            else if (c == 2) {
                day[r].sick = document.getElementById("" + r + c).value;
            }
            else if (c == 3) {
                day[r].absence = document.getElementById("" + r + c).value;
            }
            else if (c == 4) {
                day[r].training = document.getElementById("" + r + c).value;
            }
            else if (c == 5) {
                day[r].other = document.getElementById("" + r + c).value;
            }
        }
        data.push(day);
    }

    // Stringify object array
    console.log(JSON.stringify(data));
}


// Whenever the input table get changed create new data string
$("input").change(function() {
    TableToString();
});