using pbxdata.dalfactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
     public partial class CheckProductbll : dataoperatingbll
    {
        dal.CheckProducdal dal = (dal.CheckProducdal)ReflectFactory.CreateIDataOperatingByReflect("CheckProducdal");
        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <returns></returns>
        public DataTable SearchProduct(Dictionary<string, string> dic, int page, int pages, out string counts)
        {
            return dal.GetSearchList(dic, page, pages, out counts);
        }

    }
}
