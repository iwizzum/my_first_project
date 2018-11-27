using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace ASP.NET_MVC研修.Models
{
    public class ApplicationInfoModel
    {
        //画面上の項目に書き込む部分(呼び出し時に定義）
        public string 氏名 { get; set; }
        public string 所属 { get; set; }
        public string 役職 { get; set; }
        public string ユーザID { get; set; } //これは画面に出ない。たぶん
        public int 表示件数 { get; set; } //0が全て

        //内部でセッション単位で保管する部分
        public string 状態 { get; set; } //画面と連動
        public List<ApplicationInfoRowModel> 書類申請全件; //全件リスト
        public List<ApplicationInfoRowModel> 画面表示分; //画面上に出すリスト
        //public int 表示件数 = 10; //0が全て
        public int 現ページ = 1;
        public string ソート列 = "状態";
        public string ソート順 = "▼";



        //-----------------------------------------------------------------------------------
        //@brief
        //  内部プロパティを用いて検索を実行します
        //　表示一覧への転記は別処理
        //
        //@param
        //-----------------------------------------------------------------------------------
        public void Find()
        {
            string s_WHERE句;
            if (状態 == "0")
            {
                s_WHERE句 = "";
            }else{
                s_WHERE句 = " AND apply_status = " + 状態;
            }

            //初期SQL
            string sql = "select * from application_info WHERE user_id='" + ユーザID + "'" + s_WHERE句;
			
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

            //検索結果の初期化
            書類申請全件 = new List<ApplicationInfoRowModel>();
            foreach (DataRow row in tb.Rows)
            {
                //仮の値：調べるの面倒なのでひとまずuseridで。
                ApplicationInfoRowModel result = new ApplicationInfoRowModel();
                result.タイトル = row["TITLE"].ToString();
                result.承認日 = row["APPROVE_TIME"].ToString();
                result.申請ID = row["APPLY_ID"].ToString();
                result.申請種類 = row["APPLY_TYPE"].ToString();
                result.申請日 = row["APPLY_TIME"].ToString();
                result.連絡事項 = row["NOTICE_MATTER"].ToString();
                result.状態 = row["APPLY_STATUS"].ToString();

                書類申請全件.Add(result);
            }
        }


        //-----------------------------------------------------------------------------------
        //@brief
        //  表示件数とページ番号を元に、画面表示分listに表示データを格納する
        //
        //@param
        //-----------------------------------------------------------------------------------
        public void GetPage()
        {
            //case 1:表示件数が0（すべて見せる）か件数以下のデータしかない場合
            if (表示件数 == 0 || 書類申請全件.Count <= 表示件数)
            {
                画面表示分 = 書類申請全件;
            }
            else
            {
                画面表示分 = 書類申請全件
                            .Where((item, index) => index >= (現ページ - 1) * 表示件数 && index < 現ページ * 表示件数)
                            .ToList();
            }
        }


        //-----------------------------------------------------------------------------------
        //@brief
        //  ソート列とソート順を元に、結果一覧をソートする
        //  ソートするのは表示リストではなく検索リストなので注意
        //
        //@param
        //
        //@note
        //-----------------------------------------------------------------------------------
        public void SortAll()
        {
            //昇順
            if (ソート順 == "▼")
            {
                if (ソート列 == "タイトル")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.タイトル).ToList();
                }
                if (ソート列 == "承認日")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.承認日).ToList();
                }
                if (ソート列 == "状態")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.状態).ToList();
                }
                if (ソート列 == "申請ID")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.申請ID).ToList();
                }
                if (ソート列 == "申請日")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.申請日).ToList();
                }
                if (ソート列 == "連絡事項")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.連絡事項).ToList();
                }
                if (ソート列 == "申請種類")
                {
                    書類申請全件 = 書類申請全件.OrderBy(x => x.申請種類).ToList();
                }
            }
            //降順
            else
            {
                if (ソート列 == "タイトル")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.タイトル).ToList();
                }
                if (ソート列 == "承認日")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.承認日).ToList();
                }
                if (ソート列 == "状態")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.状態).ToList();
                }
                if (ソート列 == "申請ID")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.申請ID).ToList();
                }
                if (ソート列 == "申請日")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.申請日).ToList();
                }
                if (ソート列 == "連絡事項")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.連絡事項).ToList();
                }
                if (ソート列 == "申請種類")
                {
                    書類申請全件 = 書類申請全件.OrderByDescending(x => x.申請種類).ToList();
                }
            }

        }


    }



}