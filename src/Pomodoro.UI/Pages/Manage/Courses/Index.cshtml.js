let course;

document.addEventListener('DOMContentLoaded', init);

function init() {
    drawChart();
}

function drawChart() {
    $.ajax({
        url: `?handler=ProgressStats&courseId=${course.Id}`,
        dataType: 'json',
        success: function (data) {
            console.log("New data:", data);

            const dates = data.map(item => item.date);
            const counts = data.map(item => item.count);

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
        },
        error: function (error) {
            console.log(error);
        }
    });
}

