using pbxdata.dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.bll
{
    public partial class ProductStockBLL : dataoperatingbll
    {
        /// <summary>
        /// 根据货号返回库存和供应商(库存查product表)
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public Dictionary<string, int> getScodeBalance(string scode)
        {
            return psd.getScodeBalance(scode);
        }

        /// <summary>
        /// 根据主订单获取子订单，并对其子订单进行查询，返回库存和供应商和价格。(库存查productstock表)
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public DataTable getScodeBalance(string scode, bool isParentOrder)
        {
            return psd.getScodeBalance(scode, isParentOrder);
        }


        /// <summary>
        /// 根据货号返回已售库存(库存查product表)
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int getScodeBalanceSales(string scode)
        {
            return psd.getScodeBalanceSales(scode);
        }

        /// <summary>
        /// 根据货号返回VIP价格（报关）
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getPriceA(string scode)
        {
            return psd.getPriceA(scode);
        }
    }
}
