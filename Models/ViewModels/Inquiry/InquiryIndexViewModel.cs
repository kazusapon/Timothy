using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Inquiry.View.Models
{
    public class InquiryIndexViewModel
    {
        public InquirySearchForm inquirySearchForm {get; set;}

        public List<InquiryIndexLists> inquiryIndexLists {get; set;}
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

    public class InquiryIndexLists
    {
        [Display(Name = "ID")]
        public int Id {get; set;}

        [Display(Name = "着信日時")]
        public DateTime IncomingDateTime {get; set;}

        [Display(Name = "問合せ元")]
        public String CompanyName {get; set;}

        [Display(Name = "担当者")]
        public String InquirerName {get; set;}

        [Display(Name = "電話番号")]
        public String TelephoneNumber {get; set;}

        [Display(Name = "システム名")]
        public String SystemName {get; set;}

        [Display(Name = "回答者")]
        public String UserName {get; set;}

        [Display(Name = "問合せ")]
        public String Question {get; set;}

        [Display(Name = "回答")]
        public String Answer {get; set;}
    }
}