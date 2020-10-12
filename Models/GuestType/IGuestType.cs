using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuestType.Model
{
    public interface IGuestType
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}