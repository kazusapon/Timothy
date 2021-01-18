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

        public async Task<PieModel> BuildEachGuestTypeInquiryCountAsync(DateTime date, string searchType)
        {
            var eachGuestTypeCount = await this._inquiry.GetGuestTypeOfCount(date, searchType);

            var pieModel = new PieModel
            {
                Datasets = eachGuestTypeCount.OrderBy(guest => guest.GuestType.Id).Select(guest => guest.InquiryCount).ToList(),
                Labels = eachGuestTypeCount.OrderBy(guest => guest.GuestType.Id).Select(guest => guest.GuestType.GuestTypeName).ToList()
            };

            return pieModel;
        }

        public async Task<ChartModel> BuildEachSystemInquiryCountAndTodayAsync(DateTime date)
        {
            var eachSystemCountForToday = await this._inquiry.GetTodaySystemsCountAsync(date);

            var chartModel = new ChartModel
            {
                Datasets = BuildChartModelForEachSystemInquiryCountAsync(eachSystemCountForToday),
                Labels = HourText()
            };

            return chartModel;
        }

        public async Task<ChartModel> BuildEachSystemInquiryCountAndMonthlyAsync(DateTime date)
        {
            var eachSystemCountForMonth = await this._inquiry.GetMonthlySystemsCountAsync(date);

            var chartModel = new ChartModel
            {
                Datasets = BuildChartModelForEachSystemInquiryCountAsync(eachSystemCountForMonth),
                Labels = MonthText()
            };

            return chartModel;
        }

        public async Task<ChartModel> BuildEachSystemInquiryCountAndYaerAsync(DateTime date)
        {
            var eachSystemCountForYear = await this._inquiry.GetYearSystemsCountAsync(date);

            var chartModel = new ChartModel
            {
                Datasets = BuildChartModelForEachSystemInquiryCountAsync(eachSystemCountForYear),
                Labels = YearText(date)
            };

            return chartModel;
        }

        public async Task<ChartModel> BuildEachSystemInquiryCountAndWeekAsync(DateTime date)
        {
            var eachSystemCountForWeek = await this._inquiry.GetWeekSystemsCountAsync(date);

            var chartModel = new ChartModel
            {
                Datasets = BuildChartModelForEachSystemInquiryCountAsync(eachSystemCountForWeek),
                Labels = DayOfWeekText()
            };

            return chartModel;
        }

        private List<DatasetModel> BuildChartModelForEachSystemInquiryCountAsync(List<SystemsCountModel> systemsCount)
        {
            List<DatasetModel> datasets = new();

            foreach(var system in this._context.System)
            {
                DatasetModel datasetModel = new DatasetModel
                {
                    Label = system.SystemName,
                    Data = systemsCount
                                .Where(inquiry => inquiry.System.Id == system.Id)
                                .OrderBy(inquiry => inquiry.YearOrMonth)
                                .Select(inquiry => inquiry.InquiryCount)
                                .ToList()
                };

                datasets.Add(datasetModel);
            };

            return datasets;
        }

        private List<string> HourText()
        {
            List<string> hours = new();

            for(var i=0; i < 24; i++)
            {
                hours.Add(i.ToString() + "時");
            }

            return hours;
        }

        private List<string> MonthText()
        {
            return new List<string>{ "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
        }

        private List<string> YearText(DateTime date)
        {
            int MaxYear = 11;
            int baseYear = date.Year - 5;

            List<string> text = new();

            for(var i = 0; i < MaxYear; i++)
            {
               text.Add((baseYear + i) + "年");
            }

            return text;
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