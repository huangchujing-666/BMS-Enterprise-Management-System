using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.dal;
using System.Data;
namespace pbxdata.bll
{
    public class ProductTypeBLL
    {
        ProductTypeDAL Ptd = new ProductTypeDAL();
        public List<ProductTypeModel> GetProductType() 
        {
            return Ptd.GetProductType();
        }
        public List<ProductTypeModel> GetProductTypeReplace()
        {
            return Ptd.GetProductTypeReplace();
        }

        /// <summary>
        /// 得到所有的大类型   -----------fj
        /// </summary>
        /// <returns></returns>
        public DataTable GetBigType()
        {
            return Ptd.GetBigType();
        }
        /// <summary>
        /// 通过大类型Id获得小类型   -----------fj
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeByBigId(int BigId)
        {
            return Ptd.GetTypeByBigId(BigId);
        }

        /// <summary>
        /// 获得所有的角色
        /// </summary>
        /// <returns></returns>
        public DataTable GetPersona()
        {
            return Ptd.GetPersona();
        }
        /// <summary>
        /// 通过用户名查找当前用户所属角色
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable GetPersonaIdByUserName(string UserName)
        {
            return Ptd.GetPersonaIdByUserName(UserName);
        }
        /// <summary>
        /// 如果当前角色并无权限则添加
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public bool InsertPersion(string typeId, string PersionId,string BigType,string vencode)
        {
            return Ptd.InsertPersion(typeId,PersionId,BigType,vencode);
        }
        /// <summary>
        /// 通过当前角色编号获得权限
        /// </summary>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public DataTable GetQxByPersonaId(string PersionId)
        {
            return Ptd.GetQxByPersonaId(PersionId);
        }


        /// <summary>
        /// 查询用户配置表中当前用户是否已经添加配置
        /// </summary>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        /// <summary>
        /// 如果还没有配置 则添加配置
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool InsertPersonaTypeConfit(string personalId, string typeId,string vencode)
        {
            return Ptd.InsertPersonaTypeConfit(personalId, typeId,vencode);
        }
        /// <summary>
        /// 通过用户名查找用户的编号
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SelectUserIdByUserName(string userName)
        {
            return Ptd.SelectUserIdByUserName(userName);
        }

        /// <summary>
        /// 通过用户Id 查询出当前用户想看的权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable SelectConfigByUserId(string userName)
        {
            return Ptd.SelectConfigByUserId(userName);
        }
        /// <summary>
        /// 通过BigId找出当前的所有信息
        /// </summary>
        /// <param name="bigid"></param>
        /// <returns></returns>
        public DataTable GetBigIdAllByBigId(string bigid)
        {
            return Ptd.GetBigIdAllByBigId(bigid);
        }
        //通过小类型编号  获得小类型信息
        public DataTable GetTypeIdByTypeNo(string typeno)
        {
            return Ptd.GetTypeIdByTypeNo(typeno);
        }
        /// <summary>
        /// //----------通过角色Id查找角色名称
        /// </summary>
        /// <returns></returns>
        public string GetPersionNameByid(string persionId)
        {
            return Ptd.GetPersionNameByid(persionId);
        }

        //***********类别配置 新增************//
        public bool ClearTypeConfigByUserId(string persionid,string table,string vencode) 
        {
            return Ptd.ClearTypeConfigByUserId(persionid, table,vencode);
        }
         /// <summary>
        /// 通过类别名称找到大类别编号
        /// </summary>
        /// <param name="TypeNo"></param>
        /// <returns></returns>
        public string GetBigByTypeNo(string TypeNo) 
        {
            return Ptd.GetBigByTypeNo(TypeNo);
        }
                /// <summary>
        /// 当前用户的所有大类别权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetPerssionBigId(string userid) 
        {
            return Ptd.GetPerssionBigId(userid);
        }
        //获得当前用户的小类别权限
        public DataTable GetPerssionTypeNo(string userid, string bigid) 
        {
            return Ptd.GetPerssionTypeNo(userid,bigid);
        }
                /// <summary>
        /// -------当前用户是否已经给自己配置权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable ThisUserIsPersion(string userid) 
        {
            return Ptd.ThisUserIsPersion(userid);
        }
                /// <summary>
        /// 通过大权限ID 获得小权限Id
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bigid"></param>
        /// <returns></returns>
        public DataTable GetQxTypeIdByQxBigId(string userid, string bigid) 
        {
            return Ptd.GetQxTypeIdByQxBigId(userid, bigid);
        }
                /// <summary>
        /////---------获得当前用户的小类别权限
        /// </summary>
        public string[] GetThisUserTypeId(string userid) 
        {
            return Ptd.GetThisUserTypeId(userid);
        }
                /// <summary>
        /// 清除用户的个人配置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ClearUserTypeByUserId(string userid,string vencode) 
        {
            return Ptd.ClearUserTypeByUserId(userid,vencode);
        }


        //******************类别 新增    2015.8.24******************///
        /// <summary>
        /// 得到当前角色在某个供应商的类别权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="vencode">供应商编号</param>
        /// <param name="BigType">out 返回大类别数据</param>
        /// <returns>返回小类别数组</returns>
        public string[] GetTypePerssoin(string roleId, string vencode, out string[] BigType)
        {
            return Ptd.GetTypePerssoin(roleId, vencode,out BigType);
        }
        /// <summary>
        /// 得到用户自己配置的类别权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetTypeUser(string userId, string vencode) 
        {
            return Ptd.GetTypeUser(userId, vencode);
        }
                /// <summary>
        /// 得到角色类别
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string[] GetBrandRole(string vencode, string roleId) 
        {
            return Ptd.GetBrandRole(vencode, roleId);
        }
    }
}
