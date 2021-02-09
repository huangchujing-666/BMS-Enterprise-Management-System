using Maticsoft.DBUtility;
using pbxdata.idal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class SoCalProductDal : dataoperating
    {
        /// <summary>
        /// 通过配置获取连接字符串
        /// </summary>
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        /// <summary>
        /// 得到所有现货数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSoCalProduct(string[] str, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",str[1]),
               new SqlParameter("scode",str[0]),
               new SqlParameter("priceMin",str[2]),
               new SqlParameter("priceMax",str[3]),
               new SqlParameter("cat",str[7]),
               new SqlParameter("BalanceMin",str[5]),
               new SqlParameter("BalanceMax",str[6]),
               new SqlParameter("Cat2",str[10]),
               new SqlParameter("Cat1",str[4]),
               new SqlParameter("LastgrndMin",str[8]),
               new SqlParameter("LastgrndMax",str[9]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("minid",minid),
               new SqlParameter("maxid",maxid),
               new SqlParameter("vencode",str[11]),
               new SqlParameter("CustomerId",str[12]),
               new SqlParameter("ShopId",str[13]),
               new SqlParameter("Imagefile",str[14])
            };
            DataTable ds = Select(ipr, "GetSoCalProduct");
            return ds;
        }
        /// <summary>
        /// 得到现货的数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalProductCount(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",str[1]),
               new SqlParameter("scode",str[0]),
               new SqlParameter("priceMin",str[2]),
               new SqlParameter("priceMax",str[3]),
               new SqlParameter("cat",str[6]),
               new SqlParameter("BalanceMin",str[7]),
               new SqlParameter("BalanceMax",str[8]),
               new SqlParameter("Cat2",str[5]),
               new SqlParameter("Cat1",str[4]),
               new SqlParameter("LastgrndMin",str[9]),
               new SqlParameter("LastgrndMax",str[10]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("vencode",str[11]),
               new SqlParameter("CustomerId",str[12]),
               new SqlParameter("ShopId",str[13]),
               new SqlParameter("Imagefile",str[14])
            };
            try
            {
                DataTable ds = Select(ipr, "GetSoCalProductCount");
                if (ds.Rows.Count > 0)
                {
                    return int.Parse(ds.Rows[0][0].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                return 0;
            }

        }
        /// <summary>
        /// 保存选货
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertSoCaloutsideProduct(DataTable dt)
        {
            string conStr = PubConstant.ConnectionString;
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conStr, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    #region
                    bulkCopy.DestinationTableName = "dbo.[SoCaloutsideProduct]";//目标表，就是说您将要将数据插入到哪个表中去
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
                    bulkCopy.ColumnMappings.Add("Balance", "Balance");//库存
                    bulkCopy.ColumnMappings.Add("ShopId", "Loc");//库存
                    bulkCopy.ColumnMappings.Add("UserId", "Def5");//操作人
                    bulkCopy.ColumnMappings.Add("Lastgrnd", "Lastgrnd");
                    bulkCopy.ColumnMappings.Add("Imagefile", "Imagefile");
                    bulkCopy.ColumnMappings.Add("UpdateDatetime", "Def6");//操作时间
                    bulkCopy.ColumnMappings.Add("Vencode", "Vencode");//数据源
                    bulkCopy.ColumnMappings.Add("SaleState", "ShowState");//  0  未开放   1  已开放   2  已选货
                    bulkCopy.ColumnMappings.Add("Def8", "Def8");//  0  未开放   1  已开放   2  已选货
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
        /// ------按照货号和店铺查找当前现货是否已经 选货
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="ShopId"></param>
        /// <returns></returns>
        public bool GetSoCalByScodeAndShopId(string Scode, string ShopId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("shopId",ShopId)
            };
            DataTable dt = Select(ipr, "GetSoCalByScodeAndShopId");
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
        /// ---------通过货号和数据源查找出当前现货数据的信息--选货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencod"></param>
        /// <returns></returns>
        public DataTable GetSoCalProductByScodeAndVencode(string scode, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("Vencode",vencode)
            };
            DataTable dt;
            try
            {
                dt = Select(ipr, "GetSoCalProductByScodeAndVencode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ---------通过货号和数据源查找出当前现货数据的信息--查看属性
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencod"></param>
        /// <returns></returns>
        public DataTable GetSoCaloutsideProductScodeAndVencode(string scode, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("Vencode",vencode)
            };
            DataTable dt;
            try
            {
                dt = Select(ipr, "GetSoCaloutsideProductScodeAndVencode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// -----现货 选货  通过货号查询出当前货号已存在的店铺
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetShopByScodeSoCal(string Scode, string UserID)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("ShopUser",UserID)
            };
            DataTable dt;
            try
            {
                dt = Select(ipr, "GetShopByScodeSoCal");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ----现货店铺仓库库存
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalWarehouseByStyle(string[] str, int minid, int maxid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("style",str[0]),
                new SqlParameter("priceMin",str[1]),
                new SqlParameter("priceMax",str[2]),
                new SqlParameter("cat",str[3]),
                new SqlParameter("BalanceMin",str[4]),
                new SqlParameter("BalanceMax",str[5]),
                new SqlParameter("Cat2",str[6]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("sql",""),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("shopid",str[9]),
                new SqlParameter("Imagefile",str[7]),
                new SqlParameter("Def3",str[8])
            };
            DataTable dt;
            try
            {
                dt = Select(ipr, "GetSoCalWarehouseByStyle");
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// ----现货店铺仓库库存个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalWarehouseByStyleCount(string[] str)
        {

            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("style",str[0]),
                new SqlParameter("priceMin",str[1]),
                new SqlParameter("priceMax",str[2]),
                new SqlParameter("cat",str[3]),
                new SqlParameter("BalanceMin",str[4]),
                new SqlParameter("BalanceMax",str[5]),
                new SqlParameter("Cat2",str[6]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("sql",""),
                new SqlParameter("shopid",str[9]),
                new SqlParameter("Imagefile",str[7]),
                new SqlParameter("Def3",str[8])
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalWarehouseByStyleCount");
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        ///------现货  货号查询仓库
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalWarehouseBySocde(string[] str, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("Scode",str[1]),
                new SqlParameter("Style",str[2]),
                new SqlParameter("Brand",str[3]),
                new SqlParameter("Type",str[5]),
                new SqlParameter("minBalance",str[6]),
                new SqlParameter("maxBalance",str[7]),  
                new SqlParameter("minPricea",str[8]),
                new SqlParameter("maxPricea",str[9]),
                new SqlParameter("minTime",str[10]),
                new SqlParameter("maxTime",str[11]),
                new SqlParameter("ShopId",str[12]),
                new SqlParameter("ShowState",str[13]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("Imagefile",str[14])
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalWarehouseBySocde");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ------现货  货号查询仓库总数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalWarehouseBySocdeCount(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("Scode",str[1]),
                new SqlParameter("Style",str[2]),
                new SqlParameter("Brand",str[3]),
                new SqlParameter("Type",str[5]),
                new SqlParameter("minBalance",str[6]),
                new SqlParameter("maxBalance",str[7]),  
                new SqlParameter("minPricea",str[8]),
                new SqlParameter("maxPricea",str[9]),
                new SqlParameter("minTime",str[10]),
                new SqlParameter("maxTime",str[11]),
                new SqlParameter("ShowState",str[13]),
                new SqlParameter("ShopId",str[12]),
                new SqlParameter("Imagefile",str[14])
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalWarehouseByScodeCount");
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        //----------------------------------5.26日修改
        /// <summary>
        ///  ----得到现货库存中当前货号已分配的库存--修改库存
        /// </summary>
        /// <returns></returns>
        public int GetSocalSumBalanceByScode(string Scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode)
            };
            try
            {
                int count;
                DataTable dt = Select(ipr, "GetSocalSumBalanceByScode");
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// -----得到当前现货的总库存
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public int GetSoCalProductSumBalanceByScode(string Scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode)
            };
            try
            {
                int count;
                DataTable dt = Select(ipr, "GetSoCalProductSumBalanceByScode");
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// -----------显示现货   所有店铺货号库存情况
        /// </summary>
        /// <returns></returns>
        public DataTable GetSoCalShopShowByVencodeAndScode(int minid, int maxid, string scode, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("skip",minid),
                new SqlParameter("take",maxid),
                new SqlParameter("scode",scode),
                new SqlParameter("vencode",vencode)
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalShopShowByVencodeAndScode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// --------------显示外部库存 现货库存个数   所有店铺货号库存情况
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int GetSoCalShopShowByVencodeAndScodeCount(string scode, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("vencode",vencode)
            };
            try
            {
                int count;
                DataTable dt = Select(ipr, "GetSoCalShopShowByVencodeAndScodeCount");
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// ---获得现货店铺中 当前货号当前店铺的货号的库存
        /// </summary>
        /// <param name="socde"></param>
        /// <param name="shopid"></param>
        public int GetSoCalProductBalanceCountByScodeAndShopId(string socde, string shopid, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[]
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",socde),
                new SqlParameter("shopId",shopid),
                new SqlParameter("Vencode",vencode)
            };
            try
            {
                int count;
                DataTable dt = Select(ipr, "GetSoCalProductBalanceCountByScodeAndShopId");
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0]["Balance"].ToString());
                }
                else
                {
                    count = 0;
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
        /// 获得当前店铺 当前货号的所有信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable GetSoCalProductShowStateByScodeAndShopId(string scode, string shopid, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[]
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("shopId",shopid),
                new SqlParameter("Vencode",vencode)
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalProductBalanceCountByScodeAndShopId");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        //---------------------------------5.27日修改
        /// <summary>
        ///  ----修改当前店铺当前货号的库存
        /// </summary>
        /// <param name="BalanceState"></param>
        /// <param name="Balance"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTiem"></param>
        /// <param name="Loc"></param>
        /// <param name="Vencode"></param>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public bool UpdateSoCalBalanceByScode(string BalanceState, string Balance, string UpdateUser, string UpdateTime, string Loc, string Vencode, string Scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("BalanceState",BalanceState),
                new SqlParameter("Balance",Balance),
                new SqlParameter("UpdateUser",UpdateUser),
                new SqlParameter("UpdateTime",UpdateTime),
                new SqlParameter("Loc",Loc),
                new SqlParameter("Vencode",Vencode),
                new SqlParameter("Scode",Scode)
            };
            try
            {
                string result = Update(ipr, "UpdateSoCalBalanceByScode");
                if (result == "修改成功")
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
        /// <summary>
        /// ---改变当前店铺当前货号的销售状态
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="ShowState"></param>
        /// <param name="Loc"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTime"></param>
        /// <returns></returns>
        public bool SalesState(string Scode, string ShowState, string Loc, string UpdateUser, string UpdateTime)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("Loc",Loc),
                new SqlParameter("UpdateUser",UpdateUser),
                new SqlParameter("UpdateTime",UpdateTime),
                new SqlParameter("ShowState",ShowState),
            };
            try
            {
                string result = Update(ipr, "SalesState");
                if (result == "修改成功")
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
        /// <summary>
        /// -------修改通过货号查找的款号下价格相同的价格]
        /// </summary>
        /// <returns></returns>
        public bool UpdateSoCalPriceByStyleAll(string style, string shopid, string price, string updateprice)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
               new SqlParameter("sql",""),
               new SqlParameter("style",style),
               new SqlParameter("pricee",price),
               new SqlParameter("shopid",shopid),
               new SqlParameter("updateprice",updateprice),
            };
            try
            {
                string result = Update(ipr, "UpdateSoCalPriceByStyleAll");
                if (result == "修改成功")
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
        /// <summary>
        /// ----修改当前店铺当前货号的价格
        /// </summary>
        /// <param name="BalanceState"></param>
        /// <param name="Balance"></param>
        /// <param name="UpdateUser"></param>
        /// <param name="UpdateTime"></param>
        /// <param name="Loc"></param>
        /// <param name="Vencode"></param>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public bool UpdateSoCalPriceByScode(string PriceState, string Price, string UpdateUser, string UpdateTime, string Loc, string Vencode, string Scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PriceState",PriceState),
                new SqlParameter("Price",Price),
                new SqlParameter("UpdateUser",UpdateUser),
                new SqlParameter("UpdateTime",UpdateTime),
                new SqlParameter("Loc",Loc),
                new SqlParameter("Vencode",Vencode),
                new SqlParameter("Scode",Scode)
            };
            try
            {
                string result = Update(ipr, "UpdateSoCalPriceByScode");
                if (result == "修改成功")
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
        /// <summary>
        /// ----通过款号得到当前店铺中当前款号下有多少个货号
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetSoCalScodeByStyle(string style, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Style",style),
                new SqlParameter("shopId",shopid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalScodeByStyle");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        //---------------------------------5.29日修改
        /// <summary>
        ///  -----查看商品属性时可以修改信息
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="descript"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public bool UpdateSoCaloutsideProduct(string Scode, string descript, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",Scode),
                new SqlParameter("shopid",shopid),
                new SqlParameter("Descript",descript) 
            };
            try
            {
                string result = Update(ipr, "UpdateSoCaloutsideProduct");
                if (result == "修改成功")
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
        /// <summary>
        /// ----查询当前合作客户所有店铺可下单的货物
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetPlaceAnOrder(string[] str, int minid, int maxid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",str[0]),
                new SqlParameter("cat1",str[1]),
                new SqlParameter("priceMin",str[2]),
                new SqlParameter("priceMax",str[3]),
                new SqlParameter("cat",str[4]),
                new SqlParameter("BalanceMin",str[5]),
                new SqlParameter("BalanceMax",str[6]),
                new SqlParameter("Cat2",str[7]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("style",str[8]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("CustomerId",str[9]),
                new SqlParameter("Imagefile",str[10]),
                new SqlParameter("ShopId",str[11]),
            };
            try
            {
                DataTable dt = Select(ipr, "GetPlaceAnOrder");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ----查询当前合作客户所有店铺可下单的货物 个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetPlaceAnOrder(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",str[0]),
                new SqlParameter("cat1",str[1]),
                new SqlParameter("priceMin",str[2]),
                new SqlParameter("priceMax",str[3]),
                new SqlParameter("cat",str[4]),
                new SqlParameter("BalanceMin",str[5]),
                new SqlParameter("BalanceMax",str[6]),
                new SqlParameter("Cat2",str[7]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("style",str[8]),
                new SqlParameter("CustomerId",str[9]),
                new SqlParameter("Imagefile",str[10]),
                new SqlParameter("ShopId",str[11]),
            };
            try
            {
                DataTable dt = Select(ipr, "GetPlaceAnOrderCount");
                int count = int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        //*********************************5.30
        /// <summary>
        /// ----下单时  获得单号信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetSoCaloutsideProductByScodeAndShopId(string scode, string shopid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("shopid",shopid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCaloutsideProductByScodeAndShopId");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ---新增现货订单
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool InsertSoCalOrder(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("orderId",str[0]),
                new SqlParameter("orderSocde",str[1]),
                new SqlParameter("orderTime",str[2]),
                new SqlParameter("scodesum",str[3]),
                new SqlParameter("scodeprice",str[4]),
                new SqlParameter("Loc",str[5]),
                new SqlParameter("userId",str[6]),
            };
            try
            {
                string result = Add(ipr, "InsertSoCalOrder");
                if (result == "添加成功")
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
        /// <summary>
        /// 查看订单信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalOrder(string[] str, int minid, int maxid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("OrderId",str[0]),
                new SqlParameter("SettlementState",str[1]),
                new SqlParameter("CustomerState",str[2]),
                new SqlParameter("SuppliersState",str[3]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("sqlbody",""),
                new SqlParameter("UserId",str[4]),
                new SqlParameter("minTime",str[5]),
                new SqlParameter("maxTime",str[6])
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalOrder");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查看订单信息--个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalOrder(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("OrderId",str[0]),
                new SqlParameter("SettlementState",str[1]),
                new SqlParameter("CustomerState",str[2]),
                new SqlParameter("SuppliersState",str[3]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("UserId",str[4]),
                new SqlParameter("minTime",str[5]),
                new SqlParameter("maxTime",str[6])

            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalOrderCount");
                int count = 0;
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
                }
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        ////********************6.1
        /// <summary>
        /// 订单详细信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetSoCalrOrderDetailed(string[] str, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("OrderId",str[0]),
                new SqlParameter("SettlementState",str[1]),
                new SqlParameter("CustomerState",str[2]),
                new SqlParameter("SuppliersState",str[3]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("UserId",str[4]),
                new SqlParameter("minTime",str[5]),
                new SqlParameter("maxTime",str[6]),
                new SqlParameter("OrderSocde",str[7]),
                new SqlParameter("cat",str[8]),
                new SqlParameter("cat2",str[9]),
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalrOrderDetailed");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 订单详细信息个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetSoCalrOrderDetailed(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("OrderId",str[0]),
                new SqlParameter("SettlementState",str[1]),
                new SqlParameter("CustomerState",str[2]),
                new SqlParameter("SuppliersState",str[3]),
                new SqlParameter("UserId",str[4]),
                new SqlParameter("minTime",str[5]),
                new SqlParameter("maxTime",str[6]),
                new SqlParameter("OrderSocde",str[7]),
                new SqlParameter("cat",str[8]),
                new SqlParameter("cat2",str[9]),
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalrOrderDetailedCount");
                int count = 0;
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// 订单确认
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="filed">字段 客户确认字段或者供应商确认字段或者结算状态字段</param>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public bool OrderOkState(string state, string filed, string orderId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Filed",filed),
                new SqlParameter("State",state),
                new SqlParameter("OrderId",orderId),
            };
            try
            {
                string result = Update(ipr, "OrderOkState");
                if (result == "修改成功")
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

        //*****************6.2
        /// <summary>
        ///   ---修改数量
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="OrderScode"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public bool UpdateOrderSum(string OrderId, string OrderScode, string Num, string filed, string state)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("OrderId",OrderId),
                new SqlParameter("OrderSum",Num),
                new SqlParameter("OrderScode",OrderScode),
                new SqlParameter("State",state),
                new SqlParameter("Filed",filed)
            };
            try
            {
                string result = Update(ipr, "UpdateOrderSum");
                if (result == "修改成功")
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
        /// <summary>
        /// ----通过货号和订单号查找当信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="OrderScode"></param>
        /// <returns></returns>
        public DataTable GetSoCalOrderByOrderIdAndScode(string OrderId, string OrderScode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("OrderId",OrderId),
                new SqlParameter("OrderScode",OrderScode),
            };
            try
            {
                DataTable dt = Select(ipr, "GetSoCalOrderByOrderIdAndScode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 现货 导入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertSoCalProduct(DataTable dt)
        {
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Pricea"].ToString() == "")
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connectionString);
            var info = pdc.BrandVen;
            for (int i = 0; i < dt.Rows.Count; i++)//导入数据之前进行品牌转换
            {
                foreach (BrandVen temp in info)
                {
                    if (dt.Rows[i]["Cat"].ToString() == temp.BrandNameVen)
                    {
                        dt.Rows[i]["Cat"] = temp.BrandAbridge;
                    }
                }
            }
            var info1 = pdc.producttypeVen;
            for (int i = 0; i < dt.Rows.Count; i++) //导入数据之前进行类别转换
            {
                foreach (producttypeVen temp in info1)
                {
                    if (dt.Rows[i]["Cat2"].ToString() == temp.TypeNameVen)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeNo;
                    }
                }
            }
            try
            {
                using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    sqlbc.DestinationTableName = "dbo.[SoCalProduct]";//需要插入的表
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
                    sqlbc.ColumnMappings.Add("Disca", "Disca");
                    sqlbc.ColumnMappings.Add("Discb", "Discb");
                    sqlbc.ColumnMappings.Add("Discc", "Discc");
                    sqlbc.ColumnMappings.Add("Discd", "Discd");
                    sqlbc.ColumnMappings.Add("Disce", "Disce");
                    sqlbc.ColumnMappings.Add("Vencode", "Vencode");
                    sqlbc.ColumnMappings.Add("Model", "Model");
                    sqlbc.ColumnMappings.Add("Rolevel", "Rolevel");
                    sqlbc.ColumnMappings.Add("Roamt", "Roamt");
                    sqlbc.ColumnMappings.Add("Stopsales", "Stopsales");
                    sqlbc.ColumnMappings.Add("Loc", "Loc");
                    sqlbc.ColumnMappings.Add("Balance", "Balance");
                    sqlbc.ColumnMappings.Add("Lastgrnd", "Lastgrnd");
                    sqlbc.ColumnMappings.Add("Imagefile", "Imagefile");
                    sqlbc.ColumnMappings.Add("PrevStock", "PrevStock");
                    sqlbc.ColumnMappings.Add("Def2", "Def2");
                    sqlbc.ColumnMappings.Add("Def3", "Def3");
                    sqlbc.ColumnMappings.Add("Def4", "Def4");
                    sqlbc.ColumnMappings.Add("Def5", "Def5");
                    sqlbc.ColumnMappings.Add("Def6", "Def6");
                    sqlbc.ColumnMappings.Add("Def7", "Def7");
                    sqlbc.ColumnMappings.Add("Def8", "Def8");
                    sqlbc.ColumnMappings.Add("Def9", "Def9");
                    sqlbc.ColumnMappings.Add("Def10", "Def10");
                    sqlbc.ColumnMappings.Add("Def11", "Def11");
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
        /// 当前数据是否存在
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="time"></param>
        /// <param name="Vencode"></param>
        /// <returns></returns>
        public bool SelectSoCalProductByTimeAndScodeAndVencode(string Scode, string time, string Vencode)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                new SqlParameter("sql",""),
                new SqlParameter("Time",time),
                new SqlParameter("Scode",Scode),
                new SqlParameter("Vencode",Vencode),
                };
                DataTable dt = Select(ipr, "SelectSoCalProductByTimeAndScodeAndVencode");
                if (dt.Rows.Count > 0)
                {
                    return true;//表示已经存在
                }
                else
                {
                    return false;//表示不存在
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        /// <summary>
        /// --------导入数据时将将需要重复数据进行修改
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="time"></param>
        /// <param name="vencode"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool UpdateSoCalProductByFile(string scode, string time, string vencode, string balance)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("time",time),
                new SqlParameter("Balance",balance),
                new SqlParameter("Scode",scode),
                new SqlParameter("Vencode",vencode),
            };
            try
            {
                string result = Update(ipr, "UpdateSoCalProductByFile");
                if (result == "修改成功")
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

        //**************6.12
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable LookSoCalPrdouctAttr(string scode, string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("Vencode",vencode),
            };
            try
            {
                DataTable dt = Select(ipr, "LookSoCalPrdouctAttr");
                dt.Columns.Add("TypeNo");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["TypeNo"] = dt.Rows[i]["Cat2"];
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }




        //******************.6.30库存汇总功能
        /// <summary>
        /// 库存汇总分页  ---货号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockMergeScodePage(string [] str,string customer) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("customer",customer),
                    new SqlParameter("Style",str[0]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("Cat",str[2]),
                    new SqlParameter("Cat2",str[3]),
                    new SqlParameter("minPrice",str[4]),
                    new SqlParameter("maxPrice",str[5]),
                    new SqlParameter("minBalance",str[6]),
                    new SqlParameter("maxBalance",str[7]),
                    new SqlParameter("minId",str[8]),
                    new SqlParameter("maxId",str[9]),
                };
                DataTable dt = Select(ipr, "ProductStockMergeScodePage");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 库存汇总总数 ---货号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockMergeScodeCount(string[] str, string customer)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("customer",customer),
                    new SqlParameter("Style",str[0]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("Cat",str[2]),
                    new SqlParameter("Cat2",str[3]),
                    new SqlParameter("minPrice",str[4]),
                    new SqlParameter("maxPrice",str[5]),
                    new SqlParameter("minBalance",str[6]),
                    new SqlParameter("maxBalance",str[7]),
                };
                DataTable dt = Select(ipr, "ProductStockMergeScodeCount");
                int count = 0;
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else 
                {
                    count = 0;
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
        /// 库存汇总分页 ---款号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockMergeStylePage(string[] str, string customer) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("CustomerId",customer),
                    new SqlParameter("Style",str[0]),
                    new SqlParameter("cat",str[1]),
                    new SqlParameter("Cat2",str[2]),
                    new SqlParameter("priceMin",str[3]),
                    new SqlParameter("priceMax",str[4]),
                    new SqlParameter("BalanceMin",str[5]),
                    new SqlParameter("BalanceMax",str[6]),
                    new SqlParameter("minid",str[7]),
                    new SqlParameter("maxid",str[8]),
                };
                DataTable dt = Select(ipr, "ProductStockMergeStylePage");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 库存汇总总数 ---款号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockMergeStyleCount(string[] str, string customer) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("CustomerId",customer),
                    new SqlParameter("Style",str[0]),
                    new SqlParameter("cat",str[1]),
                    new SqlParameter("Cat2",str[2]),
                    new SqlParameter("priceMin",str[3]),
                    new SqlParameter("priceMax",str[4]),
                    new SqlParameter("BalanceMin",str[5]),
                    new SqlParameter("BalanceMax",str[6]),
                };
                DataTable dt = Select(ipr, "ProductStockMergeStyleCount");
                int count = 0;
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// 库存汇总---查看属性
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public DataTable GetdistinctProductStockByScod(string Scode) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("Scode",Scode)
                };
                DataTable dt = Select(ipr, "GetdistinctProductStockByScod");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
