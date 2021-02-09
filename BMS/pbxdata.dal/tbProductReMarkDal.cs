using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using System.Data;
using System.Data.SqlClient;
using Maticsoft.DBUtility;
namespace pbxdata.dal
{
    public class tbProductReMarkDal : dataoperating
    {
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        /// <summary>
        /// 按照条件查找所有数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetProductReMarkTb(Dictionary<string,string> dic, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("ProductId",dic["ProductId"]),
                new SqlParameter("Style",dic["Style"]),
                new SqlParameter("minBalance",dic["HkBalancemin"]),
                new SqlParameter("maxBalance",dic["HkBalancemax"]),
                new SqlParameter("SJBalancemin",dic["MinBalance"]),
                new SqlParameter("SJBalancemax",dic["MinBalance"]),
                new SqlParameter("Priceamin",dic["Pricemin"]),
                new SqlParameter("Priceamax",dic["Pricemax"]),
                new SqlParameter("SaleState",dic["SaleState"]),
                new SqlParameter("ShopId",dic["ShopName"]),
                new SqlParameter("Brand",dic["Cat"]),
                new SqlParameter("Type",dic["Cat2"]),
                new SqlParameter("Season",dic["Cat1"]),
                new SqlParameter("HkBlance",dic["HkBalance"]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
            };
            try
            {
                DataTable dt = Select(ipr, "GetProductReMarkTb");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查找出符合当前条件的数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetProductReMarkTbCount(Dictionary<string,string> dic) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("ProductId",dic["ProductId"]),
                new SqlParameter("Style",dic["Style"]),
                new SqlParameter("minBalance",dic["HkBalancemin"]),
                new SqlParameter("maxBalance",dic["HkBalancemax"]),
                new SqlParameter("SJBalancemin",dic["MinBalance"]),
                new SqlParameter("SJBalancemax",dic["MinBalance"]),
                new SqlParameter("Priceamin",dic["Pricemin"]),
                new SqlParameter("Priceamax",dic["Pricemax"]),
                new SqlParameter("SaleState",dic["SaleState"]),
                new SqlParameter("ShopId",dic["ShopName"]),
                new SqlParameter("Brand",dic["Cat"]),
                new SqlParameter("Type",dic["Cat2"]),
                new SqlParameter("Season",dic["Cat1"]),
                new SqlParameter("HkBlance",dic["HkBalance"])
            };
            try
            {
                DataTable dt = Select(ipr,"GetProductReMarkTbCount");
                int count =int.Parse( dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 得到店铺名称
        /// </summary>
        /// <returns></returns>
        public DataTable GetShopName() 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql","")
            };
            return Select(ipr, "GetTbShopName");
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from  Productstock ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 添加-获取淘宝商品列表数据(淘宝在售-仓库中的数据) 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Add(List<CommandInfo> list)
        {
            bool flag = false;

            int count = DbHelperSQL.ExecuteSqlTran(list);
            flag = count >= list.Count ? true : false;

            return flag;
        }
        /// <summary>
        /// 查找相同的编号
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public DataSet GetProductReMarkList(string strwhere) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from  tbProductReMark ");
            if (strwhere.Trim() != "")
            {
                strSql.Append(" where " + strwhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }


        public string[] GetAll() 
        {
            try
            {
                pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
                List<tbProductReMark> info = pddc.tbProductReMark.ToList();
                string[] str = new string[info.Count];
                for (int i = 0; i < info.Count; i++) 
                {
                    str[i] = info[i].ProductReMarkId;
                }
                return str;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
