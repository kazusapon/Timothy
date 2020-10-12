using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace User.Model
{
    public interface IUser
    {
        Task<List<SelectListItem>> GetSelectListItemsAsync();
    }
}