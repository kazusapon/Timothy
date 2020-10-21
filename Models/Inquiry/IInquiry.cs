using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquiry.View.Models;
using EntityModels;

namespace Inquiry.Model
{
    public interface IInquiry
    {
        Task<List<InquiryIndexLists>> GetIndexListAsync(DateTime? startTime=null, DateTime? endTime=null, int? systemId=0, bool check=false, string freeWord=null);

        Task<EntityModels.Inquiry> FindByIdAsync(int id);

        Task CreateInquiryAsync(EntityModels.Inquiry inquiry);
    }
}