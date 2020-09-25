using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    class Inquiry
    {
        public int Id {get; set;}

        public int UserId {get; set;}

        public int SystemId {get; set;}

        public int ContactMethodId {get; set;}

        public int GuestTypeId {get; set;}

        public int InquiryRelation {get; set;}

        public string ComapnyName {get; set;}

        public string InquirerName {get; set;}

        public string TelephoneNumber {get; set;}

        public string SpareTelephoneNumber {get; set;}

        public string Question {get; set;}

        public string Answer {get; set;}

        public bool ComplateFlag {get; set;}

        public bool ApprovalFlag {get; set;}

        public DateTime IncomingDate {get; set;}

        public DateTime StartTime {get; set;}

        public DateTime EndTime {get; set;}
    }
}