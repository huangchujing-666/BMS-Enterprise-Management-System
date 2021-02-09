/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ipaycustomsresult
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       ipaycustomsresult
    * 创建时间：       2015-08-08 14:47:34
    * 作    者：       lcg
    * 说    明：       支付报关
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface ipaycustomsresult
    {
        /// <summary>
        /// 插入订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        string addPayCustoms(model.payCustomsResult payResult);

        /// <summary>
        /// 更新订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        string updatePayCustoms(model.payCustomsResult payResult);


        /// <summary>
        /// 是否存在订单支付报关
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string existPayCustoms(string orderId);

        /// <summary>
        /// 根据订单ID获取支付单的报备状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string getPayStatus(string orderId);

    }
}
