using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EntityModels;
using Database.Models;

namespace CallRegister.Model
{
    public class CallRegisterModel
    {
        private DatabaseContext _context;

        public CallRegisterModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<EntityModels.CallRegister>> GetCallRegisters()
        {
            return await (from callRegister in this._context.CallRegister
                    join user in this._context.User
                    on callRegister.UserId equals user.Id into gj
                    from subUser in gj.DefaultIfEmpty()
                    select new EntityModels.CallRegister
                    {
                        Id = callRegister.Id,
                        CompanyName = callRegister.CompanyName,
                        IncomingDate = callRegister.IncomingDate,
                        StartTime = callRegister.StartTime,
                        EndTime = callRegister.EndTime,
                        TelephoneNumber = callRegister.TelephoneNumber,
                        UserId = callRegister.UserId
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}