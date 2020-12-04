using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Inquiry.View.Models;
using Utils;
using Summary.Model;

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

        // searchType
        //  "year" => 抽出条件なし
        //  "monthly" => "年で抽出"
        //  "weekly" => "年と月で抽出"
        public async Task<List<SystemsCountModel>> GetMonthlySystemsCountAsync(DateTime date, string searchType)
        {
            var inquiryies = await (from system in this._context.System
                                    join inquiry in (this._context.Inquiry
                                        .WhereIf(searchType == "monthly",  inquiry => inquiry.IncomingDate.Year == date.Year)
                                        .WhereIf(searchType == "weekly",  inquiry => inquiry.IncomingDate.Year == date.Year)
                                        .WhereIf(searchType == "weekly" ,inquiry => inquiry.IncomingDate.Month == date.Month)
                                    )
                                on system equals inquiry.System into gj
                                from subInquiry in gj.DefaultIfEmpty()
                                select new SystemsCountModel
                                {
                                    System = system,
                                    YearOrMonth = subInquiry.IncomingDate.Month,
                                    InquiryCount = system.Inquiries.Count()
                                }).ToListAsync();

            var eachSystemCount = inquiryies.GroupBy(inquiry => new {system = inquiry.System, month = inquiry.YearOrMonth})
                    .OrderBy(inquiry => inquiry.Key.system.Id)
                    .ThenBy(inquiry => inquiry.Key.month)
                    .Select(inquiry =>  new SystemsCountModel()
                    {
                        System = inquiry.Key.system,
                        YearOrMonth = inquiry.Key.month,
                        InquiryCount = inquiry.First().InquiryCount
                    })
                    .ToList();

            var eachSystemMonthlyCount = new List<SystemsCountModel>();
            // 各システム毎に、1～12月まででデータがないものについて、仮データ（件数ゼロ）をセットする。
            foreach (var systemCount in eachSystemCount)
            {
                for(var month=1; month <= 12; month++)
                {
                    if (systemCount.YearOrMonth != month)
                    {
                        eachSystemMonthlyCount.Add(new SystemsCountModel
                        {
                            System = systemCount.System,
                            YearOrMonth = month,
                            InquiryCount = 0
                        });
                    }
                    else
                    {
                        eachSystemMonthlyCount.Add(systemCount);
                    }
                }
            }

            return eachSystemMonthlyCount;
        }
    }
}