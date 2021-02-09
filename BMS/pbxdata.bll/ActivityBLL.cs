using pbxdata.dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class ActivityBLL
    {
        ActivityDAL AD = new ActivityDAL();
        /// <summary>
        /// 用于绑定店铺下拉列表
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable GetShopAllocationDDlist(string customer)
        {
            return AD.GetShopAllocationDDlist(customer);
        }
        /// <summary>
        ///创建销售活动
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string InsertSalesPlant(string[] str) 
        {
            return AD.InsertSalesPlant(str);
        }

        /// <summary>
        /// 得到所有活动信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSalesPlantPage(string[] str, int minid, int maxid)
        {
            return AD.GetSalesPlantPage(str, minid, maxid);
        }
        /// <summary>
        /// 得到所有活动信息个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetSalesPlantPageCount(string[] str)
        {
            return AD.GetSalesPlantPageCount(str);
        }
        /// <summary>
        /// 根据品牌缩写得到品牌名
        /// </summary>
        /// <param name="Brands"></param>
        /// <returns></returns>
        public string GetBrandNameByBrands(string Brands)
        {
            return AD.GetBrandNameByBrands(Brands);
        }
        /// <summary>
        /// 活动当前销售活动品牌所有商品
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetProductBybrand(string[] str, int minid, int maxid)
        {
            return AD.GetProductBybrand(str, minid, maxid);
        }
        /// <summary>
        /// 活动当前销售活动品牌所有商品个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetProductBybrandCount(string[] str)
        {
            return AD.GetProductBybrandCount(str);
        }
        /// <summary>
        /// 得到选择要添加的货物
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetInsertTable(string[] str) 
        {
            return AD.GetInsertTable(str);
        }
                /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertPlantProductStock(DataTable dt) 
        {
            return AD.InsertPlantProductStock(dt);
        }
        /// <summary>
        /// 得到销售活动中详细的商品信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetPlantProduct(string[] str, int minid, int maxid)
        {
            return AD.GetPlantProduct(str, minid, maxid);
        }

        /// <summary>
        /// 得到销售活动中详细的商品信息个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetPlantProductCount(string[] str)
        {
            return AD.GetPlantProductCount(str);
        }
                /// <summary>
        /// 得到对应货号的价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="plaseNo"></param>
        /// <returns></returns>
        public string GetPlantProductPrice(string scode, string plaseNo)
        {
            return AD.GetPlantProductPrice(scode, plaseNo);
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="plaseNo"></param>
        /// <param name="locprice"></param>
        /// <param name="activitprice"></param>
        /// <returns></returns>
        public string UpdatePlantProductPrice(string scode, string plaseNo, string locprice, string activitprice,string Balance)
        {
            return AD.UpdatePlantProductPrice(scode, plaseNo, locprice, activitprice,Balance);
        }
        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string UpdateSalesState(string state, string plaseno) 
        {
            return AD.UpdateSalesState(state, plaseno);
        }
        /// <summary>
        /// ----统计当前活动的总量
        /// </summary>
        /// <param name="plaseno"></param>
        /// <returns></returns>
        public DataTable StatisticsSalesMoney(string plaseno) 
        {
            return AD.StatisticsSalesMoney(plaseno);
        }
        // <summary>
        /// 修改销售活动总量
        /// </summary>
        /// <param name="countmoney"></param>
        /// <param name="countscode"></param>
        /// <param name="plaseno"></param>
        /// <returns></returns>
        public bool UpdateSalesPlantCount(string countmoney, string countscode, string plaseno)
        {
            return AD.UpdateSalesPlantCount(countmoney, countscode, plaseno);
        }
    }
}
