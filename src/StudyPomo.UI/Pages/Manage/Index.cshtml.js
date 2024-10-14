﻿const COMPLETED_CONTAINER_ID = 'completed-chart';


document.addEventListener('DOMContentLoaded', function () {
    drawCompleted();
});


function drawCompleted() {
    var chartDom = document.getElementById(COMPLETED_CONTAINER_ID);
    var myChart = echarts.init(chartDom);
    var option;

    option = {
        series: [
            {
                type: 'gauge',
                startAngle: 180,
                endAngle: 0,
                min: 0,
                max: 240,
                splitNumber: 12,
                itemStyle: {
                    color: '#58D9F9',
                    shadowColor: 'rgba(0,138,255,0.45)',
                    shadowBlur: 10,
                    shadowOffsetX: 2,
                    shadowOffsetY: 2
                },
                progress: {
                    show: true,
                    roundCap: true,
                    width: 18
                },
                pointer: {
                    show: false,
                    icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
                    length: '75%',
                    width: 16,
                    offsetCenter: [0, '5%']
                },
                axisLine: {
                    roundCap: true,
                    lineStyle: {
                        width: 18
                    }
                },
                axisTick: {
                    show: false,

                },
                splitLine: {
                    show: false,
                },
                axisLabel: {
                    show: false,
                },
                title: {
                    show: false
                },

                data: [
                    {
                        detail: {
                            show: false
                        },
                        value: 100
                    }
                ]
            }
        ]
    };

    myChart.setOption(option);

}