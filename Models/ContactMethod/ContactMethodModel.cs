using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timothy.Models.Entities;
using Database.Models;

namespace Timothy.Models.ContactMethod
{
    public class ContactMethodModel : IContactMethod
    {
        private readonly DatabaseContext _context;

        public ContactMethodModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<SelectListItem>> GetSelectListItemsAsync()
        {
            return await this._context.ContactMethod.OrderBy(contactMethod => contactMethod.Id)
                .Select(contactMethod => 
                    new SelectListItem{
                        Value = contactMethod.Id.ToString(),
                        Text = contactMethod.ContactMethodName
                    }
                ).AsNoTracking().ToListAsync();
        }
    }
}