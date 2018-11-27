using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNet.Models.Shared
{
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