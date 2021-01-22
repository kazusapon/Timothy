using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timothy.Models;
using Database.Models;

namespace Timothy.Models.ViewModels.inquiry
{
    public class InquiryIndexViewModel
    {
        public InquirySearchForm inquirySearchForm {get; set;}

        public List<Entities.Inquiry> inquiryIndexLists {get; set;}
    }

    public class InquirySearchForm
    {
        public DateTime? StartTime {get; set;}

        public DateTime? EndTime {get; set;}

        public int SystemId {get; set;}

        public List<SelectListItem> Systems {get; set;}

        public bool CheckedFlag {get; set;}

        public string FreeWord {get; set;}

        public InquirySearchForm()
        {
            this.StartTime = null;
            this.EndTime = null;
            this.SystemId = 0;
            this.FreeWord = null;
            this.CheckedFlag = true;
        }
    }
}