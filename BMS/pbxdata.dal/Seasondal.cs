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
    public class Seasondal : dataoperating,iSeason
    {

        Errorlogdal dal = new Errorlogdal();
        /// <summary>
        /// 获取供应商季节
        /// </summary>
        /// <returns></returns>
        public DataTable SearchSeasonVen(Dictionary<string, string> Dic, int page, int pages, out string counts)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.ItalyPorductStock
                    group c by new { c.Cat1, c.Vencode } into cc
                    join a in context.SeasonVen on new { Cat1 = cc.Key.Cat1, Vencode = cc.Key.Vencode } equals new {Cat1= a.Cat1Ven, Vencode=a.Vencode }
                    into aa
                    from aaa in aa.DefaultIfEmpty()
                    join d in context.productsource on cc.Key.Vencode equals d.SourceCode
                    into dd
                    from ddd in dd.DefaultIfEmpty()
                    select new
                    {
                        VenCat1 = cc.Key.Cat1,//--供应商季节
                        Vencode2 = cc.Key.Vencode,//--供应商编码
                        Id = aaa.Id == null ? 0 : aaa.Id,//判断是否存在Id 不存在说明没绑定供应商季节 默认返回0
                        Cat1 = aaa.Cat1,//--季节
                        sourceName = ddd.sourceName,//--供应商
                    };
            if (Dic["Cat1"] != "")
            {
                q = q.Where(a => a.Cat1.Contains(Dic["Cat1"]));
            }
            if (Dic["Cat1Ven"] != "")
            {
                q = q.Where(a => a.VenCat1.Contains(Dic["Cat1Ven"]));
            }
            if (Dic["Vencode"] != "")
            {
                q = q.Where(a => a.Vencode2 == Dic["Vencode"]);
            }
            if (Dic["bangd"] == "0")
            {
                q = q.Where(a => a.Cat1 != "");
            }
            else if (Dic["bangd"] == "1")
            {
                q = q.Where(a => a.Cat1 == null);
            }
            counts = q.Count().ToString();
            dt = LinqToDataTable.LINQToDataTable(q.Skip(pages * (page - 1)).Take(pages));

            //string MinNid = (pages * (page - 1)).ToString();
            //string MaxNid = (pages * page).ToString();
            //IDataParameter[] ipr = new IDataParameter[]
            //{
            //    new SqlParameter("Cat1",Dic["Cat1"]),
            //    new SqlParameter("Cat1Ven",Dic["Cat1Ven"]),
            //    new SqlParameter("Vencode",Dic["Vencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("MinNid",MinNid),
            //    new SqlParameter("MaxNid",MaxNid),
            //    new SqlParameter("sql",""),
            //};
            //IDataParameter[] iprc = new IDataParameter[]
            //{
            //    new SqlParameter("Cat1",Dic["Cat1"]),
            //    new SqlParameter("Cat1Ven",Dic["Cat1Ven"]),
            //    new SqlParameter("Vencode",Dic["Vencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("sql",""),
            //};
            //dt = Select(ipr, "SelectSeasonVen");
            //counts = Select(iprc, "SelectSeasonVenCount").Rows[0][0].ToString();

            return dt;
        }

        /// <summary>
        /// 更新供应商季节
        /// </summary>
        /// <returns></returns>
        public string UpdateSeasonVen(Dictionary<string, string> dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                if (dic["Id"] == "0")//通过id来获取是Update还是insert
                {
                    var q = context.SeasonVen.Where(a => a.Cat1Ven == dic["Cat1Ven"]).Where(a => a.Vencode == dic["Vencode"]).ToList();
                    if (q.Count == 0)//判断是否已存在
                    {
                        model.SeasonVen mc = new model.SeasonVen()
                        {
                            Cat1 = dic["Cat1"],//季节
                            Cat1Ven = dic["Cat1Ven"],//供应商季节
                            Vencode = dic["Vencode"],//供应商
                        };
                        context.SeasonVen.InsertOnSubmit(mc);
                        context.SubmitChanges();
                        dal.InsertErrorlog(new model.errorlog()
                        {
                            errorSrc = "pbxdata.dal->SeasonDal->UpdateSeasonVen()",
                            ErrorMsg = "修改",
                            errorTime = DateTime.Now,
                            operation = 2,
                            errorMsgDetails = "添加供应季节表->" + dic["Cat1Ven"],
                            UserId = UserId,
                        });
                    }
                    else
                    {
                        return "该季节已存在!";
                    }

                }
                else
                {
                    var q = from c in context.SeasonVen where c.Id == Convert.ToInt32(dic["Id"].ToString()) select c;
                    foreach (var i in q)
                    {
                        i.Cat1 = dic["Cat1"];
                        i.Cat1Ven = dic["Cat1Ven"];
                        i.Vencode = dic["Vencode"];
                    }
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->SeasonDal->UpdateSeasonVen()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改供应季节表->" + dic["Cat1Ven"],
                        UserId = UserId,
                    });
                }
                return "修改成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->SeasonDal->UpdateSeasonVen()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "修改失败!";
            }

        }
        /// <summary>
        /// 删除供应商季节
        /// </summary>通过Id删除供应商季节
        public string DeleteSeasonVen(string Id, int UserId)
        {
            try
            {
                string item = "";
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.SeasonVen.Where(a => a.Id == Convert.ToInt32(Id));
                foreach (var i in q)
                {
                    item = i.Cat1Ven;
                    context.SeasonVen.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->SeasonDal->DeleteSeasonVen()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "修改供应季节表->" + item,
                    UserId = UserId,
                });
                return "删除成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->SeasonDal->DeleteSeasonVen()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "删除失败!";
            }
        }
        /// <summary>
        /// 根据季节名称查询季节表
        /// </summary>
        /// <returns></returns>
        public DataTable GetData(string Cat1)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = context.Season.Where(a => a.Cat1.Contains(Cat1));
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }

         /// <summary>
        /// 获取季节表
        /// </summary>
        /// <returns></returns>
        public DataTable OnSearch(Dictionary<string, string> dic, int page, int Selpages, out string counts)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = context.Season.Where(a => a.Cat1.Contains(dic["Cat1"]));
            counts = q.Count().ToString();
            if (page == 0)
            {
                dt = LinqToDataTable.LINQToDataTable(q.Take(Selpages));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q.Skip((page - 1) * Selpages).Take(Selpages));
            }
            
            return dt;
        }
        /// <summary>
        /// 添加季节
        /// </summary>
        /// <returns></returns>
        public string AddSeason(string Cat1, int UserId)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.Season.Where(a => a.Cat1 == Cat1);
                if (q.Count() == 0)
                {
                    model.Season ms = new model.Season()
                    {
                        Cat1=Cat1,//季节
                    };
                    context.Season.InsertOnSubmit(ms);
                    context.SubmitChanges();
                    s = "添加成功!";
                }
                else
                {
                    s = "该季节已存在!";
                }
            }
            catch (Exception ex)
            {
                s = "添加失败!";
            }
            
            return s;
        }
        /// <summary>
        /// 更新季节
        /// </summary>
        /// <returns></returns>
        public string UpdateSeason(string Id, string Cat1, int UserId)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var Check = context.Season.Where(a => a.Cat1 == Cat1);
                var q = context.Season.Where(a => a.Id == Convert.ToInt32(Id));
                if (Check.Count() == 0)
                {
                    foreach (var i in q)
                    {
                        i.Cat1 = Cat1;
                    }
                    context.SubmitChanges();
                    s = "修改成功!";
                }
                else
                {
                    s = "该季节已存在!";
                }
            }
            catch (Exception ex)
            {
                s = "修改失败!";
            }
            
            return s;
        }
        /// <summary>
        /// 删除季节
        /// </summary>
        /// <returns></returns>
        public string DeleteSeason(string Cat1, int UserId)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.Season.Where(a => a.Cat1 == Cat1);
                foreach (var i in q)
                {
                    context.Season.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                s = "";
            }
            catch (Exception ex)
            {
                s = "删除失败!";
            }
            

            return s;
        }
    }
}
