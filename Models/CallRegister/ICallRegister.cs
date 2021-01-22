using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Timothy.Models;

namespace Timothy.Models.CallRegister
{
    public interface ICallRegister
    {
        Task<List<Entities.CallRegister>> GetCallRegisters();

        Task<Entities.CallRegister> FindById(int id);

        Task DestroyCallRegisterAsync(int id);

        Task UpdateCompanyNameAndInquierName(Timothy.Models.Entities.Inquiry inquiry);
    }
}