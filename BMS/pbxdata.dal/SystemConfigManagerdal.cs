using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public partial class SystemConfigManagerdal : pbxdata.idal.ISystemConfig
    {
        model.pbxdatasourceDataContext pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
        /// <summary>
        /// 获取所有供应商信息
        /// </summary>
        /// <param name="exx">异常信息</param>
        /// <returns></returns>
        public List<model.productsource> GetProductsource(out Exception exx)
        {
            try
            {
                pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
                /// <summary>
                OpenCon();
                var temp = from c in pbx.productsource orderby c.SourceCode ascending select c;
                List<model.productsource> list = new List<model.productsource>();
                foreach (var t in temp)
                {
                    var user = (from c in pbx.users where c.Id == t.UserId select c.userName).ToList();
                    model.productsource pro = new model.productsource();
                    pro.Id = t.Id;
                    pro.SourceCode = t.SourceCode;
                    pro.sourceName = t.sourceName;
                    pro.sourcePhone = t.sourcePhone;
                    pro.SourceManage = t.SourceManage;
                    pro.SourceLevel = t.SourceLevel;
                    pro.UserId = t.UserId;
                    pro.UserName1 = user.Count > 0 ? user[0].ToString() : "";
                    pro.Def2 = t.Def2;
                    pro.Def3 = t.Def3;
                    list.Add(pro);
                }
                exx = null;
                return list;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }
        public void OpenCon()
        {
            if (pbx.Connection.State == System.Data.ConnectionState.Closed)
            {
                pbx.Connection.Open();
            }
        }
        public void CloseCon()
        {
            if (pbx.Connection.State == System.Data.ConnectionState.Open)
            {
                pbx.Connection.Close();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool DeleteProductsources(string Code, out Exception exx)
        {
            System.Data.Common.DbTransaction tran = null;
            try
            {
                OpenCon();
                tran = pbx.Connection.BeginTransaction();
                pbx.Transaction = tran;
                var config = from c in pbx.productsourceConfig where c.SourceCode == Code select c;
                var temp = from c in pbx.productsource where c.SourceCode == Code select c;
                foreach (var c in config)
                {
                    pbx.productsourceConfig.DeleteOnSubmit(c);
                }
                foreach (var t in temp)
                {
                    pbx.productsource.DeleteOnSubmit(t);
                }
                pbx.SubmitChanges();
                tran.Commit();
                exx = null;
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                exx = ex;
                return false;
            }
            finally
            {
                CloseCon();
            }
        }

        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="pro">供应商实体类</param>
        /// <param name="exx">错误对象</param>
        /// <returns></returns>
        public bool InsertProductsources(model.productsource pro, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = (from c in pbx.productsource where c.SourceCode == pro.SourceCode select c).ToList();
                if (temp.Count > 0)
                {
                    exx = new Exception("该供应商编号已经存在");
                    return false;
                }
                pbx.productsource.InsertOnSubmit(pro);
                pbx.SubmitChanges();
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
                CloseCon();
            }
        }


        /// <summary>
        /// 查询指定供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public model.productsource GetProductsource(string Id, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = (from c in pbx.productsource where c.SourceCode == Id select c).ToList();
                model.productsource pro = new model.productsource();
                if (temp.Count > 0)
                {
                    exx = null;
                    return temp[0];
                }
                else
                {
                    pro = null;
                }
                exx = null;
                return pro;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }
        private bool IsDatetime(object obj)
        {
            try
            {
                DateTime.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取所有数据源信息集合
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetProductsourceConfig(out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = from c in pbx.productsourceConfig join a in pbx.productsource on c.SourceCode equals a.SourceCode select new { c.Id, c.SourceCode, a.sourceName, c.SourcesAddress, c.UserId, c.UserPwd, c.DataSources, c.DataSourcesLevel, c.TimeStart, c.Def1, c.Def2, c.Def3, c.Def4, c.Def5 };
                DataTable dt = new DataTable();
                bool Fast = true;
                foreach (var v in temp)
                {
                    DataRow dr = dt.NewRow();
                    foreach (var i in v.GetType().GetProperties())
                    {
                        if (Fast)
                        {
                            DataColumn cl = new DataColumn(i.Name);
                            dt.Columns.Add(cl);
                        }
                        if (i.Name == "Def2")
                        {
                            if (IsDatetime(i.GetValue(v)))
                            {
                                string str= (DateTime.Now - DateTime.Parse(i.GetValue(v).ToString())).TotalMinutes.ToString();
                                dr[i.Name] = str.Substring(0,str.LastIndexOf('.'));
                            }
                            else
                                dr[i.Name] = 0;
                        }
                        else
                            dr[i.Name] = i.GetValue(v);
                    }
                    Fast = false;
                    dt.Rows.Add(dr);
                }
                exx = null;
                return dt;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 删除数据源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool DeleteProductsourceConfig(int Id, out Exception exx)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加数据源信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool InsertProductsourceConfig(model.productsourceConfig pro, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = (from c in pbx.productsourceConfig where c.SourceCode == pro.SourceCode select c).ToList();
                if (temp.Count > 0)
                {
                    exx = null;
                    return false;
                }
                pbx.productsourceConfig.InsertOnSubmit(pro);
                pbx.SubmitChanges();
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
                CloseCon();
            }
        }
        /// <summary>
        /// 修改数据源信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool UpdateProductsourceConfig(model.productsourceConfig pro, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = from c in pbx.productsourceConfig where c.Id == pro.Id select c;
                foreach (var v in temp)
                {
                    v.SourceCode = pro.SourceCode;
                    v.SourcesAddress = pro.SourcesAddress;
                    v.UserId = pro.UserId;
                    v.UserPwd = pro.UserPwd;
                    v.DataSources = pro.DataSources;
                    v.DataSourcesLevel = pro.DataSourcesLevel;
                }
                pbx.SubmitChanges();
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
                CloseCon();
            }
        }
        /// <summary>
        /// 查询指定数据源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public model.productsourceConfig GetProductsourceConfig(int Id, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = (from c in pbx.productsourceConfig where c.Id == Id select c).ToList();
                if (temp.Count > 0)
                {
                    exx = null;
                    if (IsDatetime(temp[0].Def2))
                        temp[0].Def2 = (DateTime.Now - DateTime.Parse(temp[0].Def2)).TotalMinutes.ToString();
                    else
                        temp[0].Def2 = "0";
                    return temp[0];
                }
                exx = null;
                return null;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool UpdateProductsource(model.productsource pro, out Exception exx)
        {
            try
            {
                OpenCon();
                var temp = from c in pbx.productsource where c.Id == pro.Id select c;
                foreach (var v in temp)
                {
                    v.SourceCode = pro.SourceCode;
                    v.sourceName = pro.sourceName;
                    v.sourcePhone = pro.sourcePhone;
                    v.SourceManage = pro.SourceManage;
                    v.SourceLevel = pro.SourceLevel;
                    v.UserId = pro.UserId;
                    v.Def2 = pro.Def2;
                }
                pbx.SubmitChanges();
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
                CloseCon();
            }
        }

        /// <summary>
        /// 获取所有供应商编号 名称
        /// </summary>
        /// <param name="exx"></param>
        /// <returns></returns>
        Dictionary<string, string> idal.ISystemConfig.GetProductsource1(out Exception exx)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                OpenCon();
                var temp = from c in pbx.productsource select new { c.SourceCode, c.sourceName };
                foreach (var v in temp)
                {
                    dic.Add(v.SourceCode, v.sourceName);
                }
                exx = null;
                return dic;
            }
            catch (Exception ex)
            {
                exx = ex;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }

        /// <summary>
        /// 查询所有数据源更新日志信息
        /// </summary>
        /// <param name="exx"></param>
        /// <returns></returns>
        public DataTable GetProGetProductsourceUpdateLog(int skip, int take, string log, string beginTime, string endTime, out int count, out Exception exx)
        {
            try
            {
                OpenCon();
                //var temp = (from c in pbx.errorlog join b in pbx.productsource on c.Def3 equals b.SourceCode  join a in pbx.users on c.UserId equals a.UserId where c.operation == 3 select new {c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName,a.userName}).Skip(skip).Take(take).ToList();
                if (log == "1")
                    if (string.IsNullOrEmpty(beginTime) && string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(beginTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.errorTime >= DateTime.Parse(beginTime) select c.Id).ToList().Count;
                    else
                        count = (from c in pbx.errorlog where c.operation == 3 && c.errorTime >= DateTime.Parse(beginTime) && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                else
                    if (string.IsNullOrEmpty(beginTime) && string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def2 != "0" select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(beginTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def2 != "0" && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def2 != "0" && c.errorTime >= DateTime.Parse(beginTime) select c.Id).ToList().Count;
                    else
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def2 != "0" && c.errorTime >= DateTime.Parse(beginTime) && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[]{
                    new SqlParameter("@skipId",skip+1),
                    new SqlParameter("@takeId",skip+take),
                    new SqlParameter("@SourceCode","-1"),
                    new SqlParameter("@Def2",log),
                    new SqlParameter("@beginTime",beginTime==null?"":beginTime),
                    new SqlParameter("@endTime",endTime==null?"":endTime)
                };
                DataTable dt = Maticsoft.DBUtility.DbHelperSQL.RunProcedure("pro_SelectDataSourcesErrorLog", param, "ErrorLog").Tables["ErrorLog"];
                //DataTable dt=new DataTable();
                //bool Fast = true;
                //foreach (var v in temp)
                //{
                //    DataRow dr = dt.NewRow();
                //    foreach (var i in v.GetType().GetProperties())
                //    {
                //        if (Fast)
                //        {
                //            DataColumn cl = new DataColumn(i.Name);
                //            dt.Columns.Add(cl);
                //        }
                //        dr[i.Name] = i.GetValue(v);
                //    }
                //    Fast = false;
                //    dt.Rows.Add(dr);
                //}
                exx = null;
                return dt;
            }
            catch (Exception ex)
            {
                exx = ex;
                count = 0;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }
        /// <summary>
        /// 查询所有数据源更新日志信息
        /// </summary>
        /// <param name="exx"></param>
        /// <returns></returns>
        public DataTable GetProGetProductsourceUpdateLog(int skip, int take, string SourceCode, string log, string beginTime, string endTime, out int count, out Exception exx)
        {
            try
            {
                OpenCon();
                //var temp = (from c in pbx.errorlog join b in pbx.productsource on c.Def3 equals b.SourceCode  join a in pbx.users on c.UserId equals a.UserId where c.operation == 3 select new {c.Id,c.ErrorMsg,c.errorSrc,c.errorMsgDetails,c.UserId,c.errorTime,c.operation,c.Def2,c.Def3,c.Def4,c.Def5,b.sourceName,a.userName}).Skip(skip).Take(take).ToList();
                if (log == "1")
                    if (string.IsNullOrEmpty(beginTime) && string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(beginTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.errorTime >= DateTime.Parse(beginTime) select c.Id).ToList().Count;
                    else
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.errorTime >= DateTime.Parse(beginTime) && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                else
                    if (string.IsNullOrEmpty(beginTime) && string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.Def2 != "0" select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(beginTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.Def2 != "0" && c.errorTime <= DateTime.Parse(endTime) select c.Id).ToList().Count;
                    else if (string.IsNullOrEmpty(endTime))
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.Def2 != "0" && c.errorTime >= DateTime.Parse(beginTime) select c.Id).ToList().Count;
                    else
                        count = (from c in pbx.errorlog where c.operation == 3 && c.Def3 == SourceCode && c.Def2 != "0" && c.errorTime <= DateTime.Parse(endTime) && c.errorTime >= DateTime.Parse(beginTime) select c.Id).ToList().Count;
                //count = (from c in pbx.errorlog where c.operation == 3 select c.Id).ToList().Count;
                System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[]{
                    new SqlParameter("@skipId",skip+1),
                    new SqlParameter("@takeId",skip+take),
                    new SqlParameter("@SourceCode",SourceCode),
                    new SqlParameter("@Def2",log),
                    new SqlParameter("@beginTime",beginTime==null?"":beginTime),
                    new SqlParameter("@endTime",endTime==null?"":endTime)
                };
                DataTable dt = Maticsoft.DBUtility.DbHelperSQL.RunProcedure("pro_SelectDataSourcesErrorLog", param, "ErrorLog").Tables["ErrorLog"];
                exx = null;
                return dt;
            }
            catch (Exception ex)
            {
                exx = ex;
                count = 0;
                return null;
            }
            finally
            {
                CloseCon();
            }
        }
    }
}
