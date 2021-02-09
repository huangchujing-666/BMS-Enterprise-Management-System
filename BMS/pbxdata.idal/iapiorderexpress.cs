using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface iapiorderexpress
    {
        /// <summary>
        /// 根据订单ID获取运单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string expressNo(string orderId);
    }
}
