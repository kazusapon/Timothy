using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inquiry.View.Models
{
    public class InquiryForm
    {
        [Display(Name = "着信日")]
        public DateTime IncomingDate {get; set;}

        // 開始or終了を一括りにして着信時刻と表示したいため、DisplayName はViewにそのまま記載している。
        public DateTime IncomingStartTime {get; set;}

        // 開始or終了を一括りにして着信時刻と表示したいため、DisplayName はViewにそのまま記載している。
        public DateTime IncomingEndTime {get; set;}

        [Display(Name = "システム")]
        public IEnumerable<EntityModels.System> Systems {get; set;}

        [Display(Name = "連絡方法")]
        public IEnumerable<EntityModels.ContactMethod> ContactMethods {get; set;}

        [Display(Name = "問合せ元")]
        public string CompanyName {get; set;}

        [Display(Name = "問合せ元分類")]
        public IEnumerable<EntityModels.GuestType> GuestTypes {get; set;}

        [Display(Name = "担当者")]
        public string InquirerName {get; set;}

        [Display(Name = "電話番号")]
        public string TelephoneNumber {get; set;}

        [Display(Name = "電話番号（予備）")]
        public string SpareTelephoneNumber {get; set;}

        [Display(Name = "回答者")]
        public IEnumerable<EntityModels.User> Users {get; set;}

        [Display(Name = "問合せ")]
        public string Question {get; set;}
 
        [Display(Name = "回答")]
        public string Answer {get; set;}

        [Display(Name = "問合せ分類")]
        public IEnumerable<EntityModels.Classification> Classifications {get; set;}

        [Display(Name = "完了")]
        public IEnumerable<SelectListItem> CompletionStatus {get; set;}


        ///<summary>
        /// 問合せ内容入力フォームの完了プルダウンを設定
        ///   引数なし：新規
        ///   引数あり：編集
        ///</summary>
        public void SetCompletionStatus(int? complateStatusId = null)
        {
            var completionStateList = new List<SelectListItem>();
            foreach(CompletionState completionState in Enum.GetValues(typeof(CompletionState)))
            {
                int value = (int)completionState;
                string text = Enum.GetName(typeof(CompletionState), completionState);

                completionStateList.Add(new SelectListItem{
                    Value = value.ToString(),
                    Text = text,
                    Selected = complateStatusId == value ? true : false
                });
            }

            CompletionStatus = completionStateList.AsEnumerable();
        }
    }
}