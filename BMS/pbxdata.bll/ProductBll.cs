using pbxdata.dalfactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;

namespace pbxdata.bll
{
    public partial class ProductBll : dataoperatingbll
    {
        dal.ProductDal dal = (dal.ProductDal)ReflectFactory.CreateIDataOperatingByReflect("ProductDal");
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateProduct(model.product mm, string a, string b, string PropertyId, int UserId)
        {
            return dal.UpdateProduct(mm, a, b, PropertyId, UserId);
        }
        ///// <summary>
        ///// 更新商品属性
        ///// </summary>
        ///// <returns></returns>
        //public bool UpdateAttr(model.product mm)
        //{
        //    return dal.UpdateAttrValue(mm);
        //}

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <returns></returns>
        public DataTable SearchProduct(Dictionary<string, string> dic, int page, int pages, out string counts, out string Balcounts, int isReset)
        {
            return dal.GetSearchList(dic, page, pages, out counts, out Balcounts,isReset);
        }

        public DataTable SearchProduct1(Dictionary<string, string> dic, int page, int pages, out string counts, out string Balcounts,int isReset)
        {
            return dal.GetSearchList1(dic, page, pages, out counts, out Balcounts,isReset);
        }

        /// <summary>
        /// 获取拉列表商品类别显示相应属性
        /// </summary>
        public DataTable GetPropertyByTypeNo(string Scode, string TypeNo)
        {
            return dal.GetPropertyByTypeNo(Scode, TypeNo);
        }

        /// <summary>
        /// 获取Hs列表
        /// </summary>
        public DataTable SearchHSinfo(string HSNumber,string TypeName)
        {
            return dal.SearchHSinfo(HSNumber, TypeName);
        }

        /// <summary>
        /// 存储编辑信息 Product主款号表
        /// </summary>
        public int Updatestyledescript(string style, string productDescript,int UserId)
        {
            return dal.Updatestyledescript(style, productDescript, UserId);
        }

        public DataTable GetShopNameByStyle(string style)
        {

            return dal.GetShopNameByStyle(style);
        }
    }
}
