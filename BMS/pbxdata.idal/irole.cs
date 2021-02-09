/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       irole
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       irole
    * 创建时间：       2015/2/11 17:20:00
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
    public interface irole
    {
        List<model.persona> getRole(IDataParameter[] ipara, string procName);

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        model.persona getRoleName(int id);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        DataTable getRole();

        /// <summary>
        /// 复制角色权限
        /// </summary>
        string CopyUsers(string Id, string PersonaId);

    }
}
