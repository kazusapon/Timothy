using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Inquiry.View.Models;

namespace Inquiry.Model
{
    public class InquiryModel
    {
        private readonly DatabaseContext _context;

        public InquiryModel(DatabaseContext context)
        {
            this._context = context;
        }
        
        public async Task<List<InquiryIndexLists>> GetIndexListsAsync()
        {
            return await (from inquiry in this._context.Inquiry
                                        join system in this._context.System
                                        on inquiry.SystemId equals system.Id
                                        join user in this._context.User
                                        on inquiry.UserId equals user.Id
                                        orderby inquiry.IncomingDate descending
                                        orderby inquiry.Id descending
                                        select new InquiryIndexLists
                                        {
                                            Id = inquiry.Id,
                                            IncomingDateTime = inquiry.IncomingDate,
                                            CompanyName = inquiry.ComapnyName,
                                            InquirerName = inquiry.InquirerName,
                                            TelephoneNumber = inquiry.TelephoneNumber,
                                            SystemName = system.Abbreviation,
                                            UserName = user.UserName,
                                            Question = inquiry.Question,
                                            Answer = inquiry.Answer
                                        }).AsNoTracking().ToListAsync();
        }
    }
}