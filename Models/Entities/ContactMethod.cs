using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    class ContactMethod
    {
        public int Id {get; set;}

        public string ContactMethodName {get; set;}
    }
}