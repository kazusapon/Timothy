using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class System
    {
        public int Id {get; set;}

        public string SystemName {get; set;}

        public string Abbreviation {get; set;}

        public virtual ICollection<EntityModels.Inquiry> Inquiries { get; set; }
    }
}