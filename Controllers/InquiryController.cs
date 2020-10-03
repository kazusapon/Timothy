using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Inquiry.Model;

namespace Timothy.Controllers
{
    public class InquiryController : Controller
    {
        private readonly DatabaseContext context;

        private IInquiry inquiryModel;

        public InquiryController(DatabaseContext context)
        {
            this.context = context;
            this.inquiryModel = new InquiryModel(context);
        }
        
        [HttpGet]
        [Route("Inquiry")]
        public async Task<IActionResult> Index()
        {

            return View(await inquiryModel.GetIndexListAsync());
        }

        [HttpGet]
        [Route("Inquiry/New")]
        public IActionResult New()
        {
            return View();
        }
    }
}