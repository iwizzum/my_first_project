using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC研修.Models
{
    public class ApplicationInfoRowModel
    {
        //各項目のプロパティ設定
        //ApplicationInfoRowModel:書類申請欄
        public string 状態 { get; set; }
        public string 申請ID { get; set; }
        public string 申請種類 { get; set; }
        public string タイトル { get; set; }
        public string 申請日 { get; set; }
        public string 承認日 { get; set; }
        public string 連絡事項 { get; set; }
    }
}