using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Configuration;
using ASP.NET_MVC研修.Models.Shared;
using ASP.NET_MVC研修.Common;

namespace ASP.NET_MVC研修.Models
{
    //Sharedのモデルを継承
    public class SendMailModel : MyModel
    {
        public string 宛先 { get; set; }
        public string 件名 { get; set; }
        public string 内容 { get; set; }
        public string 添付1 { get; set; }
        public string 添付2 { get; set; }
        public string 添付3 { get; set; }

        public SendMailModel()
        { }

        public SendMailModel(UserInfoModel m)
            : base(m)
        {

        }

        /// <summary>
        /// ユーザーにメールを送信する
        /// </summary>
        /// <param name="toEmail">ユーザーのメールアドレス</param>
        /// <param name="title">件名</param>
        /// <param name="mailContent">メールの内容</param>
        /// <param name="attachFiles">添付ファイル</param>
        public bool SendMailToUser(string toEmail, string title, string mailContent, List<string> attachFiles = null, bool isHtml = true)
        {
            MailMessage message = new MailMessage();
            try
            {
                var mails = toEmail.Split(';');
                //各プロパティについて、Web.configを参照する。
                //参照方法として、commonのAppKeyクラスでで定義したプロパティ名でkeyを紐づける。
                var adminEmail = ConfigurationManager.AppSettings[AppKey.KEY_ADMIN_EMAIL];
                var forSendingEmail = ConfigurationManager.AppSettings[AppKey.KEY_FOR_SENDING_EMAIL];
                var forSendingPass = ConfigurationManager.AppSettings[AppKey.KEY_FOR_SENDING_PASS];
                var mailServer = ConfigurationManager.AppSettings[AppKey.KEY_MAIL_SERVER];

                foreach (string mail in mails)
                {
                    message.To.Add(mail);
                }

                message.CC.Add(adminEmail);
                message.From = new MailAddress(forSendingEmail);
                message.Subject = title; //Title
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Body = mailContent;  //内容
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = isHtml;  //書式
                message.Priority = MailPriority.High;  //優先

                if (attachFiles != null)
                {
                    foreach (string filepath in attachFiles)
                    {
                        Attachment attachment = new System.Net.Mail.Attachment(filepath);
                        message.Attachments.Add(attachment);
                    }
                }

                using (var client = new SmtpClient())
                {
                    client.Credentials = new System.Net.NetworkCredential(forSendingEmail, forSendingPass);
                    client.Port = 587; //ポート番号の設定
                    client.Host = mailServer;  //surver　上記で設定したテスト用サーバー
                    client.Send(message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                message.Dispose(); //オブジェクト開放
            }
        }


    }
    }