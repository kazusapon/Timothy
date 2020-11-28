using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Summary.Model
{
    public interface ISummary
    {
        Task<ChartModel> BuildEachSystemInquiryCountAndMonthlyAsync(DateTime date);
    }
}