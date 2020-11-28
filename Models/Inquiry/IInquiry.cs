using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquiry.View.Models;
using EntityModels;
using Summary.Model;

namespace Inquiry.Model
{
    public interface IInquiry : ISummaryData
    {
        Task<List<EntityModels.Inquiry>> GetIndexListAsync(DateTime? startTime=null, DateTime? endTime=null, int systemId=0, bool check=false, string freeWord=null);

        Task<EntityModels.Inquiry> FindByIdAsync(int id);

        Task CreateInquiryAsync(EntityModels.Inquiry inquiry);

        Task UpdateInquiryAsync(EntityModels.Inquiry inquiry);

        Task DeleteInquiryAsync(int id);

        Task ApprovalInquiryAsync(int id);
    }
}