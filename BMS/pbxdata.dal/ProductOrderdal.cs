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
    public class ProductOrderdal
    {
        /// <summary>
        /// 订单条件查询
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="strCondition"></param>
        /// <param name="pageCount"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public DataTable GetProductOrder(int skip, int take, string[] strCondition, out Exception exx)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "server=121.196.134.67,443\\pbx;database=TopAppData;uid=sa;pwd=123;timeOut=500";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "pro_SelectProductOrder";
                SqlParameter[] param = new SqlParameter[]{
                 new SqlParameter("@skip",skip+1),
                 new SqlParameter("@take",skip+take),
                 new SqlParameter("@beginTime",strCondition[0]),
                 new SqlParameter("@endTime",strCondition[1]),
                 new SqlParameter("@yhm",strCondition[2]),
                 new SqlParameter("@Scode",strCondition[8]),
                 new SqlParameter("@sql",""),
                 new SqlParameter("@sqlbody",""),
                 new SqlParameter("@pricemin",strCondition[6]),
                 new SqlParameter("@pricemax",strCondition[7]),
                 new SqlParameter("@Type",strCondition[4]),
                 new SqlParameter("@Brand",strCondition[3]),
                 new SqlParameter("@OrderState",strCondition[5]),
                 new SqlParameter("@orderId",strCondition[9])
                };
                com.Parameters.AddRange(param);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                com.Dispose();
                exx = null;
                BrandDAL bd = new BrandDAL();
                List<BrandModel> listbrand = bd.SelectAllBrand();
                ProductTypeDAL ptd = new ProductTypeDAL();
                List<ProductTypeModel> listType = ptd.GetProductTypeReplace();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < listbrand.Count; j++)
                    {
                        if (dt.Rows[i]["CAT"].ToString().Trim() == listbrand[j].BrandAbridge)
                        {
                            dt.Rows[i]["CAT"] = listbrand[j].BrandName;
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < listType.Count; j++)
                    {
                        if (dt.Rows[i]["CAT2"].ToString() == listType[j].TypeNo)
                        {
                            dt.Rows[i]["CAT2"] = listType[j].TypeName;
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PRICED"].ToString() != "")
                    {
                        dt.Rows[i]["PRICED"] = Decimal.Parse(Math.Round(decimal.Parse(dt.Rows[i]["PRICED"].ToString()), 2).ToString());
                    }
                    if (dt.Rows[i]["PRICEE"].ToString() != "")
                    {
                        dt.Rows[i]["PRICEE"] = Decimal.Parse(Math.Round(decimal.Parse(dt.Rows[i]["PRICEE"].ToString()), 2).ToString());
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        /// <summary>
        /// 订单总个数
        /// </summary>
        /// <param name="strCondition"></param>
        /// <returns></returns>
        public int GetProductOrderCount(string[] strCondition)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "server=121.196.134.67,443\\pbx;database=TopAppData;uid=sa;pwd=123;timeOut=500";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "pro_SelectProductOrderCount";
                SqlParameter[] param = new SqlParameter[]{
                 new SqlParameter("@skip",""),
                 new SqlParameter("@take",""),
                 new SqlParameter("@beginTime",strCondition[0]),
                 new SqlParameter("@endTime",strCondition[1]),
                 new SqlParameter("@yhm",strCondition[2]),
                 new SqlParameter("@Scode",strCondition[8]),
                 new SqlParameter("@sql",""),
                 new SqlParameter("@sqlbody",""),
                 new SqlParameter("@pricemin",strCondition[6]),
                 new SqlParameter("@pricemax",strCondition[7]),
                 new SqlParameter("@Type",strCondition[4]),
                 new SqlParameter("@Brand",strCondition[3]),
                 new SqlParameter("@OrderState",strCondition[5]),
                 new SqlParameter("@orderId",strCondition[9])
                };
                com.Parameters.AddRange(param);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                com.Dispose();
                int count = int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
