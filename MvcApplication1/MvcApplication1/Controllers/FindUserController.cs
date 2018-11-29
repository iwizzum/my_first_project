using System;
using System.Collections.Generic;
using ASP.NET_MVC研修.Models;
using ASP.NET_MVC研修.Common;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC研修.Controllers.Shared;

namespace ASP.NET_MVC研修.Controllers
{
    // FindUserController
    public class FindUserController : MyController
    {
        // Index
        // GET: /Find/
        public ActionResult Index()
        {
            return View("Find");
        }

        // Search
         [HttpPost]
        public ActionResult Search(FindConditionModel Condition)
        {
            FindModel findModel = new FindModel();

            if(Session["ユーザー検索"] != null)	
            {	
	            findModel = (FindModel)Session["ユーザー検索"];
            }	
            else	
            {	
	            findModel.ソート順 = "▲";
                findModel.ソート列 = "ユーザーID";
            }	

            findModel.Find(Condition.ユーザーID, Condition.メールアドレス, Condition.ニックネーム, Condition.氏名, Condition.性別, Condition.生年月日, Condition.年齢, Condition.電話番号, Condition.郵便番号, Condition.住所, Condition.入社日, Condition.所属, Condition.役職, Condition.趣味, Condition.特技, Condition.座右銘);

            Session["ユーザー検索"] = findModel;

            return PartialView("_FindResult", findModel);
        }

        [HttpPost]
         public ActionResult GetPage(int rowCount, int pageNum)
         {
             FindModel findModel = (FindModel)Session["ユーザー検索"];
             findModel.GetPage(rowCount, pageNum);
             return PartialView("_FindResult", findModel);

         }

        [HttpPost]
        public ActionResult Sort(string colName, string sortOrder)
        {
            FindModel findModel = (FindModel)Session["ユーザー検索"];

            findModel.Sort(colName, sortOrder);
            //存在する場合、次の画面を表示する
            return PartialView("_FindResult", findModel);
        }


        public ActionResult SendMail(string mails)
        {
            string dirFullPath = GetServerParth(AppSetting.SENDMAIL_UPLOAD_DIR );

            //メール送信を行う前、添付ファイルを一時保存するフォルダーを削除する
            var dirinfo = new DirectoryInfo(dirFullPath);
            if (dirinfo.Exists)
            {
                Directory.Delete(dirFullPath, true);
            }

            SendMailModel sm = new SendMailModel((UserInfoModel)Session[SessionVar.SHARED_USER_INFO]);
            sm.宛先 = mails;

            Session[SessionVar.SHARED_SEND_MAIL_INFO] = sm;

            return View(sm);
        }

        [HttpPost]
        public ActionResult SendMail(SendMailModel m)
        {

            string dirFullPath = GetServerParth(AppSetting.SENDMAIL_UPLOAD_DIR );

            List<string> attachFiles = new List<string>();
            if (m.添付1 != null)
            {
                attachFiles.Add(dirFullPath + "\\" + "1" + "\\" + m.添付1);
            }

            if (m.添付2 != null)
            {
                attachFiles.Add(dirFullPath + "\\" + "2" + "\\" + m.添付2);
            }

            if (m.添付3 != null)
            {
                attachFiles.Add(dirFullPath + "\\" + "3" + "\\" + m.添付3);
            }

            m.SendMailToUser(m.宛先, m.件名, m.内容, attachFiles);

            //メール送信が完了した後、添付ファイルのフォルダーを削除する
            var dirinfo = new DirectoryInfo(dirFullPath);
            if (dirinfo.Exists)
            {
                Directory.Delete(dirFullPath, true);
            }

            return Json(new { result = "success" });
        }


        [HttpPost]
        public ActionResult UploadFile()
        {
            HttpContext.Response.ContentType = "text/plain";

            //Commonのプロパティ・メソッドを使用
            SendMailModel m = (SendMailModel)Session[SessionVar.SHARED_SEND_MAIL_INFO];

            var index = HttpContext.Request["index"];
            HttpPostedFileBase file = HttpContext.Request.Files[0];
            string filename = file.FileName.Split(Path.DirectorySeparatorChar).Last();

            //string dirFullPath = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["SendMailUploadDir"] + "\\" + userid + "\\" + index);

            string dirFullPath = CreateDirPath(AppKey.KEY_SENDMAIL_UPLOAD_DIR, index, true);
            Directory.CreateDirectory(dirFullPath);

            //旧ファイルを削除
            DirectoryInfo dirInfo = new DirectoryInfo(dirFullPath);
            dirInfo.GetFiles().ToList().ForEach(x => x.Delete());

            string pathToSave = dirFullPath + "\\" + filename;
            file.SaveAs(pathToSave);

            return Json(new { result = "uploadfinish", filename = filename, index = index });
        }

        [HttpPost]
        public ActionResult DeleteUpload(string filename, int index)
        {
            HttpContext.Response.ContentType = "text/plain";

 
            string dirFullPath = GetServerParth(AppSetting.SENDMAIL_UPLOAD_DIR + "\\" + index);

            FileInfo fileinfo = new FileInfo(dirFullPath + "\\" + filename);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }

            return Json(new { result = "success" });
        }



    }

}