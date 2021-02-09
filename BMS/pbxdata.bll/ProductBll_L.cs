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
        /// <summary>
        /// 根据货号查询商品
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public model.product getProductScode(string scode)
        {
            return dal.getProductScode(scode);
        }

        /// <summary>
        /// 减去本次购买商品的数量
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="localBanlace">本次购买数量</param>
        /// <returns></returns>
        public string minusBanlace(string scode, int localBanlace)
        {
            return dal.minusBanlace(scode, localBanlace);
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
            return dal.minusBanlace(scode, localBanlace, o);
        }


        /// <summary>
        /// 获取是否存在带yy结尾的相同货号
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public bool getYyScode(string scode)
        {
            return dal.getYyScode(scode);
        }

    }
}
