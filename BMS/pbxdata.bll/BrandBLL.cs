using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.dal;
using System.Data;
using System.Data.SqlClient;
namespace pbxdata.bll
{
    public partial class BrandBLL
    {
        BrandDAL bd = new BrandDAL();
        /// <summary>
        /// 查出品牌表所有信息  用于绑定
        /// </summary>
        /// <returns></returns>
        public List<BrandModel> SelectBrand()
        {
            
            return bd.SelectBrand();
        }
        /// <summary>
        /// 查询所有品牌
        /// </summary>
        /// <returns></returns>
        public List<BrandModel> SelectAllBrand()
        {
            return bd.SelectAllBrand();
        }
        /// <summary>
        /// 通过首字母查询
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBrandByChar(string fristchar)
        {
            return bd.SelectBrandByChar(fristchar);
        }
        /// <summary>
        /// 首字母不为A-Z的品牌
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandEles()
        {
            return bd.GetBrandEles();
        }
        /// <summary>
        /// 查询当前角色是否已有权限
        /// </summary>
        /// <param name="PersonaId"></param>
        /// <returns></returns>
        public bool BrandConfigIsExist(string PersonaId)
        {
            return bd.BrandConfigIsExist(PersonaId);
        }
        /// <summary>
        /// 如果当前角色已有权限 则修改权限
        /// </summary>
        /// <param name="personaId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool UpdateBrandConfig(string personaId, string BrandId)
        {
            return bd.UpdateBrandConfig(personaId,BrandId);
        }
        /// <summary>
        /// 如果当前角色没有拥有权限则添加权限
        /// </summary>
        /// <param name="personaId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool InsertBrandConfig(string personaId, string BrandId,string brandname,string vencode)
        {
            return bd.InsertBrandConfig(personaId,BrandId,brandname,vencode);
        }
        /// <summary>
        /// 通过角色信息查查找角色权限
        /// </summary>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public DataTable GetQxByPersionId(string PersionId)
        {
            return bd.GetQxByPersionId(PersionId);
        }
        /// <summary>
        /// 查询当前用户是否已有配置
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool UserPerssionIsIn(string CustomerId)
        {
            return bd.UserPerssionIsIn(CustomerId);
        }
        /// <summary>
        /// 如果已有配置 则进行修改
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool UpdateUserPerssion(string CustomerId, string BrandId)
        {
            return bd.UpdateUserPerssion(CustomerId, BrandId);
        }
        /// <summary>
        /// 如果没有配置 则添加配置
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool InsertUserPerssion(string CustomerId, string BrandId, string brandname, string vencode)
        {
            return bd.InsertUserPerssion(CustomerId, BrandId, brandname, vencode);
        }
        /// <summary>
        /// 根据用户ID查找用户拥有的权限
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public DataTable UesrPerssionByUserId(string CustomerId)
        {
            return bd.UesrPerssionByUserId(CustomerId);
        }

        
        /***********品牌权限 新增************/
        /// <summary>
        /// ---------清除当前角色的品牌权限
        /// </summary>
        /// <param name="uerid"></param>
        public void ClearBrandConfigByPersonaId(string userid,string vencode)
        {
            bd.ClearBrandConfigByPersonaId(userid,vencode);
        }
                /// <summary>
        /// ---------获得 当前角色的权限
        /// </summary>
        /// <returns></returns>
        public string[] GetBrandConfigByPersonaId(string userid) 
        {
            return bd.GetBrandConfigByPersonaId(userid);
        }
                /// <summary>
        /// -------获得当前用户的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetBrandConfigPersionByUserId(string userid) 
        {
            return bd.GetBrandConfigPersionByUserId(userid);
        }
                /// <summary>
        /// ------清除当前用户的权限
        /// </summary>
        /// <param name="userid"></param>
        public void ClearBrandConfigPersionByUserId(string userid, string vencode) 
        {
            bd.ClearBrandConfigPersionByUserId(userid, vencode);
        }

