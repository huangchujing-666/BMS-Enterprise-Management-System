/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       shoporderbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       shoporderbll
    * 创建时间：       2015-04-01 09:22:22
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


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
    public class shoporderbll:dataoperatingbll
    {
        ishoporder dal = (ishoporder)ReflectFactory.CreateIDataOperatingByReflect("shoporderdal");

        /// <summary>
        /// 获取店铺订单数据
        /// </summary>
        /// <returns></returns>
        public int getDataCount(string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2)
        {
            return dal.getDataCount(orderId, scode, brand, type, status, shopname, servicecustom, buynick, price1, price2, pprice1, pprice2, ordertime1, ordertime2, paytime1, paytime2, sendtime1, sendtime2, sucesstime1, sucesstime2);
        }

        /// <summary>
        /// 获取店铺订单数据(分页)
        /// </summary>
        /// <returns></returns>
        public DataTable getData(int pageIndex, int pageSize, string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2)
        {
            return dal.getData(pageIndex, pageSize, orderId, scode, brand, type, status, shopname, servicecustom, buynick, price1, price2, pprice1, pprice2, ordertime1, ordertime2, paytime1, paytime2, sendtime1, sendtime2, sucesstime1, sucesstime2);
        }


        /// <summary>
        /// 添加店铺天猫订单数据
        /// </summary>
        /// <returns></returns>
        public  string addTmallOrder()
        {
            return dal.addTmallOrder();
        }

        /// <summary>
        /// 修改店铺天猫订单数据
        /// </summary>
        /// <returns></returns>
        public string updateTmallOrder(string orderId, decimal orderPrice, DateTime orderTime, DateTime orderPayTime, DateTime orderEditTime, DateTime orderSendTime, DateTime orderSucessTime, int payState, int orderState, int orderState1)
        {
            return dal.updateTmallOrder(orderId, orderPrice, orderTime, orderPayTime, orderEditTime, orderSendTime, orderSucessTime, payState, orderState, orderState1);
        }

        /// <summary>
        /// 获取某时间段店铺订单数据(根据时间获取)
        /// </summary>
        /// <returns></returns>
        public DataTable getData(string date1, string date2)
        {
            return dal.getData(date1, date2);
        }


        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是主订单)
        /// </summary>
        /// <returns>返回时间</returns>
        public string getDataTime()
        {
            return dal.getDataTime();
        }

        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是子订单)
        /// </summary>
        /// <returns>返回时间</returns>
        public string getChildDataTime()
        {
            return dal.getChildDataTime();
        }


        /// <summary>
        /// 获取某时间段的所有子订单ID
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public string[] getDataChildOrderId(string date1, string date2)
        {
            return dal.getDataChildOrderId(date1, date2);
        }

        /// <summary>
        /// 修改店铺天猫订单数据(子订单)
        /// </summary>
        /// <returns></returns>
        public string updateChildTmallOrder(string OrderChildenId, decimal DetailsPrice, DateTime orderSendTime, DateTime orderSucessTime)
        {
            return dal.updateChildTmallOrder(OrderChildenId, DetailsPrice, orderSendTime, orderSucessTime);
        }


        /// <summary>
        /// 获取某时间段的所有物流订单ID
        /// </summary>
        /// <returns></returns>
        public string[] getExpressOrderId()
        {
            return dal.getExpressOrderId();
        }


        /// <summary>
        /// 更新物流订单运单号
        /// </summary>
        /// <returns></returns>
        public string updateExpressNo(string sqlText)
        {
            return dal.updateExpressNo(sqlText);
        }


        /// <summary>
        /// 获取订单备注，备注旗帜信息
        /// </summary>
        /// <returns></returns>
        public string[] getCommentOrderRemarkId()
        {
            return dal.getCommentOrderRemarkId();
        }


        /// <summary>
        /// 获取某段时间主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public string[] getOrderId(string date1, string date2)
        {
            return dal.getOrderId(date1, date2);
        }

        /// <summary>
        /// 获取某段时间某个店铺主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        public string[] getShopOrderId(string date1, string date2, string shopName)
        {
            return dal.getShopOrderId(date1, date2, shopName);
        }


        /// <summary>
        /// 根据子订单ID获取此商品相关信息
        /// </summary>
        /// <returns></returns>
        public DataTable orderEdit(string orderchildenid)
        {
            return dal.orderEdit(orderchildenid);
        }
         /// <summary>
        /// 根据子订单ID更新此商品相关信息
        /// </summary>
        /// <returns></returns>
        public string orderUpdate(string orderchildenid,Dictionary<string,string> Dic)
        {

            return dal.orderUpdate(orderchildenid,Dic);
        }
        

    }
}
