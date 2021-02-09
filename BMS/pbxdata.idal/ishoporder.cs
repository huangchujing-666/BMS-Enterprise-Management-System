/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ishoporder
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       ishoporder
    * 创建时间：       2015-04-01 09:15:28
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
    public interface ishoporder
    {
        /// <summary>
        /// 获取店铺订单总数
        /// </summary>
        /// <returns></returns>
        int getDataCount(string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2);

        /// <summary>
        /// 获取店铺订单数据(分页)
        /// </summary>
        /// <returns></returns>
        DataTable getData(int pageIndex, int pageSize,string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2);

        /// <summary>
        /// 添加店铺天猫订单数据
        /// </summary>
        /// <returns></returns>
        string addTmallOrder();

        /// <summary>
        /// 修改店铺天猫订单数据
        /// </summary>
        /// <returns></returns>
        string updateTmallOrder(string orderId, decimal orderPrice, DateTime orderTime, DateTime orderPayTime, DateTime orderEditTime, DateTime orderSendTime, DateTime orderSucessTime,int payState,int orderState,int orderState1);


        /// <summary>
        /// 修改店铺天猫订单数据(子订单)
        /// </summary>
        /// <returns></returns>
        string updateChildTmallOrder(string OrderChildenId, decimal DetailsPrice, DateTime orderSendTime, DateTime orderSucessTime);

        /// <summary>
        /// 获取某时间段店铺订单数据(根据时间获取)
        /// </summary>
        /// <returns></returns>
        DataTable getData(string date1, string date2);


        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是主订单)
        /// </summary>
        /// <returns>返回时间</returns>
        string getDataTime();


        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是子订单)
        /// </summary>
        /// <returns>返回时间</returns>
        string getChildDataTime();


        /// <summary>
        /// 获取某时间段的所有子订单ID
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        string[] getDataChildOrderId(string date1, string date2);

        /// <summary>
        /// 获取某时间段的所有物流订单ID
        /// </summary>
        /// <returns></returns>
        string[] getExpressOrderId();


        /// <summary>
        /// 更新物流订单运单号
        /// </summary>
        /// <returns></returns>
        string updateExpressNo(string sqlText);


        /// <summary>
        /// 获取订单备注，备注旗帜信息
        /// </summary>
        /// <returns></returns>
        string[] getCommentOrderRemarkId();


        /// <summary>
        /// 获取某段时间主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        string[] getOrderId(string date1,string date2);

        /// <summary>
        /// 获取某段时间某个店铺主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        string[] getShopOrderId(string date1, string date2, string shopName);


        /// <summary>
        /// 根据订单ID获取此商品相关信息
        /// </summary>
        /// <returns></returns>
        DataTable orderEdit(string orderchildenid);

        /// <summary>
        /// 根据订单ID更新此商品相关信息
        /// </summary>
        /// <returns></returns>
        string orderUpdate(string orderchildenid, Dictionary<string, string> Dic);

    }
}
