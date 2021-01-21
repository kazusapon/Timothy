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

        public async Task<List<GuestTypePieChartModel>> GetGuestTypeOfCount(DateTime date, string searchType)
        {
            return await this._context.GuestType
                    .Select(guest => new GuestTypePieChartModel
                    {
                        GuestType = guest,
                        InquiryCount = guest.Inquiries
                                    .Where(inquiry => searchType != "monthly" || inquiry.IncomingDate.Year == date.Year)
                                    .Where(inquiry => searchType != "weekly" || inquiry.IncomingDate.Month == date.Month)
                                    .Where(inquiry => searchType != "weekly"  || inquiry.IncomingDate >= getDateByWeek(date, 0))
                                    .Where(inquiry => searchType != "weekly" || inquiry.IncomingDate <= getDateByWeek(date, 6))
                                    .Where(inquiry => searchType != "today" || inquiry.IncomingDate == date)
                                    .Count()
                    })
                    .ToListAsync();
        }

        public async Task<List<SystemsCountModel>> GetTodaySystemsCountAsync(DateTime date)
        {
            var inquiryies = await (from system in this._context.System
                                    join inquiry in (this._context.Inquiry
                                        .Where(inquiry => inquiry.IncomingDate == date)
                                    )
                                on system equals inquiry.System into gj
                                from subInquiry in gj.DefaultIfEmpty()
                                select new SystemsCountModel
                                {
                                    System = system,
                                    YearOrMonth = subInquiry.StartTime.Hour,
                                    InquiryCount = system.Inquiries.Count()
                                }).ToListAsync();

            var eachSystemCount = inquiryies.GroupBy(inquiry => new {system = inquiry.System, hour = inquiry.YearOrMonth})
                    .OrderBy(inquiry => inquiry.Key.system.Id)
                    .ThenBy(inquiry => inquiry.Key.hour)
                    .Select(inquiry =>  new SystemsCountModel()
                    {
                        System = inquiry.Key.system,
                        YearOrMonth = inquiry.Key.hour,
                        InquiryCount = inquiry.First().InquiryCount
                    })
                    .ToList();

            return FillHour(eachSystemCount);
        }

        public async Task<List<SystemsCountModel>> GetMonthlySystemsCountAsync(DateTime date)
        {
            var inquiryies = await (from system in this._context.System
                                    join inquiry in (this._context.Inquiry
                                        .Where(inquiry => inquiry.IncomingDate.Year == date.Year)
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

            return FillMonth(eachSystemCount);
        }

        public async Task<List<SystemsCountModel>> GetYearSystemsCountAsync(DateTime date)
        {
            var eachSystemCount = await this._context.System.GroupJoin(
                                    this._context.Inquiry,
                                    sys => sys,
                                    inq => inq.System,
                                    (sys, inq) => new {
                                        Systems = sys,
                                        Inquiries = inq
                                    }).SelectMany(x => x.Inquiries.DefaultIfEmpty(), (x, inquiry) => new SystemsCountModel()
                                    {
                                        System = x.Systems,
                                        YearOrMonth = inquiry.TelephoneNumber == null ? null : inquiry.IncomingDate.Year,
                                        InquiryCount = x.Systems.Inquiries.Where(x => x.IncomingDate.Year == inquiry.IncomingDate.Year).Count()
                                    })
                                    .Distinct()
                                    .OrderBy(inquiry => inquiry.System.Id)
                                    .ThenBy(inquiry => inquiry.YearOrMonth)
                                    .ToListAsync();

            return FillFiveYearsBeforeAndAfter(eachSystemCount, date.Year);
        }

        public async Task<List<SystemsCountModel>> GetWeekSystemsCountAsync(DateTime date)
        {
            DateTime sunday = getDateByWeek(date, 0);
            DateTime saturday = getDateByWeek(date, 6);
            var inquiryies = await (from system in this._context.System
                                    join inquiry in (this._context.Inquiry
                                    .Where(inquiry => inquiry.IncomingDate.Year == date.Year)
                                    .Where(inquiry => inquiry.IncomingDate.Month == date.Month)
                                    .Where(inquiry => inquiry.IncomingDate >= sunday)
                                    .Where(inquiry => inquiry.IncomingDate <= saturday)
                                )
                                on system equals inquiry.System into gj
                                from subInquiry in gj.DefaultIfEmpty()
                                select new SystemsCountModel
                                {
                                    System = system,
                                    YearOrMonth = (int?)subInquiry.IncomingDate.DayOfWeek,
                                    InquiryCount = system.Inquiries.Count()
                                }).ToListAsync();

            var eachSystemCount = inquiryies.GroupBy(inquiry => new {system = inquiry.System, week = inquiry.YearOrMonth})
                    .OrderBy(inquiry => inquiry.Key.system.Id)
                    .ThenBy(inquiry => inquiry.Key.week)
                    .Select(inquiry =>  new SystemsCountModel()
                    {
                        System = inquiry.Key.system,
                        YearOrMonth = inquiry.Key.week,
                        InquiryCount = inquiry.First().InquiryCount
                    })
                    .ToList();

            return FillWeek(eachSystemCount);
        }

        private List<SystemsCountModel> FillHour(List<SystemsCountModel> eachSystemCount)
        {
            var eachSystemMonthlyCount = new List<SystemsCountModel>();

            foreach (var systemCount in eachSystemCount)
            {
                for(var hour=0; hour <= 23; hour++)
                {
                    if (systemCount.YearOrMonth != hour)
                    {
                        eachSystemMonthlyCount.Add(new SystemsCountModel
                        {
                            System = systemCount.System,
                            YearOrMonth = hour,
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

        private List<SystemsCountModel> FillMonth(List<SystemsCountModel> eachSystemCount)
        {
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

        private List<SystemsCountModel> FillFiveYearsBeforeAndAfter(List<SystemsCountModel> eachSystemCount, int year)
        {
            var eachYearsCount = new List<SystemsCountModel>();
            int baseYear = year - 5;
            int maxYear = 11;

            foreach(var system in this._context.System)
            {
                for(int i = 0; i < maxYear; i++)
                {
                    var inquiry = eachSystemCount.Where(inquiry => inquiry.System.Id == system.Id).Where(inquiry => inquiry.YearOrMonth == baseYear + i);
                    if (inquiry.Any())
                    {
                        eachYearsCount.Add(inquiry.First());
                    }
                    else
                    {
                        var eachYears = new SystemsCountModel()
                        {
                            System = system,
                            YearOrMonth = baseYear + i,
                            InquiryCount = 0
                        };

                        eachYearsCount.Add(eachYears);
                    }
                }
            }

            return eachYearsCount;
        }

        private List<SystemsCountModel> FillWeek(List<SystemsCountModel> eachSystemCount)
        {
            var eachSystemMonthlyCount = new List<SystemsCountModel>();
            // 各システム毎に、1～12月まででデータがないものについて、仮データ（件数ゼロ）をセットする。
            foreach (var systemCount in eachSystemCount)
            {
                for(var week=0; week < 7; week++)
                {
                    if (systemCount.YearOrMonth != week)
                    {
                        eachSystemMonthlyCount.Add(new SystemsCountModel
                        {
                            System = systemCount.System,
                            YearOrMonth = week,
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

        private DateTime getDateByWeek(DateTime date, int targetWeek)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            int weekDiff = targetWeek - dayOfWeek;
            
            return date.AddDays(weekDiff);
        }
    }
}