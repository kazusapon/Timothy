using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class GuestType
    {
        public int Id {get; set;}

        public string GuestTypeName {get; set;}

        public virtual ICollection<EntityModels.Inquiry> Inquiries { get; set; }

        public virtual ICollection<EntityModels.CallRegister> CallRegisters { get; set; }
    }
}