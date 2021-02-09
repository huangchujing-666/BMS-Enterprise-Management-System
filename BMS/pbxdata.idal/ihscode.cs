/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       iapiorder
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       iapiorder
    * 创建时间：       2015-06-24 13:37:27
    * 作    者：       lcg
    * 说    明：       HSCODE
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
    public interface ihscode
    {
        /// <summary>
        /// 根据hscode编码获取商品品名
        /// </summary>
        /// <param name="hscode"></param>
        /// <returns></returns>
        string getHscodeProductName(string hscode);

    }
}
