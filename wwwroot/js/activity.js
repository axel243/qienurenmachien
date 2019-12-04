var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (jsonObject) {

    var encodedMsg = JSON.parse(jsonObject);
    var table = document.getElementById("ActivityTable");
    var row = table.insertRow(1);

    var index = 0;
    for (let [key, value] of Object.entries(encodedMsg)) {
        console.log(key);
            var cel = row.insertCell(index);
  
            cel.innerHTML = value;
            index++;
        }
    
    document.getElementById("ActivityTable").deleteRow(-1);

});