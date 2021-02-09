/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       hscodedal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       hscodedal
    * 创建时间：       2015-06-24 16:01:16
    * 作    者：       lcg
    * 说    明：       HSCODE
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
    public class hscodedal : dataoperating, ihscode
    {
        /// <summary>
        /// 根据hscode编码获取商品品名
        /// </summary>
        /// <param name="hscode">HScode编码</param>
        /// <returns></returns>
        public string getHscodeProductName(string hscode)
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.HSInfomation where c.HSNumber == hscode select new { TypeName = c.TypeName }).SingleOrDefault();
            s = p == null ? "" : p.TypeName;
            return s;
        }
    }
}
