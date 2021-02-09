/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ordercustomsresultbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       ordercustomsresultbll
    * 创建时间：       2015-06-27 14:28:44
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
    public class ordercustomsresultbll:dataoperatingbll
    {

        iordercustomsresult dal = (iordercustomsresult)ReflectFactory.CreateIDataOperatingByReflect("ordercustomsresultdal");


        /// <summary>
        /// 判断是否生成过此商品报文
        /// </summary>
        /// <param name="orderIds">订单ID集合</param>
        /// <returns>订单ID和报文名称的键值对</returns>
        public Dictionary<string, string> getUploadStatus(string orderIds)
        {
            return dal.getUploadStatus(orderIds);
        }

        /// <summary>
        /// 获取此订单是否上传成功(商检)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单ID和上传状态成功失败的键值对</returns>
        public Dictionary<string, string> getUploadStatus1(string orderId)
        {
            return dal.getUploadStatus1(orderId);
        }

        /// <summary>
        /// 获取此商品是否上传成功(海关)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单ID和上传状态成功失败的键值对</returns>
        public Dictionary<string, string> getUploadStatus2(string orderId)
        {
            return dal.getUploadStatus2(orderId);
        }

        /// <summary>
        /// 根据货号获取订单商检报关是否成功(商检订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getSJUploadCheck(string orderId)
        {
            return dal.getSJUploadCheck(orderId);
        }

        /// <summary>
        /// 根据货号获取订单海关报关是否成功(海关订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getHGUploadCheck(string orderId)
        {
            return dal.getHGUploadCheck(orderId);
        }


        /// <summary>
        /// 更新报文名称
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="name">报文名称</param>
        /// <returns></returns>
        public string updateUploadName(string orderId, string name)
        {
            return dal.updateUploadName(orderId, name);
        }


        /// <summary>
        /// 上传报文状态(插入数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string addUploadStatus(model.orderCustomsResult resultStatus)
        {
            return dal.addUploadStatus(resultStatus);
        }

        /// <summary>
        /// 更新报文状态(更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus(model.orderCustomsResult resultStatus)
        {
            return dal.updateUploadStatus(resultStatus);
        }


        /// <summary>
        /// 更新报文状态(海关--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus1(model.orderCustomsResult resultStatus)
        {
            return dal.updateUploadStatus1(resultStatus);
        }


        /// <summary>
        /// 更新报文状态(联邦--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus2(model.orderCustomsResult resultStatus)
        {
            return dal.updateUploadStatus2(resultStatus);
        }

        /// <summary>
        /// 查询状态
        /// </summary>
        /// <param name="childorderId">子订单</param>
        /// <returns></returns>
        public DataTable getOrderReportStatus(string childorderId)
        {
            return dal.getOrderReportStatus(childorderId);            
        }

    }
}
