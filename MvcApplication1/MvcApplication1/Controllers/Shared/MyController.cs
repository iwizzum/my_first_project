using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC研修.Models;
using ASP.NET_MVC研修.Models.Shared;
using ASP.NET_MVC研修.Common;
using System.IO;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.Configuration;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Web.Routing;
using System.Net.Mime;

namespace ASP.NET_MVC研修.Controllers.Shared
{
    public class MyController : Controller
    {
        public string CurrentUserID
        {
            get
            {
                UserInfoModel m = (UserInfoModel)Session[SessionVar.SHARED_USER_INFO];
                return m.ユーザー情報.ユーザーID;
            }
        }


        public string GetServerParth(string relativePath)
        {
            return Server.MapPath(relativePath);
        }

        protected string CreateDirPath(string parentDir, string subDir = null, bool hasUserDir = true)
        {
            UserInfoModel m = (UserInfoModel)Session[SessionVar.SHARED_USER_INFO];
            var userid = "";
            if (hasUserDir)
            {
                userid = m.ユーザーID;
            }
            var dir = HttpContext.Server.MapPath(ConfigurationManager.AppSettings[parentDir]);
            if (hasUserDir)
            {
                dir += "\\" + userid;
            }

            if (subDir != null)
            {
                dir += "\\" + subDir;
            }

            return dir;
        }
    }
            
}