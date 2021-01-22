using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timothy.Models.Entities
{
    public class GuestType
    {
        public int Id {get; set;}

        public string GuestTypeName {get; set;}

        public virtual ICollection<Inquiry> Inquiries { get; set; }

        public virtual ICollection<CallRegister> CallRegisters { get; set; }
    }
}