using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EntityModels;
using Database.Models;

namespace GuestType.Model
{
    public class GuestTypeModel : IGuestType
    {
        private readonly DatabaseContext _context;

        public GuestTypeModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<SelectListItem>> GetSelectListItemsAsync()
        {
            return await this._context.GuestType.OrderBy(guestType => guestType.Id)
                .Select(guestType => 
                    new SelectListItem{
                        Value = guestType.Id.ToString(),
                        Text = guestType.GuestTypeName
                    }
                ).AsNoTracking().ToListAsync();
        }
    }
}