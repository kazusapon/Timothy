using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timothy.Models.Summary
{
    public interface ISummary
    {
        Task<PieModel> BuildEachGuestTypeInquiryCountAsync(DateTime date, string searchType);

        Task<ChartModel> BuildEachSystemInquiryCountAndTodayAsync(DateTime date);
        
        Task<ChartModel> BuildEachSystemInquiryCountAndMonthlyAsync(DateTime date);

        Task<ChartModel> BuildEachSystemInquiryCountAndYaerAsync(DateTime date);

        Task<ChartModel> BuildEachSystemInquiryCountAndWeekAsync(DateTime date);
    }
}