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
        private readonly DatabaseContext _context;

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
            return await this._context.CallRegister
                    .Where(callRegister => callRegister.DaletedAt == null)
                    .OrderBy(callRegister => callRegister.IncomingDate)
                    .OrderBy(callRegister => callRegister.StartTime)
                    .OrderBy(callRegister => callRegister.Id)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task DestroyCallRegisterAsync(int id)
        {
            var callRegister = await this._context.CallRegister
                                .Where(callRegister => callRegister.Id == id)
                                .Where(callRegister => callRegister.DaletedAt == null)
                                .FirstOrDefaultAsync();

            if (callRegister == null)
            {
                return;
            }

            callRegister.DaletedAt = DateTime.Now;
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateCompanyNameAndInquierName(EntityModels.Inquiry inquiry)
        {
            var callRegiters = await this._context.CallRegister
                                .Where(callRegister => callRegister.TelephoneNumber == inquiry.TelephoneNumber)
                                .Where(callRegister => callRegister.DaletedAt == null)
                                .AsNoTracking()
                                .ToListAsync();

            foreach (var callRegiter in callRegiters)
            {
                this._context.Entry(callRegiter).State = EntityState.Modified;
                //this._context.Entry(callRegiter.User).State = EntityState.Unchanged;
                    
                callRegiter.CompanyName = inquiry.CompanyName;
                callRegiter.InquirerName = inquiry.InquirerName;
                callRegiter.GuestTypeId = inquiry.GuestTypeId;
            }

            await this._context.SaveChangesAsync();
        }
    }
}