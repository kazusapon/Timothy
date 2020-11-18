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
        
        public async Task<List<EntityModels.Inquiry>> GetIndexListAsync(DateTime? startTime=null, DateTime? endTime=null, int systemId=0, bool check=true, string freeWord=null)
        {
            return await this._context.Inquiry
                    .Where(inquiry => inquiry.DaletedAt == null)
                    .WhereIf(startTime != null, inquiry => startTime >= inquiry.StartTime)
                    .WhereIf(endTime != null, inquiry => inquiry.EndTime <= endTime)
                    .WhereIf(systemId != 0, inquiry => inquiry.System.Id == systemId)
                    .WhereIf(check, inquiry => !inquiry.ApprovalFlag)
                    .WhereIf((freeWord != null || freeWord == ""),
                        inquiry => inquiry.InquirerName.Contains(freeWord)
                        || inquiry.TelephoneNumber.Contains(freeWord)
                        || inquiry.SpareTelephoneNumber.Contains(freeWord)
                        || inquiry.User.UserName.Contains(freeWord)
                        || inquiry.Question.Contains(freeWord)
                        || inquiry.Answer.Contains(freeWord))
                    .Include(model => model.System)
                    .Include(model => model.User)
                    .OrderByDescending(inquiry => inquiry.IncomingDate)
                    .ThenByDescending(inquiry => inquiry.StartTime)
                    .ThenByDescending(inquiry => inquiry.Id)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<EntityModels.Inquiry> FindByIdAsync(int id)
        {
            return await this._context.Inquiry
                    .Where(inquiry => inquiry.Id == id)
                    .Include(model => model.System)
                    .Include(model => model.User)
                    .Include(model => model.GuestType)
                    .Include(model => model.Classification)
                    .Include(model => model.ContactMethod)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
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
            this._context.Entry(inquiry).State = EntityState.Added;
            this._context.Inquiry.Add(inquiry);
            await this._context.SaveChangesAsync();
            
            return;
        }

        public async Task UpdateInquiryAsync(EntityModels.Inquiry inquiry)
        {
            this._context.Entry(inquiry).State = EntityState.Modified;
            this._context.Entry(inquiry.System).State = EntityState.Unchanged;
            this._context.Entry(inquiry.ContactMethod).State = EntityState.Unchanged;
            this._context.Entry(inquiry.Classification).State = EntityState.Unchanged;
            this._context.Entry(inquiry.User).State = EntityState.Unchanged;
            this._context.Entry(inquiry.GuestType).State = EntityState.Unchanged;

            this._context.Update(inquiry);
            await this._context.SaveChangesAsync();
            
            return;
        }

        public async Task DeleteInquiryAsync(int id)
        {
            var inquiry = await this.FindByIdAsync(id);
            this._context.Entry(inquiry).State = EntityState.Deleted;
            this._context.Entry(inquiry.System).State = EntityState.Unchanged;
            this._context.Entry(inquiry.ContactMethod).State = EntityState.Unchanged;
            this._context.Entry(inquiry.Classification).State = EntityState.Unchanged;
            this._context.Entry(inquiry.User).State = EntityState.Unchanged;
            this._context.Entry(inquiry.GuestType).State = EntityState.Unchanged;
            
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
            
            this._context.Entry(inquiry).State = EntityState.Modified;
            inquiry.ApprovalFlag = true;
            await this._context.SaveChangesAsync();

            return;
        }
    }
}