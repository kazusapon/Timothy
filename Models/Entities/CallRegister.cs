using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class CallRegister
    {
        public int Id {get; set;}

        public string CompanyName {get; set;}

        public string InquirerName {get; set;}

        public string TelephoneNumber {get; set;}
        
        public int? UserId {get; set;}

        [ForeignKey("UserId")]
        public User User {get; set;}

        public int? GuestTypeId {get; set;}

        [ForeignKey("GuestTypeId")]
        public GuestType GuestType {get; set;}

        public DateTime IncomingDate {get; set;}

        public DateTime StartTime {get; set;}

        public DateTime EndTime {get; set;}

        public DateTime? DaletedAt {get; set;}

        public string BuildIncomingDateTimeBetWeen()
        {
            var sb = new StringBuilder();
            sb.Append(IncomingDate.ToString("yyyy/MM/dd"));
            sb.Append(" ");
            sb.Append(StartTime.ToString("HH:mm"));
            sb.Append(" ～ ");
            sb.Append(EndTime.ToString("HH:mm"));

            return sb.ToString();
        }
    }
}