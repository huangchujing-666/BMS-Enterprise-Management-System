using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class Errorlogbll
    {
        idal.IErrorlog errdal = pbxdata.dalfactory.ReflectFactory.CreateIDataOperatingByReflect("Errorlogdal") as idal.IErrorlog;
        ///// <summary>
        ///// 初始化dal实例 连接数据库
        ///// </summary>
        ///// <param name="configName">数据库名称</param>
        ///// <param name="mess"></param>
        //public Errorlogbll(string configName)
        //{
        //   // errdal = new dal.Errorlogdal(configName);
        //}
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="err">错误日志实体</param>
        public void InsertErrorlog(model.errorlog err)
        {
            errdal.InsertErrorlog(err);  
        }
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="list">错误日志实体集合</param>
        public void InsertErrorlogList(List<model.errorlog> list)
        {
            errdal.InsertErrorlogList(list);
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="id">日志id</param>
        public bool DeleteErrorlog(int id,out string mess)
        {
            return errdal.DeleteErrorlog(id,out mess);
        }
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="beginTime">删除开始时间</param>
        /// <param name="endTime">删除结束时间</param>
        public bool DeleteErrorlog(DateTime beginTime, DateTime endTime, out string mess)
        {
            return errdal.DeleteErrorlog(beginTime, endTime,out mess);
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
        public List<model.errorlog> GetListErrorlog(int skip,int take,int type,out int listCount,out string mess)
        {
            return errdal.GetListErrorlog(skip, take, type, out listCount, out mess);
        }
        //public List<model.errorlog> GetListErrorlog(Dictionary<string, string> dic, int skip, int take, int type, out int listCount, out string mess)
        //{
        //    return errdal.GetListErrorlog(dic,skip,take,type,out listCount,out mess);
        //}
        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="mess"></param>
        /// <param name="DBPath"></param>
        /// <returns></returns>
        public bool DateBackups(string pathName, out string mess,out string DBPath)
        {
            bool bol=errdal.DateBackups(pathName, out mess,out DBPath);
            return bol;
        }
        public bool Recover(string pathName, out string mess)
        {
            return errdal.Recover(pathName,out mess);
        }
    }
}
