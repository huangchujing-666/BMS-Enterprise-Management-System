/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       iapiorderpaydetails
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       iapiorderpaydetails
    * 创建时间：       2015-08-05 17:20:24
    * 作    者：       lcg
    * 说    明：       订单支付详情
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
    public interface iapiorderpaydetails
    {
        /// <summary>
        /// 根据订单ID获取payId(支付内部流水号),payOuterId(支付外部流水号),payPrice money(支付金额)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string[] getPayDetails(string orderId);

        /// <summary>
        /// 根据订单ID获取payId（这里以payId为订单号报关海关，商检，支付，物流）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string getPayId(string orderId);


    }
}
