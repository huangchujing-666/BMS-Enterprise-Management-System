using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.dal
{
    public class ActivityDAL:dataoperating
    {
        public string connctionstring
        {
            get { return PubConstant.ConnectionString; }
        }
        /// <summary>
        /// 用于绑定店铺下拉列表
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable GetShopAllocationDDlist(string customer) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("ShopManager",customer)
                };
                DataTable dt = Select(ipr, "GetShopAllocationDDlist");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        ///创建销售活动
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string InsertSalesPlant(string [] str) 
        {

            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("PlaseNo",str[0]),
                    new SqlParameter("ActivityName",str[1]),
                    new SqlParameter("Loc",str[2]),
                    new SqlParameter("Brand",str[3]),
                    new SqlParameter("ApplyTime",str[4]),
                    new SqlParameter("SalesTime",str[5]),
                    new SqlParameter("StopTime",str[6]),
                    new SqlParameter("UserId",str[7])
                };
                return Add(ipr, "InsertSalesPlant");
            }
            catch (Exception)
            {
                return "创建失败！";
                throw;
            }
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
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("PlaseNo",str[0]),
                    new SqlParameter("ActivityName",str[1]),
                    new SqlParameter("AuditState",str[2]),
                    new SqlParameter("UserId",str[3]),
                    new SqlParameter("Brand",str[4]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid),
                };
                DataTable dt = Select(ipr, "GetSalesPlantPage");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
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
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("PlaseNo",str[0]),
                    new SqlParameter("ActivityName",str[1]),
                    new SqlParameter("AuditState",str[2]),
                    new SqlParameter("UserId",str[3]),
                    new SqlParameter("Brand",str[4]),
                };
                int count = 0;
                DataTable dt = Select(ipr, "GetSalesPlantPageCount");
                if (dt.Rows.Count > 0) 
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }

                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 根据品牌缩写得到品牌名
        /// </summary>
        /// <param name="Brands"></param>
        /// <returns></returns>
        public string GetBrandNameByBrands(string Brands) 
        {
            pbxdatasourceDataContext pbc = new pbxdatasourceDataContext(connctionstring);
            List<brand> info = pbc.brand.Where(a => a.BrandAbridge == Brands).ToList();
            if (info.Count > 0)
            {
                return info[0].BrandName;
            }
            else 
            {
                return "";
            }
        }
        /// <summary>
        /// 活动当前销售活动品牌所有商品
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetProductBybrand(string[] str,int minid,int maxid) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Cat2",str[0]),
                    new SqlParameter("Cat",str[1]),
                    new SqlParameter("Scode",str[2]),
                    new SqlParameter("Style",str[3]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid)
                };
                DataTable dt = Select(ipr, "GetProductBybrand");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 活动当前销售活动品牌所有商品个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetProductBybrandCount(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Cat2",str[0]),
                    new SqlParameter("Cat",str[1]),
                    new SqlParameter("Scode",str[2]),
                    new SqlParameter("Style",str[3])
                };
                int count = 0;
                DataTable dt = Select(ipr, "GetProductBybrandCount");
                if (dt.Rows.Count > 0) 
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 得到选择要添加的货物
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetInsertTable(string[] str) 
        {
            try
            {
                string strsql = "";
                for (int i = 0; i < str.Length-1; i++) 
                {
                    strsql += ",'" + str[i+1]+"'";
                }
                strsql = strsql.Trim(',');
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("Scodes",strsql),
                };
                DataTable dt = Select(ipr, "GetInsertProductTable");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertPlantProductStock(DataTable dt) 
        {
            try
            {
                using (SqlBulkCopy sqlbc = new SqlBulkCopy(connctionstring, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    sqlbc.DestinationTableName = "dbo.[PlantProductStock]";//需要插入的表
                    sqlbc.ColumnMappings.Add("PlaseNo", "PlaseNo");
                    sqlbc.ColumnMappings.Add("Scode", "Scode");
                    sqlbc.ColumnMappings.Add("Bcode", "Bcode");
                    sqlbc.ColumnMappings.Add("Bcode2", "Bcode2");
                    sqlbc.ColumnMappings.Add("Descript", "Descript");
                    sqlbc.ColumnMappings.Add("Cdescript", "Cdescript");
                    sqlbc.ColumnMappings.Add("Unit", "Unit");
                    sqlbc.ColumnMappings.Add("Currency", "Currency");
                    sqlbc.ColumnMappings.Add("Cat", "Cat");
                    sqlbc.ColumnMappings.Add("Cat1", "Cat1");
                    sqlbc.ColumnMappings.Add("Cat2", "Cat2");
                    sqlbc.ColumnMappings.Add("Clolor", "Clolor");
                    sqlbc.ColumnMappings.Add("Size", "Size");
                    sqlbc.ColumnMappings.Add("Style", "Style");
                    sqlbc.ColumnMappings.Add("Pricea", "Pricea");
                    sqlbc.ColumnMappings.Add("Priceb", "Priceb");
                    sqlbc.ColumnMappings.Add("Pricec", "Pricec");
                    sqlbc.ColumnMappings.Add("Priced", "Priced");
                    sqlbc.ColumnMappings.Add("Pricee", "Pricee");
                    sqlbc.ColumnMappings.Add("Vencode", "Vencode");
                    sqlbc.ColumnMappings.Add("UserId", "UserId");
                    sqlbc.ColumnMappings.Add("Model", "Model");
                    //sqlbc.ColumnMappings.Add("Loc", "Loc");
                    //sqlbc.ColumnMappings.Add("Balance", "Balance");
                    sqlbc.ColumnMappings.Add("Lastgrnd", "Lastgrnd");
                    sqlbc.ColumnMappings.Add("Imagefile", "Imagefile");
                    sqlbc.WriteToServer(dt);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        /// <summary>
        /// 得到销售活动中详细的商品信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetPlantProduct(string [] str,int minid,int maxid) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Cat",str[0]),
                    new SqlParameter("Cat2",str[1]),
                    new SqlParameter("Scode",str[2]),
                    new SqlParameter("Style",str[3]),
                    new SqlParameter("UserId",str[4]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid),
                    new SqlParameter("PlaseNo",str[5])
                };
                DataTable dt = Select(ipr, "GetPlantProduct");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
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
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Cat",str[0]),
                    new SqlParameter("Cat2",str[1]),
                    new SqlParameter("Scode",str[2]),
                    new SqlParameter("Style",str[3]),
                    new SqlParameter("UserId",str[4]),
                    new SqlParameter("PlaseNo",str[5])
                };
                DataTable dt = Select(ipr, "GetPlantProductCount");
                int count = 0;
                if (dt.Rows.Count > 0) 
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 得到对应货号的价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="plaseNo"></param>
        /// <returns></returns>
        public string GetPlantProductPrice(string scode, string plaseNo) 
        {
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connctionstring);
            List<PlantProductStock> info = pdc.PlantProductStock.Where(a => a.PlaseNo == plaseNo && a.Scode == scode).ToList();
            string str;
            str = info[0].Pricea.ToString() + "❤" + info[0].Priceb.ToString() + "❤" + info[0].Pricec.ToString() + "❤" + info[0].Priced.ToString() + "❤" + info[0].Pricee.ToString();
            return str;
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="plaseNo"></param>
        /// <param name="locprice"></param>
        /// <param name="activitprice"></param>
        /// <returns></returns>
        public string UpdatePlantProductPrice(string scode, string plaseNo,string locprice,string activitprice,string Balance) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("locPrice",locprice),
                    new SqlParameter("activPirce",activitprice),
                    new SqlParameter("scode",scode),
                    new SqlParameter("plaseno",plaseNo),
                    new SqlParameter("Balance",Balance)
                };
                return Update(ipr, "UpdatePlantProductPrice");
            }
            catch (Exception)
            {
                return "修改失败！";
                throw;
            }
        }
        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string UpdateSalesState(string state,string plaseno) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("State",state),
                    new SqlParameter("PlaseNo",plaseno),
                };
                string result = Update(ipr, "UpdateSalesState");
                if (result == "修改成功")
                {
                    return "审核成功";
                }
                else 
                {
                    return "错误信息";
                }
            }
            catch (Exception)
            {
                return "错误信息";
                throw;
            }
        }

        /// <summary>
        /// ----统计当前活动的总量
        /// </summary>
        /// <param name="plaseno"></param>
        /// <returns></returns>
        public DataTable StatisticsSalesMoney(string plaseno) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("plaseno",plaseno)
                };
                DataTable dt = Select(ipr, "StatisticsSalesMoney");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 修改销售活动总量
        /// </summary>
        /// <param name="countmoney"></param>
        /// <param name="countscode"></param>
        /// <param name="plaseno"></param>
        /// <returns></returns>
        public bool UpdateSalesPlantCount(string countmoney, string countscode, string plaseno) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("countmoney",countmoney),
                    new SqlParameter("countscode",countscode),
                    new SqlParameter("plaseNo",plaseno),

                };
                string reslut = Update(ipr, "UpdateSalesPlantCount");
                if (reslut == "修改成功")
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
