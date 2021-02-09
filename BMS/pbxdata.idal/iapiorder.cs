/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       iapiorder
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       iapiorder
    * 创建时间：       2015-04-20 13:37:27
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
    public interface iapiorder
    {
        /// <summary>
        /// 返回订单详情信息(根据主订单编号查询子订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="hg">海关</param>
        /// <returns></returns>
        List<model.apiOrderDetails> getOrderDetailsMsg(string orderId,string hg);


        /// <summary>
        /// 返回订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        model.apiOrder getOrderMsg(string orderId);


        /// <summary>
        /// 返回(批量)订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<model.apiOrder> getOrderMsg(string[] orderIds);


        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DataTable getOrderMsg(string orderId, string scode);

        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DataTable getOrderMsg(string orderId, string scode,object o);


        /// <summary>
        /// 返回所有API订单
        /// </summary>
        /// <returns></returns>
        DataTable getOrderMsg(int pageIndex, int pageSize);

        /// <summary>
        /// 返回所有APP订单
        /// </summary>
        /// <returns></returns>
        DataTable getOrderMsg(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 根据主订单获取子订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        DataTable getOrderDetailsMsg(string orderId);

        /// <summary>
        /// 订单是否已拆单
        /// </summary>
        /// <returns></returns>
        string getOrderSplit(string detailsOrderId);


        /// <summary>
        /// 获取API订单总条数
        /// </summary>
        /// <returns></returns>
        int getDataCount();

        /// <summary>
        /// 添加备注
        /// </summary>
        string UpdateApiOrder(Dictionary<string, string> Dic);

         /// <summary>
        /// 删除备注
        /// </summary>
        string DeleteRemark(string Id);

        /// <summary>
        /// 修改备注
        /// </summary>
        string EditRemark(Dictionary<string, string> Dic);
         
    }
}
