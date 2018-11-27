using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC研修.Models
{
    public class FindRowModel
    {
        public string ユーザーID { get; set; }
        public string メールアドレス { get; set; }
        public string 氏名 { get; set; }
        public int 性別 { get; set; }
        public string 電話番号 { get; set; }
        public string 郵便番号 { get; set; }
        public string 住所 { get; set; }

    }
}