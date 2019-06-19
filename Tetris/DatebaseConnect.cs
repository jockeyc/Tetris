using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Tetris
{
    class DatabaseConnect
    {

        public string sqlcondata = "server=127.0.0.1;port=3306;database=PlayerRank;user=root;password=123456qwe;SslMode=none;";
        public string readerdata = "select * from playerdetail order by playerscore DESC,submissiondate ASC";
        public string insertdata = "insert into playerdetail value(@pname,@pscore,@stime)";
        public DatabaseConnect()
        { }
        /// <summary>
        /// 建立连接，传入数据库的具体信息
        /// </summary>
        public MySqlConnection Getcon()
        {
            MySqlConnection mysqlcon1 = new MySqlConnection(sqlcondata);
            mysqlcon1.Open();
            return mysqlcon1;
        }
         /// <summary>
         /// 开启读取
         /// </summary>
        public MySqlDataReader GetMyReader(MySqlConnection condetail,string query)
        {
            MySqlCommand mycom = new MySqlCommand(query, condetail);
            MySqlDataReader myreader = mycom.ExecuteReader();
            return myreader;
        }
        public void Insertdata(MySqlConnection condetail,string insert,string name,string score,string time)
        {
            MySqlCommand mycom = new MySqlCommand(insert,condetail);
            mycom.Parameters.AddWithValue("@pname",name);
            mycom.Parameters.AddWithValue("@pscore",score);
            mycom.Parameters.AddWithValue("@stime", time);
            mycom.ExecuteNonQuery();
        }

        public void Closeconnect(MySqlConnection condetail)
        {
            condetail.Close();
        }
     }


}
