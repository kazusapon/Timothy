using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timothy.Models.Entities;
using Database.Models;

namespace Timothy.Models.User
{
    public class UserModel : IUser
    {
        private readonly DatabaseContext _context;

        public UserModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<SelectListItem>> GetSelectListItemsAsync()
        {
            return await this._context.User.OrderBy(user => user.Id)
                .Select(user => 
                    new SelectListItem{
                        Value = user.Id.ToString(),
                        Text = user.UserName
                    }
                ).AsNoTracking().ToListAsync();
        }
    }
}