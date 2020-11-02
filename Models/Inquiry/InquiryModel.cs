using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Inquiry.View.Models;
using Utils;

namespace Inquiry.Model
{
    public class InquiryModel : IInquiry
    {
        private readonly DatabaseContext _context;

        public InquiryModel(DatabaseContext context)
        {
            this._context = context;
        }
        
        public async Task<List<InquiryIndexLists>> GetIndexListAsync(DateTime? startTime=null, DateTime? endTime=null, int systemId=0, bool check=true, string freeWord=null)
        {
            return await this._context.Inquiry
                    .Join(this._context.System,
                        inquiry => inquiry.SystemId,
                        system => system.Id,
                        (Inquiry, system) => new {
                            Inquiry = Inquiry,
                            SystemName = system.SystemName
                        })
                    .Join(this._context.User,
                        inquiry => inquiry.Inquiry.UserId,
                        user => user.Id,
                        (inquiry, user) => new {
                            Inquiry = inquiry,
                            UserName = user.UserName
                        })
                    .Where(inquiry => inquiry.Inquiry.Inquiry.DaletedAt == null)
                    .WhereIf(startTime != null, inquiry => startTime >= inquiry.Inquiry.Inquiry.StartTime)
                    .WhereIf(endTime != null, inquiry => inquiry.Inquiry.Inquiry.EndTime <= endTime)
                    .WhereIf(systemId != 0, inquiry => inquiry.Inquiry.Inquiry.SystemId == systemId)
                    .WhereIf(check, inquiry => !inquiry.Inquiry.Inquiry.ApprovalFlag)
                    .WhereIf((freeWord != null || freeWord == ""),
                        inquiry => inquiry.Inquiry.Inquiry.InquirerName.Contains(freeWord)
                        || inquiry.Inquiry.Inquiry.TelephoneNumber.Contains(freeWord)
                        || inquiry.Inquiry.Inquiry.SpareTelephoneNumber.Contains(freeWord)
                        || inquiry.UserName.Contains(freeWord)
                        || inquiry.Inquiry.Inquiry.Question.Contains(freeWord)
                        || inquiry.Inquiry.Inquiry.Answer.Contains(freeWord)
                    )
                    .OrderByDescending(inquiry => inquiry.Inquiry.Inquiry.IncomingDate)
                    .ThenByDescending(inquiry => inquiry.Inquiry.Inquiry.StartTime)
                    .ThenByDescending(inquiry => inquiry.Inquiry.Inquiry.Id)
                    .Select(inquiry => new InquiryIndexLists{
                            Id = inquiry.Inquiry.Inquiry.Id,
                            IncomingDateTime = inquiry.Inquiry.Inquiry.IncomingDate,
                            CompanyName = inquiry.Inquiry.Inquiry.CompanyName,
                            InquirerName = inquiry.Inquiry.Inquiry.InquirerName,
                            TelephoneNumber = inquiry.Inquiry.Inquiry.TelephoneNumber,
                            SystemName = inquiry.Inquiry.SystemName,
                            UserName = inquiry.UserName,
                            Question = inquiry.Inquiry.Inquiry.Question,
                            Answer = inquiry.Inquiry.Inquiry.Answer
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<EntityModels.Inquiry> FindByIdAsync(int id)
        {
            return await this._context.Inquiry.FirstOrDefaultAsync(inquiry => inquiry.Id == id);
        }

        public async Task<EntityModels.Inquiry> FindByTelephoneNumberLastRecordAsync(string telephoneNumber)
        {
            return await this._context.Inquiry
                    .Where(inquiry => inquiry.TelephoneNumber == telephoneNumber)
                    .Where(inquiry => inquiry.DaletedAt == null)
                    .OrderByDescending(inquiry => inquiry.IncomingDate)
                    .ThenByDescending(inquiry => inquiry.StartTime)
                    .ThenByDescending(inquiry => inquiry.Id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
        }

        public async Task CreateInquiryAsync(EntityModels.Inquiry inquiry)
        {
            this._context.Inquiry.Add(inquiry);
            await this._context.SaveChangesAsync();
            
            return;
        }

        public async Task UpdateInquiryAsync(EntityModels.Inquiry inquiry)
        {
            this._context.Update(inquiry);
            await this._context.SaveChangesAsync();
            
            return;
        }

        public async Task DeleteInquiryAsync(int id)
        {
            var inquiry = await this.FindByIdAsync(id);
            if (inquiry == null)
            {
                return;
            }
            
            inquiry.DaletedAt = DateTime.Now;
            await this._context.SaveChangesAsync();

            return;
        }

        public async Task ApprovalInquiryAsync(int id)
        {
            var inquiry = await this.FindByIdAsync(id);
            if (inquiry == null)
            {
                return;
            }
            
            inquiry.ApprovalFlag = true;
            await this._context.SaveChangesAsync();

            return;
        }
    }
}