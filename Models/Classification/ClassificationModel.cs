using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Timothy.Models.Entities;
using Database.Models;

namespace Timothy.Models.Classification
{
    public class ClassificationModel : IClassification
    {
        private readonly DatabaseContext _context;

        public ClassificationModel(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task<List<SelectListItem>> GetSelectListItemsAsync()
        {
            return await this._context.Classification.OrderBy(classification => classification.Id)
                .Select(classification => 
                    new SelectListItem{
                        Value = classification.Id.ToString(),
                        Text = classification.ClassificationName
                    }
                ).AsNoTracking().ToListAsync();
        }
    }
}