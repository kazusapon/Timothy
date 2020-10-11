using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EntityModels;


namespace System.Model
{
    public interface ISystem
    {
        Task<List<SelectListItem>> GetSelectListItemAsync();
    }
}