using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace ASP.NET_MVC研修.Models
{
    public class ApplicationApproveModel
    {
        //画面上の項目に書き込む部分(呼び出し時に定義）
        public string 氏名 { get; set; }
        public string 所属 { get; set; }
        public string 役職 { get; set; }
        public string 申請状態 { get; set; }
        public string 申請種類 { get; set; }
        public string タイトル { get; set; }
        public string 申請ファイル { get; set; }
        public string 連絡事項 { get; set; }
        public string 申請ID { get; set; }

        //-----------------------------------------------------------------------------------
        //@brief
        //  特定の申請に対してセッションモデルを作成します
        //
        //@param
        //-----------------------------------------------------------------------------------
        public ApplicationApproveModel GetApproveData(ApplicationInfoModel argAppInfo,string arg申請ID)
        {
            //初期SQL
            string sql = "select * from application_info WHERE user_id='" + argAppInfo.ユーザID + "' AND APPLY_ID='" + arg申請ID + "'";
			
            //SQL実行
            MySqlConnection con = new MySqlConnection(
                                    "server=gat-serverpc;user id=csharp;password=csharp;"
                                    + "database=csharp; convert zero datetime=True");

            con.Open();

            DataTable tb = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

            da.Fill(tb);
            da.Dispose();
            con.Close();

            ApplicationApproveModel result = new ApplicationApproveModel();

            //top1だけでいいんだけど。暫定。
            foreach (DataRow row in tb.Rows)
                {
                    result.タイトル = row["TITLE"].ToString();
                    result.申請ID = row["APPLY_ID"].ToString();
                    result.申請種類 = row["APPLY_TYPE"].ToString();
                    result.連絡事項 = row["NOTICE_MATTER"].ToString();
                    result.申請状態 = row["APPLY_STATUS"].ToString();
                    //result.申請ファイル = row["APPLY_STATUS"].ToString();
                    result.申請ファイル = "test.txt"; //暫定
                }
            
            //argから
            result.氏名 = argAppInfo.氏名;
            result.所属 = argAppInfo.所属;
            result.役職 = argAppInfo.役職;

            return result;
         }

        //-----------------------------------------------------------------------------------
        //@brief
        //  引数のIDに対してdbの値を更新します
        //
        //@param
        //  arg値 更新の値
        //-----------------------------------------------------------------------------------
        public void ChangeStatus(string argID, string argSupplyID, string arg更新値)
        {
            //初期SQL
            string sql = "update application_info set APPLY_STATUS='" + arg更新値 + "' WHERE user_id='" + argID + "' AND APPLY_ID='" + argSupplyID + "'";

            //SQL実行
            MySqlConnection con = new MySqlConnection(
                                    "server=gat-serverpc;user id=csharp;password=csharp;"
                                    + "database=csharp; convert zero datetime=True");

            MySqlCommand cmd = new MySqlCommand(sql, con);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            con.Open();
        }


     }

}
