using ASP.NET_MVC研修.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ASP.NET_MVC研修.Common
{
    public static class MyHtmlHelper
    {
        public static IHtmlString MyCheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string labelString, object htmlAttributes = null)
        {
            //string checkboxname = ExpressionHelper.GetExpressionText(expression);
            //string checkboxfullname = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(checkboxname);
            //string checkboxid = TagBuilder.CreateSanitizedId(checkboxfullname);

            StringBuilder inputItemBuilder = new StringBuilder();
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            TValue value = expression.Compile().Invoke(helper.ViewData.Model);
            string propertyName = metadata.PropertyName;
            string id = "txt" + propertyName;

            TagBuilder checkbox = new TagBuilder("input");
            checkbox.Attributes.Add("type", "checkbox");
            checkbox.Attributes.Add("name", propertyName);
            checkbox.Attributes.Add("id", id);
            checkbox.Attributes.Add("value", "true");
            inputItemBuilder.Append(checkbox.ToString(TagRenderMode.SelfClosing));

            TagBuilder label = new TagBuilder("label");
            label.Attributes.Add("for", id);
            label.InnerHtml = labelString;
            label.MergeAttributes(attributes);
            inputItemBuilder.Append(label.ToString(TagRenderMode.Normal));

            TagBuilder hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("name", propertyName);
            hidden.Attributes.Add("value", "false");
            inputItemBuilder.Append(hidden.ToString(TagRenderMode.SelfClosing));

            //IDictionary<string, object> validationAttributes = helper.GetUnobtrusiveValidationAttributes(checkboxfullname, metadata);
            //foreach (string key in validationAttributes.Keys)
            //{
            //    label.Attributes.Add(key, validationAttributes[key].ToString());
            //}

            return new MvcHtmlString(inputItemBuilder.ToString());
        }


    }

    public class Utils
    {
        public static string GetRandom()
        {
            Random random = new Random();
            string randomNumber = string.Join(string.Empty, Enumerable.Range(0, 10).Select(number => random.Next(0, 9).ToString()));
            return randomNumber;
        }

        //
        // 概要:
        //     nullのStringをブランクStringに変更
        //
        // パラメーター:
        //   input;
        //     ブランクStringに変更したい対象
        //
        // 戻り値:
        //     「input」Stringはnullであれば、""を返す
        public static T IsNull<T>(T input, T output)
        {
            if (input == null)
            {
                return output;
            }
            return input;
        }

        /// <summary>
        /// 作成したファイル・フォルダをIISユーザのための権限
        /// </summary>
        public static void GrantAccessToUser(string path, string user)
        {
            FileSecurity fSecurity = File.GetAccessControl(path);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(user,
                FileSystemRights.ChangePermissions
                | FileSystemRights.ReadAndExecute
                | FileSystemRights.Traverse
                | FileSystemRights.ReadData
                | FileSystemRights.WriteData
                , AccessControlType.Allow));

            // Set the new access settings.
            File.SetAccessControl(path, fSecurity);
        }

        /// <summary>
        /// ファイルに出力用のエラーログ作成
        /// </summary>
        public static string CreateErrorDescForFile(Exception e, string preDesc = "", string postDesc = "\n\n\n", string flusInfo = "")
        {
            string errorDesc = "";

            if (e == null)
            {
                return "";
            }
            errorDesc += preDesc;
            errorDesc += string.Format("例外の詳細：{0}\r\n", e.Message);
            //errorDesc += string.Format("ソース エラー：{0}\n", e.Source);
            if (flusInfo != "")
            {
                errorDesc += string.Format("{0}\r\n", flusInfo);
            }
            errorDesc += string.Format("スタック トレース：\r\n{0}", e.StackTrace);
            errorDesc += postDesc;

            return errorDesc;
        }

        /// <summary>
        /// ウェブ画面に表示用のエラーログ作成
        /// </summary>
        public static string CreateErrorDescForWeb(Exception e, string preDesc = "", string postDesc = "")
        {
            string errorDesc = "";

            if (e == null)
            {
                return "";
            }

            errorDesc += "<div style='text-align:left;float;left'>";
            errorDesc += "<font style='font-size:18pt;color:Red;font-weight:normal'>" + preDesc + "<br/></font>";
            errorDesc += string.Format("<b>例外の詳細：</b>{0}<br/>", e.Message);
            //errorDesc += string.Format("<b>ソース エラー：</b>{0}<br/>", e.Source);
            errorDesc += string.Format("<b>スタック トレース：</b><code><pre>{0}</pre></code>", e.StackTrace);
            errorDesc += postDesc;
            errorDesc += "</div>";

            return errorDesc;
        }

    }

    public static class SessionVar
    {
        public const string SHARED_USER_INFO = "SHARED_USER_INFO";
        public const string SHARED_FIND_USER_RESULT = "SHARED_FIND_USER_RESULT";
        public const string SHARED_SEND_MAIL_INFO = "SHARED_SEND_MAIL_INFO";
        public const string SHARED_BB_SEARCH_RESULT = "SHARED_BB_SEARCH_RESULT";
        public const string SHARED_BB_UPDATE = "SHARED_BB_UPDATE";
        public const string SHARED_BB_NEW = "SHARED_BB_NEW";
        public const string SHARED_USER_MANAGEMENT = "SHARED_USER_MANAGEMENT";
        public const string SHARED_USER_MANAGEMENT_ISSUE_LINK = "SHARED_USER_MANAGEMENT_ISSUE_LINK";
        public const string SHARED_USER_INFO_REG = "SHARED_USER_INFO_REG";

        //松川用
        public const string SHARED_FIND_APPLICATION_RESULT = "SHARED_FIND_APPLICATION_RESULT";
        public const string SHARED_APPLIED_INFO = "SHARED_APPLIED_INFO";
        public const string SHARED_APPLYING_INFO = "SHARED_APPLYING_INFO";

        //エラーコード
        public const string SHARED_ERROR_CODE = "SHARED_ERROR_CODE";
        //エラーメッセージ
        public const string SHARED_ERROR_USER_MESSAGE = "SHARED_ERROR_USER_MESSAGE";
        //エラーオブジェクト
        public const string SHARED_ERROR_OBJECT = "SHARED_ERROR_OBJECT";
        //エラーが発生したページ
        public const string SHARED_ERROR_CONTROLLER = "SHARED_ERROR_CONTROLLER";
        public const string SHARED_ERROR_ACTION = "SHARED_ERROR_ACTION";

    }

    public static class UserRole
    {
        public const int ADMIN = 1;
        public const int NORMAL = 0;
    }

    public static class AppKey
    {
        public const string KEY_PROFILE_DIR = "ProfileIagmeDir";
        public const string KEY_PROFILE_TEMP_DIR = "ProfileTempIagmeDir";
        public const string KEY_BBUPLOAD_DIR = "BBUploadDir";
        public const string KEY_BBUPLOAD_TEMP_DIR = "BBUploadTempDir";        
        public const string KEY_TIMELINE_ROOT_DIR = "TimelineDir";
        public const string KEY_TIMELINE_TEMP_DIR = "NewPostUploadDir";
        public const string KEY_COMMENT_TEMP_DIR = "NewCommentUploadDir";
        public const string KEY_TIMELINE_FIRST_LOAD_COUNT = "TimelineFirstLoadCount";
        public const string KEY_TIMELINE_NEXT_LOAD_COUNT = "TimelineNextLoadCount";
        public const string KEY_APPLICATION_ROOT_DIR = "ApplicationRootDir";
        public const string KEY_APPLICATION_TEMP_DIR = "ApplicationTempDir";
        public const string KEY_LOG_DIR = "LogDir";
        public const string KEY_ERROR_LOG_DIR = "ErrorLogDir";
        public const string KEY_ADMIN_EMAIL = "AdminEmail";
        public const string KEY_FOR_SENDING_EMAIL = "ForSendingEmail";
        public const string KEY_FOR_SENDING_PASS = "ForSendingPass";
        public const string KEY_MAIL_SERVER = "MailServer";
        public const string KEY_TEMPLATE_ROOT_DIR = "TemplateDownLoadFileDir";
        public const string KEY_SENDMAIL_UPLOAD_DIR = "SendMailUploadDir";
    }

    public class AppSetting
    {

        public static string PROFILE_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_PROFILE_DIR]; } }
        public static string PROFILE_TEMP_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_PROFILE_TEMP_DIR]; } }
        public static string BBUPLOAD_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_BBUPLOAD_DIR]; } }
        public static string BBUPLOAD_TEMP_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_BBUPLOAD_TEMP_DIR]; } }
        public static string TIMELINE_ROOT_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_TIMELINE_ROOT_DIR]; } }
        public static string TIMELINE_TEMP_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_TIMELINE_TEMP_DIR]; } }
        public static string COMMENT_TEMP_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_COMMENT_TEMP_DIR]; } }
        public static int TIMELINE_FIRST_LOAD_COUNT { get { return int.Parse(ConfigurationManager.AppSettings[AppKey.KEY_TIMELINE_FIRST_LOAD_COUNT]); } }
        public static int TIMELINE_NEXT_LOAD_COUNT { get { return int.Parse(ConfigurationManager.AppSettings[AppKey.KEY_TIMELINE_NEXT_LOAD_COUNT]); } }
        public static string APPLICATION_ROOT_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_APPLICATION_ROOT_DIR]; } }
        public static string APPLICATION_TEMP_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_APPLICATION_TEMP_DIR]; } }
        public static string LOG_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_LOG_DIR]; } }
        public static string ERROR_LOG_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_ERROR_LOG_DIR]; } }
        public static string ADMIN_EMAIL { get { return ConfigurationManager.AppSettings[AppKey.KEY_ADMIN_EMAIL]; } }
        public static string FOR_SENDING_EMAIL { get { return ConfigurationManager.AppSettings[AppKey.KEY_FOR_SENDING_EMAIL]; } }
        public static string FOR_SENDING_PASS { get { return ConfigurationManager.AppSettings[AppKey.KEY_FOR_SENDING_PASS]; } }
        public static string MAIL_SERVER { get { return ConfigurationManager.AppSettings[AppKey.KEY_MAIL_SERVER]; } }
        public static string TEMPLATE_ROOT_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_TEMPLATE_ROOT_DIR]; } }
        public static string SENDMAIL_UPLOAD_DIR { get { return ConfigurationManager.AppSettings[AppKey.KEY_SENDMAIL_UPLOAD_DIR]; } }
    }


    //松川用
    public static class UserApplicationStatus
    {
        public const int BLANK = -2;
        public const int ALL = -1;
        public const int UNAPPROVED = 0;
        public const int APPROVED = 1;
        public const int USERCANCEL = 2;
        public const int CANCEL = 3;
        public const int DELETE = 4;
    }

    public static class CustomErrorCode
    {
        //システムエラーが発生したとき
        public const int ERROR_UNKNOWN = 999;
        //エラーが発生したとき、リダイレクトを行う
        //public const int ERROR_REDIRECT = 998;
        //Sessionの有効期限が切れた
        public const int ERROR_SESSION_TIMEOUT = 997;
        //thowでエラーを生成した
        public const int ERROR_KNOWN = 996;
        //Ajaxリクエストのとき
        public const int ERROR_AJAX = 995;
        //ログに書き出すとき、エラーが発生した
        //public const int ERROR_WRITE_LOG = 994;
        //Ajaxリクエストのとき、セッションが切れた
        public const int ERROR_AJAX_SESSION_TIMEOUT = 993;
        //システムエラー
        public const int ERROR_SYSTEM = 500;
        //ページが存在しない
        public const int ERROR_NOT_FOUND = 404;
        //アクセス権限がない
        public const int ERROR_ACCESS_DENIED = 403;
    }

    public static class CustomErrorMsg
    {
        public const string ERR_TIMELINE_NOT_EXIST = "コメントしようとした投稿または対象物は削除されているため、コメントできません。";
        public const string ERR_TIMELINE_IMG_UPDDATE = "投稿画像が正しくアップロードされませんでした。";
        public const string ERR_COMMENT_IMG_UPDDATE = "コメント画像が正しくアップロードされませんでした。";
        public const string ERR_SESSION_TIMEOUT = "セッション有効期限が切れました。";
        public const string ERR_ACCESS_DENIED = "アクセス権限がありません。";
    }

    public static class SystemErrorMsg
    {
        public const string UNKNOWN_ERROR = "システムエラーが発生しました。再度ログインしてください。同じなエラーがまた発生する場合は、管理者にご連絡ください。";
    }

    public static class ControllerName
    {
        public const string APPLICATION_USER = "ApplicationForUser";
        public const string APPLICATION_ADMIN = "ApplicationForAdmin";
        public const string AUTHENTICATION = "Authentication";
        public const string BASE_INFO = "BaseInformation";
        public const string BULLET_BOARD = "BulletinBoard";
        public const string ERROR = "Error";
        public const string FIND_USER = "FindUser";
        public const string PROFILE = "ProfileInformation";
        public const string TIMELINE = "Timeline";
        public const string USER_INFO_REG = "UserInfoRegistration";
        public const string USER_MANAGEMENT = "UserManagement";
        public const string E_LEARNING = "ELearning";
    }

    public static class Log
    {
        //public static void WriteLog(string logKubun, string content, string parameter = "", string extension = "")
        //{
        //    UserInfoModel userInfo = (UserInfoModel)HttpContext.Current.Session[SessionVar.SHARED_USER_INFO];
        //    string logDirPath = HttpContext.Current.Application[AppKey.KEY_LOG_DIR].ToString();

        //    //エラーログフォルダ作成
        //    if (!Directory.Exists(logDirPath))
        //    {
        //        Directory.CreateDirectory(logDirPath);
        //        Utils.GrantAccessToUser(logDirPath, "IUSR");
        //    }

        //    string[] logContent = new string[15];
        //    logContent[0] = "";
        //    logContent[1] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    logContent[2] = userInfo.ユーザーID;
        //    logContent[3] = userInfo.氏名;
        //    logContent[5] = userInfo.所属;
        //    logContent[6] = logKubun;
        //    logContent[7] = "";
        //    logContent[8] = parameter;
        //    logContent[9] = "";
        //    logContent[0] = "";
        //    logContent[11] = "";
        //    logContent[12] = "";
        //    logContent[13] = extension;
        //    //logContent[14] = userInfo.IPAdrress;

        //    HttpContext.Current.Application.Lock();
        //    try
        //    {
        //        using (FileStream fs = new FileStream(logDirPath + @"\TD_AccessLog_" + DateTime.Now.ToString("yyyyMMdd") + @".csv", FileMode.Append, FileAccess.Write, FileShare.None))
        //        {
        //            using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("shift-jis")))
        //            {
        //                sw.WriteLine(string.Join(",", logContent));
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string errorId = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        //        Log.WriteErrorLog(errorId, e);
        //        throw new System.Web.HttpException(CustomErrorCode.ERROR_UNKNOWN, SystemErrorMsg.UNKNOWN_ERROR);
        //    }
        //    HttpContext.Current.Application.UnLock();
        //}

        /// <summary>
        /// Cube画面を表示
        /// </summary>
        /// <param name="errorId">yyyyMMdd_HHmmss</param>
        //public static void WriteErrorLog(string errorId, Exception ex, string flusInfo = "")
        //{
        //    UserInfoModel userInfo = (UserInfoModel)HttpContext.Current.Session[SessionVar.SHARED_USER_INFO];
        //    string errorLogDirPath = HttpContext.Current.Application[AppKey.KEY_ERROR_LOG_DIR].ToString();

        //    //エラーログフォルダ作成
        //    if (!Directory.Exists(errorLogDirPath))
        //    {
        //        Directory.CreateDirectory(errorLogDirPath);
        //        Utils.GrantAccessToUser(errorLogDirPath, "IUSR");
        //    }

        //    string errorDesc = "\r\n\r\n";
        //    errorDesc += string.Format("※エラーID：{0}\r\n", errorId);
        //    errorDesc += string.Format("ユーザ情報：\r\n   ユーザID＝{0}、ユーザ名＝{1}、部署名＝{2}\r\n"
        //        , userInfo.ユーザーID, userInfo.氏名, userInfo.所属);
        //    errorDesc = Utils.CreateErrorDescForFile(ex, errorDesc, flusInfo);

        //    HttpContext.Current.Application.Lock();
        //    try
        //    {
        //        using (FileStream fs = new FileStream(errorLogDirPath + @"\ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + @".log", FileMode.Append, FileAccess.Write, FileShare.None))
        //        {
        //            using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("shift-jis")))
        //            {
        //                sw.WriteLine(errorDesc);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new System.Web.HttpException(CustomErrorCode.ERROR_UNKNOWN, Utils.CreateErrorDescForWeb(e, SystemErrorMsg.UNKNOWN_ERROR));
        //    }
        //    HttpContext.Current.Application.UnLock();
        //}

        /// <summary>
        /// Cube画面を表示
        /// </summary>
        /// <param name="errorId">yyyyMMdd_HHmmss</param>
        public static Exception WriteErrorLog(string errMsg, string errLogDir)
        {
            HttpContext.Current.Application.Lock();
            try
            {
                //エラーログフォルダ作成
                if (!Directory.Exists(errLogDir))
                {
                    Directory.CreateDirectory(errLogDir);
                    Utils.GrantAccessToUser(errLogDir, "IUSR");
                }

                using (FileStream fs = new FileStream(errLogDir + @"\ErrorLog_" + DateTime.Now.ToString("yyyyMMdd") + @".log", FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("shift-jis")))
                    {
                        sw.WriteLine("\r\n\r\n" + errMsg);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //throw new HttpException(CustomErrorCode.ERROR_WRITE_LOG, Utils.CreateErrorDescForWeb(e, SystemErrorMsg.UNKNOWN_ERROR));
                return ex;
            }
            finally
            {
                HttpContext.Current.Application.UnLock();
            }
        }
    }

    //ファイルをダウンロードするとき、ファイルが存在しない場合はエラーメッセージをクライアントに送る
    //ファイルだうんろー
    public class DownloadResult : ActionResult
    {
        public string FilePath { get; set; }
        public string ReturnFileName { get; set; }
        public string ContentType { get; set; }
        public DownloadResult() { }
        public DownloadResult(string filePath, string contentType, string returnFileName)
        {
            FilePath = filePath;
            ContentType = contentType;
            ReturnFileName = returnFileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            HttpRequestBase request = context.HttpContext.Request;

            HttpCookie cookie = request.Cookies["fileDownloaded"];
            if (cookie == null)
            {
                // no cookie found, create it
                cookie = new HttpCookie("fileDownloaded");
            }

            // update the expiration timestamp
            cookie.Expires = DateTime.UtcNow.AddDays(1);

            FileInfo fileInfo = new FileInfo(FilePath);
            if (!fileInfo.Exists)
            {
                cookie.Values["fileDownloaded"] = "ダウンロードファイルが存在しません。";
                response.ContentType = "text/plain";
                response.Cookies.Add(cookie);
                response.Write("");
                return;
            }

            response.ContentType = ContentType;
            if (!string.IsNullOrEmpty(ReturnFileName))
            {
                cookie.Values["fileDownloaded"] = "true";

                //ContentDisposition disposition = new ContentDisposition();
                //disposition.FileName = ReturnFileName;
                //disposition.Inline = true;
                //response.AddHeader("Content-Disposition", disposition.ToString());
                response.AddHeader("Content-Disposition", "attachment; filename=" + ReturnFileName);
                response.Cookies.Add(cookie);
            }

            context.HttpContext.Response.TransmitFile(FilePath);
        }
    }



}