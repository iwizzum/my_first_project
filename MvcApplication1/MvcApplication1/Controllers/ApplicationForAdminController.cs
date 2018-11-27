using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC研修.Models;


namespace ASP.NET_MVC研修.Controllers
{
    public class ApplicationForAdminController : Controller
    {
        //全体的にいろんな書き方と設計を試しています
        //なんだかんだ引数の割の良さは感じますね。
        //たぶん、フォーム側のマッピングモデルで持てるかどうかがコツかな。
        //あとはフォームのマッピングモデルとかをきれいな命名規則で纏めて、
        //完全に分離させる感じにするとキレイになるかもしれない。
        //まあいろいろ試せばわかってくるかな。



        //今回のはあんまりきれいにならなかった。
        //状態をクラスに保持して、プロパティはviewからマッピングして
        //メソッドだけこっちでやりたかったけど、
        //たぶんview側で全部処理できるように組まないと、中途半端。

        // GET: /ApplicationForAdmin/
        public ActionResult Index()
        {
            ApplicationInfoModel sm = new ApplicationInfoModel();
            //これは遷移で持ってくるやつ
            sm.氏名 = "testuser";
            sm.所属 = "営業部";
            sm.役職 = "主査";
            sm.ユーザID = "a";

            //ここから普通
            sm.状態 = "2";
            sm.表示件数 = 10;
            sm.Find();
            sm.SortAll();
            sm.GetPage();
            Session["書類申請"] = sm;
            return View("ApplicationManage", sm);
        }

        [HttpGet]
        public ActionResult Return()
        {
            ApplicationInfoModel sm = (ApplicationInfoModel)Session["書類申請"];
            sm.Find();
            sm.SortAll();
            sm.GetPage();
            return View("ApplicationManage", sm);
        }

        [HttpPost]
        public ActionResult Find(ApplicationInfoModel m)
        {
            ApplicationInfoModel sm = (ApplicationInfoModel)Session["書類申請"];
            sm.状態 = m.状態;
            sm.表示件数 = m.表示件数;
            sm.現ページ = 1;
            sm.Find();
            sm.SortAll();
            sm.GetPage();
            Session["書類申請"] = sm;
            return PartialView("_ApplicationList", sm);
        }

        [HttpPost]
        public ActionResult GetPage(ApplicationInfoModel m)
        {
            ApplicationInfoModel sm = (ApplicationInfoModel)Session["書類申請"];
            sm.表示件数 = m.表示件数;
            sm.現ページ = 1;
            sm.SortAll();
            sm.GetPage();
            Session["書類申請"] = sm;
            return PartialView("_ApplicationList", sm);
        }

        [HttpPost]
        public ActionResult MovePage(int 新規ページ数)
        {
            ApplicationInfoModel sm = (ApplicationInfoModel)Session["書類申請"];
            sm.現ページ = 新規ページ数;
            sm.SortAll();
            sm.GetPage();
            Session["書類申請"] = sm;
            return PartialView("_ApplicationList", sm);
        }

        [HttpPost]
        public ActionResult Sort(string ソート列, string ソート順)
        {
            ApplicationInfoModel sm = (ApplicationInfoModel)Session["書類申請"];
            sm.ソート順 = ソート順;
            sm.ソート列 = ソート列;
            sm.現ページ = 1;
            //sm.Find();
            sm.SortAll();
            sm.GetPage();
            Session["書類申請"] = sm;
            return PartialView("_ApplicationList", sm);
        }


        //-----------------------------------------------------------------------------------
        //@brief
        //  承認画面用のモデル設定
        //  受け取ったIDを元にデータを設定する（予定）
        //
        //@note
        //  一覧クリック→これに行く→成功でwindow.hrefでhttpgetを取得する→画面遷移完了。
        //-----------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult ApproveDataGet(string 申請ID)
        {
            ApplicationInfoModel smInfo = (ApplicationInfoModel)Session["書類申請"];
            ApplicationApproveModel sm = new ApplicationApproveModel();
            sm = sm.GetApproveData(smInfo, 申請ID);
            Session["申請内容承認用"] = sm;
            return Json(new { result = "Success" });
        }

        //-----------------------------------------------------------------------------------
        //@brief
        //  申請内容承認用モデルのget。セッションモデルは行クリックでトリガー。
        //
        //@note
        //  データの引き渡しが難しい。設計上手くやったらきれいにまとまりそうだけど。
        //  とりあえずモデル設定はコントローラで良いのか…？
        //  データを引き渡すだけだけど、モデルに引数で渡すべきかも。
        //-----------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult ApplicationApprove()
        {
            ApplicationApproveModel sm = (ApplicationApproveModel)Session["申請内容承認用"];
            return View("ApplicationApprove", sm);
            //return RedirectToAction("ApplicationApprove", sm);
        }


        //-----------------------------------------------------------------------------------
        //@brief
        //  承認/申請の引き渡しメソッド。引数の値でdb更新する。
        //
        //-----------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult ApplicationChange(string argID, string argSupplyID, string arg更新値)
        {
            ApplicationApproveModel m = new ApplicationApproveModel();
            m.ChangeStatus(argID, argSupplyID, arg更新値);
            return Json(new { result = "Success" });
        }

        //-----------------------------------------------------------------------------------
        //@brief
        //  承認/申請の引き渡しメソッド。引数の値でdb更新する。
        //http://jirolabo.hatenablog.com/entry/2015/01/31/132723
        //https://teratail.com/questions/78749
        //http://blog.ishinao.net/2011/11/15/11291/
        //http://www.atmarkit.co.jp/ait/articles/0907/10/news109.html
        //https://code.i-harness.com/ja/q/370052
        //-----------------------------------------------------------------------------------
        //申請したファイルをダウンロード
        public ActionResult ApplicationDownload(string fileName)
        {
            string dirFullPath = HttpContext.Server.MapPath("/ApplicationFiles") + "\\" + fileName;// +userID;
            //第一引数：サーバーにあるダウンロードさせたいファイル; 第二引数：MIME(今回は様々なMIMEタイプを使う); 第三引数：ダウンロード時のファイル名
            //return new DownloadResult(dirFullPath, "multipart/form-data", fileName);

            //byte[] filedata = System.IO.File.ReadAllBytes(dirFullPath);
            string contentType = MimeMapping.GetMimeMapping(dirFullPath);
 

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = fileName,
            //    Inline = true,
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());

            //var result = new FilePathResult(fileName, contentType); // content-typeは適当にどうぞ
            //return result;

            return File(dirFullPath, contentType, fileName);
            //return File(filedata, contentType, fileName);

        }
        
	}
}