using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.idal
{
    public partial interface iProductStock
    {
        /// <summary>
        /// 根据货号返回库存和供应商
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        Dictionary<string,int> getScodeBalance(string scode);


        /// <summary>
        /// 根据主订单获取子订单，并对其子订单进行查询，返回库存和供应商和价格。
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        DataTable getScodeBalance(string scode,bool isParentOrder);


        /// <summary>
        /// 根据货号返回已售库存
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        int getScodeBalanceSales(string scode);


        /// <summary>
        /// 根据货号返回VIP价格（报关）
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        string getPriceA(string scode);

    }
}
