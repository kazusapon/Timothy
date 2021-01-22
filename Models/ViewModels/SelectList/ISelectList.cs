using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Timothy.Models.ViewModels.SelectList
{
    public interface ISelectList
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}