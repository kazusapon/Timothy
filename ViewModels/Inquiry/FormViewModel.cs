using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Form.View.Models
{
    public class InquiryForm
    {
        public IEnumerable<EntityModels.System> Systems {get; set;}

        public IEnumerable<EntityModels.ContactMethod> ContactMethods {get; set;}

        public IEnumerable<EntityModels.GuestType> GuestTypes {get; set;}

        public IEnumerable<EntityModels.User> Users {get; set;}

        public IEnumerable<EntityModels.Classification> Classifications {get; set;}
    }
}