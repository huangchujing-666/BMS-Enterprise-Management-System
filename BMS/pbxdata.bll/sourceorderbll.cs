/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       sourceorderbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       sourceorderbll
    * 创建时间：       2015-05-04 14:23:50
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
    public class sourceorderbll : dataoperatingbll
    {
        isourceorder dal = (isourceorder)ReflectFactory.CreateIDataOperatingByReflect("sourceorderdal");


        /// <summary>
        /// 根据订单编号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderSendData(string orderId)
        {
            return dal.getOrderSendData(orderId);
        }


        /// <summary>
        /// 根据订单编号、货号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderSendData(string orderId, string scode)
        {
            return dal.getOrderSendData(orderId, scode);
        }


        /// <summary>
        /// 获取源头订单
        /// </summary>
        /// <returns>返回table集合</returns>
        public DataTable getData(string orderId)
        {
            return dal.getData(orderId);
        }

        /// <summary>
        /// 获取源头订单总数量
        /// </summary>
        /// <returns></returns>
        public int getDataCount()
        {
            return dal.getDataCount();
        }


        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        public string updateShowOrderState(string orderId)
        {
            return dal.updateShowOrderState(orderId);
        }


        /// <summary>
        /// 订单状态更改为待发货状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string updateSendOrderState(string orderId)
        {
            return dal.updateSendOrderState(orderId);
        }


        /// <summary>
        /// 取消订单(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string cancelOrder(string orderId)
        {
            return dal.cancelOrder(orderId);
        }


        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="o">实体连接</param>
        /// <returns></returns>
        public string cancelOrder(string orderId, object o)
        {
            return dal.cancelOrder(orderId, o);
        }


        #region 主订单批量操作
        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        public string updateParentShowOrderState(string orderId)
        {
            return dal.updateParentShowOrderState(orderId);
        }


        /// <summary>
        /// 订单状态更改为待发货状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string updateParentSendOrderState(string orderId)
        {
            return dal.updateParentSendOrderState(orderId);
        }

        /// <summary>
        /// 取消订单(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string cancelParentOrder(string orderId)
        {
            return dal.cancelParentOrder(orderId);
        }

        #endregion


        /// <summary>
        /// 获取拆单订单的主订单ID，主订单状态
        /// </summary>
        /// <returns></returns>
        public DataTable getSourceParentOrderId(int pageIndex, int pageSize)
        {
            return dal.getSourceParentOrderId(pageIndex,pageSize);
        }

         /// <summary>
        /// 获取拆单订单的主订单ID
        /// </summary>
        /// <returns></returns>
        public DataTable getSourceParentOrderId(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.getSourceParentOrderId(Dic, pageIndex, pageSize,out counts);
        }
        /// <summary>
        /// 根据订单编号获取买家信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public model.apiOrder selectBuyer(string orderId)
        {
            return dal.selectBuyer(orderId);
        }
        /// <summary>
        /// 根据子订单更新发货源头
        /// </summary>
        public string UpdatesendSource(List<string> detailsOrderIdlist, List<string> sendSourcelist)
        {
            return dal.UpdatesendSource(detailsOrderIdlist, sendSourcelist);
        }

        //def2 nvarchar(20), --商检报关状态(0失败，1成功，2未上传)
        //def2 nvarchar(20), --海关报关状态(0失败，1成功，2未上传)
        //def3 nvarchar(20), --联邦报关状态(0失败，1成功，2未上传)
        /// <summary>
        /// 根据子订单更新商检订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateSJStates(string orderChildId, string status)
        {
            return dal.UpdateSJStates(orderChildId, status);
        }


        /// <summary>
        /// 根据子订单更新海关订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateHGStates(string orderChildId, string status)
        {
            return dal.UpdateHGStates(orderChildId, status);
        }


        /// <summary>
        /// 根据子订单更新联邦订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateBBCStates(string orderChildId, string status)
        {
            return dal.UpdateBBCStates(orderChildId, status);
        }


    }
}
