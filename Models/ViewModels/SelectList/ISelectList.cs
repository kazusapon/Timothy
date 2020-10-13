using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SelectList.View.Model
{
    public interface ISelectList
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}