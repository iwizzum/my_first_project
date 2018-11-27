using ASP.NET_MVC研修.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//ユーザーIDとパスワードをチェックする処理
namespace ASP.NET_MVC研修.Controllers
{

    //コントローラ
    public class AuthenticationController : Controller
    {

        public ActionResult Index()
        {
            return View("Login_Javascript_Submit");
        }

        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            //ユーザーIDとパスワードがデータベースに存在するかどうかチェック					
            //アクションメソッドの引数の名前は画面の要素のnameプロパティと同じ名前にする
            UserModel u = new UserModel();
            bool result = user.Check(user.ユーザーID, user.メールアドレス, user.パスワード);

            //存在しない場合、クライアントにエラーメッセージを送る					
            if (result == false)
            {
                ViewBag.error = "入力したログイン情報が存在しません";
                return View("Login_Javascript_Submit");
            }
            
            //存在する場合、空欄のエラーメッセージを返す					
            return View("次の画面");
        }

        /*
        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            //ユーザーIDとパスワードがデータベースに存在するかどうかチェック					
            //アクションメソッドの引数の名前は画面の要素のnameプロパティと同じ名前にする
            UserModel u = new UserModel();
            bool result = user.Check(user.ユーザーID, user.パスワード);

            //存在しない場合、クライアントにエラーメッセージを送る					
            if (result == false)
            {
                return Json(new { errmsg = "ユーザーIDまたはパスワードが存在しません" });
            }


            //存在する場合、空欄のエラーメッセージを返す					
            return Json(new { errmsg = "" });
        }
        */
    }


}