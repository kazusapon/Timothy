using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class GuestType
    {
        public int Id {get; set;}

        public string GuestTypeName {get; set;}
    }
}