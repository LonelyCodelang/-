using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 数据库表改列
{
    class Program
    {


        static void Main(string[] args)
        {
            // new Program().exSql("");//修改数据
            new Program().exListDBSql();//扫描整个数据库修改列
            //new Program().DropColumn("");//删除列
            Console.WriteLine("ok");
        }

        public List<string> UpdateSql()
        {
            List<string> result = new List<string>();
            string sql = " update [table] set [column]=null where [column]='暂无数据' ; ";
            string sql1 = " update [table] set [column]=null where [column]='暂无价格' ; ";
            string sql2 = " update [table] set [column]=null where [column]='暂无'  ;";
            string sql3 = " update [table] set [column]=null where [column]='暂无资料'  ;";
            string sql4 = " update [table] set [column]=null where [column]='暂无均价'  ;";
            string sql5 = " update [table] set [column]=replace([column],'&#183;','·') where [column] like '%&#183;%'  ;";

            result.Add(sql);
            result.Add(sql1);
            result.Add(sql2);
            result.Add(sql3);
            result.Add(sql4);
            result.Add(sql5);
            return result;
        }

        public void exListDBSql()
        {
            List<string> listDb = new DBTableHelper("").GetDBName();
            foreach (var item in listDb)
            {
                //List<string> list = new List<string>();
                //list.Add("宝鸡");
                //list.Add("保定");
                //list.Add("滨州");
                //list.Add("沧州");
                //list.Add("常德");

                if (item.Contains("TempDB_"))
                {
                    string SqlConnection = "Data Source=192.168.11.202;Initial Catalog =[DbName];User Id = sa;Password = gh001;";
                    SqlConnection = SqlConnection.Replace("[DbName]", item);
                    Console.WriteLine(string.Format("数据库名称:{0},连接字符串:{1}", item, SqlConnection));
                    exSql(SqlConnection);
                }
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        /// <param name="DBName"></param>
        public void exSql(string SqlConnection)
        {
            List<string> listSql = new Program().UpdateSql();
            List<string> listTable = new DBTableHelper(SqlConnection).GetDbTable();
            foreach (var item in listTable)
            {
                //if (!item.Contains("二手房"))
                //{
                //    continue;
                //}

                List<string> listColumn = new DBTableHelper(SqlConnection).GetTableColumn(item);
                foreach (var Column in listColumn)
                {
                    foreach (var SqlTemp in listSql)
                    {
                        try
                        {
                            string sql = SqlTemp.Replace("[table]", "[" + item + "]").Replace("[column]", "[" + Column + "]");

                            int Num = new SqlHelper(SqlConnection).ExecuteNonQuery(sql);
                            if (Num > 0)
                            {
                                Console.WriteLine(string.Format("sql:{0},影响行数:{1}", sql, Num));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("表:{2},列:{0}出错,msg:{1}", Column, ex.ToString(), item));
                        }

                    }
                }
            }
        }

        private void DropColumn(string SqlConnection)
        {
            string sqlTemp = "alter table [table] drop column [column]";
            List<string> listTable = new DBTableHelper(SqlConnection).GetDbTable();
            foreach (var item in listTable)
            {
                List<string> listColumn = new DBTableHelper(SqlConnection).GetTableColumn(item);
                foreach (var Column in listColumn)
                {
                    if (Column.Contains("F"))
                    {
                        try
                        {
                            string sql = sqlTemp.Replace("[table]", "[" + item + "]").Replace("[column]", "[" + Column + "]");
                            int Num = new SqlHelper(SqlConnection).ExecuteNonQuery(sql);
                            Console.WriteLine(string.Format("sql:{0},影响行数:{1}", sql, Num));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("表:{2},列:{0}出错,msg:{1}", Column, ex.ToString(), item));
                        }
                    }
                }
            }
        }

    }
}
