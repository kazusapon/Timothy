using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timothy.Models.Entities;
using Database.Models;

namespace Timothy.Models.System
{
    public class SystemModel : ISystem
    {
        private readonly DatabaseContext _context;

        public SystemModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<SelectListItem>> GetSelectListItemsAsync()
        {
            return await this._context.System.OrderBy(system => system.Id).Select(sys => 
                new SelectListItem{
                    Value = sys.Id.ToString(),
                    Text = sys.SystemName
                }
            ).AsNoTracking().ToListAsync();
        }
    }
}