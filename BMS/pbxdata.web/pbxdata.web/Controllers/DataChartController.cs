using Maticsoft.DBUtility;
using pbxdata.dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class DataChartController : BaseController
    {

        bll.DataChartbll bll = new bll.DataChartbll();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnProduct()
        {
            return bll.ReturnProduct();
        }

        /// <summary>
        /// 商品统计 根据库存
        /// </summary>
        /// <returns></returns>
        public string ProductCount()
        {
            string s = string.Empty;
            string sql = "";
            string Balance = Request.Form["Balance"].ToString();
            DataTable dt = new DataTable();
            string[] arry = { "", "0", "5", "10", "20" };//--判断传入的值 只能为 "" 0 5 10 20
            if (arry.Contains(Balance))
            {
                if (Balance == "")//--传值为""的时候是查询库存为0的
                {
                    sql = @"select COUNT(*) from(select Cat from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS=0) group by Cat) tt
                            union all
                            select COUNT(*) from(select Cat2 from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS=0) group by Cat2) tt
                            union all
                            select COUNT(*) from(select Style from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS=0) group by Style) tt
                            union all
                            select SUM(Balance) from(select * from productstock a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS=0)) tt
                            union all
                            select COUNT(*) from(select * from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS=0) and Imagefile is not null and Imagefile!='') tt";
                }
                else
                {
                    sql = @"select COUNT(*) from(select Cat from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS>" + Balance + ") group by Cat) tt";
                    sql += " union all ";
                    sql += @"select COUNT(*) from(select Cat2 from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS>" + Balance + ") group by Cat2) tt";
                    sql += " union all ";
                    sql += @"select COUNT(*) from(select Style from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS>" + Balance + ") group by Style) tt";
                    sql += " union all ";
                    sql += @"select SUM(Balance) from(select * from productstock a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock group by Scode 
                            ) t where BalanceS>" + Balance + ")) tt";
                    sql += " union all ";
                    sql += @"select COUNT(*) from(select * from product a where Scode in (select Scode from (
                            select Scode,SUM(Balance) as BalanceS from productstock where Def4!='1' group by Scode 
                            ) t where BalanceS>" + Balance + ") and Imagefile is not null and Imagefile!='') tt";
                }
            }
            dt = DbHelperSQL.Query(sql).Tables[0];
            s = "[{\"Brand\":" + dt.Rows[0][0] + ",\"TypeName\":" + dt.Rows[1][0] + ",\"Style\":" + dt.Rows[2][0] + ",\"Balance\":" + dt.Rows[3][0] + ",\"Imagefile\":" + dt.Rows[4][0] + "}]";
            return s;
        }

        /// <summary>
        /// Top20 品牌以及类别
        /// </summary>
        /// <returns></returns>
        public string TopBrand()
        {
            StringBuilder s = new StringBuilder(200);
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var sql = @"select top 20 BrandName,SUM(Balance) as balance from productstock a left join brand b on a.Cat=b.BrandAbridge where BrandName is not null group by BrandName order by balance desc";
            var sql1 = @"select top 20 TypeName,SUM(Balance) as balance from productstock a left join producttype b on a.Cat2=b.TypeNo where TypeName is not null group by TypeName order by balance desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            for (int i = 0; i < 20; i++)
            {
                int n = i + 1;
                s.Append(n + "." + dt.Rows[i]["BrandName"].ToString() + "<br />");
            }
            s.Append( "-*-");
            for (int i = 0; i < 20; i++)
            {
                int n = i + 1;
                s.Append(n + "." + dt1.Rows[i]["TypeName"].ToString() + "<br />");
            }
            return s.ToString() ;
        }

        /// <summary>
        /// 综合报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ZongheReport()
        {
            return View();
        }

        /// <summary>
        /// 退款退货统计-总数
        /// </summary>
        /// <returns></returns>
        public string Refund()
        {
            return bll.Refund();
        }

        /// <summary>
        /// 来货报表（HK数据源)
        /// </summary>
        /// <returns></returns>
        public ActionResult LaiHuoReport()
        {
            return View();
        }

        /// <summary>
        /// 获取来货报表（HK数据源)
        /// </summary>
        /// <returns></returns>
        public string GetLaiHuoReport()
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Brand = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string Type = helpcommon.StrSplit.StrSplitData(ss[1], ':')[1].Replace("'", "").Replace("}", "");
            string Mindate = helpcommon.StrSplit.StrSplitData(ss[2], ':')[1].Replace("'", "").Replace("}", "");
            string Maxdate = helpcommon.StrSplit.StrSplitData(ss[3], ':')[1].Replace("'", "").Replace("}", "");

           
            DataTable dt = new DataTable();
            string counts = string.Empty;
            string Balances = string.Empty;
            string[] ssName = { "Lastgrnd", "Balance", "BrandName", "Cat", "TypeName", "Cat2" };
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Mindate", Mindate);
            Dic.Add("Maxdate", Maxdate);
            Dic.Add("Cat", Brand);
            Dic.Add("Cat2", Type);
            dt = bll.GetLaiHuoReport(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts, out Balances);
            int pageCount = Convert.ToInt32(counts) % Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts)/Convert.ToInt32(pageSize) : Convert.ToInt32(counts)/Convert.ToInt32(pageSize)+1;
            StringBuilder s = new StringBuilder(200);
            s.Append("<tr>");
            s.Append("<th>编号</th>");
            for (int i = 0; i < ssName.Length; i++)
            {
                if (ssName[i] == "Lastgrnd")
                    s.Append("<th>日期</th>");
                if (ssName[i] == "Balance")
                    s.Append("<th>来货数</th>");
                if (ssName[i] == "BrandName")
                    s.Append("<th>品牌名</th>");
                if (ssName[i] == "Cat")
                    s.Append("<th>品牌缩写</th>");
                if (ssName[i] == "TypeName")
                    s.Append("<th>类别</th>");
                if (ssName[i] == "Cat2")
                    s.Append("<th>类别编号</th>");
            }
            s.Append("</tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr>");
                    s.Append("<td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ssName[j] == "Lastgrnd")
                        {
                            s.Append("<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd") + "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }

                    }
                    s.Append("</tr>");
                }
            }
            
            return s.ToString() + "-----" + pageCount + "-----"+counts + "-----" + Balances;
        }

        /// <summary>
        /// 获取今日来货报表
        /// </summary>
        /// <returns></returns>
        public string GetLaiHuoReportT()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            DateTime Mindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            string[] ssName = { "Lastgrnd", "Balance", "BrandName", "Cat", "TypeName", "Cat2" };
            dt = bll.GetLaiHuoReportT();
            s.Append("<tr>");
            for (int i = 0; i < ssName.Length; i++)
            {
                if (ssName[i] == "Lastgrnd")
                    s.Append("<th>日期</th>");
                if (ssName[i] == "Balance")
                    s.Append("<th>来货数</th>");
                if (ssName[i] == "BrandName")
                    s.Append("<th>品牌名</th>");
                if (ssName[i] == "Cat")
                    s.Append("<th>品牌缩写</th>");
                if (ssName[i] == "TypeName")
                    s.Append("<th>类别</th>");
                if (ssName[i] == "Cat2")
                    s.Append("<th>类别编号</th>");
            }
            s.Append( "</tr>");
            if (dt.Rows.Count == 0)
            {
                s.Append("<tr>");
                s.Append("<td>" + Mindate.ToString("yyyy-MM-dd") + "</td><td>0</td><td>无</td><td>无</td><td>无</td><td>无</td>");
                s.Append("</tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr>");
                    s.Append("<td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ssName[j] == "Lastgrnd")
                        {
                            s.Append("<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd") + "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }

                    }
                    s.Append("</tr>");
                }
            }


            return s.ToString(); ;
        }
        /// <summary>
        /// app销售报表
        /// </summary>
        /// <returns></returns>
        public ActionResult AppSalesReport()
        {
            return View();
        }

        /// <summary>
        /// 销售统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnInfo()
        {
            return bll.ReturnInfo();
        }

        /// <summary>
        /// 销售统计--根据日期
        /// </summary>
        /// <returns></returns>
        public string ReturnInfoT()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Mintime", Request.Form["Mintime"].ToString());
            Dic.Add("Maxtime", Request.Form["Maxtime"].ToString());
            return bll.ReturnInfo(Dic);
        }

        /// <summary>
        /// 退款退货报表
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundReport()
        {
            return View();
        }

        /// <summary>
        /// 退款报表
        /// </summary>
        /// <returns></returns>
        public string GetRefundReport()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string MinTime = Request.Form["MinTime"].ToString();
            string MaxTime = Request.Form["MaxTime"].ToString();
            string[] ssName = { "Def3", "counts", "SellPrice" };
            dt = bll.GetRefundReport(MinTime, MaxTime);
            s.Append("<tr><th>日期</th><th>退款笔数</th><th>退款金额</th></tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<tr>");
                    for (int j = 0; j < ssName.Length; j++)
                    {

                        if (ssName[j].ToLower().Contains("price"))
                        {
                            s.Append("<td>" + Convert.ToDecimal(dt.Rows[i][ssName[j]]).ToString("f2") + "</td>");
                        }
                        else if (ssName[j].ToLower().Contains("def3"))
                        {
                            s.Append("<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]]).ToString("yyyy-MM-dd") + "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }

                    }
                    s.Append("</tr>");
                }
            }
            
            return s.ToString(); ;
        }

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        public string GetReturnBalanceReport()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string[] ssName = { "Def3", "counts", "SellPrice", "Balance" };
            string MinTime = Request.Form["MinTime"].ToString();
            string MaxTime = Request.Form["MaxTime"].ToString();
            dt = bll.GetReturnBalanceReport(MinTime, MaxTime);
            s.Append("<tr><th>日期</th><th>退货笔数</th><th>退货金额</th><th>退货件数</th></tr>");
            if (dt.Rows.Count <= 0 || dt == null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<tr>");
                    for (int j = 0; j < ssName.Length; j++)
                    {

                        if (ssName[j].ToLower().Contains("price"))
                        {
                            s.Append("<td>" + Convert.ToDecimal(dt.Rows[i][ssName[j]]).ToString("f2") + "</td>");
                        }
                        else if (ssName[j].ToLower().Contains("def3"))
                        {
                            s.Append("<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]]).ToString("yyyy-MM-dd") + "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }

                    }
                    s.Append("</tr>");
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 退货率报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnRateReport()
        {
            return View();
        }

        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public string GetReturnRateReport()
        {
            string s = string.Empty;
            string Cat = Request.Form["Cat"].ToString();
            string TypeNo = Request.Form["TypeNo"].ToString();
            string SellPriceMin = Request.Form["Brand"].ToString();
            string SellPriceMax = Request.Form["Brand"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Cat", Cat);
            Dic.Add("TypeNo", TypeNo);
            Dic.Add("SellPriceMin", SellPriceMin);
            Dic.Add("SellPriceMax", SellPriceMax);

            return s;
        }

        /// <summary>
        /// 获取退货率报表--品牌
        /// </summary>
        /// <returns></returns>
        public string GetBrandRateReport()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string Cat = Request.Form["Cat"].ToString();
            DataTable dt = new DataTable();
            string[] ssName = { "BrandName", "Cat", "Rate" };
            dt = bll.GetBrandRateReport(Cat);
            s.Append( "<tr><th>品牌名</th><th>品牌缩写</th><th>退货率</th></tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<tr>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ssName[j] == "Rate")
                        {
                            var Rate = dt.Rows[i][ssName[j]].ToString() == "" ? "0" : dt.Rows[i][ssName[j]].ToString();
                            s.Append("<td>" + Convert.ToDecimal(Rate).ToString("f2") + "%</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }
                    }
                    s.Append("</tr>");
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// 获取退货率报表--类别
        /// </summary>
        /// <returns></returns>
        public string GetTypeRateReport()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string TypeNo = Request.Form["TypeNo"].ToString();
            string[] ssName = { "TypeName", "Cat2", "Rate" };
            dt = bll.GetTypeRateReport(TypeNo);
            s.Append("<tr><th>类别名</th><th>类别编号</th><th>退货率</th></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ssName[j] == "Rate")
                    {
                        var Rate = dt.Rows[i][ssName[j]].ToString() == "" ? "0" : dt.Rows[i][ssName[j]].ToString();
                        s.Append("<td>" + Convert.ToDecimal(Rate).ToString("f2") + "%</td>");
                    }
                    else
                    {
                        s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                    }

                }
                s.Append("</tr>");
            }
            return s.ToString();
        }

        /// <summary>
        /// 获取退货率报表--价格区间
        /// </summary>
        /// <returns></returns>
        public string GetSellPriceRateReport()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string SellPriceMin = Request.Form["SellPriceMin"].ToString();
            string SellPriceMax = Request.Form["SellPriceMax"].ToString();
            string[] ssName = { "SellPrice", "Rate" };
            dt = bll.GetSellPriceRateReport(SellPriceMin, SellPriceMax);
            s.Append("<tr><th>价格</th><th>退货率</th></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ssName[j] == "Rate")
                    {
                        var Rate = dt.Rows[i][ssName[j]].ToString() == "" ? "0" : dt.Rows[i][ssName[j]].ToString();
                        s.Append("<td>" + Convert.ToDecimal(Rate).ToString("f2") + "%</td>");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i][ssName[j]].ToString()))
                        {
                             s.Append("<td>" + ""+ "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + Convert.ToDecimal(dt.Rows[i][ssName[j]]).ToString("f2") + "</td>");
                        }

                    }
                }
                s.Append("</tr>");
            }
            return s.ToString();
        }

        /// <summary>
        /// 出货报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShipmentReport()
        {
            return View();
        }

        /// <summary>
        /// 获取出货报表
        /// </summary>
        /// <returns></returns>
        public string GetShipmentReport()
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Mindate = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string Maxdate = helpcommon.StrSplit.StrSplitData(ss[1], ':')[1].Replace("'", "").Replace("}", "");
            string sendSource = helpcommon.StrSplit.StrSplitData(ss[2], ':')[1].Replace("'", "").Replace("}", "");


            StringBuilder s = new StringBuilder(200);
            string counts = string.Empty;
            DataTable dt = new DataTable();
            string[] ssName = { "createTime", "weifa", "yifa", "sourceName" };
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Mindate", Mindate);
            Dic.Add("Maxdate", Maxdate);
            Dic.Add("sendSource", sendSource);
            dt = bll.GetShipmentReport(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            int pageCount = Convert.ToInt32(counts) % Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts) / Convert.ToInt32(pageSize) : Convert.ToInt32(counts) / Convert.ToInt32(pageSize) + 1;
            s.Append("<tr>");
            s.Append("<th>编号</th>");
            s.Append("<th>日期</th><th>发货数</th><th>未发货数</th><th>供应商</th>");
            s.Append("</tr>");
            if (dt.Rows.Count <= 0 || dt == null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr>");
                    s.Append("<td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ssName[j].ToLower().Contains("time"))
                        {
                            s.Append("<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]]).ToString("yyyy-MM-dd") + "</td>");
                        }
                        else
                        {
                            s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                        }

                    }
                    s.Append("</tr>");
                }
            }
            return s.ToString() + "-----" + pageCount + "-----" + counts;
            #region 原来代码
            //string s = string.Empty;
            //string counts = string.Empty;
            //DataTable dt = new DataTable();
            //string[] ssName = { "createTime", "weifa", "yifa", "sourceName" };
            //string[] Searchlist = lists.Split(',');
            //Dictionary<string, string> Dic = new Dictionary<string, string>();
            //Dic.Add("Mindate", Searchlist[0]);
            //Dic.Add("Maxdate", Searchlist[1]);
            //Dic.Add("sendSource", Searchlist[2]);
            //dt = bll.GetShipmentReport(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
            //s += "<tr>";
            //s += "<th>编号</th>";
            //s += "<th>日期</th><th>发货数</th><th>未发货数</th><th>供应商</th>";
            //s += "</tr>";
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    int n = i + 1;
            //    s += "<tr>";
            //    s += "<td>" + n + "</td>";
            //    for (int j = 0; j < ssName.Length; j++)
            //    {
            //        if (ssName[j].ToLower().Contains("time"))
            //        {
            //            s += "<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]]).ToString("yyyy-MM-dd") + "</td>";
            //        }
            //        else
            //        {
            //            s += "<td>" + dt.Rows[i][ssName[j]] + "</td>";
            //        }

            //    }
            //    s += "</tr>";
            //}
            //return s + "-*-" + counts; 
            #endregion
        }
        /// <summary>
        /// 获取出货报表--今日
        /// </summary>
        /// <returns></returns>
        public string GetShipmentReportT()
        {
            try
            {
                StringBuilder s = new StringBuilder(200);
                DataTable dt = new DataTable();
                DateTime Mindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                string[] ssName = { "createTime", "weifa", "yifa", "sourceName" };
                dt = bll.GetShipmentReportT();
                s.Append("<tr>");
                s.Append("<th>日期</th><th>发货数</th><th>未发货数</th><th>供应商</th>");
                s.Append("</tr>");

                //s += "<tr>";
                //s += "<td>" + Mindate.ToString("yyyy-MM-dd") + "</td><td>0</td><td>0</td><td>无</td>";
                //s += "</tr>";
                if (dt.Rows.Count == 0)
                {
                    s.Append("<tr>");
                    s.Append("<td>" + Mindate.ToString("yyyy-MM-dd") + "</td><td>0</td><td>0</td><td>无</td>");
                    s.Append("</tr>");
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s.Append("<tr>");
                        for (int j = 0; j < ssName.Length; j++)
                        {
                            if (ssName[j].ToLower().Contains("time"))
                            {
                                //s += "<td>" + Convert.ToDateTime(dt.Rows[i][ssName[j]]).ToString("yyyy-MM-dd") + "</td>";
                                s.Append("<td>" + dt.Rows[i][ssName[j]].ToString() + "</td>");
                            }
                            else
                            {
                                s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                            }

                        }
                        s.Append("</tr>");
                    }
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
           
        }
        /// <summary>
        /// 品牌报表
        /// </summary>
        /// <returns></returns>
        public ActionResult BrandReport()
        {
            return View();
        }

        /// <summary>
        /// 获取品牌报表
        /// </summary>
        /// <returns></returns>
        public string GetBrandReport()//string lists, string page, string Selpages
        {

            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Vencode = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string Brand = helpcommon.StrSplit.StrSplitData(ss[1], ':')[1].Replace("'", "").Replace("}", "");

            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string counts = string.Empty;
            string[] ssName = { "BrandName", "Cat", "Style", "Balance", "SourceName" };
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Vencode", Vencode);
            Dic.Add("Cat", Brand);
            dt = bll.GetBrandReport(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            int pageCount = Convert.ToInt32(counts) % Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts) / Convert.ToInt32(pageSize) : Convert.ToInt32(counts) / Convert.ToInt32(pageSize) + 1;
            s.Append( "<tr>");
            s.Append("<th>编号</th>");
            for (int i = 0; i < ssName.Length; i++)
            {
                if (ssName[i] == "BrandName")
                    s.Append("<th>品牌名</th>");
                if (ssName[i] == "Cat")
                    s.Append( "<th>品牌缩写</th>");
                if (ssName[i] == "Style")
                    s.Append( "<th>款数</th>");
                if (ssName[i] == "Balance")
                    s.Append("<th>件数</th>");
                if (ssName[i] == "SourceName")
                    s.Append("<th>供应商</th>");

            }
            s.Append("</tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr>");
                    s.Append("<td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                    }
                    s.Append("</tr>");
                }
            }
            return s.ToString() + "-----"+pageCount.ToString()+"-----" + counts;
        }
        /// <summary>
        /// 类别报表
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeReport()
        {
            return View();
        }

        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        public string GetTypeReport()//string lists, string page, string Selpages
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Vencode = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string Type = helpcommon.StrSplit.StrSplitData(ss[1], ':')[1].Replace("'", "").Replace("}", "");
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string counts = string.Empty;
            string[] ssName = { "TypeName", "TypeNo", "Style", "Balance", "SourceName" };
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Vencode", Vencode);
            Dic.Add("TypeNo", Type);
            dt = bll.GetTypeReport(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            int pageCount = Convert.ToInt32(counts) % Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts) / Convert.ToInt32(pageSize) : Convert.ToInt32(counts) / Convert.ToInt32(pageSize) + 1;
            s.Append("<tr>");
            s.Append("<th>编号</th>");
            for (int i = 0; i < ssName.Length; i++)
            {
                if (ssName[i] == "TypeName")
                    s.Append("<th>类别名</th>");
                if (ssName[i] == "TypeNo")
                    s.Append("<th>类别编号</th>");
                if (ssName[i] == "Style")
                    s.Append("<th>款数</th>");
                if (ssName[i] == "Balance")
                    s.Append("<th>件数</th>");
                if (ssName[i] == "SourceName")
                    s.Append("<th>供应商</th>");
            }
            s.Append("</tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr>");
                    s.Append("<td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        s.Append("<td>" + dt.Rows[i][ssName[j]] + "</td>");
                    }
                    s.Append("</tr>");
                }
            }
            
            return s.ToString() +"-----" +pageCount+"-----"+ counts; 
        }



        //--------公共方法
        /// <summary>
        /// 获取退过货以及退过款品牌
        /// </summary>
        /// <returns></returns>
        public string GetBrandlist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string sql = @"select c.BrandAbridge,c.BrandName from ProductInfo a
                           left join product b on a.Scode=b.Scode
                           left join brand c on b.Cat=c.BrandAbridge
                           group by c.BrandAbridge,c.BrandName";
            dt = DbHelperSQL.Query(sql).Tables[0];
            s.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<option value='" + dt.Rows[i]["BrandAbridge"] + "'>" + dt.Rows[i]["BrandName"] + "</option>");
            }
            return s.ToString();
        }

        /// <summary>
        /// 获取退过货以及退过款类别
        /// </summary>
        /// <returns></returns>
        public string GetTypelist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string sql = @"select c.TypeNo,c.TypeName from ProductInfo a
                           left join product b on a.Scode=b.Scode
                           left join producttype c on b.Cat2=c.TypeNo
                           group by c.TypeNo,c.TypeName";
            dt = DbHelperSQL.Query(sql).Tables[0];
            s.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<option value='" + dt.Rows[i]["TypeNo"] + "'>" + dt.Rows[i]["TypeName"] + "</option>");
            }
            return s.ToString();
        }
        /// <summary>
        /// 类别下拉框
        /// </summary>
        /// <param name="TypeNo"></param>
        /// <returns></returns>
        public string GetType(string TypeNo)
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = DbHelperSQL.Query(@"select * from producttype").Tables[0];
            s.Append("<option value='' >请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (TypeNo == dt.Rows[i]["TypeNo"].ToString())
                {
                    s.Append("<option value='" + dt.Rows[i]["TypeNo"] + "' selected='selected'>" + dt.Rows[i]["TypeName"] + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt.Rows[i]["TypeNo"] + "'>" + dt.Rows[i]["TypeName"] + "</option>");
                }

            }
            return s.ToString();
        }

        /// <summary>
        /// 品牌下拉框
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        public string GetBrand(string BrandName)
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = DbHelperSQL.Query(@"select * from brand").Tables[0];
            s.Append("<option value='' >请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (BrandName == dt.Rows[i]["BrandName"].ToString())
                {
                    s.Append("<option value='" + dt.Rows[i]["BrandAbridge"] + "' selected='selected'>" + dt.Rows[i]["BrandName"] + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt.Rows[i]["BrandAbridge"] + "'>" + dt.Rows[i]["BrandName"] + "</option>");
                }

            }
            return s.ToString();
        }

    }

}