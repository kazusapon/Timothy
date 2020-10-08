using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using EntityModels;
using Inquiry.Model;
using System.Model;
using Inquiry.View.Models;

namespace Timothy.Controllers
{
    public class InquiryController : Controller
    {
        private readonly DatabaseContext context;

        private IInquiry inquiryModel;

        private readonly ISystem system;

        public InquiryController(DatabaseContext context, IInquiry inquiry, ISystem system)
        {
            this.context = context;
            this.inquiryModel = inquiry;
            this.system = system;
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
            var inquiryForm = new InquiryForm();
            inquiryForm.CompletionStates = Enum.GetValues(typeof(EntityModels.CompletionState)).AsQueryable();

            return View(inquiryForm);
        }
    }
}