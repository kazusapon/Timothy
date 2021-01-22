using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Timothy.Models.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Timothy.Models.ViewModels.inquiry
{
    public class InquiryForm
    {
        public IEnumerable<SelectListItem> Systems {get; set;}

        public IEnumerable<SelectListItem> ContactMethods {get; set;}

        public IEnumerable<SelectListItem> GuestTypes {get; set;}

        public IEnumerable<SelectListItem> Users {get; set;}

        public IEnumerable<SelectListItem> Classifications {get; set;}
    }
}