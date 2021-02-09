/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       users
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       users
    * 创建时间：       2015/2/9 16:22:01
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
    public interface iusers
    {
        List<model.users> GetModel(IDataParameter[] ipara, string procName);

        List<model.users> getUserData(string sql);

        /// <summary>
        /// 根据权限字段组成sql语句返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable getData();


        /// <summary>
        /// 根据权限字段组成sql语句返回集合(可带条件查询)
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        DataTable getData(Dictionary<string, string> dic = null);

        /// <summary>
        /// 编辑-根据ID获取信息
        /// </summary>
        /// <returns></returns>
        DataTable getDataEdit(int id);



        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        string del(int id);

        /// <summary>
        /// 分配店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        string InsertShopAllocation(string shopid, string userid);
        /// <summary>
        /// 当前用户是否已经分配当前店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool ShopIsAllocation(string shopid, string userid);
        /// <summary>
        /// 得到当前用户拥有的店铺的数组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string [] GetShopAllocation(string userid);
        //List<model.users> getUsers(IDataParameter[] ipara, string procName);
         /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        string UserRegistered(Dictionary<string, string> Dic);
        /// <summary>
        /// 用户修改
        /// </summary>
        /// <returns></returns>
        string UpdateUserInfo(Dictionary<string, string> Dic);
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <returns></returns>
        string CheckUserName(string UserName);
        /// <summary>
        /// 获取客户端用户信息
        /// </summary>
        DataTable GetClientUsersTable(string UserName, string Vencode, int page, int Selpages, out string counts);
        /// <summary>
        /// 修改客户端用户信息
        /// </summary>
        string UpdateClientUsers(string UserName, string PassWord, string Vencode, string Email);
        /// <summary>
        /// 添加客户端用户信息
        /// </summary>
        string AddClientUsers(string UserName, string PassWord, string Vencode, string Email);
        /// <summary>
        /// 删除客户端用户
        /// </summary>
        string DeleteClientUsers(string UserName);
    }
}
