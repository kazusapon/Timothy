using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Database.Models;
using Inquiry.View.Models;
using Utils;
using Microsoft.EntityFrameworkCore;
using EntityModels;
using Inquiry.Model;
using Summary.Model;

namespace Summary.Model
{
    public class SummaryModel : ISummary
    {
        private readonly DatabaseContext _context;

        private readonly IInquiry _inquiry; 

        public SummaryModel(DatabaseContext context, IInquiry inquiry)
        {
            this._context = context;
            this._inquiry = inquiry;
        }

        public async Task<ChartModel> BuildEachSystemInquiryCountAndMonthlyAsync(DateTime date)
        {
            var chartModel = new ChartModel
            {
                Datasets = await BuildChartModelForEachSystemInquiryCountAsync(date, "monthly"),
                Labels = MonthText()
            };

            return chartModel;
        }

        private async Task<List<DatasetModel>> BuildChartModelForEachSystemInquiryCountAsync(DateTime date, string searchType)
        {
            List<DatasetModel> datasets = new();
            var eachSystemCountForMonth = await this._inquiry.GetMonthlySystemsCountAsync(date, searchType);

            foreach(var system in this._context.System)
            {
                DatasetModel datasetModel = new DatasetModel
                {
                    Label = system.SystemName,
                    Data = eachSystemCountForMonth
                                .Where(inquiry => inquiry.System.Id == system.Id)
                                .OrderBy(inquiry => inquiry.YearOrMonth)
                                .Select(inquiry => inquiry.InquiryCount)
                                .ToList()
                };

                datasets.Add(datasetModel);
            };

            return datasets;
        }

        private List<string> MonthText()
        {
            return new List<string>{ "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
        }

        private List<string> DayOfWeekText()
        {
            return new List<string>{ "日", "月", "火", "水", "木", "金", "土" };
        }
    }

    public class VerticalChartModel
    {
        public EntityModels.System System {get; set;}

        public int YearOrMonth {get; set;}

        public int InquiryCount { get; set;}
    }
}