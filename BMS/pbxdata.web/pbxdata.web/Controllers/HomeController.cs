using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
            ViewData["myMenuId"] = menuId;
            return View();
        }

        public ActionResult Welcome()
        {

            ViewData["UserName"] = userInfo.User.userName;
            return View();
        }

        //public string ReturnInfo()
        //{
        //    string s = string.Empty;
        //    model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
        //    var q = from c in context.apiOrderDetails
        //            join t in context.apiOrder on c.orderId equals t.orderId
        //            into tt
        //            from ss in tt.DefaultIfEmpty()
        //            select new
        //            {
        //                OrderId = c.orderId,            //--订单号
        //                paidPrice = ss.paidPrice,     //--  实际支付价格  
        //                isPay = ss.isPay,//--是否支付
        //                detailsSaleCount = c.detailsSaleCount,//--交易数量
        //                createTime = ss.createTime,//--创建时间
        //            };

        //    DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        //    DateTime tomorrow = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

        //    var Count = q.Where(a => a.isPay == 1).GroupBy(a => a.OrderId).Count();

        //    var SumPrice = q.Where(a => a.isPay == 1).Sum(a => a.paidPrice);
        //    SumPrice = SumPrice == null ? 0 : SumPrice;

        //    var BalaceCount = q.Where(a => a.isPay == 1).Sum(a => a.detailsSaleCount);
        //    BalaceCount = BalaceCount == null ? 0 : BalaceCount;

        //    var SumPrice1 = q.Where(a => a.isPay == 2).Sum(a => a.paidPrice);
        //    SumPrice1 = SumPrice1 == null ? 0 : SumPrice1;

        //    var BalaceCount1 = q.Where(a => a.isPay == 2).Sum(a => a.detailsSaleCount);
        //    BalaceCount1 = BalaceCount1 == null ? 0 : BalaceCount1;

        //    var tCount = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).GroupBy(a => a.OrderId).Count();

        //    var tSumPrice = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.paidPrice);
        //    tSumPrice = tSumPrice == null ? 0 : tSumPrice;

        //    var tBalaceCount = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.detailsSaleCount);
        //    tBalaceCount = tBalaceCount == null ? 0 : tBalaceCount;

        //    var tSumPrice1 = q.Where(a => a.isPay == 2 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.paidPrice);
        //    tSumPrice1 = tSumPrice1 == null ? 0 : tSumPrice1;
        //    var tBalaceCount1 = q.Where(a => a.isPay == 2 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.detailsSaleCount);
        //    tBalaceCount1 = tBalaceCount1 == null ? 0 : tBalaceCount1;
        //    s += "<ul class='ulCount'>";
        //    s += "<li>今日支付笔数:<span>" + tCount + "</span>.</li>";
        //    s += "<li>今日支付总金额:<span>" + Convert.ToDecimal(tSumPrice).ToString("f2") + "</span>￥</li>";
        //    s += "<li>今日销售总件:<span>" + tBalaceCount + "</span>.</li>";
        //    s += "<li>今日未支付笔数:<span>" + tBalaceCount1 + "</span>.</li>";
        //    s += "<li>今日未支付金额:<span>" + Convert.ToDecimal(tSumPrice1).ToString("f2") + "</span>￥.</li>";
        //    s += "</ul>";
        //    s += "<div class='clearfix'></div>";
        //    s += "<ul class='ulCount'>";
        //    s += "<li>支付笔数:<span>" + Count + "</span>.</li>";
        //    s += "<li>支付总金额:<span>" + Convert.ToDecimal(SumPrice).ToString("f2") + "</span>￥.</li>";
        //    s += "<li>销售总件:<span>" + BalaceCount + "</span>.</li>";
        //    s += "<li>未支付笔数:<span>" + BalaceCount1 + "</span>.</li>";
        //    s += "<li>未支付金额:<span>" + Convert.ToDecimal(SumPrice1).ToString("f2") + "</span>￥.</li>";
        //    s += "</ul>";

        //    //s = "[{\"Count\":" + Count + ",\"SumPrice\":" + SumPrice + ",\"BalaceCount\":" + BalaceCount + ",\"SumPrice1\":" + SumPrice1 + ",\"BalaceCount1\":" + BalaceCount1 + ",\"tCount\":" + tCount + ",\"tSumPrice\":" + tSumPrice + ",\"tBalaceCount\":" + tBalaceCount + ",\"tSumPrice1\":" + tSumPrice1 + ",\"tBalaceCount1\":" + tBalaceCount1 + "}]";
        //    return s;
        //}

        //public string ReturnProduct()
        //{
        //    string s = string.Empty;
        //    model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
        //    var sql = @"select top 20 BrandName,SUM(Balance) as balance from productstock a left join brand b on a.Cat=b.BrandAbridge where BrandName is not null group by BrandName order by balance desc";
        //    var sql1 = @"select top 20 TypeName,SUM(Balance) as balance from productstock a left join producttype b on a.Cat2=b.TypeNo where TypeName is not null group by TypeName order by balance desc";
        //    //var q = from c in context.productstock
        //    //        group c by new { c.Cat2 } into g
        //    //        select new
        //    //        {
        //    //            Cat2 = g.Key.Cat2,
        //    //            CountBa = g.Sum(a => a.Balance),
        //    //        };
        //    //var qCat2 = from c in q.OrderByDescending(a => a.CountBa)
        //    //            join p in context.producttype on c.Cat2 equals p.TypeNo
        //    //             into pp
        //    //            from ppp in pp.DefaultIfEmpty()

        //    //            select new
        //    //            {
        //    //                TypeName = ppp.TypeName,
        //    //                CountBa = c.CountBa,
        //    //            };
        //    //DataTable dt3 = LinqToDataTable.LINQToDataTable(qCat2.Where(a => a.TypeName != null));
        //    var Brand = context.product.GroupBy(a => a.Cat2).Count();
        //    var Balance = context.productstock.Sum(a => a.Balance);
        //    var Imagefile = context.product.Where(a => a.Imagefile != "" && a.Imagefile != null).Count();
        //    var TypeName = context.product.GroupBy(a => a.Cat).Count();
        //    var Style = context.product.GroupBy(a => a.Style).Count();
        //    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
        //    s += "<ul class='ulCount'>";
        //    s += "<li>总品牌数:<span>" + Brand + "</span>.</li>";
        //    s += "<li>总类别数:<span>" + TypeName + "</span>.</li>";
        //    s += "<li>总款数:<span>" + Style + "</span>.</li>";
        //    s += "<li>总件数:<span>" + Balance + "</span>.</li>";
        //    s += "<li>有图片sku数:<span>" + Imagefile + "</span>.</li>";
        //    s += "</ul>";
        //    s += "-*-";
        //    for (int i = 0; i < 20; i++)
        //    {
        //        int n = i + 1;
        //        s += n + "." + dt.Rows[i]["BrandName"].ToString() + "<br />";
        //    }
        //    s += "-*-";
        //    for (int i = 0; i < 20; i++)
        //    {
        //        int n = i + 1;
        //        s += n + "." + dt1.Rows[i]["TypeName"].ToString() + "<br />";
        //    }


        //    return s;
        //}
    }
}
