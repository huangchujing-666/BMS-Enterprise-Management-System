using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class Errorlogdal : IErrorlog
    {
        model.pbxdatasourceDataContext pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="err">错误日志实体</param>
        public void InsertErrorlog(model.errorlog err)
        {
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                pbx.errorlog.InsertOnSubmit(err);
                pbx.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="list">错误日志实体集合</param>
        public void InsertErrorlogList(List<model.errorlog> list)
        {
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                pbx.errorlog.InsertAllOnSubmit(list);
                pbx.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="id">日志id</param>
        public bool DeleteErrorlog(int id, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                var temp = from c in pbx.errorlog where c.Id == id select c;
                if (temp.ToList().Count == 0)
                {
                    mess = "该条信息已经不存在";
                    return false;
                }
                pbx.errorlog.DeleteAllOnSubmit(temp);
                pbx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="beginTime">删除开始时间</param>
        /// <param name="endTime">删除结束时间</param>
        public bool DeleteErrorlog(DateTime beginTime, DateTime endTime, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                var temp = from c in pbx.errorlog where c.errorTime >= beginTime && c.errorTime <= endTime select c;
                pbx.errorlog.DeleteAllOnSubmit(temp);
                pbx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        /// <summary>
        /// 查询日志信息
        /// </summary>
        /// <param name="skip">跳过条数</param>
        /// <param name="take">取多少条</param>
        /// <param name="type">日志类型</param>
        /// <param name="listCount">数据中条数</param>
        /// <param name="mess">错误信息</param>
        /// <returns></returns>
        public List<model.errorlog> GetListErrorlog(int skip, int take, int type, out int listCount, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                var temp = (from c in pbx.errorlog where c.operation == type select c).ToList();
                listCount = temp.Count;
                List<model.errorlog> list = temp.Skip(skip).Take(take).ToList();
                return list;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                listCount = 0;
                return null;
            }
            finally
            {
                pbx.Connection.Close();
            }
        }

        //public List<model.errorlog> GetListErrorlog(Dictionary<string, string> dic, int skip, int take, int type, out int listCount, out string mess)
        //{
        //    listCount = 0;
        //    mess = "";
        //    return null;
        //}
        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="mess"></param>
        /// <param name="DBPath"></param>
        /// <returns></returns>
        public bool DateBackups(string pathName, out string mess, out string DBPath)
        {
            mess = "";
            DBPath = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                pbx.Connection.Open();
                string name = pbx.Connection.Database;
                string path = "pbxDB" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak";
                string DBname = pathName + "\\" + path;
                //BACKUP DATABASE " + name + " TO  DISK = N'" + DBname + "' WITH NOFORMAT, NOINIT,NAME = N'数据备份'" + DateTime.Now + "', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
              //  pbx.ExecuteCommand("BACKUP DATABASE " + name + "  TO disk = '" + DBname + "'  WITH FORMAT, NAME = '数据备份" + DateTime.Now + "'");
                pbx.ExecuteCommand("BACKUP DATABASE " + name + " TO  DISK = N'" + DBname + "' WITH NOFORMAT, NOINIT,NAME = N'数据备份" + DateTime.Now + "', SKIP, NOREWIND, NOUNLOAD,  STATS = 10");
                mess = "数据备份成功！备份路径：" + pathName + "\\pbxDB";
                DBPath = path;
                return true;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();

                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }

        }

        /// <summary>
        /// 数据恢复
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool Recover(string pathName, out string mess)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionMaster"].ToString());

            try
            {
                // pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                // pbx.Connection.Open();
                con.Open();
                string name = pbx.Connection.Database;
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "P_KillConnections";
                com.StatementCompleted += COMEvent;
                SqlParameter[] par = new SqlParameter[] { 
                    new SqlParameter("@dbname", "pbxdDB")
                };
                com.Parameters.AddRange(par);
                com.ExecuteNonQuery();
                com.CommandType = CommandType.Text;
                com.CommandText = "ALTER DATABASE [pbxDB] SET OFFLINE WITH ROLLBACK IMMEDIATE";
                com.ExecuteNonQuery();
                com.Dispose();
                SqlCommand com1 = new SqlCommand("RESTORE DATABASE pbxDB FROM disk =N'" + pathName + "'  WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 10", con);

                com1.CommandTimeout = 0;

                com1.ExecuteNonQuery();

                com1.Dispose();
                //exec P_KillConnections pbxDB
                //exec pro_DataRecover 'pbxDB','D:\pbxDB20150401031609.bak'
                // USE master  GO  RESTORE DATABASE pbxDB  FROM disk = 'c:\test_wt'
                mess = "数据恢复成功";

                return true;
            }
            catch (Exception ex)
            {
                mess = ex.Message;
                return false;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        public void COMEvent(object sender, StatementCompletedEventArgs e)
        {

        }
    }
}
