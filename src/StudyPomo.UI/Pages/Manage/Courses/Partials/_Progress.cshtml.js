document.addEventListener('DOMContentLoaded', function () {

    drawProgressChart();

});

document.addEventListener('htmx:afterSwap', function () {
    drawProgressChart();
});

function drawProgressChart() {

    let myChart = echarts.init(document.getElementById('main'));

    let option = {
        xAxis: {
            type: 'category',
            data: dates,
            axisLabel: {
                formatter: function (value) {
                    return value;
                }
            }
        },
        yAxis: {
            type: 'value',
            minInterval: 1,  // Ensure Y-axis increments by 1
            axisLabel: {
                formatter: function (value) {
                    return Math.floor(value);  // Show whole numbers only
                }
            }
        },
        series: [
            {
                data: counts,
                type: 'line',
                smooth: true
            }
        ],
        grid: {
            left: '0%',
            right: '0%',
            top: '5%',
            bottom: '5%',
            containLabel: true
        }
    };

    myChart.setOption(option);
}