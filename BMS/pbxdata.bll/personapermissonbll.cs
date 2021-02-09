/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       personapermissonbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       personapermissonbll
    * 创建时间：       2015/3/2 14:18:07
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
    public class personapermissonbll:dataoperatingbll
    {
        ipersonapermisson dal = (ipersonapermisson)ReflectFactory.CreateIDataOperatingByReflect("personapermissondal");

        /// <summary>
        /// 分配角色的菜单权限
        /// </summary>
        /// <param name="mdRole"></param>
        /// <returns></returns>
        public string AddRolePersonapermisson(List<model.personapermisson> list)
        {
            return dal.AddRolePersonapermisson(list);
        }


        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的字段
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <param name="funid">功能ID</param>
        /// <returns></returns>
        public int[] selectFiledPersonaPermisson(int roleid, int menuid, int funid)
        {
            return dal.selectFiledPersonaPermisson(roleid, menuid, funid);
        }

        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的功能
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <returns></returns>
        public int[] selectFunPersonaPermisson(int roleid, int menuid)
        {
            return dal.selectFunPersonaPermisson(roleid, menuid);
        }


        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public bool selectMenuIsExists(int roleid, int menuid)
        {
            return dal.selectMenuIsExists(roleid, menuid);
        }

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        public bool selectFunIsExists(int roleid, int menuid, int funid)
        {
            return dal.selectFunIsExists(roleid, menuid, funid);
        }


        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool selectFiledIsExists(int roleid, int menuid, int funid, int filed)
        {
            return dal.selectFiledIsExists(roleid, menuid, funid, filed);
        }


        /// <summary>
        /// 撤销角色的菜单权限
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string DelRolePersonapermisson(List<model.personapermisson> list)
        {
            return dal.DelRolePersonapermisson(list);
        }


        #region 返回数据库存在的实体
        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public model.personapermisson selectMenuIsExists1(int roleid, int menuid)
        {
            return dal.selectMenuIsExists1(roleid, menuid);
        }

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        public model.personapermisson selectFunIsExists1(int roleid, int menuid, int funid)
        {
            return dal.selectFunIsExists1(roleid, menuid, funid);
        }


        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public model.personapermisson selectFiledIsExists1(int roleid, int menuid, int funid, int filed)
        {
            return dal.selectFiledIsExists1(roleid, menuid, funid, filed);
        }
        #endregion
    }
}
