var ctx;
var config;
var endpoint;
var url;
var data;
var myChart;
// function that populates the chart, gets data via api call
document.addEventListener('DOMContentLoaded', function() {
    var endpoint = 'https://localhost:44398/api/data/total'
    $.ajax({
        method: "GET",
        url: endpoint,
        success: function (api_data) {
            var data = JSON.parse(api_data);
            var ctx = document.getElementById("line-chart").getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: data.data
                },
                options: {
                    reponsive: true,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        },
        error: function (error_data) {
            console.log(error_data)
        }
    })
}, false);

$(("input")).change(function () {
    myChart.destroy();
    var timeout = setTimeout(loadChart, 500)
    console.log("CHANGE")
});

function loadChart() {
    var url = document.getElementById("Url").innerHTML;
    var endpoint = 'https://localhost:44398/api/data/days/' + url;
    $.ajax({
        method: "GET",
        url: endpoint,
        success: function (api_data) {
            var data = JSON.parse(api_data);
            console.log(data.data);
            console.log(data.backgroundColor);
            var ctx = document.getElementById("doughnut");
            console.log("whyyyyy")
            myChart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: data.labels,
                    datasets: [
                        {
                            data: data.data,
                            backgroundColor: data.backgroundColor,
                            borderColor: data.borderColor
                        }
                    ]
                }


            });
        },
        error: function (error_data) {
            console.log(error_data)
        }
    })
};




 