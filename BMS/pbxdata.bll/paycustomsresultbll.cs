/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       paycustomsresultbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       paycustomsresultbll
    * 创建时间：       2015-08-08 15:00:12
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
    public class paycustomsresultbll : dataoperatingbll
    {
        ipaycustomsresult dal = (ipaycustomsresult)ReflectFactory.CreateIDataOperatingByReflect("paycustomsresultdal");

        /// <summary>
        /// 插入订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        public string addPayCustoms(model.payCustomsResult payResult)
        {
            return dal.addPayCustoms(payResult);
        }

        /// <summary>
        /// 更新订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        public string updatePayCustoms(model.payCustomsResult payResult)
        {
            return dal.updatePayCustoms(payResult);
        }


        /// <summary>
        /// 是否存在订单支付报关
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string existPayCustoms(string orderId)
        {
            return dal.existPayCustoms(orderId);
        }

        /// <summary>
        /// 根据订单ID获取支付单的报备状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getPayStatus(string orderId)
        {
            return dal.getPayStatus(orderId);
        }
    }
}
