/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       dataoperatingbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       dataoperatingbll
    * 创建时间：       2015/2/10 9:59:20
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class dataoperatingbll
    {
        dataoperating dal = new dataoperating();


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public string Add(IDataParameter[] ipara, string procName)
        {
            string s = string.Empty;
            s = dal.Add(ipara, procName);

            return s;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public string Update(IDataParameter[] ipara, string procName)
        {
            string s = string.Empty;
            s = dal.Update(ipara, procName);
            return s;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public string Delete(IDataParameter[] ipara, string procName)
        {
            string s = string.Empty;
            s = dal.Delete(ipara, procName);
            return s;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public DataTable Select(IDataParameter[] ipara, string procName)
        {
            DataTable dt = new DataTable();
            dt = dal.Select(ipara, procName);
            return dt;
        }


        /// <summary>
        /// 返回所有字段名称集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string[] getDataName(string tableName)
        {
            string[] ss = dal.getDataName(tableName);
            return ss;
        }
    }
}
