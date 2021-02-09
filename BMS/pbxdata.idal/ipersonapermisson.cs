/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ipersonapermisson
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       ipersonapermisson
    * 创建时间：       2015/3/2 14:14:39
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface ipersonapermisson
    {
        /// <summary>
        /// 分配角色的菜单权限
        /// </summary>
        /// <param name="mdRole"></param>
        /// <returns></returns>
        string AddRolePersonapermisson(List<model.personapermisson> list);


        #region 功能权限
        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的字段
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <param name="funid">功能ID</param>
        /// <returns></returns>
        int[] selectFiledPersonaPermisson(int roleid,int menuid,int funid);


        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的功能
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <returns></returns>
        int[] selectFunPersonaPermisson(int roleid, int menuid);

        #endregion


        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        bool selectMenuIsExists(int roleid, int menuid);

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        bool selectFunIsExists(int roleid, int menuid, int funid);


        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        bool selectFiledIsExists(int roleid, int menuid, int funid, int filed);


        /// <summary>
        /// 撤销角色的菜单权限
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string DelRolePersonapermisson(List<model.personapermisson> list);


        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        model.personapermisson selectMenuIsExists1(int roleid, int menuid);

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        model.personapermisson selectFunIsExists1(int roleid, int menuid, int funid);


        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        model.personapermisson selectFiledIsExists1(int roleid, int menuid, int funid, int filed);

    }
}
