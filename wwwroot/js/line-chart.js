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