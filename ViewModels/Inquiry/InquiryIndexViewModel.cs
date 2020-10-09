using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Inquiry.View.Models
{
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