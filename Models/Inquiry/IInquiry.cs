using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timothy.Models.ViewModels.inquiry;
using Timothy.Models;
using Timothy.Models.Summary;

namespace Timothy.Models.Inquiry
{
    public interface IInquiry : ISummaryData
    {
        Task<List<Entities.Inquiry>> GetIndexListAsync(DateTime? startTime=null, DateTime? endTime=null, int systemId=0, bool check=false, string freeWord=null);

        Task<Entities.Inquiry> FindByIdAsync(int id);

        Task CreateInquiryAsync(Entities.Inquiry inquiry);

        Task UpdateInquiryAsync(Entities.Inquiry inquiry);

        Task DeleteInquiryAsync(int id);

        Task ApprovalInquiryAsync(int id);
    }
}