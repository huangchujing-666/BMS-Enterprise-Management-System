/*************************************************************************************
    * CLR版本：       4.0.30319.18063
    * 类 名 称：       menubll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       IDataOperating
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

namespace pbxdata.idal
{
    public interface IDataOperating
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        string Add(IDataParameter[] ipara,string procName);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        string Update(IDataParameter[] ipara, string procName);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        string Delete(IDataParameter[] ipara, string procName);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="ipara">参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        DataTable Select(IDataParameter[] ipara,string procName);

      
    }
}
