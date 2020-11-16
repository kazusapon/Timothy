using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class ContactMethod
    {
        public int Id {get; set;}

        public string ContactMethodName {get; set;}

        public virtual ICollection<EntityModels.Inquiry> Inquiries { get; set; }
    }
}