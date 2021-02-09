using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface iDataChart
    {

        /// <summary>
        /// 销售统计
        /// </summary>
        /// <returns></returns>
        string ReturnInfo();

        /// <summary>
        /// 销售统计
        /// </summary>
        /// <returns></returns>
        string ReturnInfo(Dictionary<string, string> Dic);



        /// <summary>
        /// 获取来货报表（HK数据源)
        /// </summary>
        /// <returns></returns>
        DataTable GetLaiHuoReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts, out string Balances);

        /// <summary>
        /// 获取来货报表（HK数据源)--今日
        /// </summary>
        /// <returns></returns>
        DataTable GetLaiHuoReportT();

        /// <summary>
        /// 退款退货统计-总数
        /// </summary>
        /// <returns></returns>
        string Refund();

        /// <summary>
        /// 商品统计--总数
        /// </summary>
        /// <returns></returns>
        string ReturnProduct();

        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        DataTable GetBrandReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts);

        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        DataTable GetTypeReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts);

        /// <summary>
        /// 退款报表
        /// </summary>
        /// <returns></returns>
        DataTable GetRefundReport(string MinTime, string MaxTime);

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        DataTable GetReturnBalanceReport(string MinTime, string MaxTime);

        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetReturnRateReport(Dictionary<string, string> Dic);

        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetBrandRateReport(string Cat);
        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetTypeRateReport(string TypeNo);
        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        DataTable GetSellPriceRateReport(string SellPriceMin, string SellPriceMax);

        /// <summary>
        /// 获取出货报表
        /// </summary>
        /// <returns></returns>
        DataTable GetShipmentReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts);
        /// <summary>
        /// 获取出货报表--今日
        /// </summary>
        /// <returns></returns>
        DataTable GetShipmentReportT();
    }
}
