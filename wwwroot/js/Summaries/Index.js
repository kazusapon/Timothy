(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        VerticalChartHelper.init();
    });

    class VerticalChartHelper {
        static init() {
            this._initVerticalChart(); 
        }

        static async _initVerticalChart() {
            const chartData = await this._fetchEachSystemCountMonthly();
            if (chartData === null) {
                return;
            }

            this._buildVerticalChart(chartData);
        }

        static async _fetchEachSystemCountMonthly() {
            const dateString = document.querySelector("#reference-date").value;
            
            return await fetch(`/api/SummaryRest/Monthly/${dateString}`, {
                method: 'GET'
            }).then((responce) => {
                if (responce.ok) {
                   return responce.json()
                }
            });
        }

        static _buildVerticalChart(chartData) {
            const canvas = document.querySelector("#chart");
            const myChart = new Chart(canvas, {
                type: 'bar',
                data: chartData,
                options: {
                    legend: {
                        display: true,
                        position: 'bottom'
                    },
                    scales: {
                        yAxes: [{
                          ticks: {
                            suggestedMax: 100,
                            suggestedMin: 0,
                            stepSize: 10,
                            callback: function(value, index, values){
                              return  value +  '件'
                            }
                          }
                        }]
                      },
                    plugins: {
                        colorschemes: {
                            scheme: 'brewer.PastelOne8'
                        }
                    }
                }
            });
        }
    }
})();