using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
   public  interface IErrorlog
    {
        
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="err">错误日志实体</param>
         void InsertErrorlog(model.errorlog err);
        /// <summary>
        /// 添加错误日志信息
        /// </summary>
        /// <param name="list">错误日志实体集合</param>
         void InsertErrorlogList(List<model.errorlog> list);
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="id">日志id</param>
         bool DeleteErrorlog(int id,out string mess);
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="beginTime">删除开始时间</param>
        /// <param name="endTime">删除结束时间</param>
         bool DeleteErrorlog(DateTime beginTime, DateTime endTime,out string mess);
        /// <summary>
        /// 查询日志信息
        /// </summary>
        /// <param name="skip">跳过条数</param>
        /// <param name="take">取多少条</param>
        /// <param name="type">日志类型</param>
        /// <param name="listCount">数据中条数</param>
        /// <param name="mess">错误信息</param>
        /// <returns></returns>
         List<model.errorlog> GetListErrorlog(int skip,int take,int type,out int listCount,out string mess);

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
         bool DateBackups(string pathName, out string mess,out string DBPath);
       /// <summary>
       /// 数据恢复
       /// </summary>
       /// <param name="pathName">备份路径</param>
       /// <param name="mess"></param>
       /// <returns></returns>
         bool Recover(string pathName, out string mess);
    }
}
