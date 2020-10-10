using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public enum CompletionState
    {
        未完了 = 0,
        完了 = 1
    }

    public class Inquiry
    {
        [Key]
        [Display(Name = "ID")]
        public int Id {get; set;}

        [Display(Name = "回答者")]
        public int UserId {get; set;}

        [Display(Name = "システム")]
        public int SystemId {get; set;}

        [Display(Name = "連絡方法")]
        public int ContactMethodId {get; set;}

        [Display(Name = "問合せ元分類")]
        public int GuestTypeId {get; set;}

        [Display(Name = "問合せ分類")]
        public int ClassificationId {get; set;}

        [Display(Name = "関連問合せ")]
        public int InquiryRelation {get; set;}

        [Display(Name = "問合せ元")]
        public string ComapnyName {get; set;}

        [Display(Name = "担当者")]
        public string InquirerName {get; set;}

        [Display(Name = "電話番号")]
        public string TelephoneNumber {get; set;}

        [Display(Name = "電話番号（予備）")]
        public string SpareTelephoneNumber {get; set;}

        [Display(Name = "問合せ")]
        public string Question {get; set;}

        [Display(Name = "回答")]
        public string Answer {get; set;}

        [Display(Name = "完了")]
        public bool ComplateFlag {get; set;}

        [Display(Name = "承認状態")]
        public bool ApprovalFlag {get; set;}

        [Display(Name = "着信日")]
        [DataType(DataType.Date)]
        public DateTime IncomingDate {get; set;}

        [Display(Name = "着信開始時刻")]
        [DataType(DataType.Time)]
        public DateTime StartTime {get; set;}

        [Display(Name = "着信終了時刻")]
        [DataType(DataType.Time)]
        public DateTime EndTime {get; set;}
    }
}