        /// <summary>
        /// 获取供应商品牌表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBrandVen(Dictionary<string, string> dic, int page, int Selpages, out string counts)
        {
            return bd.SearchBrandVen(dic, page, Selpages, out counts);
        }
        /// <summary>
        /// 更新供应商品牌表
        /// </summary>
        /// <returns></returns>
        public string UpdateBrandVen(Dictionary<string, string> dic, int UserId)
        {
            return bd.UpdateBrandVen(dic, UserId);
        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string deleteBrandVen(string Id, int UserId)
        {
            return bd.deleteBrandVen(Id, UserId);
        }
        /// <summary>
        /// 更新品牌对应的淘宝编号
        /// </summary>
        public string UpdateBrand(string BrandName, string BrandAbridge, string Vid,string Def2, int UserId)
        {
            return bd.UpdateBrand(BrandName, BrandAbridge, Vid,Def2, UserId);
        }
        /// <summary>
        /// 添加品牌
        /// </summary>
        public string AddBrand(string BrandName, string BrandAbridge, string Def2, int UserId)
        {
            return bd.AddBrand(BrandName, BrandAbridge,Def2, UserId);
        }
        /// <summary>
        /// 删除品牌
        /// </summary>
        public string DeleteBrand(string BrandName, string BrandAbridge,int UserId)
        {
            return bd.DeleteBrand(BrandName, BrandAbridge, UserId);
        }
        /// <summary>
        /// 获取淘宝品牌下拉表
        /// </summary>
        public DataTable GetTBbrandlist(string TBBrandName)
        {
            return bd.GetTBbrandlist(TBBrandName);
        }
        /// <summary>
        /// 更新淘宝品牌
        /// </summary>
        public string UpdateTBBrand(string TBBrandName, string vid, string Id, int UserId)
        {
            return bd.UpdateTBBrand(TBBrandName, vid, Id, UserId);
        }
        /// <summary>
        /// 添加淘宝品牌
        /// </summary>
        public string AddTBBrand(string TBBrandName, string vid, int UserId)
        {
            return bd.AddTBBrand(TBBrandName, vid, UserId);
        }
        /// <summary>
        /// 供应商类别搜索
        /// </summary>
        public DataTable SearchBrandDDlist(string BrandName)
        {
            return bd.SearchBrandDDlist(BrandName);
        }
        //******************7.6
        /// <summary>
        /// 根据excel内容添加对应的品牌
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string InsertBrand(string[] str) 
        {
            return bd.InsertBrand(str);
        }
        /// <summary>
        /// 得到所有excel的对应品牌数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetBrandExcel(string[] str, int minid, int maxid)
        {
            return bd.GetBrandExcel(str, minid, maxid);
        }
        /// <summary>
        /// 得到所有excel的对应品牌数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetBrandExcelCount(string[] str)
        {
            return bd.GetBrandExcelCount(str);
        }
        /// <summary>
        /// ---删除EXCEL对应关系的品牌
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteBrandExcel(string Id)
        {
            return bd.DeleteBrandExcel(Id);
        }

              /***********品牌权限   新修*********/
        /// <summary>
        /// 跟据品牌缩写和数据源得到当前数据源全称
        /// </summary>
        /// <param name="BrandSx"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string GetBrandNameVencode(string BrandSx, string vencode)
        {
            return bd.GetBrandNameVencode(BrandSx, vencode);
        }
                /// <summary>
        /// 得到角色当前数据源的权限品牌
        /// </summary>
        /// <param name="Rrole"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandName(string Role, string vencode, string ZfChar) 
        {
            return bd.GetUserBrandName(Role, vencode, ZfChar);
        }
        /// <summary>
        /// 得到角色权限
        /// </summary>
        /// <param name="role">角色Id</param>
        /// <param name="vencode">数据源编号</param>
        /// <param name="ZfChar">A-Z首字母</param>
        /// <returns></returns>
        public List<BrandConfig> GetAllUserBrand(string role, string vencode, string ZfChar)
        {
            return bd.GetAllUserBrand(role, vencode, ZfChar);
        }
                /// <summary>
        /// 得到角色权限  所有品牌
        /// </summary>
        /// <param name="role"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<BrandConfig> GetAllUserBrand(string role, string vencode)
        {
            return bd.GetAllUserBrand(role, vencode);
        }
                /// <summary>
        /// 得到用户已配置权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandSx(string userId, string vencode) 
        {
            return bd.GetUserBrandSx(userId, vencode);
        }
        /// <summary>
        /// 得到角色当前数据源的权限所有品牌
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandName(string Role, string vencode)
        {
            return bd.GetUserBrandName(Role, vencode);
        }
    }
}
