using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class DataChartbll : dataoperatingbll
    {
        iDataChart dal = (iDataChart)ReflectFactory.CreateIDataOperatingByReflect("DataChartdal");


        /// <summary>
        /// 销售统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnInfo()
        {
            return dal.ReturnInfo();
        }

        /// <summary>
        /// 销售统计
        /// </summary>
        /// <returns></returns>
        public string ReturnInfo(Dictionary<string, string> Dic)
        {
            return dal.ReturnInfo(Dic);
        }

        
        /// <summary>
        /// 获取来货报表（HK数据源)
        /// </summary>
        /// <returns></returns>
        public DataTable GetLaiHuoReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts,out string Balances)
        {
            return dal.GetLaiHuoReport(Dic, pageIndex, pageSize, out counts, out Balances);
        }

        /// <summary>
        /// 获取来货报表（HK数据源)--今日
        /// </summary>
        /// <returns></returns>
        public DataTable GetLaiHuoReportT()
        {
            return dal.GetLaiHuoReportT();
        }
        /// <summary>
        /// 退款退货统计-总数
        /// </summary>
        /// <returns></returns>
        public string Refund()
        {
            return dal.Refund();
        }

        /// <summary>
        /// 商品统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnProduct()
        {
            return dal.ReturnProduct();
        }
        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            return dal.GetBrandReport(Dic, pageIndex, pageSize, out counts);
        }

        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            return dal.GetTypeReport(Dic, pageIndex, pageSize, out counts);
        }


        /// <summary>
        /// 退款报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetRefundReport(string MinTime,string MaxTime)
        {
            return dal.GetRefundReport(MinTime, MaxTime);
        }

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetReturnBalanceReport(string MinTime, string MaxTime)
        {
            return dal.GetReturnBalanceReport(MinTime, MaxTime);
        }

        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetReturnRateReport(Dictionary<string,string> Dic)
        {
            return dal.GetReturnRateReport(Dic);
        }
        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandRateReport(string Cat)
        {
            return dal.GetBrandRateReport(Cat);
        }
        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeRateReport(string TypeNo)
        {
            return dal.GetTypeRateReport(TypeNo);
        }
        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSellPriceRateReport(string SellPriceMin, string SellPriceMax)
        {
            return dal.GetSellPriceRateReport(SellPriceMin, SellPriceMax);
        }

        /// <summary>
        /// 获取出货报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetShipmentReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            return dal.GetShipmentReport(Dic,pageIndex,pageSize,out counts);
        }
         /// <summary>
        /// 获取出货报表--今日
        /// </summary>
        /// <returns></returns>
        public DataTable GetShipmentReportT()
        {
            return dal.GetShipmentReportT();
        }
    }
}
