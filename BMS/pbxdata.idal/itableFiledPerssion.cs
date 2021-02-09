/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       itableFiledPerssion
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       itableFiledPerssion
    * 创建时间：       2015/2/11 11:10:19
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

namespace pbxdata.idal
{
    public interface itableFiledPerssion
    {
        List<model.tableFiledPerssion> getTable(IDataParameter[] ipara, string procName);

        /// <summary>
        /// 根据字段ID返回字段名称
        /// </summary>
        /// <param name="filedid"></param>
        /// <returns></returns>
        string getFiledName(int filedid);

        /// <summary>
        /// 根据字段ID集合返回字段名称集合
        /// </summary>
        /// <param name="filedids"></param>
        /// <returns></returns>
        string[] getFiledName(int[] filedids);


        /// <summary>
        /// 根据字段ID集合返回有权限的字段,并组合成sql语句
        /// </summary>
        /// <param name="filedids">字段集合</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        string getFiledPermissionSQL(int[] filedids, string tableName);

        /// <summary>
        /// 添加表
        /// </summary>
        string AddTabName(string tableName, int UserId);

        /// <summary>
        /// 添加列
        /// </summary>
        string AddFiledName(string tableName, string tableLevel, int UserId);

         /// <summary>
        /// 添加字段描述,添加表描述tableFiledPerssion表
        /// </summary>
        string AddDescript(string Id, string Descript);

         /// <summary>
        /// 修改表名,列名
        /// </summary>
        string UpdateTabName(string Id, string TabName);

        /// <summary>
        /// 删除表,列
        /// </summary>
        string DeletetableFiled(string Id, int UserId);

    }
}
