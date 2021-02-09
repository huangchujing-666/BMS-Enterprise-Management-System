/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderexpressdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       apiorderexpressdal
    * 创建时间：       2015-08-17 14:31:08
    * 作    者：       lcg
    * 说    明：       物流运单号
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
    public class apiorderexpressdal : dataoperating, iapiorderexpress
    {
        /// <summary>
        /// 根据订单ID获取运单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string expressNo(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string p = context.apiOrderExpress.SingleOrDefault(c => c.orderId == orderId).expressNo;
            s = p;
            return s;
        }
    }
}
