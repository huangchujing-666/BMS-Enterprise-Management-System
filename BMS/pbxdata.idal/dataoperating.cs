﻿/*************************************************************************************
    * CLR版本：       4.0.30319.18063
    * 类 名 称：       menubll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       dataoperating
    * 创建时间：       2015/2/2 13:55:09
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maticsoft.DBUtility;

namespace pbxdata.idal
{
    public class dataoperating:IDataOperating
    {


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public string Add(IDataParameter[] ipara, string procName)
        {
            string s = string.Empty;
            int i = 0;
            DbHelperSQL.RunProcedure(procName, ipara, out i);
            s = i > 0 ? "添加成功" : "添加失败";
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
            int i = 0;
            DbHelperSQL.RunProcedure(procName, ipara, out i);
            s = i > 0 ? "修改成功" : "修改失败";
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
            int i = 0;
            DbHelperSQL.RunProcedure(procName, ipara, out i);
            s = i > 0 ? "删除成功" : "删除失败";
            return s;
        }

        /// <summary>
        /// 查询(datatable)
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public DataTable Select(IDataParameter[] ipara, string procName)
        {
            DataTable dt = new DataTable();
            dt = DbHelperSQL.RunProcedure(procName, ipara, "tt").Tables[0];
            return dt;
        }

        /// <summary>
        /// 查询(dataset)
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public DataSet Select(IDataParameter[] ipara, string procName,string dataset)
        {
            DataSet ds = new DataSet();
            ds = DbHelperSQL.RunProcedure(procName, ipara, "tt");
            return ds;
        }


        /// <summary>
        /// 返回所有字段名称集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string[] getDataName(string tableName)
        {
            List<model.users> list = new List<model.users>();
            string sql = "select top 1 * from " + tableName;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            string[] ss = new string[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ss[i] = dt.Columns[i].ColumnName;
            }

            return ss;
        }

    }
}
