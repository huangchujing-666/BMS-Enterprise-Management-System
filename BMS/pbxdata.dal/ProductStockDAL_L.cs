using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.idal;
using System.Data.SqlClient;
using System.Threading;
namespace pbxdata.dal
{
    public partial class ProductStockDAL : dataoperating,iProductStock
    {
        /// <summary>
        /// 根据货号返回库存和供应商(库存查product表)
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public Dictionary<string, int> getScodeBalance(string scode)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            //var p = from c in context.productstock where c.Scode == scode select new { vencode = c.Vencode, balance = c.Balance };
            var p = from c in context.product where c.Scode == scode select new { vencode = c.Vencode, balance = c.Balance };
            DataTable dt = LinqToDataTable.LINQToDataTable(p);
            if (dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string vencode = dt.Rows[i]["vencode"].ToString();
                    int balance = int.Parse(dt.Rows[i]["balance"].ToString());

                    dic.Add(vencode, balance);
                }
            }
            
            return dic;
        }


        /// <summary>
        /// 根据主订单获取子订单，并对其子订单进行查询，返回库存和供应商和价格。(库存查productstock表)
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="isParentOrder"></param>
        /// <returns></returns>
        public DataTable getScodeBalance(string scode, bool isParentOrder)
        {
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = from c in context.productstock where c.Scode == scode select new { vencode = c.Vencode, balance = c.Balance, price = c.Pricee };
            //var p = from c in context.product where c.Scode == scode select new { vencode = c.Vencode, balance = c.Balance, price = c.Pricee };
            DataTable dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 根据货号返回已售库存(库存查product表)
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int getScodeBalanceSales(string scode)
        {
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            model.product p = (from c in context.product where c.Scode == scode select c).SingleOrDefault();
            int i = p == null ? 0 : int.Parse(p.Def3.ToString());

            return i;
        }


        /// <summary>
        /// 根据货号返回VIP价格（报关）
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getPriceA(string scode)
        {
            string s = string.Empty;
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = (from c in context.productstock where c.Scode == scode select c).SingleOrDefault();
            s = p == null ? "0" : p.Pricec.ToString();
            return s;
        }


    }
}
