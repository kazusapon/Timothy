using System.Collections.Generic;
using System.Threading.Tasks;
using EntityModels;


namespace System.Model
{
    public interface ISystem
    {
        Task<List<EntityModels.System>> GetSystems();
    }
}