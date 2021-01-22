using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntityModels
{
    public enum CompletionState
    {
        未完了,
        完了
    }

    public class Inquiry
    {
        [Key]
        [Display(Name = "#")]
        public int Id {get; set;}

        [Display(Name = "回答者")]
        public int UserId {get; set;}

        [ForeignKey("UserId")]
        public User User {get; set;}

        [Display(Name = "システム")]
        public int SystemId {get; set;}

        [ForeignKey("SystemId")]
        public System System {get; set;}

        [Display(Name = "連絡方法")]
        public int ContactMethodId {get; set;}

        [ForeignKey("ContactMethodId")]
        public ContactMethod ContactMethod {get; set;}

        [Display(Name = "問合せ元分類")]
        public int GuestTypeId {get; set;}

        [ForeignKey("GuestTypeId")]
        public GuestType GuestType {get; set;}

        [Display(Name = "問合せ分類")]
        public int ClassificationId {get; set;}

        [ForeignKey("ClassificationId")]
        public Classification Classification {get; set;}

        [Display(Name = "関連問合せ")]
        public int InquiryRelationId {get; set;}

        [Display(Name = "着信履歴ID")]
        public int CallRegisterId {get; set;}

        [Display(Name = "問合せ元")]
        [Required(ErrorMessage = "必須項目です")]
        [MaxLength(30, ErrorMessage = "30文字以下で入力してください")]
        public string CompanyName {get; set;}

        [Display(Name = "担当者")]
        [Required(ErrorMessage = "必須項目です")]
        [MaxLength(15, ErrorMessage = "15文字以下で入力してください")]
        public string InquirerName {get; set;}

        [Display(Name = "電話番号")]
        [Required(ErrorMessage = "必須項目です")]
        [MaxLength(13, ErrorMessage = "13文字以下（ハイフンあり）で入力してください")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "電話番号として認識できません")]
        public string TelephoneNumber {get; set;}

        [Display(Name = "電話番号（予備）")]
        [MaxLength(13, ErrorMessage = "13文字以下（ハイフンあり）で入力してください")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "電話番号として認識できません")]
        public string SpareTelephoneNumber {get; set;}

        [Display(Name = "問合せ")]
        [Required(ErrorMessage = "必須項目です")]
        [MaxLength(500, ErrorMessage = "500文字以下で入力してください")]
        public string Question {get; set;}

        [Display(Name = "回答")]
        [Required(ErrorMessage = "必須項目です")]
        [MaxLength(500, ErrorMessage = "500文字以下で入力してください")]
        public string Answer {get; set;}

        [Display(Name = "完了ステータス")]
        public bool ComplateFlag {get; set;}

        [Display(Name = "承認状態")]
        public bool ApprovalFlag {get; set;}

        [Display(Name = "着信日")]
        [DataType(DataType.Date, ErrorMessage = "日付として認識できません")]
        public DateTime IncomingDate {get; set;}

        [Display(Name = "着信開始時刻")]
        [DataType(DataType.Time, ErrorMessage = "時刻として認識できません")]
        public DateTime StartTime {get; set;}

        [Display(Name = "着信終了時刻")]
        [DataType(DataType.Time, ErrorMessage = "時刻として認識できません")]
        public DateTime EndTime {get; set;}

        [Display(Name = "削除日")]
        [DataType(DataType.DateTime)]
        public DateTime? DaletedAt {get; set;}

        [NotMapped]
        public string IncomingDateTimeText
        {
            get
            {
                var date = this.IncomingDate.ToString("yy/MM/dd");
                var time = this.StartTime.ToString("HH:mm");
                
                return HttpUtility.HtmlDecode(date + "&nbsp;" + time);
            }
        }

        [NotMapped]
        public string RelationInquiryText
        {
            get
            {
                return this.BuildRelationInquiryText();
            }
        }

        public DateTime GetIncomingStartDateTime()
        {
            return this.IncomingDate.Date.Add(this.StartTime.TimeOfDay);
        }

        public IEnumerable<SelectListItem> GetCompletionStateSelectListItem()
        {
            var completionStateList = new List<SelectListItem>();
            foreach(CompletionState completionState in Enum.GetValues(typeof(CompletionState)))
            {
                int value = (int)completionState;
                string text = Enum.GetName(typeof(CompletionState), completionState);

                completionStateList.Add(new SelectListItem{
                    Value = value == 1 ? true.ToString() : false.ToString(),
                    Text = text
                });
            }

            return completionStateList;
        }

        private string BuildRelationInquiryText()
        {
            var sb = new StringBuilder();
            sb.Append("ID：");
            sb.Append(this.Id);
            sb.Append("　（着信日時：");
            sb.Append(this.IncomingDateTimeText);
            sb.Append("　会社名：");
            sb.Append(this.CompanyName);
            sb.Append(")");

            return sb.ToString();
        }

        public void BuildInquiryFromCallRegister(EntityModels.CallRegister callRegister)
        {
            if (callRegister.Id == 0)
            {
                return;
            }

            CallRegisterId = callRegister.Id;
            IncomingDate = callRegister.IncomingDate;
            StartTime = callRegister.StartTime;
            EndTime = callRegister.EndTime;
            CompanyName = callRegister.CompanyName;
            InquirerName = callRegister.InquirerName;
            TelephoneNumber = callRegister.TelephoneNumber;
            UserId = callRegister.UserId == null ? 0 : (int)callRegister.UserId;
            GuestType = callRegister.GuestType;
        }
    }
}