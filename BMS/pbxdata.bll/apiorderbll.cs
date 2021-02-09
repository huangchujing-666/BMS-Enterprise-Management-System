/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       apiorderbll
    * 创建时间：       2015-04-20 13:38:47
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class apiorderbll:dataoperatingbll
    {

        iapiorder dal = (iapiorder)ReflectFactory.CreateIDataOperatingByReflect("apiorderdal");


        /// <summary>
        /// 返回订单详情信息(根据主订单编号查询子订单)11
        /// </summary>
        /// <param name="orderId">主订单ID</param>
        /// <param name="hg">海关</param>
        /// <returns></returns>
        public List<model.apiOrderDetails> getOrderDetailsMsg(string orderId, string hg)
        {
            return dal.getOrderDetailsMsg(orderId,hg);
        }

        /// <summary>
        /// 返回订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public model.apiOrder getOrderMsg(string orderId)
        {
            return dal.getOrderMsg(orderId);
        }


        /// <summary>
        /// 返回(批量)订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<model.apiOrder> getOrderMsg(string[] orderIds)
        {
            return dal.getOrderMsg(orderIds);
        }


        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderMsg(string orderId, string scode)
        {
            return dal.getOrderMsg(orderId, scode);
        }

        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderMsg(string orderId, string scode, object o)
        {
            return dal.getOrderMsg(orderId, scode, o);
        }

        /// <summary>
        /// 返回所有API订单
        /// </summary>
        /// <returns></returns>
        public DataTable getOrderMsg(int pageIndex, int pageSize)
        {
            return dal.getOrderMsg(pageIndex, pageSize);
        }

        /// <summary>
        /// 返回所有APP订单
        /// </summary>
        /// <returns></returns>
        public DataTable getOrderMsg(Dictionary<string, string> Dic, int pageIndex, int pageSize,out int count)
        {
            return dal.getOrderMsg(Dic, pageIndex, pageSize, out count);
        }
        /// <summary>
        /// 根据主订单获取子订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderDetailsMsg(string orderId)
        {
            return dal.getOrderDetailsMsg(orderId);
        }

        /// <summary>
        /// 订单是否已拆单
        /// </summary>
        /// <returns></returns>
        public string getOrderSplit(string detailsOrderId)
        {
            return dal.getOrderSplit(detailsOrderId);
        }

        /// <summary>
        /// 获取API订单总条数
        /// </summary>
        /// <returns></returns>
        public int getDataCount()
        {
            return dal.getDataCount();
        }
        /// <summary>
        /// 添加备注
        /// </summary>
        public string UpdateApiOrder(Dictionary<string,string> Dic)
        {
            return dal.UpdateApiOrder(Dic);
        }
         /// <summary>
        /// 删除备注
        /// </summary>
        public string DeleteRemark(string Id)
        {
            return dal.DeleteRemark(Id);
        }
        /// <summary>
        /// 修改备注
        /// </summary>
        public string EditRemark(Dictionary<string, string> Dic)
        {
            return dal.EditRemark(Dic);
        }
    }
}
