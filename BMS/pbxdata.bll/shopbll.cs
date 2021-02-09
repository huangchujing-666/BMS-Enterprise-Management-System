using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.dal;
using System.Data;
using pbxdata.model;
namespace pbxdata.bll
{
    public class shopbll
    {
        Shopdal sd = new Shopdal();
        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllShop(int minid, int maxid, string shopname, string shoptypeid, string managerid, out int count)
        {
            return sd.SelectAllShop(minid, maxid, shopname, shoptypeid, managerid, out count);
        }
        /// <summary>
        /// 查询店铺中是否有数据
        /// </summary>
        /// <returns></returns>
        public bool SelectShopIdIsIn(string id)
        {
            return sd.SelectShopIdIsIn(id);
        }
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteShop(string id)
        {
            return sd.DeleteShop(id);
        }
        /// <summary>
        /// 绑定店铺类型
        /// </summary>
        /// <returns></returns>
        public List<shoptype> BindShopType()
        {
            return sd.BindShopType();
        }
        /// <summary>
        /// 绑定店铺管理人
        /// </summary>
        /// <returns></returns>
        public List<users> BindUserName()
        {
            return sd.BindUserName();
        }
        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <param name="shopname">店铺名称</param>
        /// <param name="shoptypeid">店铺类型ID</param>
        /// <param name="shopstate">店铺状态</param>
        /// <param name="shopmanager">d店铺管理人Id</param>
        /// <returns></returns>
        public string InsertShop(string shopname, string shoptypeid, string shopstate, string shopmanager, string telphone)
        {
            return sd.InsertShop(shopname, shoptypeid, shopstate, shopmanager, telphone);
        }
        /// <summary>
        /// 店铺名称是否已存在
        /// </summary>
        /// <param name="shopname"></param>
        /// <returns></returns>
        public bool ShopNameIsOn(string shopname)
        {
            return sd.ShopNameIsOn(shopname);
        }
        /// <summary>
        /// 修改店铺状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string UpdateShopState(string state, string id)
        {
            return sd.UpdateShopState(state, id);
        }
        /// <summary>
        /// 店铺当前状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ShopState(string id)
        {
            return sd.ShopState(id);
        }
        /// <summary>
        /// 修改店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shopname"></param>
        /// <param name="shoptyppe"></param>
        /// <param name="shopmanager"></param>
        /// <param name="shopstate"></param>
        /// <returns></returns>
        public string UpdateShop(string id, string shopname, string shoptyppe, string shopmanager, string shopstate)
        {
            return sd.UpdateShop(id, shopname, shoptyppe, shopmanager, shopstate);
        }
        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable DropListShop()
        {
            return sd.DropListShop();
        }
        /*------------------------------------------外部店铺----------------------------------------------*/
        /// <summary>
        /// 查询所有合作店铺
        /// </summary>
        /// <returns></returns>
        public DataTable SelectCollaborationShop(string userId)
        {
            return sd.SelectCollaborationShop(userId);
        }
        /// <summary>
        /// 店铺仓库库存
        /// </summary>
        /// <returns></returns>
        public DataTable SelectWarehouseByShop(string[] str, int minid, int maxid)
        {
            return sd.SelectWarehouseByShop(str, minid, maxid);
        }
        /// <summary>
        /// 获取当前店铺所有款号总数
        /// 
        /// </summary>
        /// <returns></returns>
        public int SelectWarehouseByShopCount(string[] str)
        {
            return sd.SelectWarehouseByShopCount(str);
        }
        /// <summary>
        /// 店铺仓库库存  按照货号
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable SelectWarehouseByScode(string[] str, int minid, int maxid)
        {
            return sd.SelectWarehouseByScode(str, minid, maxid);
        }
        /// <summary>
        /// 获取当前店铺所有商品总数
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int SelectWarehouseByScodeCount(string[] str)
        {
            return sd.SelectWarehouseByScodeCount(str);
        }
        /// <summary>
        /// 通过用户Id查找出用户名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserRealNameByUserId(string userid)
        {
            return sd.GetUserRealNameByUserId(userid);
        }
        /// <summary>
        /// 开放销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string OpenSalesByStyle(string style, string shopid, string showstate, string time, string userid)
        {
            return sd.OpenSalesByStyle(style, shopid, showstate, time, userid);
        }
        /// <summary>
        /// 取消销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string CancelSalesByStyle(string style, string shopid)
        {
            return sd.CancelSalesByStyle(style, shopid);
        }
        /// <summary>
        /// 当前店铺   通过款号删除
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string DeleteByStyleAndShopid(string style, string shopid)
        {
            return sd.DeleteByStyleAndShopid(style, shopid);
        }
        /// <summary>
        /// 当前店铺   通过货号删除
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string DeletebyScodeAndShopid(string scode, string shopid)
        {
            return sd.DeletebyScodeAndShopid(scode, shopid);
        }
        /// <summary>
        /// 查找当前店铺当前货号的库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public int GetBalanceByScodeAndShopid(string scode, string shopid, string vencode)
        {
            return sd.GetBalanceByScodeAndShopid(scode, shopid, vencode);
        }
        /// <summary>
        /// 修改当前店铺库存
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源编号</param>
        /// <param name="balance">库存</param>
        /// <returns></returns>
        public string UpdateBalanceByAndShopIdVencode(string scode, string shopid, string vencode, string balance, string showstate, string time, string userid)
        {
            return sd.UpdateBalanceByAndShopIdVencode(scode, shopid, vencode, balance, showstate, time, userid);
        }
        /// <summary>
        /// 查找当前店铺下某货号的价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetPriceByScodeandShopid(string scode, string shopid)
        {
            return sd.GetPriceByScodeandShopid(scode, shopid);
        }
        /// <summary>
        /// //---通过款号和店铺查询当前款号有多少条货号
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetOutSideByStyleAndShopId(string style, string shopid)
        {
            return sd.GetOutSideByStyleAndShopId(style, shopid);
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="price"></param>
        /// <param name="updataprie"></param>
        /// <returns></returns>
        public string UpdatePricecByStyleAndPrice(string style, string price, string updataprie, string shopid, string time, string userid)
        {
            return sd.UpdatePricecByStyleAndPrice(style, price, updataprie, shopid);
        }
        /// <summary>
        /// ---通过修改价格时修改状态 - --将当前的货号是选货状态改为未开放状态
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string UpdateShowStateByUpdatePrice(string Scode, string shopid, string time, string userid)
        {
            return sd.UpdateShowStateByUpdatePrice(Scode, shopid, time, userid);
        }
        /// <summary>
        /// 得到当前店铺当前货号的 是否已经修改价格或者库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetThisScodeState(string scode, string shopid)
        {
            return sd.GetThisScodeState(scode, shopid);
        }
        /// <summary>
        /// 查找该条货号属性
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源</param>
        /// <returns></returns>
        public DataTable GetShopProductByScodeAndShopIdVencode(string scode, string shopid, string vencode)
        {
            return sd.GetShopProductByScodeAndShopIdVencode(scode, shopid, vencode);
        }
        /// <summary>
        /// 查看货号属性
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable GetProductstockByScodeAndVencode(string scode, string vencode, string shopid)
        {
            return sd.GetProductstockByScodeAndVencode(scode, vencode, shopid);
        }

        /// <summary>
        /// 查看货号属性  选货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable LookAttrByCheck(string scode, string vencode)
        {
            return sd.LookAttrByCheck(scode, vencode);
        }
        /// <summary>
        /// 得到当前店铺该货号的状态
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string GetShowStateByScodeAndLoc(string scode, string shopid)
        {
            return sd.GetShowStateByScodeAndLoc(scode, shopid);
        }
        /// <summary>
        /// 通过角色Id获得当前角色的标识
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public string GetUserTypeByPersonaId(string RoleId)
        {
            return sd.GetUserTypeByPersonaId(RoleId);
        }
        //*******************选货*************/
        /// <summary>
        /// 通过当前登录用户获取当前的登录用户的合作店铺
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetShopNameByUserId(string userid)
        {
            return sd.GetShopNameByUserId(userid);
        }
        /// <summary>
        /// ---------选货  通过货号查询出当前货号已存在的店铺
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetShopByScode(string Scode, string user)
        {
            return sd.GetShopByScode(Scode, user);
        }
        /*************************分配店铺*********************/
        /// <summary>
        /// 得到所有店铺
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllShop()
        {
            return sd.GetAllShop();
        }
        /// <summary>
        /// 得到所有用户
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUsers()
        {
            return sd.GetAllUsers();
        }
        //*************************6.1
        /// <summary>
        /// ---清除店铺权限
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public bool ClearShopAllocationByManagerId(string managerId)
        {
            return sd.ClearShopAllocationByManagerId(managerId);
        }

        //********************6.5
        /// <summary>
        /// ---通过商品编号获淘宝数据信息
        /// </summary>
        /// <param name="RemarkId"></param>
        /// <returns></returns>
        public DataTable GetTbProductByRemarkId(string RemarkId)
        {
            return sd.GetTbProductByRemarkId(RemarkId);
        }


        /// <summary>
        /// 通过淘宝款号获得本地数据
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public DataTable GetProductByTbStyle(string style, string scode)
        {
            return sd.GetProductByTbStyle(style, scode);
        }

        /********更新淘宝SKU   2015年8月27日 10:13:15********/
        /// <summary>
        /// 得到所有淘宝商品编号
        /// </summary>
        /// <returns></returns>
        public List<string> GetTbProductRemarkId()
        {
            return sd.GetTbProductRemarkId();
        }
        /// <summary>
        /// 插入SKU信息
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool InserttbSkuScode(TbSkuScode ts)
        {
            return sd.InserttbSkuScode(ts);
        }


        /// <summary>
        /// skuId个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int SelectSkuId(string[] str)
        {
            return sd.SelectSkuId(str);
        }
        /// <summary>
        /// 商品SKU信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable SelectSkuId(string[] str, int minid, int maxid)
        {
            return sd.SelectSkuId(str, minid, maxid);
        }
    }
}
