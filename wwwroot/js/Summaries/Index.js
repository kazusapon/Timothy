(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        SwicherHelper.init();
        VerticalChartHelper.init();
        PieChartHelper.init();
    });

    class SwicherHelper {
        static init() {
            this._swicherEventListner();
        }

        static _swicherEventListner() {
            const swichers = document.querySelectorAll("#display-swicher a");

            [...swichers].forEach(swicher => {
                swicher.addEventListener('click', () => {
                    const displaySwicherHidden = document.querySelector("#display-swicher input[type=hidden]");

                    displaySwicherHidden.value = swicher.className;
                    VerticalChartHelper._initVerticalChart();
                    PieChartHelper._initGuestTypeChart();
                })
            })
        }
    }

    class PieChartHelper {
        static init() {
            const guestPieChart = null;
            this._initGuestTypeChart();
        }
        
        static async _initGuestTypeChart() {
            const chartData = await this._fetchEachGuestTypeCount();
            if (chartData === null) {
                return;
            }

            if (this.guestPieChart) {
                this.guestPieChart.destroy();
            }
            
            this._buildGuestTypePieChart(chartData);
        }

        static async _fetchEachGuestTypeCount() {
            const dateString = document.querySelector("#reference-date").value;
            const displaySwicher = document.querySelector("#display-swicher input[type=hidden]").value;

            return await fetch(`/api/SummaryRest/guestType/${dateString}/${displaySwicher}`, {
                method: 'GET'
            }).then((responce) => {
                if (responce.ok) {
                   return responce.json()
                }
            });
        }

        static _buildGuestTypePieChart(chartData) {
            const canvas1 = document.querySelector("#guest-type-chart");
            this.guestPieChart = new Chart(canvas1, {
                type: 'pie',
                data: {
                    labels: chartData.labels,
                    datasets: [{
                        data: chartData.datasets
                    }]
                },
                options: {
                    legend: {
                        display: true,
                        position: 'bottom'
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

    class VerticalChartHelper {
        static init() {
            const myChart = null;
            this._initVerticalChart();
        }

        static async _initVerticalChart() {
            const chartData = await this._fetchEachSystemCountMonthly();
            if (chartData === null) {
                return;
            }

            if (this.myChart) {
                this.myChart.destroy();
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

        static _getMaxCount(datasets) {
            var maxCount = 0;
            var total = 0;
            [...datasets].forEach(dataset => {
                const data = dataset.data;

                total = data.reduce(function(a, x) { return a + x; });
                if (total > maxCount) {
                    maxCount = total;
                }
            });
            
            return maxCount;
        }

        static _buildVerticalChart(chartData) {
            const canvas = document.querySelector("#chart");
            this.myChart = new Chart(canvas, {
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
                            suggestedMax: this._getMaxCount(chartData.datasets) + 10,
                            suggestedMin: 0,
                            stepSize: 50,
                            callback: function(value, index, values){
                              return  value + '件'
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