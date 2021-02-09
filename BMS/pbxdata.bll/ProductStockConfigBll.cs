using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.dal;
namespace pbxdata.bll
{
    public class ProductStockConfigBll
    {
        ProductStockConfigDal pscd = new ProductStockConfigDal();
        /// <summary>
        /// 查看所有可更新的数据源
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAll()
        {
            DataTable dt = pscd.SelectAll();
            dt.Columns.Remove("Def5");
            return dt;
        }
        /// <summary>
        /// 更新商品
        /// </summary>
        /// <returns></returns>
        public DataTable SelectShopUpdateAll()
        {
            DataTable dt = pscd.SelectShopUpdateAll();
            dt.Columns.Remove("Def4");
            dt.Columns.Remove("Def5");
            dt.Columns.Remove("SourceCode");
            dt.Columns.Remove("TableName");
            return dt;
        }
        /// <summary>
        /// 开始执行  修改状态，修改时间---数据源更新
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <param name="satarttime"></param>
        public void UpdateStartTime(string sourcecode, string starttime,int processId)
        {
            pscd.UpdateStartTime(sourcecode, starttime, processId);
        }
        /// <summary>
        /// 开始执行  修改状态，修改时间---商品更新
        /// </summary>
        /// <param name="sourcecode"></param>
        /// <param name="satarttime"></param>
        public void UpdateStartTimeShop(string Def3, string satarttime, int processId)
        {
            pscd.UpdateStartTimeShop(Def3,satarttime,processId);
        }
        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="SourceCode"></param>
        public void UpdateSetTime(string time, string SourceCode)
        {
            pscd.UpdateSetTime(time,SourceCode);
        }
        /// <summary>
        /// 设置时间间隔
        /// </summary>
        /// <param name="time"></param>
        /// <param name="SourceCode"></param>-----商品更新
        public void UpdateSetTimeShop(string time, string Def3)
        {
            pscd.UpdateSetTimeShop(time,Def3);
        }
        /// <summary>
        /// 停止更新  修改状态
        /// </summary>
        public void UpdateStateByConfig(string sourceCode)
        {
            pscd.UpdateStateByConfig(sourceCode);
        }
        /// <summary>
        /// 停止更新  修改状态--商品更新
        /// </summary>
        public void UpdateStateByConfigShop(string Def3)
        {
            pscd.UpdateStateByConfigShop(Def3);
        }
        /// <summary>
        /// 设置线程个数 --商品
        /// </summary>
        /// <param name="Thread"></param>
        /// <param name="Def3"></param>
        /// <returns></returns>
        public bool UpdateThread(string Thread, string Def3) 
        {
            return pscd.UpdateThread(Thread,Def3);
        }
        /// <summary>
        /// 设置线程个数  ---数据源
        /// </summary>
        public bool UpdateThreadStock(string Thread, string SourceCode)
        {
            return pscd.UpdateThreadStock(Thread,SourceCode);
        }
        /// <summary>
        /// 通过数据源编号查找当前数据源程序的运行状态
        /// </summary>
        /// <param name="SourceCode"></param>
        /// <returns></returns>
        public int ScProcssIdBySourcesCode(string SourceCode)
        {
            return pscd.ScProcssIdBySourcesCode(SourceCode);
        }
        /// <summary>
        /// 通过功能编号 查找当前程序的运行状态
        /// </summary>
        /// <param name="Def3"></param>
        /// <returns></returns>
        public int ScProcssIdByDef(string Def3)
        {
            return pscd.ScProcssIdByDef(Def3);
        }
    }
}
