using System;
using Timothy.Models;
using Timothy.Models.ViewModels.inquiry;

namespace Timothy.Models.ViewModels.inquiry
{
    public class InquiryViewModel
    {
        public Entities.Inquiry inquiry {get; set;}

        public InquiryForm inquiryFrom {get; set;}
    }
} 