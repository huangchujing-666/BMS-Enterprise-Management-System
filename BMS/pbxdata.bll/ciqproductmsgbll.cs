/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ciqproductmsgbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       ciqproductmsgbll
    * 创建时间：       2015-05-20 14:31:44
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
    public class ciqproductmsgbll : dataoperatingbll
    {

        iciqproductmsg dal = (iciqproductmsg)ReflectFactory.CreateIDataOperatingByReflect("ciqproductmsgdal");

        /// <summary>
        /// 海关订单报备需要的商品信息
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public model.CiqProductDetails getCiqMsg(string scode)
        {
            return dal.getCiqMsg(scode);
        }
    }
}
