using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Utils;

namespace Timothy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryRestController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public InquiryRestController(DatabaseContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("{id?}/telephoneNumber/{telephoneNumber}")]
        public async Task<ActionResult<IEnumerable<EntityModels.Inquiry>>> GetInquiry(int? id, string telephoneNumber)
        {
            var telephoneNumberHyphenDelete =  telephoneNumber == null ? null : telephoneNumber.Replace("-", "");
            return await this._context.Inquiry
                        .WhereIf(id >= 1, inquiry => inquiry.Id == id)
                        .WhereIf(telephoneNumberHyphenDelete != null, inquiry => inquiry.TelephoneNumber.Replace("-", "") == telephoneNumberHyphenDelete)
                        .OrderByDescending(inquiry => inquiry.IncomingDate)
                        .ThenByDescending(inquiry => inquiry.StartTime)
                        .ThenByDescending(inquiry => inquiry.Id)
                        .AsNoTracking()
                        .ToListAsync();
        }
    }
}