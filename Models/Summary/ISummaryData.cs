using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Summary.Model
{
    public interface ISummaryData
    {
        Task<List<SystemsCountModel>> GetMonthlySystemsCountAsync(DateTime date, string searchType);
    }
}
