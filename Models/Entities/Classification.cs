using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timothy.Models.Entities
{
    public class Classification
    {
        public int Id {get; set;}

        public string ClassificationName {get; set;}

        public virtual ICollection<Timothy.Models.Entities.Inquiry> Inquiries { get; set; }
    }
}