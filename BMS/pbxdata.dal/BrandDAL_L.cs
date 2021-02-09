using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using Maticsoft.DBUtility;
using pbxdata.idal;
using System.Data;
using System.Data.SqlClient;
namespace pbxdata.dal
{
    public partial class BrandDAL : dataoperating, iBrandDAL
    {
        /// <summary>
        /// 根据品牌简称获取全称
        /// </summary>
        /// <param name="brandName">品牌简称</param>
        /// <returns></returns>
        public string getBrandFullName(string brandName)
        {
            string s = string.Empty;
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            brand MdBrand = new brand();
            s = (from c in context.brand where c.BrandAbridge == brandName select c).SingleOrDefault().BrandName;
            return s;
        }
    }
}
