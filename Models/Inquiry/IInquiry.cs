using System.Collections.Generic;
using System.Threading.Tasks;
using Inquiry.View.Models;
using EntityModels;

namespace Inquiry.Model
{
    public interface IInquiry
    {
        Task<List<InquiryIndexLists>> GetIndexListAsync();

        Task CreateInquiryAsync(EntityModels.Inquiry inquiry);
    }
}