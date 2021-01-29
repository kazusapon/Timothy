using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Timothy.Models;
using Timothy.Models.ViewModels.inquiry;
using Timothy.Utils;
using Timothy.Models.Summary;

namespace Timothy.Models.Inquiry
{
    public class InquiryModel : IInquiry
    {
        private readonly DatabaseContext _context;

        public InquiryModel(DatabaseContext context)
        {
            this._context = context;
        }
        
        public async Task<List<Entities.Inquiry>> GetIndexListAsync(DateTime? startDate=null, DateTime? endDate=null, int systemId=0, bool check=true, string freeWord=null)
        {
            return await this._context.Inquiry
                    .Where(inquiry => inquiry.DaletedAt == null)
                    .WhereIf(startDate != null, inquiry => inquiry.IncomingDate >= startDate)
                    .WhereIf(endDate != null, inquiry => inquiry.IncomingDate <= endDate)
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

        public async Task<Entities.Inquiry> FindByIdAsync(int id)
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

        public async Task<Entities.Inquiry> FindByTelephoneNumberLastRecordAsync(string telephoneNumber)
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

        public async Task CreateInquiryAsync(Entities.Inquiry inquiry)
        {
            this._context.Entry(inquiry).State = EntityState.Added;
            this._context.Inquiry.Add(inquiry);
            await this._context.SaveChangesAsync();
            
            return;
        }

        public async Task UpdateInquiryAsync(Entities.Inquiry inquiry)
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
                                    .Where(inquiry => inquiry.DaletedAt == null)
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
            var eachSystemCount = await this._context.System.GroupJoin(
                                    this._context.Inquiry
                                        .Where(inquiry => inquiry.DaletedAt == null)
                                        .Where(inquiry => inquiry.IncomingDate == date),
                                    sys => sys,
                                    inq => inq.System,
                                    (sys, inq) => new {
                                        Systems = sys,
                                        Inquiries = inq
                                    }).SelectMany(x => x.Inquiries.DefaultIfEmpty(), (x, inquiry) => new SystemsCountModel()
                                    {
                                        System = x.Systems,
                                        YearOrMonth = inquiry.TelephoneNumber == null ? null : inquiry.StartTime.Hour,
                                        InquiryCount = x.Systems.Inquiries.Where(inquiry => inquiry.IncomingDate == date).Count()
                                    })
                                    .Distinct()
                                    .OrderBy(inquiry => inquiry.System.Id)
                                    .ThenBy(inquiry => inquiry.YearOrMonth)
                                    .ToListAsync();

            return FillHour(eachSystemCount);
        }

        public async Task<List<SystemsCountModel>> GetMonthlySystemsCountAsync(DateTime date)
        {
            var eachSystemCount = await this._context.System.GroupJoin(
                                    this._context.Inquiry.Where(inquiry => inquiry.DaletedAt == null),
                                    sys => sys,
                                    inq => inq.System,
                                    (sys, inq) => new {
                                        Systems = sys,
                                        Inquiries = inq
                                    }).SelectMany(x => x.Inquiries.DefaultIfEmpty(), (x, inquiry) => new SystemsCountModel()
                                    {
                                        System = x.Systems,
                                        YearOrMonth = inquiry.TelephoneNumber == null ? null : inquiry.IncomingDate.Month,
                                        InquiryCount = x.Systems.Inquiries
                                                        .Where(inquiry => inquiry.DaletedAt == null)
                                                        .Where(inquiry => inquiry.IncomingDate.Year == date.Year)
                                                        .Where(x => x.IncomingDate.Month == inquiry.IncomingDate.Month).Count()
                                    })
                                    .Distinct()
                                    .OrderBy(inquiry => inquiry.System.Id)
                                    .ThenBy(inquiry => inquiry.YearOrMonth)
                                    .ToListAsync();

            return FillMonth(eachSystemCount);
        }

        public async Task<List<SystemsCountModel>> GetYearSystemsCountAsync(DateTime date)
        {
            var eachSystemCount = await this._context.System.GroupJoin(
                                    this._context.Inquiry.Where(inquiry => inquiry.DaletedAt == null),
                                    sys => sys,
                                    inq => inq.System,
                                    (sys, inq) => new {
                                        Systems = sys,
                                        Inquiries = inq
                                    }).SelectMany(x => x.Inquiries.DefaultIfEmpty(), (x, inquiry) => new SystemsCountModel()
                                    {
                                        System = x.Systems,
                                        YearOrMonth = inquiry.TelephoneNumber == null ? null : inquiry.IncomingDate.Year,
                                        InquiryCount = x.Systems.Inquiries
                                                            .Where(inquiry => inquiry.DaletedAt == null)    
                                                            .Where(x => x.IncomingDate.Year == inquiry.IncomingDate.Year).Count()
                                    })
                                    .Distinct()
                                    .OrderBy(inquiry => inquiry.System.Id)
                                    .ThenBy(inquiry => inquiry.YearOrMonth)
                                    .ToListAsync();

            return FillFiveYearsBeforeAndAfter(eachSystemCount, date.Year);
        }

