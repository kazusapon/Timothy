using System;
using Form.View.Models;

namespace Inquiry.View.Models
{
    public class InquiryViewModel
    {
        public EntityModels.Inquiry inquiry {get; set;}

        public InquiryForm inquiryFrom {get; set;}
    }
} 