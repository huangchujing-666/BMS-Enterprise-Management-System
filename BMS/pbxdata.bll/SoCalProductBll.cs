using pbxdata.dal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class SoCalProductBll
    {
        SoCalProductDal scd = new SoCalProductDal();
        /// <summary>
        /// 得到所有现货数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSoCalProduct(string[] str, int minid, int maxid)
        {
            return scd.GetSoCalProduct(str, minid, maxid);
        }
        /// <summary>
        /// 得到现货的数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalProductCount(string[] str)
        {
            return scd.GetSoCalProductCount(str);
        }
        /// <summary>
        /// 保存选货
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertSoCaloutsideProduct(DataTable dt)
        {
            return scd.InsertSoCaloutsideProduct(dt);
        }
        /// <summary>
        /// ------按照货号和店铺查找当前现货是否已经 选货
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="ShopId"></param>
        /// <returns></returns>
        public bool GetSoCalByScodeAndShopId(string Scode, string ShopId)
        {
            return scd.GetSoCalByScodeAndShopId(Scode, ShopId);
        }
        /// <summary>
        /// ---------通过货号和数据源查找出当前现货数据的信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencod"></param>
        /// <returns></returns>
        public DataTable GetSoCalProductByScodeAndVencode(string scode, string vencode)
        {
            return scd.GetSoCalProductByScodeAndVencode(scode, vencode);
        }
        /// <summary>
        /// ---------通过货号和数据源查找出当前现货数据的信息--查看属性
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencod"></param>
        /// <returns></returns>
        public DataTable GetSoCaloutsideProductScodeAndVencode(string scode, string vencode)
        {
            return scd.GetSoCaloutsideProductScodeAndVencode(scode, vencode);
        }
        /// <summary>
        /// -----现货 选货  通过货号查询出当前货号已存在的店铺
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetShopByScodeSoCal(string Scode, string UserID)
        {
            return scd.GetShopByScodeSoCal(Scode, UserID);
        }

        /// <summary>
        /// ----现货店铺仓库库存
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalWarehouseByStyle(string[] str, int minid, int maxid)
        {
            return scd.GetSoCalWarehouseByStyle(str, minid, maxid);
        }
        /// <summary>
        /// ----现货店铺仓库库存个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalWarehouseByStyleCount(string[] str)
        {
            return scd.GetSoCalWarehouseByStyleCount(str);
        }
        /// <summary>
        ///------现货  货号查询仓库
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalWarehouseBySocde(string[] str, int minid, int maxid)
        {
            return scd.GetSoCalWarehouseBySocde(str, minid, maxid);
        }
        /// <summary>
        /// ------现货  货号查询仓库总数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalWarehouseBySocdeCount(string[] str)
        {
            return scd.GetSoCalWarehouseBySocdeCount(str);
        }
        //----------------------------------5.26日修改
        /// <summary>
        ///  ----得到现货库存中当前货号已分配的库存--修改库存
        /// </summary>
        /// <returns></returns>
        public int GetSocalSumBalanceByScode(string Scode)
        {
            return scd.GetSocalSumBalanceByScode(Scode);
        }
        /// <summary>
        /// -----得到当前现货的总库存
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public int GetSoCalProductSumBalanceByScode(string Scode)
        {
            return scd.GetSoCalProductSumBalanceByScode(Scode);
        }
        /// <summary>
        /// -----------显示现货   所有店铺货号库存情况
        /// </summary>
        /// <returns></returns>
        public DataTable GetSoCalShopShowByVencodeAndScode(int minid, int maxid, string scode, string vencode)
        {
            return scd.GetSoCalShopShowByVencodeAndScode(minid, maxid, scode, vencode);
        }
        /// <summary>
        /// --------------显示外部库存 现货库存个数   所有店铺货号库存情况
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int GetSoCalShopShowByVencodeAndScodeCount(string scode, string vencode)
        {
            return scd.GetSoCalShopShowByVencodeAndScodeCount(scode, vencode);
        }
        /// <summary>
        /// ---获得现货店铺中 当前货号当前店铺的货号的库存
        /// </summary>
        /// <param name="socde"></param>
        /// <param name="shopid"></param>
        public int GetSoCalProductBalanceCountByScodeAndShopId(string scode, string shopid, string vencode)
        {
            return scd.GetSoCalProductBalanceCountByScodeAndShopId(scode, shopid, vencode);
        }
        /// <summary>
        /// 获得当前店铺 当前货号的所有信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable GetSoCalProductShowStateByScodeAndShopId(string scode, string shopid, string vencode)
        {
            return scd.GetSoCalProductShowStateByScodeAndShopId(scode, shopid, vencode);
        }
        /// <summary>
        ///  ----修改当前店铺当前货号的库存
        /// </summary>
        /// <param name="BalanceState"></param>
        /// <param name="Balance"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTiem"></param>
        /// <param name="Loc"></param>
        /// <param name="Vencode"></param>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public bool UpdateSoCalBalanceByScode(string BalanceState, string Balance, string UpdateUser, string UpdateTime, string Loc, string Vencode, string Scode)
        {
            return scd.UpdateSoCalBalanceByScode(BalanceState, Balance, UpdateUser, UpdateTime, Loc, Vencode, Scode);
        }
        /// <summary>
        /// ---改变当前店铺当前货号的销售状态
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="ShowState"></param>
        /// <param name="Loc"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTime"></param>
        /// <returns></returns>
        public bool SalesState(string Scode, string ShowState, string Loc, string UpdateUser, string UpdateTime)
        {
            return scd.SalesState(Scode, ShowState, Loc, UpdateUser, UpdateTime);
        }
        /// <summary>
        /// -------修改通过货号查找的款号下价格相同的价格]
        /// </summary>
        /// <returns></returns>
        public bool UpdateSoCalPriceByStyleAll(string style, string shopid, string price, string updateprice)
        {
            return scd.UpdateSoCalPriceByStyleAll(style, shopid, price, updateprice);
        }
        /// <summary>
        /// ----修改当前店铺当前货号的价格
        /// </summary>
        /// <param name="BalanceState"></param>
        /// <param name="Balance"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTime"></param>
        /// <param name="Loc"></param>
        /// <param name="Vencode"></param>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public bool UpdateSoCalPriceByScode(string PriceState, string Price, string UpdateUser, string UpdateTime, string Loc, string Vencode, string Scode)
        {
            return scd.UpdateSoCalPriceByScode(PriceState, Price, UpdateUser, UpdateTime, Loc, Vencode, Scode);
        }
        /// <summary>
        /// ----通过款号得到当前店铺中当前款号下有多少个货号
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetSoCalScodeByStyle(string style, string shopid)
        {
            return scd.GetSoCalScodeByStyle(style, shopid);
        }
        //---------------------------------5.29日修改
        /// <summary>
        ///  -----查看商品属性时可以修改信息
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="descript"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public bool UpdateSoCaloutsideProduct(string Scode, string descript, string shopid)
        {
            return scd.UpdateSoCaloutsideProduct(Scode, descript, shopid);
        }

        /// <summary>
        /// ----查询当前合作客户所有店铺可下单的货物
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetPlaceAnOrder(string[] str, int minid, int maxid)
        {
            return scd.GetPlaceAnOrder(str, minid, maxid);
        }
        /// <summary>
        /// ----查询当前合作客户所有店铺可下单的货物 个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetPlaceAnOrder(string[] str)
        {
            return scd.GetPlaceAnOrder(str);
        }

        //*********************************5.30
        /// <summary>
        /// ----下单时  获得单号信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetSoCaloutsideProductByScodeAndShopId(string scode, string shopid)
        {
            return scd.GetSoCaloutsideProductByScodeAndShopId(scode, shopid);
        }
        /// <summary>
        /// ---新增现货订单
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool InsertSoCalOrder(string[] str)
        {
            return scd.InsertSoCalOrder(str);
        }
        //******6.1
        /// <summary>
        /// 查看订单信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalOrder(string[] str, int minid, int maxid)
        {
            return scd.GetSoCalOrder(str, minid, maxid);
        }
        /// <summary>
        /// 查看订单信息--个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalOrder(string[] str)
        {
            return scd.GetSoCalOrder(str);
        }
        /// <summary>
        /// 订单详细信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalrOrderDetailed(string[] str, int minid, int maxid)
        {
            return scd.GetSoCalrOrderDetailed(str, minid, maxid);
        }
        /// <summary>
        /// 订单详细信息个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalrOrderDetailed(string[] str)
        {
            return scd.GetSoCalrOrderDetailed(str);
        }
        /// <summary>
        /// 订单确认
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="filed">字段 客户确认字段或者供应商确认字段或者结算状态字段</param>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public bool OrderOkState(string state, string filed, string orderId)
        {
            return scd.OrderOkState(state, filed, orderId);
        }
        //*****************6.2
        /// <summary>
        ///   ---修改数量
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="OrderScode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public bool UpdateOrderSum(string OrderId, string OrderScode, string Num, string filed, string state)
        {
            return scd.UpdateOrderSum(OrderId, OrderScode, Num, filed, state);
        }
                /// <summary>
        /// ----通过货号和订单号查找当信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="OrderScode"></param>
        /// <returns></returns>
        public DataTable GetSoCalOrderByOrderIdAndScode(string OrderId, string OrderScode) 
        {
            return scd.GetSoCalOrderByOrderIdAndScode(OrderId, OrderScode);
        }
        //*********************************6.3
                /// <summary>
        /// 现货 导入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertSoCalProduct(DataTable dt)
        {
            return scd.InsertSoCalProduct(dt);
        }
        /// <summary>
        /// 当前数据是否存在
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="time"></param>
        /// <param name="Vencode"></param>
        /// <returns></returns>
        public bool SelectSoCalProductByTimeAndScodeAndVencode(string Scode, string time, string Vencode) 
        {
            return scd.SelectSoCalProductByTimeAndScodeAndVencode(Scode, time, Vencode);
        }
        /// <summary>
        /// --------导入数据时将将需要重复数据进行修改
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="time"></param>
        /// <param name="vencode"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool UpdateSoCalProductByFile(string scode, string time, string vencode, string balance) 
        {
            return scd.UpdateSoCalProductByFile(scode, time, vencode, balance);
        }
        //**************6.12
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable LookSoCalPrdouctAttr(string scode, string vencode)
        {
            return scd.LookSoCalPrdouctAttr(scode, vencode);
        }



        //******************.6.30库存汇总功能
        /// <summary>
        /// 库存汇总分页
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockMergeScodePage(string[] str, string customer)
        {
            return scd.ProductStockMergeScodePage(str, customer);
        }
        /// <summary>
        /// 库存汇总总数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockMergeScodeCount(string[] str, string customer)
        {
            return scd.ProductStockMergeScodeCount(str, customer);
        }
        /// <summary>
        /// 库存汇总分页 ---款号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockMergeStylePage(string[] str, string customer)
        {
            return scd.ProductStockMergeStylePage(str, customer);
        }
        /// <summary>
        /// 库存汇总总数 ---款号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockMergeStyleCount(string[] str, string customer)
        {
            return scd.ProductStockMergeStyleCount(str, customer);
        }
        /// <summary>
        /// 库存汇总---查看属性
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public DataTable GetdistinctProductStockByScod(string Scode)
        {
            return scd.GetdistinctProductStockByScod(Scode);
        }
    }
}
