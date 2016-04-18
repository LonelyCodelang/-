using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数据库表改列
{
    public class DBTableHelper
    {
        private string SqlConnection;

        public DBTableHelper(string _SqlConnection)
        {
            SqlConnection = _SqlConnection;
        }

        /// <summary>
        /// 查询所有的数据库的数据库名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetDBName()
        {
            List<string> result = new List<string>();
            string sql = "Select name from master..sysdatabases ";

            DataTable dt = new SqlHelper(SqlConnection).ExecuteDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row["name"].ToString());
            }
            return result;
        }


        /// <summary>
        /// 获取数据库所有的表名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetDbTable()
        {
            List<string> result = new List<string>();
            string sql = "select * from sysobjects where xtype='u' ";

            DataTable dt = new SqlHelper(SqlConnection).ExecuteDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row["name"].ToString());
            }
            return result;
        }

        /// <summary>
        /// 获取表中所有的列
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <returns></returns>
        public List<string> GetTableColumn(string TableName)
        {
            List<string> result = new List<string>();
            string sql = string.Format("	select * from sys.columns where object_id=object_id('[{0}]')", TableName);
            DataTable dt = new SqlHelper(SqlConnection).ExecuteDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row["name"].ToString());
            }
            return result;
        }

    }
}
