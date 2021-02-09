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
    public partial class ProductStockDAL : dataoperating
    {
        /// <summary>
        /// 通过配置获取连接字符串
        /// </summary>
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        //*多条件查询和分页**///
        /// <summary> 
        ///查询并分页 已完成存储过程
        /// </summary>
        /// <param name="str">查询条件参数</param>
        /// <param name="count">跳过多少页</param>
        /// <returns></returns>
        public DataTable SerachShowProductStock(Dictionary<string,string> dic, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",dic["Style"]),
               new SqlParameter("scode",dic["Scode"]),
               new SqlParameter("priceMin",dic["PriceMin"]),
               new SqlParameter("priceMax",dic["PriceMax"]),
               new SqlParameter("cat",dic["Brand"]),
               new SqlParameter("BalanceMin",dic["StockMin"]),
               new SqlParameter("BalanceMax",dic["StockMax"]),
               new SqlParameter("Cat2",dic["Type"]),
               new SqlParameter("Cat1",dic["Season"]),
               new SqlParameter("LastgrndMin",dic["TimeMin"]),
               new SqlParameter("LastgrndMax",dic["TimeMax"]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("vencode",dic["Vencode"]),
               new SqlParameter("CustomerId",dic["CustomerId"]),
               new SqlParameter("isCheckProduct",dic["isCheckProduct"]),
               new SqlParameter("ShopId",dic["ShopId"]),
               new SqlParameter("Imagefile",dic["Imagefile"]),
               new SqlParameter("Descript",dic["Descript"]),
               new SqlParameter("minid",minid),
               new SqlParameter("maxid",maxid),
               new SqlParameter("orderValue",dic["orderValue"]),
               new SqlParameter("order",dic["order"]),
               new SqlParameter("orderSql","")
            };
            DataTable ds = Select(ipr, "SelectPjByScodePage");
            return ds;
        }
        /// <summary>
        /// 根据数据源名获得数据源编号
        /// </summary>
        /// <param name="sourname"></param>
        /// <returns></returns>
        public string GetSourcode(string sourname)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            var info = pddc.productsource.Where(a => a.sourceName == sourname);
            string scourccode = "";
            foreach (var temp in info)
            {
                scourccode = temp.SourceCode;
            }
            return scourccode;
        }
        /// <summary>
        /// 根据数据源code获得数据源名字
        /// </summary>
        /// <param name="sourname"></param>
        /// <returns></returns>
        public string GetSourname(string sourname)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            var info = pddc.productsource.Where(a => a.SourceCode == sourname);
            string scourccode = "";
            foreach (var temp in info)
            {
                scourccode = temp.sourceName;
            }
            return scourccode;
        }
        /// <summary>
        /// 查询条数 已完成存储过程
        /// </summary>
        /// <param name="str"></param>
        /// <param name="skip"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public DataTable SerachShowProductStock(Dictionary<string,string> dic, out int count)
        {
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",dic["Style"]),
               new SqlParameter("scode",dic["Scode"]),
               new SqlParameter("priceMin",dic["PriceMin"]),
               new SqlParameter("priceMax",dic["PriceMax"]),
               new SqlParameter("cat",dic["Brand"]),
               new SqlParameter("BalanceMin",dic["StockMin"]),
               new SqlParameter("BalanceMax",dic["StockMax"]),
               new SqlParameter("Cat2",dic["Type"]),
               new SqlParameter("Cat1",dic["Season"]),
               new SqlParameter("LastgrndMin",dic["TimeMin"]),
               new SqlParameter("LastgrndMax",dic["TimeMax"]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("vencode",dic["Vencode"]),
               new SqlParameter("CustomerId",dic["CustomerId"]),
               new SqlParameter("isCheckProduct",dic["isCheckProduct"]),
               new SqlParameter("ShopId",dic["ShopId"]),
               new SqlParameter("Imagefile",dic["Imagefile"]),
               new SqlParameter("Descript",dic["Descript"])
            };
            DataTable ds = Select(ipr, "SelectPjByScode");
            count = int.Parse(ds.Rows[0][0].ToString());
            return ds;
        }
        /// <summary>
        /// 按照款号查询  已完成存储过程
        /// </summary>
        /// <param name="str">查询的字段</param>
        /// <param name="count">跳过多少条查询</param>
        /// <returns></returns>
        public DataTable ProductStcokByStyle(string[] str, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            string cat = str[4].ToString();
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",str[1]),
               new SqlParameter("priceMin",str[2]),
               new SqlParameter("priceMax",str[3]),
               new SqlParameter("cat",cat),
               new SqlParameter("BalanceMin",str[5]),
               new SqlParameter("BalanceMax",str[6]),
               new SqlParameter("Cat2",str[7]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("minid",minid),
               new SqlParameter("maxid",maxid),
               new SqlParameter("Vencode",str[10]),
               new SqlParameter("CustomerId",str[11])
               
            };
            DataTable ds = Select(ipr, "SelectByPjPage");
            return ds;
        }
        /// <summary>
        /// 款号查找   已完成存储 过程
        /// </summary>
        /// <param name="str">条件</param>
        /// <returns></returns>
        public DataTable ProductStcokByStyle(string[] str, out int count)
        {
            string cat = str[4].ToString();
            IDataParameter[] ipr = new IDataParameter[]{
               new SqlParameter("style",str[1]),
               new SqlParameter("priceMin",str[2]),
               new SqlParameter("priceMax",str[3]),
               new SqlParameter("cat",cat),
               new SqlParameter("BalanceMin",str[5]),
               new SqlParameter("BalanceMax",str[6]),
               new SqlParameter("Cat2",str[7]),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("Vencode",str[10]),
               new SqlParameter("CustomerId",str[11])
            };
            DataTable ds = Select(ipr, "SelectByPj");
            count = int.Parse(ds.Rows[0]["count"].ToString());
            return ds;
        }
        /// <summary>
        /// 数据源下拉列表绑定
        /// </summary>
        /// <returns></returns>
        public List<productsource> DropList()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            var info = pddc.productsource;
            List<productsource> list = new List<productsource>();
            foreach (productsource temp in info)
            {
                productsource pd = new productsource();
                pd.SourceCode = temp.SourceCode;
                pd.sourceName = temp.sourceName;
                list.Add(pd);
            }
            return list;
        }
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable Excel(string[] str)
        {
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
               new SqlParameter("vencode",str[11]),
               new SqlParameter("orderValue",str[12]),
               new SqlParameter("order",str[13]),
               new SqlParameter("orderSql","")
            };
            DataTable ds = Select(ipr, "SelectPjByScodeExcel");
            return ds;
        }
        /*------------------------------外部库存---------------------------------------*/
        /// <summary>
        /// 按照货号和数据源查出当前货号的总库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int CountByScode(string scode, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("Vencdoe",vencode),
            };
            DataTable dt = Select(ipr, "CountByScode");
            if (dt.Rows.Count > 0)
            {
                int count = int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 通过数据源ID找出数据源名称
        /// </summary>
        /// <param name="vencdoe"></param>
        /// <returns></returns>
        public string SelectVenNameByVencode(string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sourcecode",vencode)
            };
            try
            {
                DataTable dt = Select(ipr, "SelectVenNameByVencode");
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 通过货号和数据源查找出当前数据的信息
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="socde"></param>
        /// <returns></returns>
        public DataTable SelectDataByScodeAndVencode(string vencode, string scode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("Vencode",vencode)
            };
            return Select(ipr, "SelectDataByScodeAndVencode");
        }
        //****************************7.2订单管理
        public bool InsertOrder(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("OrderGoodsNo",str[0]),
                    new SqlParameter("OrderUser",str[1]),
                    new SqlParameter("OrderTime",str[2]),
                    new SqlParameter("Brand",str[3]),
                    new SqlParameter("ExpectTime",str[4]),
                };
                string reslut = Add(ipr, "InsertOrderGoods");
                if (reslut == "添加成功")
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
        /// 得到所有订单信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetOrderGoods(Dictionary<string,string> dic, int minid, int maxid)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("OrderGoodsNo",dic["OrderId"]),
                    new SqlParameter("OrderBrand",dic["Brand"]),
                    new SqlParameter("minTime",dic["MinTime"]),
                    new SqlParameter("maxTime",dic["MaxTime"]),
                    new SqlParameter("minId",minid),
                    new SqlParameter("maxId",maxid),
                };
                DataTable dt = Select(ipr, "GetOrderGoods");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到所有订单个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetOrderGoodsCount(Dictionary<string,string> dic)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("OrderGoodsNo",dic["OrderId"]),
                    new SqlParameter("OrderBrand",dic["Brand"]),
                    new SqlParameter("minTime",dic["MinTime"]),
                    new SqlParameter("maxTime",dic["MaxTime"]),
                };
                DataTable dt = Select(ipr, "GetOrderGoodsCount");
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
        /// 导入当前订单的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertProductStockOrder(DataTable dt, string OrderId, string Vencode)
        {
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["Pricea"].ToString() == "")
            //    {
            //        dt.Rows.RemoveAt(i);
            //    }
            //}
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connectionString);
            var info = pdc.brand.Where(a => a.BrandName == Vencode);
            for (int i = 0; i < dt.Rows.Count; i++)//导入数据之前进行品牌转换
            {
                foreach (brand temp in info)
                {
                    if (dt.Rows[i]["Cat"].ToString() == temp.BrandName)
                    {
                        dt.Rows[i]["Cat"] = temp.BrandAbridge;
                    }
                }
            }
            var info1 = pdc.producttypeVen.Where(a => a.Vencode == Vencode);
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
            dt.Columns.Add("OrderId");//插入订单编号列
            for (int i = 0; i < dt.Rows.Count; i++)  //并将订单编号插入表中
            {
                dt.Rows[i]["OrderId"] = OrderId;
            }
            try
            {
                using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    sqlbc.DestinationTableName = "dbo.[productstockOrder]";//需要插入的表
                    sqlbc.ColumnMappings.Add("OrderId", "OrderId");
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
        public bool IsProductStockOrder(string Scode, string OrderNo)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("OrderNo",OrderNo),
                };
                DataTable dt = Select(ipr, "IsProductStockOrder");
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
        public bool UpdateProductStockOrder(string scode, string balance, string OrderNo, string Pricea, string Pricee)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("OrderNo",OrderNo),
                new SqlParameter("Balance",balance),
                new SqlParameter("Pricea",Pricea),
                new SqlParameter("Pricee",Pricee),
            };
            try
            {
                string result = Update(ipr, "UpdateProductStockOrder");
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
        /// 上传成功 修改上传时间
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public bool UpdateOrderGoodsImportTime(string OrderNo, string Time, string countstyle, string countbalance)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("OrderNo",OrderNo),
                    new SqlParameter("ImportTime",Time),
                    new SqlParameter("CountStyle",countstyle),
                    new SqlParameter("CountBalance",countbalance)
                };
                string result = Update(ipr, "UpdateOrderGoodsImportTime");
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
        /// 订单详情
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockOrderPage(Dictionary<string,string> dic, string customer)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("customer",customer),
                    new SqlParameter("Style",dic["Style"]),
                    new SqlParameter("Scode",dic["Scode"]),
                    new SqlParameter("Cat",dic["Brand"]),
                    new SqlParameter("Cat2",dic["Type"]),
                    new SqlParameter("minPrice",dic["PriceMin"]),
                    new SqlParameter("maxPrice",dic["PriceMax"]),
                    new SqlParameter("minBalance",dic["StockMin"]),
                    new SqlParameter("maxBalance",dic["StockMax"]),
                    new SqlParameter("OrderNo",dic["OrderId"]),
                    new SqlParameter("minId",dic["MinId"]),
                    new SqlParameter("maxId",dic["MaxId"]),
                };
                DataTable dt = Select(ipr, "ProductStockOrderPage");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 订单详情  个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockOrderPageCount(Dictionary<string,string> dic,string customer)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("customer",customer),
                    new SqlParameter("Style",dic["Style"]),
                    new SqlParameter("Scode",dic["Scode"]),
                    new SqlParameter("Cat",dic["Brand"]),
                    new SqlParameter("Cat2",dic["Type"]),
                    new SqlParameter("minPrice",dic["PriceMin"]),
                    new SqlParameter("maxPrice",dic["PriceMax"]),
                    new SqlParameter("minBalance",dic["StockMin"]),
                    new SqlParameter("maxBalance",dic["StockMax"]),
                    new SqlParameter("OrderNo",dic["OrderId"])
                };
                DataTable dt = Select(ipr, "ProductStockOrderPageCount");
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
        /// -----统计当前单号有多少款 总公共多少件货
        /// </summary>
        /// <returns></returns>
        public DataTable ProductStockOrderStatistics(string orderid)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("OrderId",orderid),
                };
                DataTable dt = Select(ipr, "ProductStockOrderStatistics");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        //////*********************************7.22
        /// <summary>
        /// 标记当前商品是否为残次品
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string Marker(string scode, string vencode)
        {
            try
            {
                pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
                var info = pddc.productstock.Where(a => a.Scode == scode && a.Vencode == vencode);
                foreach (productstock temp in info)
                {
                    temp.Def2 = "1";
                }
                pddc.SubmitChanges();
                return "标记成功！";
            }
            catch (Exception)
            {
                return "标记失败";

            }
        }
        /// <summary>
        /// 此货号当前残次品的数量
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int MarkerCount(string scode, string vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<DefectiveRemark> info = pddc.DefectiveRemark.Where(a => a.Scode == scode && a.Vencode == vencode).ToList();
                int count = info.Count;
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 查出库存表中当前数据的信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<productstock> SelectProduct(string scode, string vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<productstock> list = pddc.productstock.Where(a => a.Scode == scode && a.Vencode == vencode).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 将标记的残次品加入到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string InsertDefectiveRemark(List<productstock> list, string ProductRemark, string ScodeMarKer, int sum)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                DefectiveRemark dfr = new DefectiveRemark()
                {
                    #region  赋值
                    Def2 = list[0].Def2,
                    Def3 = list[0].Def3,
                    Def4 = list[0].Def4,
                    Def5 = list[0].Def5,
                    Def6 = list[0].Def6,
                    Def7 = list[0].Def7,
                    Def8 = list[0].Def8,
                    Def9 = list[0].Def9,
                    Def10 = list[0].Def10,
                    Def11 = list[0].Def11,
                    Balance = sum,
                    Bcode = list[0].Bcode,
                    Bcode2 = list[0].Bcode2,
                    Descript = list[0].Descript,
                    Disca = list[0].Disca,
                    Discb = list[0].Discb,
                    Discc = list[0].Discc,
                    Discd = list[0].Discd,
                    Disce = list[0].Disce,
                    Imagefile = list[0].Imagefile,
                    Model = list[0].Model,
                    Pricea = list[0].Pricea,
                    Priceb = list[0].Priceb,
                    Pricec = list[0].Pricec,
                    Priced = list[0].Priced,
                    Pricee = list[0].Pricee,
                    Vencode = list[0].Vencode,
                    ProductRemark = ProductRemark,
                    ScodeMarKer = ScodeMarKer,
                    Scode = list[0].Scode,
                    Unit = list[0].Unit,
                    Style = list[0].Style,
                    Size = list[0].Size,
                    Cat = list[0].Cat,
                    Cat1 = list[0].Cat1,
                    Cat2 = list[0].Cat2,
                    Cdescript = list[0].Cdescript,
                    Loc = list[0].Loc,
                    Currency = list[0].Currency,
                    Clolor = list[0].Clolor,
                    Lastgrnd = DateTime.Now
                    #endregion
                };
                pddc.DefectiveRemark.InsertOnSubmit(dfr);
                pddc.SubmitChanges();
                return "标记成功";
            }
            catch (Exception)
            {
                return "标记失败！";
                throw;
            }
        }
        /// <summary>
        /// 查询所有标记货品
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetDefectiveRemark(string[] str, int minid, int maxid)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("ScodeMarker",str[0]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("Cat",str[2]),
                    new SqlParameter("Cat2",str[3]),
                    new SqlParameter("Style",str[4]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid)
                };
                DataTable dt = Select(ipr, "GetDefectiveRemark");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查询所有标记货品个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetDefectiveRemarkCount(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("ScodeMarker",str[0]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("Cat",str[2]),
                    new SqlParameter("Cat2",str[3]),
                    new SqlParameter("Style",str[4]),
                };
                DataTable dt = Select(ipr, "GetDefectiveRemarkCount");
                int Count = 0;
                if (dt.Rows.Count > 0)
                {
                    Count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    Count = 0;
                }
                return Count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// ---得到合并数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetMergeDefectiveRemark(string[] str, int minid, int maxid)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Scode",str[0]),
                    new SqlParameter("cat",str[1]),
                    new SqlParameter("cat2",str[2]),
                    new SqlParameter("style",str[3]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid),
                };
                DataTable dt = Select(ipr, "GetMergeDefectiveRemark");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ---得到合并数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetMergeDefectiveRemarkCount(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Scode",str[0]),
                    new SqlParameter("cat",str[1]),
                    new SqlParameter("cat2",str[2]),
                    new SqlParameter("style",str[3]),
                };
                DataTable dt = Select(ipr, "GetMergeDefectiveRemarkCount");
                int Count = 0;
                if (dt.Rows.Count > 0)
                {
                    Count = int.Parse(dt.Rows[0][0].ToString());
                }
                return Count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 通过标记编号获得数据
        /// </summary>
        /// <returns></returns>
        public List<DefectiveRemark> GetDefectiveRemarkByScodeMarker(string scodemarker)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<DefectiveRemark> list = pddc.DefectiveRemark.Where(a => a.ScodeMarKer == scodemarker).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 修改标记品的价格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UpdatePrice(decimal[] str, string scodemarker)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                var info = pddc.DefectiveRemark.Where(a => a.ScodeMarKer == scodemarker);
                foreach (DefectiveRemark temp in info)
                {
                    temp.Pricea = str[0];
                    temp.Priceb = str[1];
                    temp.Pricec = str[2];
                    temp.Priced = str[3];
                }
                pddc.SubmitChanges();
                return "修改成功！";
            }
            catch (Exception)
            {
                return "修改失败！";
                throw;
            }
        }
        /// <summary>
        /// -得到当前货号被标记的总库存
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int GetDefctiveRemarkBalanceCount(string vencode, string scode)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("vencode",vencode),
                    new SqlParameter("Scode",scode)
                };
                DataTable dt = Select(ipr, "GetDefctiveRemarkBalanceCount");
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
        /// 得到所有数据源
        /// </summary>
        /// <returns></returns>
        public List<productsource> GetVencodeProduct()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<productsource> Checklist = new List<productsource>();
                List<productsource> list = pddc.productsource.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].SourceCode.Length < 3)
                    {
                        Checklist.Add(list[i]);
                    }
                }
                return Checklist;

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查询当前标记编号是否存在
        /// </summary>
        /// <param name="ScodeMarker"></param>
        /// <returns></returns>
        public bool IsExistScodeMarker(string ScodeMarker)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<DefectiveRemark> list = pddc.DefectiveRemark.Where(a => a.ScodeMarKer == ScodeMarker).ToList();
                if (list.Count > 0)
                {
                    return false;
                }
                else
                {
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
        /// 通过货号的到当前货号的最大标记号
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public string GetMaxScodeMarker(string Scode)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("Scode",Scode)
                };
                DataTable dt = Select(ipr, "GetMaxScodeMarker");
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ScodeMarKer"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "错误:" + ex.Message;
                throw;
            }
        }
        /// <summary>
        /// 取消当前标记
        /// </summary>
        /// <param name="ScodeMarker"></param>
        /// <returns></returns>
        public string DeleteScodeMarker(string ScodeMarker)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                var info = pddc.DefectiveRemark.Where(a => a.ScodeMarKer == ScodeMarker);
                foreach (DefectiveRemark temp in info)
                {
                    pddc.DefectiveRemark.DeleteOnSubmit(temp);
                }
                pddc.SubmitChanges();
                return "取消成功！";
            }
            catch (Exception)
            {
                return "取消失败！";
                throw;
            }
        }
        /// </summary>
        /// 得到货号图
        /// <param name="scode"></param>
        /// <returns></returns>
        public List<scodepic> GetImageFile(string scode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<scodepic> list = pddc.scodepic.Where(a => a.scode == scode && a.Def4 == "1").ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到款号图
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public List<stylepic> GetStyleImageFile(string style)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<stylepic> list = pddc.stylepic.Where(a => a.style == style && a.Def4 == "1").ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 暂停开放某个数据源的某件货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string CloseStock(string scode, string vencode, string state)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                var info = pddc.productstock.Where(a => a.Scode == scode && a.Vencode == vencode);
                foreach (productstock temp in info)
                {
                    temp.Def4 = state;
                }
                pddc.SubmitChanges();
                return "标记成功！";
            }
            catch (Exception)
            {
                return "标记错误！";
                throw;
            }
        }
        /// <summary>
        /// ---得到需要开放或者关闭的货号以及数据源
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetProductOpenScode(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
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
               new SqlParameter("vencode",str[11]),
               new SqlParameter("CustomerId",str[12]),
               new SqlParameter("Imagefile",str[15]),
               new SqlParameter("Descript",str[16])
                };
                DataTable dt = Select(ipr, "GetProductOpenScode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ---开放或者暂停 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="state">开放或者暂停   暂停  1   开放  2</param>
        /// <returns></returns>
        public string ProductOpenScode(DataTable dt, string state)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var info = pddc.productstock.Where(a => a.Scode == dt.Rows[i]["Scode"].ToString() && a.Vencode == dt.Rows[i]["Vencode"].ToString());
                    foreach (productstock temp in info)
                    {
                        temp.Def4 = state;
                    }
                    pddc.SubmitChanges();
                }
                return "操作成功！";
            }
            catch (Exception)
            {
                return "操作失败！";

                throw;
            }
        }
        ////////8.11
        /// <summary>
        /// 得到供应商品牌
        /// </summary>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<BrandVen> GetBrandByVencode(string vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandVen> list = pddc.BrandVen.Where(a => a.Vencode == vencode).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到供应商类别
        /// </summary>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<producttypeVen> GetProductTypeVencode(string Vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<producttypeVen> list = pddc.producttypeVen.Where(a => a.Vencode == Vencode).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到供应商类别  供应商+大类别编号
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="TypeBigId"></param>
        /// <returns></returns>
        public List<producttypeVen> GetProductTypeVencode(string Vencode, int TypeBigId)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<producttypeVen> list = pddc.producttypeVen.Where(a => a.Vencode == Vencode && a.BigId == TypeBigId).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到所有大类别
        /// </summary>
        /// <returns></returns>
        public List<productbigtype> GetProductBigTypeVencode()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<productbigtype> list = pddc.productbigtype.ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 打开或者暂停库存
        /// </summary>
        /// <returns></returns>
        public bool OpenPorudctBrand(string[] str, string openstate, string vencode)
        {
            try
            {
                string sql = "update productstock set Def4='" + openstate + "' where Vencode='" + vencode + "' and Cat in(''";
                for (int i = 0; i < str.Length; i++)
                {
                    sql += ",'" + str[i] + "'";
                }
                sql += ")";
                int count = DbHelperSQL.ExecuteSql(sql);
                if (count > 0)
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
        /// 打开或者暂停库存
        /// </summary>
        /// <returns></returns>
        public bool OpenPorudctType(string[] str, string openstate, string vencode)
        {
            try
            {
                string sql = "update productstock set Def4='" + openstate + "' where Vencode='" + vencode + "' and Cat2 in(''";
                for (int i = 0; i < str.Length; i++)
                {
                    sql += ",'" + str[i] + "'";
                }
                sql += ")";
                int count = DbHelperSQL.ExecuteSql(sql);
                if (count > 0)
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
        /// 得到所有数据源
        /// </summary>
        /// <returns></returns>
        public List<productsource> GetProductSourceAll()
        {
            try
            {
                pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
                List<productsource> list = pddc.productsource.Where(a => a.SourceCode.Length < 4).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到数据源中开放或者未开放的货物数量
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int GetProductStockOpenCount(string vencode, string Open)
        {
            try
            {
                string sql = "";
                if (Open == "1")
                {
                    sql = "select COUNT(*) from productstock where Vencode=" + vencode + " and Def4=1";
                }
                else
                {
                    sql = "select COUNT(*) from productstock where Vencode=" + vencode + " and (Def4=2 or Def4 is null)";
                }
                DataSet ds = DbHelperSQL.Query(sql);
                DataTable dt = ds.Tables[0];
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
        /// 开放或者暂停整个数据源
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="openstate"></param>
        /// <returns></returns>
        public bool OpenAll(string vencode, string openstate)
        {
            try
            {
                string sql = "update productstock set Def4='" + openstate + "' where Vencode='" + vencode + "'";
                int result = DbHelperSQL.ExecuteSql(sql);
                if (result > 0)
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
        /// 导出数据库存按照款号导出
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable OutPutExcelStyle(string[] str) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("Style",str[0]),
                    new SqlParameter("Cat",str[1]),
                    new SqlParameter("Cat2",str[2]),
                    new SqlParameter("priceMin",str[3]),
                    new SqlParameter("priceMax",str[4]),
                    new SqlParameter("BalanceMin",str[5]),
                    new SqlParameter("BalanceMax",str[6]),
                    new SqlParameter("Vencode",str[7]),
                    new SqlParameter("CustomerId",str[8])

             };
            DataTable dt = Select(ipr, "OutPutExcelStyle");
            return dt;
        }
    }
}
