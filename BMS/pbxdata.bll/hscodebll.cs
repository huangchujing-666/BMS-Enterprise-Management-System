/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       hscodebll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       hscodebll
    * 创建时间：       2015-06-24 16:07:49
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
    public class hscodebll : dataoperatingbll
    {
        ihscode dal = (ihscode)ReflectFactory.CreateIDataOperatingByReflect("hscodedal");

        /// <summary>
        /// 根据hscode编码获取商品品名
        /// </summary>
        /// <param name="hscode">HScode编码</param>
        /// <returns></returns>
        public string getHscodeProductName(string hscode)
        {
            return dal.getHscodeProductName(hscode);
        }
    }
}
