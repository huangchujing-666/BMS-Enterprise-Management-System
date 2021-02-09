using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.idal
{
    public interface iProductTypeDAL
    {
        string connectionString { get; }
        /// <summary>
        /// 得到商品类型
        /// </summary>
        /// <returns></returns>
        List<ProductTypeModel> GetProductType();
    }
}
