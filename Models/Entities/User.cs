using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timothy.Models.Entities
{
    public class User
    {
        public int Id {get; set;}

        public string UserName {get; set;}

        public string LoginId {get; set;}

        public string Password {get; set;}

        public byte[] Salt {get; set;}

        public string Hostname {get; set;}
 
        public bool ConnectableFlag {get; set;}

        public virtual ICollection<Timothy.Models.Entities.Inquiry> Inquiries { get; set; }

        public virtual ICollection<Timothy.Models.Entities.CallRegister> CallRegisters { get; set; }
    }
}