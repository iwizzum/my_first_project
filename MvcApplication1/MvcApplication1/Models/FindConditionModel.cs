using System;
using System.Data;

namespace ASP.NET_MVC研修.Models
{
    public class FindConditionModel
    {
        public string ユーザーID      { get; set; }
        public string メールアドレス  { get; set; }
        public string ニックネーム    { get; set; }
        public string 氏名            { get; set; }
        public int    性別            { get; set; }
        public string 生年月日        { get; set; }
        public int    年齢            { get; set; }
        public string 電話番号        { get; set; }
        public string 郵便番号        { get; set; }
        public string 住所            { get; set; }
        public string 入社日          { get; set; }
        public string 所属            { get; set; }
        public string 役職            { get; set; }
        public string 趣味            { get; set; }
        public string 特技            { get; set; }
        public string 座右銘          { get; set; }
    }
}