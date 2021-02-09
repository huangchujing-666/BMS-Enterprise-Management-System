using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public partial class ProductDal : dataoperating
    {

        /// <summary>
        /// 根据货号查询商品
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public model.product getProductScode(string scode)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            model.product MdProduct = (from c in context.product where c.Scode == scode select c).FirstOrDefault();

            return MdProduct;
        }


        /// <summary>
        /// 减去本次购买商品的数量
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="localBanlace">本次购买数量</param>
        /// <returns></returns>
        public string minusBanlace(string scode, int localBanlace)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.product MdProduct = (from c in context.product where c.Scode == scode select c).FirstOrDefault();
                if (MdProduct!=null)
                {
                    MdProduct.Scode = scode;
                    MdProduct.Def3 = MdProduct.Def3 - localBanlace;

                    context.SubmitChanges();
                    s = "减去销售数量成功";
                }
                else
                {
                    s = "减去销售数量失败";
                }
            }
            catch (Exception ex)
            {
                s = "减去销售数量失败" + (ex.Message);
            }
            return s;
        }


        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="localBanlace">本次购买数量</param>
        /// <param name="o">实体连接</param>
        /// <returns></returns>
        public string minusBanlace(string scode, int localBanlace, object o)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = (model.pbxdatasourceDataContext)o;
                model.product MdProduct = (from c in context.product where c.Scode == scode select c).FirstOrDefault();

                if (MdProduct != null && MdProduct.Def3 > 0)
                {
                    MdProduct.Scode = scode;
                    MdProduct.Def3 = MdProduct.Def3 - localBanlace;

                    context.SubmitChanges();
                    s = "减去销售数量成功";
                }
                else
                {
                    s = "减去销售数量失败";
                }
            }
            catch (Exception ex)
            {
                s = "减去销售数量失败" + (ex.Message);
            }
            return s;
        }


        /// <summary>
        /// 获取是否存在带yy结尾的相同货号
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public bool getYyScode(string scode)
        {
            bool flag = false;
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var p = (from c in context.product where c.Scode == scode select c ).SingleOrDefault();

                if (p != null)
                {
                    flag = true;
                }
            }
            catch (Exception ex) { }

            return flag;
        }
        
    }
}
