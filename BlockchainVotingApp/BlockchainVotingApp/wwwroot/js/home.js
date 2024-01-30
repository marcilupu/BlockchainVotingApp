// Data
const HomeComponent = function () {
    context = {
        ids: {
            select: {
                target: '[data-election-select]'
            },
            winnerSpan: 'winnerId',
            barchart: 'chart-election-results'
        },
        state: {
            jQuery: {
                electionSelect: null,
                barchart: null
            },
            electionId: null,
            electionName: null,
            chartData: null,
            chartObject: null,
        },
        chart: {
            chartData: null,
            chartOptions: null
        },
        apis: {
            candidates: "/Home/GetCandidates"
        }
    }

    // Get data for chart
    const initChart = function () {
        Swal.fire({
            title: 'Getting results...',
            html: 'Please wait...',
            didOpen: () => {
                Swal.showLoading()
            },
            allowOutsideClick: false
        });

        context.state.electionId = context.state.jQuery.electionSelect.find(':selected').val();
        context.state.electionName = context.state.jQuery.electionSelect.find(':selected').text()

        $.ajax({
            url: context.apis.candidates + '?electionId=' + context.state.electionId,
            type: 'GET',
            success: function (data) {

                Swal.close()

                context.state.chartData = data
                const labels = Object.keys(context.state.chartData.candidatesDict);
                const values = Object.values(context.state.chartData.candidatesDict);
                const winner = context.state.chartData.winner;

                const customColors = [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(100, 200, 100, 0.2)',
                    'rgba(200, 100, 200, 0.2)',
                    'rgba(120, 120, 120, 0.2)',
                    'rgba(0, 255, 0, 0.2)',
                    'rgba(0, 0, 255, 0.2)',
                    'rgba(255, 0, 0, 0.2)',
                    // Add more colors as needed
                ];

                const backgroundColors = customColors.slice(0, labels.length);

                $("#" + context.ids.winnerSpan).text("");
                $("#" + context.ids.winnerSpan).text(winner.key);
                barchartInit(labels, values, backgroundColors, winner);
            }
        });
    }

    const barchartInit = function (labelsData, values, backgroundColors, winner) {
        context.chart.chartData = {
            type: 'bar',
            data: {
                labels: labelsData,
                datasets: [{
                    label: '# of ' + context.state.chartData.electionResult + ' Votes',
                    data: values,
                    backgroundColor: backgroundColors,
                    borderWidth: 3,
                },
                ],
            },
        };

        // Options
        context.chart.chartOptions = {
            plugins: {
                title: {
                    display: true,
                    text: 'My Chart Title', // Set your chart title text
                    font: {
                        size: 18, // Set the font size for the chart title
                    },
                },
            },
            scales: {
                x:
                {
                    stepSize: 1,
                    ticks: {
                        color: '#4285F4',
                    },
                },
                y:
                {
                    stepSize: 1,
                    ticks: {
                        color: '#f44242',
                    },
                },
            },
        };

        // Destroy chart.js bar graph to redraw other graph in same <canvas>
        if (context.state.chartObject != null) {
            context.state.chartObject.destroy();
        }

        context.state.chartObject = new Chart(
            context.state.jQuery.barchart,
            context.chart.chartData,
            context.chart.chartOptions
        );
    }

    const init = function () {

        context.state.jQuery.electionSelect = $(context.ids.select.target);
        context.state.jQuery.barchart = $("#" + context.ids.barchart);

        context.state.jQuery.electionSelect.select2({
            width: '50%'
        });

        initChart();

        context.state.jQuery.electionSelect.on('select2:select', function (e) {
            initChart();
        });
    }

    return {
        init: init
    }

}();

$(function () {
    HomeComponent.init();
})