using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace ASP.NET_MVC研修.Models.Shared
{
    //-------------------------------------------------------
    //2018/8/26 丸コピ
    //
    //単純にリザルト画面を使いまわすときに、サーバー側で
    //使いまわす画面に埋める値を受け渡すために利用
    //-------------------------------------------------------
    public class MessageModel
    {
        public string Message { get; set; }
        public string ButtonCaption { get; set; }
        public string ButtonAction { get; set; }

        public MessageModel()
        {
            Message = "";
            ButtonCaption = "";
            ButtonAction = "";
        }
    }
}