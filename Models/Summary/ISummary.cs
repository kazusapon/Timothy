using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Summary.Model
{
    public interface ISummary
    {
        Task<ChartModel> BuildEachSystemInquiryCountAndTodayAsync(DateTime date);
        
        Task<ChartModel> BuildEachSystemInquiryCountAndMonthlyAsync(DateTime date);

        Task<ChartModel> BuildEachSystemInquiryCountAndYaerAsync(DateTime date);

        Task<ChartModel> BuildEachSystemInquiryCountAndWeekAsync(DateTime date);
    }
}