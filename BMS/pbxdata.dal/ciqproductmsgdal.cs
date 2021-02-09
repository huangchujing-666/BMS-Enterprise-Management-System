/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ciqproductmsgdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       ciqproductmsgdal
    * 创建时间：       2015-05-20 14:29:19
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
    public class ciqproductmsgdal : dataoperating, iciqproductmsg
    {
        /// <summary>
        /// 海关订单报备需要的商品信息
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public model.CiqProductDetails getCiqMsg(string scode)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            model.CiqProductDetails MDCiqProductDetails = context.CiqProductDetails.FirstOrDefault(c => c.ciqScode == scode);

            return MDCiqProductDetails;
        }
    }
}
