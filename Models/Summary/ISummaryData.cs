using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timothy.Models.Summary
{
    public interface ISummaryData
    {
        Task<List<GuestTypePieChartModel>> GetGuestTypeOfCount(DateTime date, string searchType);

        Task<List<SystemsCountModel>> GetTodaySystemsCountAsync(DateTime date);
        
        Task<List<SystemsCountModel>> GetMonthlySystemsCountAsync(DateTime date);

        Task<List<SystemsCountModel>> GetYearSystemsCountAsync(DateTime date);

        Task<List<SystemsCountModel>> GetWeekSystemsCountAsync(DateTime date);
    }
}
