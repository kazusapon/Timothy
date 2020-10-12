using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Classification.Model
{
    public interface IClassification
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}