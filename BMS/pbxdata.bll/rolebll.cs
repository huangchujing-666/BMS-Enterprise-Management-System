/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       rolebll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       rolebll
    * 创建时间：       2015/2/11 17:24:21
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
    public class rolebll:dataoperatingbll
    {
        irole dal = (irole)ReflectFactory.CreateIDataOperatingByReflect("roledal");

        public List<model.persona> getRole(IDataParameter[] ipara, string procName)
        {
            return dal.getRole(ipara,procName);
        }


                /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public model.persona getRoleName(int id)
        {
            return dal.getRoleName(id);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public DataTable getRole()
        {
            return dal.getRole();
        }
        /// <summary>
        /// 复制角色权限
        /// </summary>
        public string CopyUsers(string Id, string PersonaId)
        {
            return dal.CopyUsers(Id, PersonaId);
        }
    }
}
