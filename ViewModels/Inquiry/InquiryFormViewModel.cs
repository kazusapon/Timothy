using System;
using System.Collections.Generic;
using EntityModels;
using System.Linq;

namespace Inquiry.View.Models
{
    public class InquiryForm
    {
        public DateTime IncomingDate {get; set;}

        public DateTime IncomingStartTime {get; set;}

        public DateTime IncomingEndTime {get; set;}

        public IEnumerable<EntityModels.System> Systems {get; set;}

        public IEnumerable<EntityModels.ContactMethod> ContactMethods {get; set;}

        public string CompanyName {get; set;}

        public IEnumerable<EntityModels.GuestType> GuestTypes {get; set;}

        public string InquirerName {get; set;}

        public string TelephoneNumber {get; set;}

        public string SpareTelephoneNumber {get; set;}

        public IEnumerable<EntityModels.User> Users {get; set;}

        public string Question {get; set;}
 
        public string Answer {get; set;}

        public IEnumerable<EntityModels.Classification> Classifications {get; set;}

        public IQueryable CompletionStates {get; set;}
    }
}