        public async Task<List<SystemsCountModel>> GetWeekSystemsCountAsync(DateTime date)
        {
            var eachSystemCount = new List<SystemsCountModel>();

            DateTime sunday = getDateByWeek(date, 0);
            DateTime saturday = getDateByWeek(date, 6);
            
            var findDateRange = getWeekRange(sunday, saturday);
            foreach(var findDate in findDateRange)
            {
                eachSystemCount.AddRange(
                    await this._context.System.GroupJoin(
                        this._context.Inquiry.Where(inquiry => inquiry.DaletedAt == null)
                                     .Where(inquiry => findDate == inquiry.IncomingDate),
                        sys => sys,
                        inq => inq.System,
                        (sys, inq) => new {
                            Systems = sys,
                            Inquiries = inq
                        }).SelectMany(x => x.Inquiries.DefaultIfEmpty(), (x, inquiry) => new SystemsCountModel()
                        {
                            System = x.Systems,
                            YearOrMonth = inquiry.TelephoneNumber == null ? null : (int?)inquiry.IncomingDate.DayOfWeek,
                            InquiryCount = x.Systems.Inquiries
                                            .Where(inquiry => inquiry.DaletedAt == null)
                                            .Where(inquiry => findDate == inquiry.IncomingDate).Count()
                        })
                        .Distinct()
                        .OrderBy(inquiry => inquiry.System.Id)
                        .ThenBy(inquiry => inquiry.YearOrMonth)
                        .ToListAsync()
                );
            }

            return FillWeek(eachSystemCount);
        }

        private List<SystemsCountModel> FillHour(List<SystemsCountModel> eachSystemCount)
        {
            var eachSystemTodayCount = new List<SystemsCountModel>();

            foreach (var system in this._context.System)
            {
                for(var hour=0; hour <= 23; hour++)
                {
                    var inquiry = eachSystemCount.Where(inquiry => inquiry.System.Id == system.Id).Where(inquiry => inquiry.YearOrMonth == hour);
                    if (inquiry.Any())
                    {
                        eachSystemTodayCount.Add(inquiry.First());
                    }
                    else
                    {
                        eachSystemTodayCount.Add(new SystemsCountModel
                        {
                            System = system,
                            YearOrMonth = hour,
                            InquiryCount = 0
                        });
                    }
                }
            }

            return eachSystemTodayCount;
        }

        private List<SystemsCountModel> FillMonth(List<SystemsCountModel> eachSystemCount)
        {
            var eachSystemMonthlyCount = new List<SystemsCountModel>();
            // 各システム毎に、1～12月まででデータがないものについて、仮データ（件数ゼロ）をセットする。
            foreach(var system in this._context.System)
            {
                for(var month=1; month <= 12; month++)
                {
                    var inquiry = eachSystemCount.Where(inquiry => inquiry.System.Id == system.Id).Where(inquiry => inquiry.YearOrMonth == month);
                    if (inquiry.Any())
                    {
                        eachSystemMonthlyCount.Add(inquiry.First());
                    }
                    else
                    {
                        var eachMonth = new SystemsCountModel()
                        {
                            System = system,
                            YearOrMonth = month,
                            InquiryCount = 0
                        };

                        eachSystemMonthlyCount.Add(eachMonth);
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
            var eachSystemWeeklyCount = new List<SystemsCountModel>();
            foreach (var system in this._context.System)
            {
                for(var week=0; week < 7; week++)
                {
                    var inquiry = eachSystemCount.Where(inquiry => inquiry.System.Id == system.Id).Where(inquiry => inquiry.YearOrMonth == week);
                    if (inquiry.Any())
                    {
                        eachSystemWeeklyCount.Add(inquiry.First());
                    }
                    else
                    {
                        eachSystemWeeklyCount.Add(new SystemsCountModel
                        {
                            System = system,
                            YearOrMonth = week,
                            InquiryCount = 0
                        });
                    }
                }
            }

            return eachSystemWeeklyCount;
        }

        private DateTime getDateByWeek(DateTime date, int targetWeek)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            int weekDiff = targetWeek - dayOfWeek;
            
            return date.AddDays(weekDiff);
        }

        private List<DateTime> getWeekRange(DateTime begin, DateTime end)
        {
            var days = new List<DateTime>();
            days.Add(begin);

            while(begin < end)
            {
                begin = begin.AddDays(1);
                days.Add(begin);
            }
        
            return days;
        }
    }
}