using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    class CallRegister
    {
        public int Id {get; set;}

        public string CompanyName {get; set;}

        public string TelephoneNumber {get; set;}

        public int UserId {get; set;}

        public DateTime IncomingDate {get; set;}

        public DateTime StartTime {get; set;}

        public DateTime EndTime {get; set;}
    }
}