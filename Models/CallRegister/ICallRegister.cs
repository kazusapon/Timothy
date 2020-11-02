using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CallRegister.Model
{
    public interface ICallRegister
    {
        Task<List<EntityModels.CallRegister>> GetCallRegisters();

        Task<EntityModels.CallRegister> FindById(int id);

        Task DestroyCallRegisterAsync(int id);
    }
}