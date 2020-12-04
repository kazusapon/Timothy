(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        VerticalChartHelper.init();
    });

    class VerticalChartHelper {
        static init() {
            this._download(); 
        }

        static async _download() {
            const chartData = await this._fetchEachSystemCountMonthly();
            if (chartData === null) {
                return;
            }

            console.log(chartData);

            this._buildVerticalChart(chartData)
        }

        static async _fetchEachSystemCountMonthly() {
            return await fetch(`/api/SummaryRest/Monthly`, {
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