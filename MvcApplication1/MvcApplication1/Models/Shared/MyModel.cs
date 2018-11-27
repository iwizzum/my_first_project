using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;

namespace ASP.NET_MVC研修.Models.Shared
{
    public class MyModel
    {
        public UserInfoModel ユーザー情報 { get; set; }

        public MyModel()
        { }

        public MyModel(UserInfoModel m)
        {
            ユーザー情報 = new UserInfoModel();
            if (m != null)
            {
                ユーザー情報 = m;
            }
        }

    }
}