/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       iciqproductmsg
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       iciqproductmsg
    * 创建时间：       2015-05-20 14:26:57
    * 作    者：       lcg
    * 说    明：       海关订单报备需要的一些商品信息
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
    public interface iciqproductmsg
    {
        /// <summary>
        /// 海关订单报备需要的商品信息
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        model.CiqProductDetails getCiqMsg(string scode);
    }
}
