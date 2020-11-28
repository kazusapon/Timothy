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
using Summary.Model;

namespace Timothy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryRestController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly ISummary _summary;

        public SummaryRestController(DatabaseContext context, ISummary summary)
        {
            this._context = context;
            this._summary = summary;
        }

        [HttpGet]
        [Route("monthly")]
        public async Task<ActionResult<ChartModel>> GetEachSystemCountForManthly()
        {
            string dateString = "2020-11-20";
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return new ChartModel();
            }

            return await this._summary.BuildEachSystemInquiryCountAndMonthlyAsync(date);
        }
    }
}
