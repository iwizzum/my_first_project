using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace ASP.NET_MVC研修.Models
{
    public class UserModel
    {
        public string ユーザーID { get; set; } //ユーザ情報のユーザID
        public string メールアドレス { get; set; } //ユーザ情報のメールアドレス
        public string パスワード { get; set; } //ユーザ情報のパスワード

        public bool Check(string userid, string email, string pass)
        {
            string sql = "select 1 from user_master where PASSWORD = '" + pass + "'";

            if (userid != "" && userid != null)
            {
                sql = sql + " and USER_ID='" + userid + "'";
            }
            if (email != "" && email != null)
            {
                sql = sql + " and EMAIL='" + email + "'";
            }

            MySqlConnection con = new MySqlConnection(
                "server=gat-serverpc;user id=csharp;password=csharp;"
                + "database=csharp; convert zero datetime=True");

            con.Open();

            DataTable tb = new DataTable();

            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);


            da.Fill(tb);
            da.Dispose();

            con.Close();

            if (tb.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

}