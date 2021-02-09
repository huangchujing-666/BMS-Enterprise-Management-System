/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderpaydetailsbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       apiorderpaydetailsbll
    * 创建时间：       2015-08-05 17:33:22
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class apiorderpaydetailsbll : dataoperatingbll
    {
        iapiorderpaydetails dal = (iapiorderpaydetails)ReflectFactory.CreateIDataOperatingByReflect("apiorderpaydetailsdal");

        /// <summary>
        /// 根据订单ID获取payId(支付宝流水),payOuterId(系统流水),payPrice money(支付金额)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string[] getPayDetails(string orderId)
        {
            return dal.getPayDetails(orderId);
        }


        /// <summary>
        /// 根据订单ID获取payId（这里以payId为订单号报关海关，商检，支付，物流）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getPayId(string orderId)
        {
            return dal.getPayId(orderId);
        }
    }
}
