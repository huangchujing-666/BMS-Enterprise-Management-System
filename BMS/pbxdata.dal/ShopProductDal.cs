using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.idal;
using System.Data.SqlClient;
using System.Data;
using Maticsoft.DBUtility;
namespace pbxdata.dal
{
    public class ShopProductDal : dataoperating
    {
        /// <summary>
        /// 当前货号已分配的库存总和
        /// </summary>
        /// <returns></returns>
        public int BalanceByScode(string Scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode)
            };
            DataTable dt = Select(ipr, "BalanceByScode");
            int count;
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0][0].ToString()==""?0:int.Parse(dt.Rows[0][0].ToString());
            }
            else
            {
                count = 0;
            }
            return count;
        }
        /// <summary>
        /// 按照货号和店铺Id查找当前数据是否存在
        /// </summary>
        /// <param name="Scode">货号</param>
        /// <param name="ShopId">店铺编号</param>
        /// <returns>true  表示存在该条记录  false 表示不存在</returns>
        public bool DataByScodeAndShopId(string Scode, string ShopId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("shopId",ShopId)
            };
            DataTable dt = Select(ipr, "DataByScodeAndShopId");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 通过店铺编号获得店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetShopNameByShopId(string id) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("id",id)
            };
            DataTable dt;
            try
            {
                dt = Select(ipr, "GetShopNameByShopId");
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ShopName"].ToString();
                }
                else 
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }
        /// <summary>
        /// 如果存在当前店铺存在该货号库存则修改
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopId"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        public string UpdateBalanceByShopidAndScode(string scode, string shopId, string Balance,string updatetime,string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("shopId",shopId),
                new SqlParameter("balance",Balance),
                new SqlParameter("updatetime",updatetime),
                new SqlParameter("vencode",vencode)
            };
            if (Update(ipr, "UpdateBalanceByShopidAndScode") == "修改成功")
            {
                return "分配成功！";
            }
            else
            {
                return "库存错误！";
            }
        }
        /// <summary>
        /// 如果当前店铺不存在该货号则添加
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertBalance(DataTable dt)
        {
            string conStr = PubConstant.ConnectionString;
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    #region 
                    bulkCopy.DestinationTableName = "dbo.[outsideProduct]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("Scode", "Scode");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("Bcode", "Bcode");
                    bulkCopy.ColumnMappings.Add("Bcode2", "Bcode2");
                    bulkCopy.ColumnMappings.Add("Descript", "Descript");
                    bulkCopy.ColumnMappings.Add("Cdescript", "Cdescript");
                    bulkCopy.ColumnMappings.Add("Unit", "Unit");
                    bulkCopy.ColumnMappings.Add("Currency", "Currency");
                    bulkCopy.ColumnMappings.Add("Cat", "Cat");
                    bulkCopy.ColumnMappings.Add("Cat1", "Cat1");
                    bulkCopy.ColumnMappings.Add("Cat2", "Cat2");
                    bulkCopy.ColumnMappings.Add("Clolor", "Clolor");
                    bulkCopy.ColumnMappings.Add("Size", "Size");
                    bulkCopy.ColumnMappings.Add("Style", "Style");
                    bulkCopy.ColumnMappings.Add("Pricea", "Pricea");
                    bulkCopy.ColumnMappings.Add("Priceb", "Priceb");
                    bulkCopy.ColumnMappings.Add("Pricec", "Pricec");
                    bulkCopy.ColumnMappings.Add("Priced", "Priced");
                    bulkCopy.ColumnMappings.Add("Pricee", "Pricee");
                    bulkCopy.ColumnMappings.Add("Disca", "Disca");
                    bulkCopy.ColumnMappings.Add("Discb", "Discb");
                    bulkCopy.ColumnMappings.Add("Discc", "Discc");
                    bulkCopy.ColumnMappings.Add("Discd", "Discd");
                    bulkCopy.ColumnMappings.Add("Disce", "Disce");
                    bulkCopy.ColumnMappings.Add("Model", "Model");
                    bulkCopy.ColumnMappings.Add("Rolevel", "Rolevel");
                    bulkCopy.ColumnMappings.Add("Balance1", "Balance");//库存
                    bulkCopy.ColumnMappings.Add("ShopId", "Loc");//库存
                    bulkCopy.ColumnMappings.Add("UserId", "Def5");//操作人
                    bulkCopy.ColumnMappings.Add("Lastgrnd", "Lastgrnd");
                    bulkCopy.ColumnMappings.Add("Imagefile", "Imagefile");
                    bulkCopy.ColumnMappings.Add("UpdateDatetime", "Def6");//操作时间
                    bulkCopy.ColumnMappings.Add("Vencode", "Vencode");//数据源
                    bulkCopy.ColumnMappings.Add("SaleState", "ShowState");//  0  未开放   1  已开放   2  已选货
                    #endregion
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                    return true;
                }
            }
            catch (Exception) 
            {
                return false;
            }
        }
        /// <summary>
        /// 取出外部库存
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public DataTable BalanceAndShopShow(int skip,int take,string vencode,string scode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("skip",skip),
                new SqlParameter("take",take),
                new SqlParameter("vencode",vencode),
                new SqlParameter("scode",scode)
            };
            return Select(ipr, "BalanceAndShopShow");
        }
        /// <summary>
        /// 取出外部库存
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public int BalanceAndShopShowCount(string vencode, string scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("vencode",vencode),
                new SqlParameter("scode",scode)
            };
            try
            {
                DataTable dt = Select(ipr, "BalanceAndShopShowCount");
                int count = dt.Rows[0][0] == "" ? 0 : int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            
            
        }
        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopId"></param>
        /// <param name="Balance"></param>
        /// <param name="updatetime"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string UpdateBalanceByShopid(string scode, string shopId, string Balance, string updatetime, string userid,string vencode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("shopId",shopId),
                new SqlParameter("balance",Balance),
                new SqlParameter("updatetime",updatetime),
                new SqlParameter("userId",userid),
                new SqlParameter("vencode",vencode)
            };
            if (Update(ipr, "UpdateBalanceByShopid") == "修改成功")
            {
                return "分配成功！";
            }
            else
            {
                return "库存错误！";
            }
        }
    }
}
