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
using ContactMethod.Model;
using Inquiry.View.Models;
using GuestType.Model;
using User.Model;
using Classification.Model;
using CallRegister.Model;
using Form.View.Models;

namespace Timothy.Controllers
{
    public class InquiryController : Controller
    {
        private readonly DatabaseContext _context;

        private IInquiry _inquiryModel;

        private readonly ISystem _system;

        private readonly IContactMethod _contactMethod;

        private readonly IGuestType _guestType;

        private readonly ICallRegister _callRegister;

        private readonly IUser _user;

        private readonly IClassification _classification;

        public InquiryController(DatabaseContext context, IInquiry inquiry, ISystem system, IContactMethod contactMethod, IGuestType guestType, IUser user, IClassification classification, ICallRegister callRegister)
        {
            this._context = context;
            this._inquiryModel = inquiry;
            this._system = system;
            this._contactMethod = contactMethod;
            this._guestType = guestType;
            this._user = user;
            this._classification = classification;
            this._callRegister = callRegister;
        }
        
        [HttpGet]
        [Route("Inquiry")]
        public async Task<IActionResult> Index()
        {
            var inquiryIndexViewModel = new InquiryIndexViewModel
            {
                inquirySearchForm = new InquirySearchForm()
                {
                    Systems = await this._system.GetSelectListItemsAsync()
                },
                inquiryIndexLists = await this._inquiryModel.GetIndexListAsync
                (
                    null, // 着信日（To）
                    null, // 着信日（From）
                    0, // システムID
                    true, // 確認フラグ
                    null  // フリーワード
                )
            };
            return View(inquiryIndexViewModel);
        }

        [HttpGet]
        [Route("Inquiry/Search")]
        public async Task<IActionResult> Search(InquiryIndexViewModel form)
        {
            form.inquirySearchForm.Systems = await this._system.GetSelectListItemsAsync();
            var inquirySearchViewModel = new InquiryIndexViewModel
            {
                inquirySearchForm = form.inquirySearchForm,
                inquiryIndexLists = await this._inquiryModel.GetIndexListAsync
                (
                    form.inquirySearchForm.StartTime,
                    form.inquirySearchForm.EndTime,
                    form.inquirySearchForm.SystemId,
                    form.inquirySearchForm.CheckedFlag,
                    form.inquirySearchForm.FreeWord
                )
            };
            
            return View(nameof(Index), inquirySearchViewModel);
        }

        [HttpGet]
        [Route("Inquiry/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var inquiry = await this._inquiryModel.FindByIdAsync(id);

            return inquiry == null ? View(nameof(Index)) : View(nameof(Detail), inquiry);
        }

        [HttpGet]
        [Route("Inquiry/New")]
        public async Task<IActionResult> New(EntityModels.CallRegister callRegister = null)
        {
            EntityModels.Inquiry inquiry = new EntityModels.Inquiry();
            inquiry.BuildInquiryFromCallRegister(callRegister);

            var inquiryViewModel = new InquiryViewModel
            {
                inquiry = inquiry,
                inquiryFrom = await SetInquiryFormValuesAsync()
            };

            var now = DateTime.Now;
            ViewBag.toDate = now.ToString("yyyy-MM-dd");
            ViewBag.toTime = now.ToString("HH:mm");
            ViewBag.fromTime = now.ToString("HH:mm");

            ViewBag.relationInquiryText = "";

            return View(inquiryViewModel);
        }

        [BindProperty]
        public EntityModels.Inquiry inquiry {get; set;}

        [HttpPost]
        [Route("Inquiry/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            if (ModelState.IsValid)
            {
                await this._inquiryModel.CreateInquiryAsync(inquiry);
                await this._callRegister.UpdateCompanyNameAndInquierName(inquiry);

                if (inquiry.CallRegisterId > 0)
                {
                    await this._callRegister.DestroyCallRegisterAsync(inquiry.CallRegisterId);
                }

                return RedirectToAction(nameof(Index));
            }

            var inquiryViewModel = new InquiryViewModel
            {
                inquiry = inquiry,
                inquiryFrom = await SetInquiryFormValuesAsync()
            };

            ViewBag.toDate = inquiry.IncomingDate.ToString("yyyy-MM-dd");
            ViewBag.toTime = inquiry.StartTime.ToString("HH:mm");
            ViewBag.fromTime = inquiry.EndTime.ToString("HH:mm");

            ViewBag.relationInquiryText = "";

            return View(nameof(New), inquiryViewModel);
        }

        [HttpGet]
        [Route("Inquiry/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var inquiry = await this._inquiryModel.FindByIdAsync(id);
            if (inquiry == null)
            {
                return View(nameof(Index));
            }

            var inquiryViewModel = new InquiryViewModel
            {
                inquiry = inquiry,
                inquiryFrom = await SetInquiryFormValuesAsync()
            };

            ViewBag.toDate = inquiry.IncomingDate.ToString("yyyy-MM-dd");
            ViewBag.toTime = inquiry.StartTime.ToString("HH:mm");
            ViewBag.fromTime = inquiry.EndTime.ToString("HH:mm");

            var relationInquiry = await this._inquiryModel.FindByIdAsync(inquiry.InquiryRelationId);
            ViewBag.relationInquiryText = relationInquiry == null ? "" : relationInquiry.RelationInquiryText;

            return View(inquiryViewModel);
        }

        [HttpPost]
        [Route("Inquiry/Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update()
        {
            if (ModelState.IsValid)
            {
                await this._inquiryModel.UpdateInquiryAsync(inquiry);
                await this._callRegister.UpdateCompanyNameAndInquierName(inquiry);

                return RedirectToAction(nameof(Detail), new {id = inquiry.Id});
            }

            var inquiryViewModel = new InquiryViewModel
            {
                inquiry = inquiry,
                inquiryFrom = await SetInquiryFormValuesAsync()
            };

            ViewBag.toDate = inquiry.IncomingDate.ToString("yyyy-MM-dd");
            ViewBag.toTime = inquiry.StartTime.ToString("HH:mm");
            ViewBag.fromTime = inquiry.EndTime.ToString("HH:mm");

            var relationInquiry = await this._inquiryModel.FindByIdAsync(inquiry.InquiryRelationId);
            ViewBag.relationInquiryText = relationInquiry == null ? "" : relationInquiry.RelationInquiryText;

            return View(inquiryViewModel);

        }

        [HttpGet]
        [Route("Inquiry/Destory")]
        public async Task<IActionResult> Destroy(int id)
        {
            await this._inquiryModel.DeleteInquiryAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Inquiry/Approval")]
        public async Task<IActionResult> Approval(int id)
        {
            await this._inquiryModel.ApprovalInquiryAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<InquiryForm> SetInquiryFormValuesAsync()
        {
            var inquiryForm = new InquiryForm();
            inquiryForm.Systems = await this._system.GetSelectListItemsAsync();
            inquiryForm.ContactMethods = await this._contactMethod.GetSelectListItemsAsync();
            inquiryForm.GuestTypes = await this._guestType.GetSelectListItemsAsync();
            inquiryForm.Users = await this._user.GetSelectListItemsAsync();
            inquiryForm.Classifications = await this._classification.GetSelectListItemsAsync();

            return inquiryForm;
        }
    }
}