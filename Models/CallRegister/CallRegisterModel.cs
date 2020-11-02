using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntityModels;
using Database.Models;

namespace CallRegister.Model
{
    public class CallRegisterModel : ICallRegister
    {
        private DatabaseContext _context;

        public CallRegisterModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<EntityModels.CallRegister> FindById(int id)
        {
            return await this._context.CallRegister
                .Where(callRegister => callRegister.DaletedAt == null)
                .Where(callRegister => callRegister.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<List<EntityModels.CallRegister>> GetCallRegisters()
        {
            return await (from callRegister in this._context.CallRegister
                    join user in this._context.User
                    on callRegister.UserId equals user.Id into gj
                    from subUser in gj.DefaultIfEmpty()
                    where callRegister.DaletedAt == null
                    orderby callRegister.IncomingDate
                    orderby callRegister.StartTime
                    orderby callRegister.Id
                    select new EntityModels.CallRegister
                    {
                        Id = callRegister.Id,
                        CompanyName = callRegister.CompanyName == null ? "新規利用者" : callRegister.CompanyName,
                        IncomingDate = callRegister.IncomingDate,
                        StartTime = callRegister.StartTime,
                        EndTime = callRegister.EndTime,
                        InquirerName = callRegister.InquirerName,
                        TelephoneNumber = callRegister.TelephoneNumber,
                        UserId = callRegister.UserId,
                        GuestTypeId = callRegister.GuestTypeId
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task DestroyCallRegisterAsync(int id)
        {
            var callRegister = await this._context.CallRegister
                                .Where(callRegister => callRegister.Id == id)
                                .Where(callRegister => callRegister.DaletedAt == null)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();
            
            if (callRegister == null)
            {
                return;
            }
            
            callRegister.DaletedAt = DateTime.Now;

            await this._context.SaveChangesAsync();
        }
    }
}