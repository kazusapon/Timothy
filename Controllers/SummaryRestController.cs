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
        [Route("count/today/{dateString}")]
        public async Task<ActionResult<ChartModel>> GetEachSystemCountForToday(string dateString)
        {
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return new ChartModel();
            }

            return await this._summary.BuildEachSystemInquiryCountAndTodayAsync(date);
        }

        [HttpGet]
        [Route("count/weekly/{dateString}")]
        public async Task<ActionResult<ChartModel>> GetEachSystemCountForWeek(string dateString)
        {
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return new ChartModel();
            }

            return await this._summary.BuildEachSystemInquiryCountAndWeekAsync(date);
        }

        [HttpGet]
        [Route("count/monthly/{dateString}")]
        public async Task<ActionResult<ChartModel>> GetEachSystemCountForManthly(string dateString)
        {
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return new ChartModel();
            }

            return await this._summary.BuildEachSystemInquiryCountAndMonthlyAsync(date);
        }

        [HttpGet]
        [Route("count/year/{dateString}")]
        public async Task<ActionResult<ChartModel>> GetEachSystemCountForYear(string dateString)
        {
            DateTime date;
            if (!DateTime.TryParse(dateString, out date))
            {
                return new ChartModel();
            }

            return await this._summary.BuildEachSystemInquiryCountAndYaerAsync(date);
        }
    }
}
