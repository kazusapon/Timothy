using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquiry.View.Models;

namespace Inquiry.Model
{
    public interface IInquiry
    {
        Task<List<InquiryIndexLists>> GetIndexListAsync();
    }
}