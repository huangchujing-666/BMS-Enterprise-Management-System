/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderexpressbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       apiorderexpressbll
    * 创建时间：       2015-08-17 14:52:46
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
    public class apiorderexpressbll : dataoperatingbll
    {
        iapiorderexpress dal = (iapiorderexpress)ReflectFactory.CreateIDataOperatingByReflect("apiorderexpressdal");

        /// <summary>
        /// 根据订单ID获取运单号
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        public string expressNo(string orderId)
        {
            return dal.expressNo(orderId);
        }


    }
}
