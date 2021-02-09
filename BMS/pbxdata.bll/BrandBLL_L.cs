using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.dal;
using System.Data;
using System.Data.SqlClient;
namespace pbxdata.bll
{
    public partial class BrandBLL
    {
        /// <summary>
        /// 根据品牌简称获取全称
        /// </summary>
        /// <param name="brandName">品牌简称</param>
        /// <returns></returns>
        public string getBrandFullName(string brandName)
        {
            return bd.getBrandFullName(brandName);
        }
    }
}
