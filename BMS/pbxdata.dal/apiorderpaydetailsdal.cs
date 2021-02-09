/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderpaydetailsdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       apiorderpaydetailsdal
    * 创建时间：       2015-08-05 17:34:47
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class apiorderpaydetailsdal : dataoperating, iapiorderpaydetails
    {
        /// <summary>
        /// 根据订单ID获取payId(支付宝流水),payOuterId(系统流水),payPrice money(支付金额)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string[] getPayDetails(string orderId)
        {
            string[] ss = new string[3];
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.apiOrderPayDetails where c.orderId == orderId select c;

            foreach (var item in p)
            {
                ss[0] = item.payId;
                ss[1] = item.payOuterId;
                ss[2] = item.payPrice.ToString();
            }

            return ss;
        }


        /// <summary>
        /// 根据订单ID获取payId（这里以payId为订单号报关海关，商检，支付，物流）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getPayId(string orderId)
        {
            string payId = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.apiOrderPayDetails where c.orderId == orderId select c;
            if (p==null)
            {
                return "";
            }
            foreach (var item in p)
            {
                payId = item.payId;
            }

            return payId;
        }

    }
}
