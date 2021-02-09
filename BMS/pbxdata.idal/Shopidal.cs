using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.idal
{
    public interface Shopidal 
    {
        string connectionString { get; }
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        DataTable SelectAllShop(int minid, int maxid, string shopname, string shoptypeid, string managerid, out int count);
        /// <summary>
        /// 查询店铺中是否有商品
        /// </summary>
        /// <returns></returns>
        bool SelectShopIdIsIn(string id);
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <returns></returns>
        string DeleteShop(string id);
        /// <summary>
        /// 绑定下拉列表店铺类型
        /// </summary>
        /// <returns></returns>
        List<shoptype> BindShopType();
        /// <summary>
        /// 绑定下拉列表店铺管理人
        /// </summary>
        /// <returns></returns>
        List<users> BindUserName();
        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <returns></returns>
        string InsertShop(string shopname, string shoptypeid, string shopstate, string shopmanager,string telphone);
        /// <summary>
        /// 店铺名称是否已存在
        /// </summary>
        /// <param name="shopname"></param>
        /// <returns></returns>
        bool ShopNameIsOn(string shopname);
        /// <summary>
        /// 店铺状态
        /// </summary>
        /// <returns></returns>
        int ShopState(string id);
        /// <summary>
        /// 修改店铺状态
        /// </summary>
        /// <param name="state">状态值</param>
        /// <param name="id">需要修改的店铺Id</param>
        /// <returns></returns>
        string UpdateShopState(string state,string id);
        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <returns></returns>
        DataTable DropListShop();
        /// <summary>
        /// 查询合作商家
        /// </summary>
        /// <returns></returns>
        DataTable SelectCollaborationShop(string userId);
                /// <summary>
        /// 店铺仓库库存
        /// </summary>
        /// <returns></returns>
        DataTable SelectWarehouseByShop(string[] str,int minid,int maxid);
                /// <summary>
        /// 获取当前店铺所有款号总数
        /// 
        /// </summary>
        /// <returns></returns>
        int SelectWarehouseByShopCount(string[] str);
        /// <summary>
        /// 店铺仓库库存  按照货号
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        DataTable SelectWarehouseByScode(string[] str, int minid, int maxid);
                /// <summary>
        /// 获取当前店铺所有商品总数
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        int SelectWarehouseByScodeCount(string[] str);
        /// <summary>
        /// 通过用户Id查找出用户名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetUserRealNameByUserId(string userid);
        /// <summary>
        /// 开放销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string OpenSalesByStyle(string style, string shopid,string showstate,string time,string userid);
        /// <summary>
        /// 取消销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string CancelSalesByStyle(string style, string shopid);
        /// <summary>
        /// 当前店铺   通过款号删除
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string DeleteByStyleAndShopid(string style, string shopid);
        /// <summary>
        /// 当前店铺   通过货号删除
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string DeletebyScodeAndShopid(string scode, string shopid);
        /// <summary>
        /// 查找当前店铺当前货号的库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        int GetBalanceByScodeAndShopid(string scode, string shopid,string vencode);
        /// <summary>
        /// 修改当前店铺库存
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源编号</param>
        /// <param name="balance">库存</param>
        /// <returns></returns>
        string UpdateBalanceByAndShopIdVencode(string scode, string shopid, string vencode, string balance,string showstate,string time,string userid);
        /// <summary>
        /// 查找当前店铺下某货号的价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        DataTable GetPriceByScodeandShopid(string scode, string shopid);
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="price"></param>
        /// <param name="updataprie"></param>
        /// <returns></returns>
        string UpdatePricecByStyleAndPrice(string style, string price, string updataprie, string shopid);
        /// <summary>
        /// ---通过修改价格时修改状态
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string UpdateShowStateByUpdatePrice(string Scode, string shopid,string time,string userid);
        /// <summary>
        /// 查找该条货号属性
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源</param>
        /// <returns></returns>
        DataTable GetShopProductByScodeAndShopIdVencode(string scode,string shopid,string vencode);
        /// <summary>
        /// 得到当前用户所拥有的店铺
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetShopNameByUserId(string userid);
        /// <summary>
        /// ---------选货  通过货号查询出当前货号已存在的店铺
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetShopByScode(string Scode, string user);
        /// <summary>
        /// 得到当前店铺该货号的状态
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        string GetShowStateByScodeAndLoc(string scode, string shopid);
        /// <summary>
        /// //---通过款号和店铺查询当前款号有多少条货号
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        DataTable GetOutSideByStyleAndShopId(string style,string shopid);
        /// <summary>
        /// 得到当前店铺当前货号的 是否已经修改价格或者库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        DataTable GetThisScodeState(string scode, string shopid);
    }
}
