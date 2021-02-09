/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       tableFiledPerssionbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       tableFiledPerssionbll
    * 创建时间：       2015/2/11 11:11:36
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.dal;
using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class tableFiledPerssionbll : dataoperatingbll
    {
        itableFiledPerssion dal = (itableFiledPerssion)ReflectFactory.CreateIDataOperatingByReflect("tableFiledPerssiondal");

        public List<model.tableFiledPerssion> getTable(IDataParameter[] ipara, string procName)
        {
            return dal.getTable(ipara, procName);
        }


        /// <summary>
        /// 根据字段ID返回字段名称
        /// </summary>
        /// <param name="filedid"></param>
        /// <returns></returns>
        public string getFiledName(int filedid)
        {
            return dal.getFiledName(filedid);
        }

        /// <summary>
        /// 根据字段ID集合返回字段名称集合
        /// </summary>
        /// <param name="filedids"></param>
        /// <returns></returns>
        public string[] getFiledName(int[] filedids)
        {
            return dal.getFiledName(filedids);
        }

        /// <summary>
        /// 根据字段ID集合返回有权限的字段,并组合成sql语句
        /// </summary>
        /// <param name="filedids"></param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public string getFiledPermissionSQL(int[] filedids, string tableName)
        {
            return dal.getFiledPermissionSQL(filedids, tableName);
        }
        /// <summary>
        /// 添加表
        /// </summary>
        public string AddTabName(string TabName,int UserId)
        {
            return dal.AddTabName(TabName, UserId);
        }
        /// <summary>
        /// 添加列
        /// </summary>
        public string AddFiledName(string tableName, string tableLevel,int UserId)
        {
            return dal.AddFiledName(tableName, tableLevel, UserId);
        }
        /// <summary>
        /// 添加字段描述,添加表描述tableFiledPerssion表
        /// </summary>
        public string AddDescript(string Id,string Descript)
        {
            return dal.AddDescript(Id, Descript);
        }
         /// <summary>
        /// 修改表名,列名
        /// </summary>
        public string UpdateTabName(string Id,string TabName)
        {
            return dal.UpdateTabName(Id, TabName);
        }
        /// <summary>
        /// 删除表,列
        /// </summary>
        public string DeletetableFiled(string Id, int UserId)
        {
            return dal.DeletetableFiled(Id,UserId);
        } 
    }
}
