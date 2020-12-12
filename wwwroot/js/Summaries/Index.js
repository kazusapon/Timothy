(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        VerticalChartHelper.init();
    });

    class VerticalChartHelper {
        static init() {
            this._swicherEventListner();
            this._initVerticalChart();
        }

        static _swicherEventListner() {
            const swichers = document.querySelectorAll("#display-swicher a");

            [...swichers].forEach(swicher => {
                swicher.addEventListener('click', () => {
                    const displaySwicherHidden = document.querySelector("#display-swicher input[type=hidden]");

                    displaySwicherHidden.value = swicher.className;
                    this._initVerticalChart();

                    return this;
                })
            })
        }

        static async _initVerticalChart() {
            const chartData = await this._fetchEachSystemCountMonthly();
            if (chartData === null) {
                return;
            }

            if (this.myChart != null) {
                this.myChart.destory();
            }
            
            this._setTotalCount(chartData.datasets);
            this._buildVerticalChart(chartData);
        }

        static async _fetchEachSystemCountMonthly() {
            const displaySwicher = document.querySelector("#display-swicher input[type=hidden]").value;
            const dateString = document.querySelector("#reference-date").value;
            
            return await fetch(`/api/SummaryRest/${displaySwicher}/${dateString}`, {
                method: 'GET'
            }).then((responce) => {
                if (responce.ok) {
                   return responce.json()
                }
            });
        }

        static _setTotalCount(datasets) {
            const totalCount = document.querySelector("#total-count");
            var total = 0;

            [...datasets].forEach(dataset => {
                const data = dataset.data;

                total += data.reduce(function(a, x) { return a + x; });
            });
            
            totalCount.textContent = total;
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