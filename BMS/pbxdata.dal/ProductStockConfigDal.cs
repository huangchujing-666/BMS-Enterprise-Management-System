using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using System.Data;
using System.Data.SqlClient;
namespace pbxdata.dal
{
    public class ProductStockConfigDal : dataoperating
    {

        /// <summary>
        /// 查询出所有的程序Id 然后进行判断
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllProcss() 
        {
            return null;
        }
        /// <summary>
        /// 查看所有可更新的数据源
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAll() 
        {
            IDataParameter[] ipr = new IDataParameter[] {
                new SqlParameter("sql",""),
            };
            return Select(ipr, "SelectPscAll");
        }
        /// <summary>
        /// 更新商品
        /// </summary>
        /// <returns></returns>
        public DataTable SelectShopUpdateAll() 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql","")
            };
            return Select(ipr, "SelectShopUpdateAll");
        }
        /// <summary>
        /// 开始执行  修改状态，修改时间---数据源更新
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <param name="satarttime"></param>
        public void UpdateStartTime(string sourcecode,string satarttime,int processId) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("SourceCode",sourcecode),
                new SqlParameter("StartTime",satarttime),
                new SqlParameter("ProcssId",processId)
            };
            Update(ipr, "UpdateStartTime");
        }
        /// <summary>
        /// 开始执行 修改状态，修改时间---商品更新
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <param name="satarttime"></param>
        /// <param name="processId"></param>
        public void UpdateStartTimeShop(string Def3, string satarttime, int processId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Def3",Def3),
                new SqlParameter("StartTime",satarttime),
                new SqlParameter("ProcssId",processId)
            };
            Update(ipr, "UpdateStartTimeShop");
        }
        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="SourceCode"></param>---数据更新
        public void UpdateSetTime(string time,string SourceCode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("SetTime",time),
                new SqlParameter("SourceCode",SourceCode)
            };
            Update(ipr, "UpdateSetTime");
        }
        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="SourceCode"></param>-----商品更新
        public void UpdateSetTimeShop(string time, string Def3)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("SetTime",time),
                new SqlParameter("Def3",Def3)
            };
            Update(ipr, "UpdateSetTimeShop");
        }
        /// <summary>
        /// 停止更新  修改状态
        /// </summary>
        public void UpdateStateByConfig(string sourceCode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("SourceCode",sourceCode)
            };
            Update(ipr, "UpdateStateByConfig");
        }
        /// <summary>
        /// 停止更新  修改状态--商品更新
        /// </summary>
        public void UpdateStateByConfigShop(string Def3)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Def3",Def3)
            };
            Update(ipr, "UpdateStateByConfigShop");
        }
        /// <summary>
        /// 设置线程个数  ---商品
        /// </summary>
        public bool UpdateThread(string Thread,string Def3) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Thread",Thread),
                new SqlParameter("Def3",Def3)
            };
            try
            {
                Update(ipr, "UpdateThread");
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            
        }
        /// <summary>
        /// 设置线程个数  ---数据源
        /// </summary>
        public bool UpdateThreadStock(string Thread,string SourceCode) 
        {

            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Thread",Thread),
                new SqlParameter("SourceCode",SourceCode)
            };
            try
            {
                Update(ipr, "UpdateThreadStock");
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
        }
        /// <summary>
        /// 通过数据源编号查找当前数据源程序的运行状态
        /// </summary>
        /// <param name="SourceCode"></param>
        /// <returns></returns>
        public int ScProcssIdBySourcesCode(string SourceCode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("SourceCode",SourceCode)
            };
            DataTable dtr= Select(ipr, "ScProcssIdBySourcesCode");
            int ProcessId = int.Parse(dtr.Rows[0][0].ToString());
            return ProcessId;
        }
        /// <summary>
        /// 通过功能编号 查找当前程序的运行状态
        /// </summary>
        /// <param name="Def3"></param>
        /// <returns></returns>
        public int ScProcssIdByDef(string Def3)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Def3",Def3)
            };
            DataTable dtr = Select(ipr, "ScProcssIdByDef");
            int ProcessId = int.Parse(dtr.Rows[0][0].ToString());
            return ProcessId;
        }

    }
}
