using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class ProductOrderbll
    {
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
         dal.ProductOrderdal pod = new dal.ProductOrderdal();
        public DataTable GetProductOrder(int skip, int take, string [] str, out Exception exx)
        {
            return pod.GetProductOrder(skip, take, str, out exx);
        }
        /// <summary>
        /// 总个数
        /// </summary>
        /// <param name="strCondition"></param>
        /// <returns></returns>
        public int GetProductOrderCount(string[] strCondition) 
        {
            return pod.GetProductOrderCount(strCondition);
        }
    }
}
