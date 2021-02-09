/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       users
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       users
    * 创建时间：       2015/2/9 16:22:40
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
    public class usersbll : dataoperatingbll
    {
        iusers dal = (iusers)ReflectFactory.CreateIDataOperatingByReflect("usersdal");

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="ipara"></param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public List<model.users> GetModel(IDataParameter[] ipara, string procName)
        {
            return dal.GetModel(ipara, procName);
        }

        /// <summary>
        /// 根据权限字段组成sql语句返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<model.users> getUserData(string sql)
        {
            return dal.getUserData(sql);
        }


        /// <summary>
        /// 根据权限字段组成sql语句返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable getData()
        {
            return dal.getData();
        }


        /// <summary>
        /// 根据权限字段组成sql语句返回集合(可带条件查询)
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public DataTable getData(Dictionary<string, string> dic = null)
        {
            return dal.getData(dic);
        }

        /// <summary>
        /// 编辑-根据ID获取信息
        /// </summary>
        /// <returns></returns>
        public DataTable getDataEdit(int id)
        {
            return dal.getDataEdit(id);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public string del(int id)
        {
            return dal.del(id);
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public string UserRegistered(Dictionary<string, string> Dic)
        {
            return dal.UserRegistered(Dic);
        }
        /// <summary>
        /// 用户修改
        /// </summary>
        /// <returns></returns>
        public string UpdateUserInfo(Dictionary<string, string> Dic)
        {
            return dal.UpdateUserInfo(Dic);
        }
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <returns></returns>
        public string CheckUserName(string UserName)
        {
            return dal.CheckUserName(UserName);
        }
        /// <summary>
        /// 分配店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string InsertShopAllocation(string shopid, string userid)
        {
            usersdal udal = new usersdal();
            return udal.InsertShopAllocation(shopid, userid);
        }
        /// <summary>
        /// 当前用户是否已经分配当前店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ShopIsAllocation(string shopid, string userid)
        {
            usersdal udal = new usersdal();
            return udal.ShopIsAllocation(shopid, userid);
        }
        /// <summary>
        /// 得到当前用户拥有的店铺的数组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetShopAllocation(string userid)
        {
            usersdal udal = new usersdal();
            return udal.GetShopAllocation(userid);
        }
        /// <summary>
        /// 获取客户端用户信息
        /// </summary>
        public DataTable GetClientUsersTable(string UserName, string Vencode, int page, int Selpages, out string counts)
        {
            return dal.GetClientUsersTable(UserName, Vencode, page, Selpages, out counts);
        }
        /// <summary>
        /// 修改客户端用户信息
        /// </summary>
        public string UpdateClientUsers(string UserName, string PassWord, string Vencode, string Email)
        {
            return dal.UpdateClientUsers(UserName, PassWord, Vencode, Email);
        }
        /// <summary>
        /// 添加客户端用户信息
        /// </summary>
        public string AddClientUsers(string UserName, string PassWord, string Vencode, string Email)
        {
            return dal.AddClientUsers(UserName, PassWord, Vencode, Email);
        }
        /// <summary>
        /// 删除客户端用户
        /// </summary>
        public string DeleteClientUsers(string UserName)
        {
            return dal.DeleteClientUsers(UserName);
        }
    }
}
