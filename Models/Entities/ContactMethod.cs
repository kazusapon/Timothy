using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timothy.Models.Entities
{
    public class ContactMethod
    {
        public int Id {get; set;}

        public string ContactMethodName {get; set;}

        public virtual ICollection<Inquiry> Inquiries { get; set; }
    }
}