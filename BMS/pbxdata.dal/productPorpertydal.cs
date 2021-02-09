using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using pbxdata.model;

namespace pbxdata.dal
{
    public class productPorpertydal
    {
        /// <summary>
        /// 数据连接对象
        /// </summary>
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
        /// <summary>
        /// 添加属性值信息
        /// </summary>
        /// <param name="list">属性对象集合</param>
        /// <param name="lists">添加失败的属性集合</param>
        /// <returns></returns>
        public bool Insert(List<PorpertyModel> list, out Dictionary<int, string> lists)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
            Dictionary<int, string> listS = new Dictionary<int, string>();
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                SqlDataReader dr = null;
                foreach (PorpertyModel p in list)
                {
                    com.CommandText = "select * from productPorperty where PropertyName='" + p.PropertyName + "' and TypeId=" + p.TypeId;
                    dr = com.ExecuteReader();
                    //判断是否存在该属性名称
                    if (dr.Read())
                    {
                        dr.Close();
                        continue;
                    }
                    dr.Close();
                    com.CommandText = "Insert into productPorperty ([TypeId],PropertyName,PorpertyIndex,UserId) values(" + p.TypeId + ",'" + p.PropertyName + "',1,1)";
                    try
                    {
                        com.ExecuteNonQuery();
                    }
                    catch
                    {
                        listS.Add(p.TypeId, p.PropertyName);
                    }
                }
                lists = listS;
                return true;
            }
            catch (Exception ex)
            {
                lists = null;
                return false;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        /// <summary>
        /// 查询该类别typeId
        /// </summary>
        /// <param name="TypeName">类别名称</param>
        /// <returns></returns>
        public int SeletTypeId(string TypeName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "select Id from producttype where TypeNo='" + TypeName.Replace("$", "") + "'";
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    int typeid = int.Parse(dr["Id"].ToString());
                    dr.Close();
                    com.Dispose();
                    return typeid;
                }
                dr.Close();
                con.Dispose();
                return -1;
            }
            catch
            {
                return -2;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
        /// <summary>
        /// 取得当前商品的属性信息
        /// </summary>
        /// <param name="Scode">商品编号</param>
        public List<productPorpertyModel> GetProductPorpertys(string Scode)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
            List<productPorpertyModel> list = new List<productPorpertyModel>();
            try
            {
                string str = "select * from productPorperty where TypeId =( select top 1 Id from producttype where TypeNo = ( select top 1 Cat2 from product where Scode='" + Scode + "'))";
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = str;
                DataTable dt = new DataTable();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    productPorpertyModel pro = new productPorpertyModel();
                    pro.Id = int.Parse(dr["Id"].ToString());
                    pro.TypeId = int.Parse(dr["TypeId"].ToString());
                    pro.PropertyName = dr["PropertyName"].ToString();
                    list.Add(pro);
                }
                //SqlDataAdapter da = new SqlDataAdapter(com);
                //da.Fill(dt);
                //da.Dispose();
                dr.Close();
                com.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        /// <summary>
        /// 添加属性值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertPropertyValue(DataTable dt, out Exception exx)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                SqlDataReader dr;
                foreach (DataRow r in dt.Rows)
                {
                    List<productPorpertyModel> list = GetProductPorpertys(r["货品编号"].ToString());
                    List<string> sqlList = new List<string>();
                    if (list != null)
                        foreach (DataColumn c in dt.Columns)
                        {
                            if (c.ColumnName == "货品编号" || c.ColumnName == "货号")
                                continue;
                            List<productPorpertyModel> pro = list.Where(a => a.PropertyName == c.ColumnName).ToList();
                            if (pro.Count > 0)
                            {
                                com.CommandText = "select * from productPorpertyValue where TypeId=" + pro[0].TypeId + " and PropertyId=" + pro[0].Id + " and Scode='" + r["货品编号"].ToString() + "'";
                                dr = com.ExecuteReader();
                                string sql = "";
                                if (dr.Read())
                                {
                                    dr.Close();
                                    sql = "update productPorpertyValue set PropertyValue='" + r[c.ColumnName].ToString() + "' where TypeId=" + pro[0].TypeId + " and PropertyId=" + pro[0].Id + " and Scode='" + r["货品编号"].ToString() + "'";
                                    sqlList.Add(sql);
                                }
                                else
                                {
                                    dr.Close();
                                    sql = "insert into productPorpertyValue (TypeId,PropertyId,Scode,PropertyValue,PorpertyIndex,UserId) values(" + pro[0].TypeId + "," + pro[0].Id + ",'" + r["货品编号"].ToString() + "','" + r[c.ColumnName].ToString() + "',1,1)";
                                    sqlList.Add(sql);
                                }

                            }
                            //else
                            //{
                            //    if (OnAddProValDefeat != null)
                            //        OnAddProValDefeat(r["货品编号"].ToString());
                            //}
                        }
                    if (sqlList.Count == 0)
                    {
                        if (OnAddProValDefeat != null)
                            OnAddProValDefeat(r["货品编号"].ToString());
                    }
                    else
                    {
                        bool bol = InsertProVal(sqlList, out exx);
                        if (!bol)
                            OnAddProVal(r["货品编号"].ToString());
                    }
                }
                com.Dispose();
                exx = null;
                return true;
            }
            catch (Exception ex)
            {
                exx = ex;
                return false;
            }

        }
        /// <summary>
        /// 添加属性值信息调用
        /// </summary>
        /// <param name="str"></param>
        public delegate void AddProValHander(string str);
        /// <summary>
        /// 添加属性值信息成功事件
        /// </summary>
        public static event AddProValHander OnAddProVal = null;
        /// <summary>
        /// 添加属性值信息失败事件
        /// </summary>
        public static event AddProValHander OnAddProValDefeat = null;
        /// <summary>
        /// 添加属性值
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        private bool InsertProVal(List<string> sqlList, out Exception exx)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["clientConStr"].ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                foreach (string s in sqlList)
                {
                    com.CommandText = s;
                    try
                    {
                        com.ExecuteNonQuery();
                    }
                    catch
                    {
                        //if (OnAddProValDefeat != null)
                        //    OnAddProValDefeat(s);
                    }
                }
                exx = null;
                return true;
            }
            catch (Exception ex)
            {
                exx = ex;
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        /// <summary>
        /// 设置dal添加属性完和添加属性值完调用调用方法
        /// </summary>
        /// <param name="del">OnAddProVal事件委托</param>
        /// <param name="del1">OnAddProValDefeat事件委托</param>
        public static void SetOnAddProValAndOnAddProValDefeat(pbxdata.dal.productPorpertydal.AddProValHander del, pbxdata.dal.productPorpertydal.AddProValHander del1)
        {
            OnAddProVal = null;
            OnAddProVal += del;
            OnAddProValDefeat = null;
            OnAddProValDefeat += del1;
        }
    }
}
