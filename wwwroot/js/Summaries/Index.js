(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        VerticalChartHelper.init();
    });

    class VerticalChartHelper {
        static init() {
            this._buildChart();
            this._download(); 
        }

        static async _download() {
            const chartData = await this._fetchEachSystemCountMonthly();
            if (chartData === null) {
                return;
            }

            console.log(chartData);
        }

        static async _fetchEachSystemCountMonthly() {
            return await fetch(`/api/SummaryRest`, {
                method: 'GET'
            }).then((responce) => {
                if (responce.ok) {
                   return responce.json()
                }
            });
        }

        static _buildChart() {
            const canvas = document.querySelector("#chart");
            const myChart = new Chart(canvas, {
                type: 'bar',
                data: {
                    labels: ["M", "T", "W", "T", "F", "S", "S"],
                    datasets: [{
                        label: 'apples',
                        data: [12, 19, 3, 17, 28, 24, 7]
                    }, 
                    {
                        label: 'oranges',
                        data: [30, 29, 5, 5, 20, 3, 10]
                    }]
                },
                options: {
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