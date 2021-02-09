/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       isourceorder
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       isourceorder
    * 创建时间：       2015-05-04 14:20:45
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
    public interface isourceorder
    {
        /// <summary>
        /// 根据订单编号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DataTable getOrderSendData(string orderId);

        /// <summary>
        /// 根据订单编号、货号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DataTable getOrderSendData(string orderId, string scode);

        /// <summary>
        /// 获取源头订单
        /// </summary>
        /// <returns>返回table集合</returns>
        DataTable getData(string orderId);

        /// <summary>
        /// 获取源头订单总数量
        /// </summary>
        /// <returns></returns>
        int getDataCount();


        #region 拆单订单单个操作
        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        string updateShowOrderState(string orderId);


        /// <summary>
        /// 订单状态更改为待发货状态(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string updateSendOrderState(string orderId);


        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string cancelOrder(string orderId);


        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="o">实体连接</param>
        /// <returns></returns>
        string cancelOrder(string orderId, object o);

        #endregion

        #region 主订单批量操作
        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        string updateParentShowOrderState(string orderId);


        /// <summary>
        /// 订单状态更改为待发货状态(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string updateParentSendOrderState(string orderId);


        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string cancelParentOrder(string orderId);
        #endregion


        /// <summary>
        /// 获取拆单订单的主订单ID
        /// </summary>
        /// <returns></returns>
        DataTable getSourceParentOrderId(int pageIndex,int pageSize);


        /// <summary>
        /// 根据订单编号获取买家信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        model.apiOrder selectBuyer(string orderId);

        /// <summary>
        /// 根据子订单更新发货源头
        /// </summary>
        string UpdatesendSource(List<string> detailsOrderIdlist, List<string> sendSourcelist);


        //def2 nvarchar(20), --商检报关状态(0失败，1成功，2未上传)
        //def2 nvarchar(20), --海关报关状态(0失败，1成功，2未上传)
        //def3 nvarchar(20), --联邦报关状态(0失败，1成功，2未上传)
        /// <summary>
        /// 根据子订单更新商检订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        string UpdateSJStates(string orderChildId, string status);
        
        
        /// <summary>
        /// 根据子订单更新海关订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        string UpdateHGStates(string orderChildId,string status);


        /// <summary>
        /// 根据子订单更新联邦订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        string UpdateBBCStates(string orderChildId, string status);

         /// <summary>
        /// 获取拆单订单的主订单ID
        /// </summary>
        /// <returns></returns>
        DataTable getSourceParentOrderId(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

    }
}
