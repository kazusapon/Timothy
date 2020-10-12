using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactMethod.Model
{
    public interface IContactMethod
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}