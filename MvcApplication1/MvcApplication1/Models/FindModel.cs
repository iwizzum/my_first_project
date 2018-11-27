using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ASP.NET_MVC研修.Models
{
    public class FindModel
    {
        public List<FindRowModel> 検索結果一覧;
        public List<FindRowModel> 表示一覧;
        public int 表示件数;
        public int 現ページ;
        public string ソート列;
        public string ソート順;


        public void Find(string ユーザーID, string メールアドレス, string ニックネーム, string 氏名, int 性別, string 生年月日, int 年齢, string 電話番号, string 郵便番号, string 住所, string 入社日, string 所属, string 役職, string 趣味, string 特技, string 座右銘)
        {
            現ページ = 1;
            ソート順 = "▲";

            string sql = @"select
                               *,
                               cast(TIMESTAMPDIFF(YEAR,
                               str_to_date(p.BIRTHDAY,'%Y%m%d'),
                               CURDATE()) as char) as YEAR_OLD
                           from
                               user_master u
                                   inner join profile_info p
                                       on u.USER_ID = p.USER_ID
                           where 1 = 1
                           and   type = '2'";

            if (ユーザーID != null && ユーザーID != "")
            {
                sql = sql + " and u.USER_ID like '%" + ユーザーID + "%' ";
            }
            if (メールアドレス != null && メールアドレス != "")
            {
                sql = sql + " and u.EMAIL like '%" + メールアドレス + "%' ";
            }
            if (ニックネーム != null && ニックネーム != "")
            {
                sql = sql + " and p.NICK_NAME like '%" + ニックネーム + "%' ";
            }
            if (氏名 != null && 氏名 != "")
            {
                sql = sql + " and p.USER_NAME like '%" + 氏名 + "%' ";
            }
            if (性別 != 0)
            {
                sql = sql + " and p.SEX = " + 性別;
            }
            if (生年月日 != null && 生年月日 != "")
            {
                sql = sql + " and p.BIRTHDAY like '" + 生年月日 + "' ";
            }
            if (電話番号 != null && 電話番号 != "")
            {
                sql = sql + " and p.TEL like '%" + 電話番号 + "%' ";
            }
            if (郵便番号 != null && 郵便番号 != "")
            {
                sql = sql + " and p.POSTCODE like '%" + 郵便番号 + "%' ";
            }
            if (住所 != null && 住所 != "")
            {
                sql = sql + " and p.ADDRESS like '%" + 住所 + "%' ";
            }
            if (入社日 != null && 入社日 != "")
            {
                sql = sql + " and p.HIRE_DATE like '" + 入社日 + "' ";
            }
            if (所属 != null && 所属 != "")
            {
                sql = sql + " and p.POSITION like '%" + 所属 + "%' ";
            } if (役職 != null && 役職 != "")
            {
                sql = sql + " and p.AFFILIATION like '%" + 役職 + "%' ";
            } if (趣味 != null && 趣味 != "")
            {
                sql = sql + " and p.HOBBY like '%" + 趣味 + "%' ";
            } if (特技 != null && 特技 != "")
            {
                sql = sql + " and p.SPECIAL_SKILL like '%" + 特技 + "%' ";
            } if (座右銘 != null && 座右銘 != "")
            {
                sql = sql + " and p.COMMENT like '%" + 座右銘 + "%' ";
            }


            MySqlConnection con = new MySqlConnection(
                "server=localhost;user id=csharp;password=csharp;"
               + "database=csharp; convert zero datetime=True");
            con.Open();

            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

            DataTable tb = new DataTable();

            da.Fill(tb);
            da.Dispose();
            con.Close();

            検索結果一覧 = new List<FindRowModel>();
            foreach (DataRow row in tb.Rows)
            {
                FindRowModel result = new FindRowModel();
                result.ユーザーID = row["USER_ID"].ToString();
                result.メールアドレス = row["EMAIL"].ToString();
                result.氏名 = row["USER_NAME"].ToString();
                result.住所 = row["ADDRESS"].ToString();
                result.性別 = (int)row["SEX"];
                result.電話番号 = row["TEL"].ToString();
                result.郵便番号 = row["POSTCODE"].ToString();

                検索結果一覧.Add(result);
            }

            SortAll(ソート列, ソート順);
            GetPage(表示件数,現ページ);


        }

        public void GetPage(int rowCount, int pageNum)
        {

            if (rowCount == 0 || 検索結果一覧.Count <= rowCount)
            {
                表示一覧 = 検索結果一覧;
            }
            else
            {
                if (rowCount == 表示件数)
                {

                    表示一覧 = 検索結果一覧

                                .Where((item, index) => index >= (pageNum - 1) * rowCount && index < pageNum * rowCount)

                                .ToList();
                }
                else//1ページ目のデータを選択する						
                {
                    表示一覧 = 検索結果一覧.Where((item, index) => index < rowCount).ToList();
                }
            }

            表示件数 = rowCount;
            現ページ = pageNum;
        }

        public void SortAll(string colName, string sortOrder)			
        {			
	        //昇順でソートする		
	        if (sortOrder == "▲" || sortOrder == "")		
	        {		
		        if (ソート列 == "ユーザーID")	
		        {	
			
			
			        検索結果一覧 = 検索結果一覧.OrderBy(x => x.ユーザーID).ToList();
		        }

                if (ソート列 == "氏名")
                {


                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.氏名).ToList();
                }
                if (ソート列 == "性別")
                {


                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.性別).ToList();
                }
                if (ソート列 == "電話番号")
                {


                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.電話番号).ToList();
                }
                if (ソート列 == "郵便番号")
                {


                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.郵便番号).ToList();
                }
                if (ソート列 == "住所")
                {


                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.住所).ToList();
                }
			
	        }
		    else		
            {		
	            if (ソート列 == "ユーザーID")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.ユーザーID).ToList();
	            }	
	            if (ソート列 == "氏名")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.氏名).ToList();
	            }
		        	            if (ソート列 == "性別")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.性別).ToList();
	            }
                	            if (ソート列 == "電話番号")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.電話番号).ToList();
	            }
                	            if (ソート列 == "郵便番号")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.郵便番号).ToList();
	            }
                	            if (ソート列 == "住所")	
	            {	
		
		
		            検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.住所).ToList();
	            }
	            
		
            }		


		        			
	        }

        public void Sort(string colName, string order)
        {
            ソート列 = colName;
            ソート順 = order;
            現ページ = 1;
            SortAll(colName, order);
            GetPage(表示件数, 1);
        }



    }
}