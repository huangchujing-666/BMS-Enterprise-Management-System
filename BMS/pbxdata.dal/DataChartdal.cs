using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class DataChartdal : dataoperating, iDataChart
    {

        dal.Errorlogdal errorlog = new Errorlogdal();

        /// <summary>
        /// 销售统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnInfo()
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.apiOrderDetails
                    join t in context.apiOrder on c.orderId equals t.orderId
                    into tt
                    from ss in tt.DefaultIfEmpty()
                    select new
                    {
                        OrderId = c.orderId,            //--订单号
                        paidPrice = ss.paidPrice,     //--  实际支付价格  
                        orderPrice = ss.orderPrice,     //--订单金额
                        isPay = ss.isPay,//--是否支付 0：未支付，1：已支付
                        detailsSaleCount = c.detailsSaleCount,//--交易数量
                        createTime = ss.createTime,//--创建时间
                    };



            var Count = q.Where(a => a.isPay == 1).GroupBy(a => a.OrderId).Count();//销售所有数量

            var SumPrice = q.Where(a => a.isPay == 1).Sum(a => a.orderPrice);//销售已支付总金额
            SumPrice = SumPrice == null ? 0 : SumPrice;

            var BalaceCount = q.Where(a => a.isPay == 1).Sum(a => a.detailsSaleCount);//销售已支付总库存
            BalaceCount = BalaceCount == null ? 0 : BalaceCount;

            var SumPrice1 = q.Where(a => a.isPay == 0).Sum(a => a.orderPrice);//销售未支付总金额
            SumPrice1 = SumPrice1 == null ? 0 : SumPrice1;

            var BalaceCount1 = q.Where(a => a.isPay == 0).Sum(a => a.detailsSaleCount);//销售未支付库存
            BalaceCount1 = BalaceCount1 == null ? 0 : BalaceCount1;



            //s += "<div class='clearfix'></div>";
            //s += "<ul class='ulCount'>";
            //s += "<li>支付笔数:<span>" + Count + "</span>.</li>";
            //s += "<li>支付总金额:<span>" + Convert.ToDecimal(SumPrice).ToString("f2") + "</span>￥.</li>";
            //s += "<li>销售总件:<span>" + BalaceCount + "</span>.</li>";
            //s += "<li>未支付笔数:<span>" + BalaceCount1 + "</span>.</li>";
            //s += "<li>未支付金额:<span>" + Convert.ToDecimal(SumPrice1).ToString("f2") + "</span>￥.</li>";
            //s += "</ul>";

            s = "[{\"Count\":" + Count + ",\"SumPrice\":" + Convert.ToDecimal(SumPrice).ToString("f2") + ",\"BalaceCount\":" + BalaceCount + ",\"SumPrice1\":" + Convert.ToDecimal(SumPrice1).ToString("f2") + ",\"BalaceCount1\":" + BalaceCount1 + "}]";
            return s;
        }

        /// <summary>
        /// 销售统计--根据日期
        /// </summary>
        /// <returns></returns>
        public string ReturnInfo(Dictionary<string, string> Dic)
        {
            string s = string.Empty;
            string Mintime = Dic["Mintime"];
            string Maxtime = Dic["Maxtime"];
            DateTime Mindate = Mintime == ""||Mintime=="1" ? Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) : Convert.ToDateTime(Mintime);
            DateTime Maxdate = Maxtime == "" || Maxtime == "1" ? Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")) : Convert.ToDateTime(Maxtime);
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.apiOrderDetails
                    join t in context.apiOrder on c.orderId equals t.orderId
                    into tt
                    from ss in tt.DefaultIfEmpty()
                    select new
                    {
                        OrderId = c.orderId,            //--订单号
                        paidPrice = ss.paidPrice,     //--  实际支付价格  
                        orderPrice = ss.orderPrice,     //--订单金额
                        isPay = ss.isPay,//--是否支付0：未支付，1：已支付
                        detailsSaleCount = c.detailsSaleCount,//--交易数量
                        createTime = ss.createTime,//--创建时间
                    };
            if (Mintime == "1")
            {
                q = q.Where(a => a.createTime <= Maxdate && a.createTime >= Mindate);
            }
            else
            {
                if (Mintime != "")
                {
                    if (Maxtime != "")
                    {
                        q = q.Where(a => a.createTime <= Maxdate && a.createTime >= Mindate);
                    }
                    else
                    {
                        q = q.Where(a => a.createTime >= Mindate);
                    }
                }
                else
                {
                    if (Maxtime != "")
                    {
                        q = q.Where(a => a.createTime <= Maxdate);
                    }
                }
            }


            //根据日期查询
            var Count = q.Where(a => a.isPay == 1).GroupBy(a => a.OrderId).Count();//销售所有数量

            var SumPrice = q.Where(a => a.isPay == 1).Sum(a => a.orderPrice);//销售已支付总金额
            SumPrice = SumPrice == null ? 0 : SumPrice;

            var BalaceCount = q.Where(a => a.isPay == 1).Sum(a => a.detailsSaleCount);//销售已支付总库存
            BalaceCount = BalaceCount == null ? 0 : BalaceCount;

            var SumPrice1 = q.Where(a => a.isPay == 0).Sum(a => a.orderPrice);//销售未支付总金额
            SumPrice1 = SumPrice1 == null ? 0 : SumPrice1;
            var BalaceCount1 = q.Where(a => a.isPay == 0).Sum(a => a.detailsSaleCount);//销售未支付库存
            BalaceCount1 = BalaceCount1 == null ? 0 : BalaceCount1;

            //s += "<ul class='ulCount'>";
            //s += "<li>今日支付笔数:<span>" + tCount + "</span>.</li>";
            //s += "<li>今日支付总金额:<span>" + Convert.ToDecimal(tSumPrice).ToString("f2") + "</span>￥</li>";
            //s += "<li>今日销售总件:<span>" + tBalaceCount + "</span>.</li>";
            //s += "<li>今日未支付笔数:<span>" + tBalaceCount1 + "</span>.</li>";
            //s += "<li>今日未支付金额:<span>" + Convert.ToDecimal(tSumPrice1).ToString("f2") + "</span>￥.</li>";
            //s += "</ul>";
            s = "[{\"Count\":" + Count + ",\"SumPrice\":" + Convert.ToDecimal(SumPrice).ToString("f2") + ",\"BalaceCount\":" + BalaceCount + ",\"SumPrice1\":" + Convert.ToDecimal(SumPrice1).ToString("f2") + ",\"BalaceCount1\":" + BalaceCount1 + "}]";
            return s;
        }


        /// <summary>
        /// 获取来货报表（HK数据源)
        /// </summary>
        /// <returns></returns>
        public DataTable GetLaiHuoReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts, out string Balances)
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productstock
                    where c.Cat != null && c.Cat != "" && c.Vencode == "1"
                    group c by new { c.Cat, c.Cat2, c.Lastgrnd } into t
                    select new
                    {
                        Cat = t.Key.Cat,//品牌缩写
                        Cat2 = t.Key.Cat2,//类别编号
                        Lastgrnd = t.Key.Lastgrnd,//来货时间
                        Balance = t.Sum(a => a.Balance),//来货库存
                    };
            var q1 = from c in q
                     join b in context.brand on c.Cat equals b.BrandAbridge
                     into bb
                     from bbb in bb.DefaultIfEmpty()
                     join p in context.producttype on c.Cat2 equals p.TypeNo
                     into pp
                     from ppp in pp.DefaultIfEmpty()
                     select new
                     {
                         Cat = c.Cat,//品牌缩写
                         BrandName = bbb.BrandName,//品牌名称
                         Cat2 = c.Cat2,//类别编号
                         TypeName = ppp.TypeName,//类别名称
                         Balance = c.Balance,//来货库存
                         Lastgrnd = c.Lastgrnd,//来货时间
                     };
            if (Dic["Cat"] != "")
            {
                q1 = q1.Where(a => a.Cat == Dic["Cat"]);
            }
            if (Dic["Cat2"] != "")
            {
                q1 = q1.Where(a => a.Cat2 == Dic["Cat2"]);
            }
            if (Dic["Mindate"] != "")
            {
                if (Dic["Maxdate"] != "")
                {
                    q1 = q1.Where(a => a.Lastgrnd >= Convert.ToDateTime(Dic["Mindate"]) && a.Lastgrnd <= Convert.ToDateTime(Dic["Maxdate"]));
                }
                else
                {
                    q1 = q1.Where(a => a.Lastgrnd >= Convert.ToDateTime(Dic["Mindate"]));
                }
            }
            else
            {
                if (Dic["Maxdate"] != "")
                {
                    q1 = q1.Where(a => a.Lastgrnd <= Convert.ToDateTime(Dic["Maxdate"]));
                }
            }
            counts = q1.Count().ToString();//返回查询数量
            int? balanceCount = q1.Sum(a => a.Balance);//返回查询库存
            Balances = balanceCount.ToString();
            q1 = q1.OrderByDescending(a => a.Balance).OrderByDescending(a => a.Lastgrnd);//根据库存和来货时间排序
            if (pageIndex == 0)
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Take(pageSize));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            }
            return dt;
        }

        /// <summary>
        /// 获取来货报表（HK数据源)--今日
        /// </summary>
        /// <returns></returns>
        public DataTable GetLaiHuoReportT()
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            DateTime Mindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime Maxdate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productstock
                    where c.Cat != null && c.Cat != "" && c.Vencode == "1"
                    group c by new { c.Cat, c.Cat2, c.Lastgrnd } into t
                    select new
                    {
                        Cat = t.Key.Cat,//品牌缩写
                        Cat2 = t.Key.Cat2,//类别编号
                        Lastgrnd = t.Key.Lastgrnd,//来货时间
                        Balance = t.Sum(a => a.Balance),//来货库存
                    };
            var q1 = from c in q
                     join b in context.brand on c.Cat equals b.BrandAbridge
                     into bb
                     from bbb in bb.DefaultIfEmpty()
                     join p in context.producttype on c.Cat2 equals p.TypeNo
                     into pp
                     from ppp in pp.DefaultIfEmpty()
                     select new
                     {
                         Cat = c.Cat,
                         BrandName = bbb.BrandName,
                         Cat2 = c.Cat2,
                         TypeName = ppp.TypeName,
                         Balance = c.Balance,
                         Lastgrnd = c.Lastgrnd,
                     };
            q1 = q1.Where(a => a.Lastgrnd >= Mindate && a.Lastgrnd < Maxdate);
            q1 = q1.OrderByDescending(a => a.Balance).OrderByDescending(a => a.Lastgrnd);
            dt = LinqToDataTable.LINQToDataTable(q1);
            return dt;
        }
        /// <summary>
        /// 商品统计--总数
        /// </summary>
        /// <returns></returns>
        public string ReturnProduct()
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();

            var Brand = context.product.GroupBy(a => a.Cat2).Count();//品牌数量
            var Balance = context.productstock.Sum(a => a.Balance);//库存数量
            var Imagefile = context.product.Where(a => a.Imagefile != "" && a.Imagefile != null).Count();//缩略图数量
            var TypeName = context.product.GroupBy(a => a.Cat).Count();//类别数量
            var Style = context.product.GroupBy(a => a.Style).Count();//款数
            s = "[{\"Brand\":" + Brand + ",\"TypeName\":" + TypeName + ",\"Style\":" + Style + ",\"Balance\":" + Balance + ",\"Imagefile\":" + Imagefile + "}]";
            return s;
        }
        /// <summary>
        /// 退款退货统计-总数
        /// </summary>
        /// <returns></returns>
        public string Refund()
        {
            try
            {
                string s = string.Empty;
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                DateTime Mindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime Maxdate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                var q = from c in context.RetProductInfo select c;
                var Refund = 0;//--退款笔数
                var RefundM = Convert.ToDecimal(0);//--退款金额
                var ReturnGoods = 0;//--退货笔数
                var ReturnGoodsM = Convert.ToDecimal(0);//--退货金额
                var ReturnBalance = Convert.ToDecimal(0);//--退货件数
                var qc = from c in context.RetBalance select c; //--判断退货数据
                if (q.Where(a => a.Def1 == "3").Count() != 0)
                {
                    Refund = q.Where(a => a.Def1 == "3").Count();//--退款笔数
                    RefundM = q.Where(a => a.Def1 == "3").Sum(a => Convert.ToDecimal(a.RetPrice == null ? "0" : a.RetPrice));//--退款金额
                }
                if (q.Where(a => a.Def1 == "4").Count() != 0)
                {
                    ReturnGoods = q.Where(a => a.Def1 == "4").Count();//--退货笔数
                    ReturnGoodsM = q.Where(a => a.Def1 == "4").Sum(a => Convert.ToDecimal(a.RetPrice == null ? "0" : a.RetPrice));//--退货金额
                }
                if (qc.Count() != 0)
                {
                    ReturnBalance = qc.Sum(a => Convert.ToDecimal(a.Number == null ? "0" : a.Number));//--退货件数
                }

                q = q.Where(a => a.Def4 >= Mindate && a.Def4 < Maxdate);
                var Refund1 = 0;//--退款笔数
                var RefundM1 = Convert.ToDecimal(0);//--退款金额
                var ReturnGoods1 = 0;//--退货笔数
                var ReturnGoodsM1 = Convert.ToDecimal(0);//--退货金额
                var ReturnBalance1 = Convert.ToDecimal(0);//--退货件数
                q = q.Where(a => a.Def4 >= Mindate && a.Def4 < Maxdate);//--今日
                qc = qc.Where(a => a.RetTime >= Mindate && a.RetTime < Maxdate);
                if (q.Count() != 0)
                {
                    if (q.Where(a => a.Def1 == "3").Count() != 0)
                    {
                        Refund1 = q.Where(a => a.Def1 == "3").Count();//--退款笔数
                        RefundM1 = q.Where(a => a.Def1 == "3").Sum(a => Convert.ToDecimal(a.RetPrice == null ? "0" : a.RetPrice));//--退款金额
                    }
                    if (q.Where(a => a.Def1 == "4").Count() != 0)
                    {
                        ReturnGoods1 = q.Where(a => a.Def1 == "4").Count();//--退货笔数
                        ReturnGoodsM1 = q.Where(a => a.Def1 == "4").Sum(a => Convert.ToDecimal(a.RetPrice == null ? "0" : a.RetPrice));//--退货金额
                    }
                    if (qc.Count() != 0)
                    {
                        ReturnBalance1 = qc.Sum(a => Convert.ToDecimal(a.Number == null ? "0" : a.Number));//--退货件数
                    }

                }

                ////--判断参数为空的时候为0
                //RefundM = RefundM == null ? 0 : RefundM;
                //ReturnGoodsM = ReturnGoodsM == null ? 0 : ReturnGoodsM;
                //ReturnBalance = ReturnBalance == null ? 0 : ReturnBalance;
                s += "[{";
                s += "\"Refund\":" + Refund + ",\"RefundM\":" + Convert.ToDecimal(RefundM).ToString("f2") + ",\"ReturnGoods\":" + ReturnGoods + ",\"ReturnGoodsM\":" + Convert.ToDecimal(ReturnGoodsM).ToString("f2") + ",\"ReturnBalance\":" + Convert.ToInt32(ReturnBalance) + ",";
                s += "\"Refund1\":" + Refund1 + ",\"RefundM1\":" + Convert.ToDecimal(RefundM1).ToString("f2") + ",\"ReturnGoods1\":" + ReturnGoods1 + ",\"ReturnGoodsM1\":" + Convert.ToDecimal(ReturnGoodsM1).ToString("f2") + ",\"ReturnBalance1\":" + Convert.ToInt32(ReturnBalance1) + "";
                s += "}]";

                return s;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        /// <summary>
        /// 获取品牌报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productstock
                    where c.Cat != null && c.Cat != ""
                    group c by new { c.Cat, c.Style, c.Vencode } into t
                    select new
                    {
                        Cat = t.Key.Cat,//品牌缩写
                        Style = t.Key.Style.Count(),//款数
                        Balance = t.Sum(a => a.Balance),//库存数
                        Vencode = t.Key.Vencode,//供应商
                    };
            var q1 = from c in q
                     group c by new { c.Cat, c.Vencode } into t
                     join p in context.brand on t.Key.Cat equals p.BrandAbridge
                     into pp
                     from ppp in pp.DefaultIfEmpty()
                     join b in context.productsource on t.Key.Vencode equals b.SourceCode
                     into bb
                     from bbb in bb.DefaultIfEmpty()
                     select new
                     {
                         Cat = t.Key.Cat,//品牌缩写
                         BrandName = ppp.BrandName,//品牌名称
                         Style = t.Sum(a => a.Style),//款数
                         Balance = t.Sum(a => a.Balance),//库存数
                         Vencode = t.Key.Vencode,//供应商
                         SourceName = bbb.sourceName,//供应商名称
                     };
            if (Dic["Vencode"] != "")
            {
                q1 = q1.Where(a => a.Vencode == Dic["Vencode"]);
            }
            if (Dic["Cat"] != "")
            {
                q1 = q1.Where(a => a.Cat == Dic["Cat"]);
            }
            counts = q1.Count().ToString();//返回查询数量
            q1 = q1.OrderByDescending(a => a.Style).OrderByDescending(a => a.Balance);//排序
            if (pageIndex == 0)//翻页
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Take(pageSize));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            }
            return dt;
        }

        /// <summary>
        /// 获取类别报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productstock
                    where c.Cat2 != null && c.Cat2 != ""
                    group c by new { c.Cat2, c.Style, c.Vencode } into t
                    select new
                    {
                        Cat2 = t.Key.Cat2,
                        Style = t.Key.Style.Count(),
                        Balance = t.Sum(a => a.Balance),
                        Vencode = t.Key.Vencode,
                    };
            var q1 = from c in q
                     group c by new { c.Cat2, c.Vencode } into t
                     join p in context.producttype on t.Key.Cat2 equals p.TypeNo
                     into pp
                     from ppp in pp.DefaultIfEmpty()
                     join b in context.productsource on t.Key.Vencode equals b.SourceCode
                     into bb
                     from bbb in bb.DefaultIfEmpty()
                     select new
                     {
                         TypeNo = t.Key.Cat2,
                         TypeName = ppp.TypeName,
                         Style = t.Sum(a => a.Style),
                         Balance = t.Sum(a => a.Balance),
                         Vencode = t.Key.Vencode,
                         SourceName = bbb.sourceName,
                     };
            if (Dic["Vencode"] != "")
            {
                q1 = q1.Where(a => a.Vencode == Dic["Vencode"]);
            }
            if (Dic["TypeNo"] != "")
            {
                q1 = q1.Where(a => a.TypeNo == Dic["TypeNo"]);
            }
            counts = q1.Count().ToString();
            q1 = q1.OrderByDescending(a => a.Style).OrderByDescending(a => a.Balance);
            if (pageIndex == 0)
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Take(pageSize));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q1.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            }
            return dt;
        }

        /// <summary>
        /// 退款报表 Convert.ToDateTime(Convert.ToDateTime(c.Def3).ToString("yyyy-mm-dd"))}
        /// </summary>
        /// <returns></returns>
        public DataTable GetRefundReport(string MinTime, string MaxTime)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            DataTable dt = new DataTable();

            var q = from c in context.ProductInfo
                    where c.Def1 == "6"//表示退款的订单
                    group c by Convert.ToDateTime(c.Def3 == null ? "2000-01-01" : c.Def3.ToString().Substring(0, 10)) into t
                    select new
                    {
                        Def3 = Convert.ToDateTime(t.Key == null ? "2000-01-01" : t.Key.ToString().Substring(0, 10)),//退款日期
                        counts = t.Count(),//退款数量
                        SellPrice = t.Sum(c => Convert.ToDecimal((c.SellPrice == null || c.SellPrice == "") ? "0" : c.SellPrice)),//退款总金额c => Convert.ToDecimal(c.SellPrice)
                    };
            if (MinTime != "")
            {
                if (MaxTime != "")
                {
                    q = q.Where(a => a.Def3 >= Convert.ToDateTime(MinTime) && a.Def3 <= Convert.ToDateTime(MaxTime));
                }
                else
                {
                    q = q.Where(a => a.Def3 >= Convert.ToDateTime(MinTime));
                }
            }
            else
            {
                if (MaxTime != "")
                {
                    q = q.Where(a => a.Def3 <= Convert.ToDateTime(MaxTime));
                }
            }
            dt = LinqToDataTable.LINQToDataTable(q);

            return dt;
        }

        /// <summary>
        /// 退货报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetReturnBalanceReport(string MinTime, string MaxTime)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            var q = from c in context.ProductInfo
                    where c.Def1 == "4"//退货订单
                    group c by Convert.ToDateTime(c.Def3 == null ? "2000-01-01" : c.Def3.ToString().Substring(0, 10)) into t
                    select new
                    {
                        Def3 = Convert.ToDateTime(t.Key == null ? "2000-01-01" : t.Key.ToString().Substring(0, 10)),//退货时间
                        counts = t.Count(),//退货数量
                        //SellPrice = decimal.Parse( t.Sum(c => Convert.ToDecimal(c.SellPrice == null ? "0" : c.SellPrice)).ToString()),//退货金额
                        SellPrice = t.Sum(c => Convert.ToDecimal((c.SellPrice == null || c.SellPrice == "") ? "0" : c.SellPrice)),
                        Balance = t.Sum(c => Convert.ToInt32(c.Number)),//退货数量
                    };
            if (MinTime != "")
            {
                if (MaxTime != "")
                {
                    q = q.Where(a => a.Def3 >= Convert.ToDateTime(MinTime) && a.Def3 <= Convert.ToDateTime(MaxTime));
                }
                else
                {
                    q = q.Where(a => a.Def3 >= Convert.ToDateTime(MinTime));
                }
            }
            else
            {
                if (MaxTime != "")
                {
                    q = q.Where(a => a.Def3 <= Convert.ToDateTime(MaxTime));
                }
            }


            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }

        /// <summary>
        /// 获取退货率报表信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetReturnRateReport(Dictionary<string, string> Dic)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            //var q = from c in context.ProductInfo
            //        join a in context.product on c.Scode equals a.Scode
            //        into aa
            //        from aaa in aa.DefaultIfEmpty()
            //        join b in context.brand on aaa.Cat equals b.BrandAbridge
            //        into bb
            //        from bbb in bb.DefaultIfEmpty()
            //        join d in context.producttype on aaa.Cat2 equals d.TypeNo
            //        into dd
            //        from ddd in dd.DefaultIfEmpty()
            //        select new
            //        {
            //            OrderId = c.OrderId,
            //            Scode = c.Scode,
            //            Brand = bbb.BrandAbridge,
            //            BrandName = bbb.BrandName,
            //            TypeNo = ddd.TypeNo,
            //            TypeName = ddd.TypeName,
            //            SellPrice = c.SellPrice,
            //        };
            //if (Dic["Brand"] != "")//--根据品牌
            //{
            //    q = q.Where(a => a.Brand == Dic["Brand"]);
            //}
            //if (Dic["TypeNo"] != "")//--根据类别
            //{
            //    q = q.Where(a => a.TypeNo == Dic["TypeNo"]);
            //}
            //if (Dic["SellPriceMin"] != "")//--根据价格 SellPriceMin最小--SellPriceMax最大  价格区间
            //{
            //    if (Dic["SellPriceMax"] != "")
            //    {
            //        q = q.Where(a => Convert.ToDecimal(a.SellPrice) <= Convert.ToDecimal(Dic["SellPriceMax"]) && Convert.ToDecimal(a.SellPrice) >= Convert.ToDecimal(Dic["SellPriceMin"]));
            //    }
            //    else
            //    {
            //        q = q.Where(a => Convert.ToDecimal(a.SellPrice) >= Convert.ToDecimal(Dic["SellPriceMin"]));
            //    }
            //}
            //else
            //{
            //    if (Dic["SellPrice2"] != "")
            //    {
            //        q = q.Where(a => Convert.ToDecimal(a.SellPrice) <= Convert.ToDecimal(Dic["SellPriceMax"]));
            //    }
            //}

            //dt = LinqToDataTable.LINQToDataTable(q);
            return dt;

        }

        /// <summary>
        /// 获取退货率报表信息--品牌
        /// </summary>
        /// <returns></returns>rate退货率 BrandName退货品牌
        public DataTable GetBrandRateReport(string Cat)
        {
            DataTable dt = new DataTable();
            string sql = @"select b.BrandName,t.Cat,(1.00*counts/allcounts)*100 as rate from( 
select b.Cat,COUNT(*) as allcounts from productinfo a 
left join product b on a.Scode=b.Scode 
group by Cat 
) t 
left join ( 
select b.Cat,COUNT(*) as counts from productinfo a 
left join product b on a.Scode=b.Scode 
where a.Def1=4 or a.Def1=6 
group by Cat 
) tt on t.Cat=tt.Cat 
left join brand b on t.Cat=b.BrandAbridge ";
            if (Cat != "")
            {
                sql += " where t.Cat='" + Cat + "'";
            }
            dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取退货率报表信息--类别
        /// </summary>
        /// <returns></returns>退货类别率
        public DataTable GetTypeRateReport(string TypeNo)
        {
            DataTable dt = new DataTable();
            string sql = @"select b.TypeName,t.Cat2,(1.00*counts/allcounts)*100 as rate from( 
select b.Cat2,COUNT(*) as allcounts from productinfo a 
left join product b on a.Scode=b.Scode 
group by Cat2 
) t 
left join ( 
select b.Cat2,COUNT(*) as counts from productinfo a 
left join product b on a.Scode=b.Scode 
where a.Def1=4 or a.Def1=6  
group by Cat2 
) tt on t.Cat2=tt.Cat2 
left join producttype b on t.Cat2=b.TypeNo ";
            if (TypeNo != "")
            {
                sql += " where t.Cat2='" + TypeNo + "'";
            }
            dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取退货率报表信息--价格
        /// </summary>
        /// <returns></returns>
        public DataTable GetSellPriceRateReport(string SellPriceMin, string SellPriceMax)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ProductInfo
                    group c by c.SellPrice into t
                    select new
                    {
                        SellPrice = t.Key,
                        allcounts = t.Key.Count(),
                    };
            var q1 = from c in context.ProductInfo
                     where c.Def1 == "4" || c.Def1 == "6"//退货状态
                     group c by c.SellPrice into t
                     select new
                     {
                         SellPrice = t.Key,
                         counts = t.Key.Count(),
                     };
            var lq = from c in q
                     join a in q1 on c.SellPrice equals a.SellPrice
                     into aa
                     from aaa in aa.DefaultIfEmpty()
                     select new
                     {
                         SellPrice = c.SellPrice,//退货金额
                         Rate = Convert.ToDecimal((aaa.counts == null||aaa.counts.ToString()=="") ? 0 : aaa.counts) / Convert.ToDecimal((c.allcounts==null||c.allcounts.ToString()=="")?0:c.allcounts) * 100,//退货率
                     };
            if (SellPriceMin != "")
            {
                if (SellPriceMax != "")
                {
                    lq = lq.Where(a => Convert.ToDecimal(a.SellPrice) >= Convert.ToDecimal(SellPriceMin) && Convert.ToDecimal(a.SellPrice) <= Convert.ToDecimal(SellPriceMax));
                }
                else
                {
                    lq = lq.Where(a => Convert.ToDecimal(a.SellPrice) >= Convert.ToDecimal(SellPriceMin));
                }
            }
            else
            {
                if (SellPriceMax != "")
                {
                    lq = lq.Where(a => Convert.ToDecimal(a.SellPrice) <= Convert.ToDecimal(SellPriceMax));
                }
            }

            //            string sql = @"select t.SellPrice,(1.00*counts/allcounts)*100 as rate from( 
            //select SellPrice,COUNT(SellPrice) as allcounts from productinfo group by SellPrice) 
            //t 
            //left join (select SellPrice,COUNT(SellPrice) as counts from productinfo  where Def1=4 or Def1=6  group by SellPrice) 
            //tt on t.SellPrice=tt.SellPrice where 1=1 ";
            //            if (SellPriceMin != "")
            //            {
            //                if (SellPriceMax != "")
            //                {
            //                    sql += "and t.SellPrice >='" + SellPriceMin + "' and t.SellPrice<='" + SellPriceMax + "'";
            //                }
            //                else
            //                {
            //                    sql += "and t.SellPrice >='" + SellPriceMin + "'";
            //                }
            //            }
            //            else
            //            {
            //                if (SellPriceMax != "")
            //                {
            //                    sql += "and t.SellPrice >='" + SellPriceMin + "' and t.SellPrice<='" + SellPriceMax + "'";
            //                }
            //            }
            //            dt = DbHelperSQL.Query(sql).Tables[0];
            dt = LinqToDataTable.LINQToDataTable(lq.OrderByDescending(a =>Convert.ToDecimal((a.SellPrice==null||a.SellPrice=="")?"0":a.SellPrice)));
            return dt;
        }

        /// <summary>
        /// 获取出货报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetShipmentReport(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            DataTable dt = new DataTable();
            // = Convert.ToDateTime(c.createTime.ToString().Substring(0, 10))
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            int Minnid = pageSize * (pageIndex - 1);
            int Maxnid = pageSize * (pageIndex);
            IDataParameter[] ipr = new IDataParameter[]{
                 new SqlParameter("Mindate",Dic["Mindate"]),//出货时间段
                 new SqlParameter("Maxdate",Dic["Maxdate"]),//出货时间段
                 new SqlParameter("sendSource",Dic["sendSource"]),//供应商
                 new SqlParameter("MinNid",Minnid),
                 new SqlParameter("MaxNid",Maxnid),
                 new SqlParameter("sql",""),
            };
            dt = Select(ipr, "GetShipmentReport");
            IDataParameter[] iprs = new IDataParameter[]{
                 new SqlParameter("Mindate",Dic["Mindate"]),//出货时间段
                 new SqlParameter("Maxdate",Dic["Maxdate"]),//出货时间段
                 new SqlParameter("sendSource",Dic["sendSource"]),//供应商
                 new SqlParameter("MinNid","0"),
                 new SqlParameter("MaxNid","999999"),
                 new SqlParameter("sql",""),
            };
            counts = Select(iprs, "GetShipmentReport").Rows.Count.ToString();
            //var q = from c in context.apiSendOrder
            //        group c by new { createTime = Convert.ToDateTime(c.createTime.ToString().Substring(0, 10)), c.sendSource } into t
            //        join a in context.productsource on t.Key.sendSource equals a.SourceCode
            //        into aa
            //        from aaa in aa.DefaultIfEmpty()
            //        select new
            //        {
            //            createTime = Convert.ToDateTime(t.Key.createTime.ToString().Substring(0, 10)),
            //            sendSource = t.Key.sendSource,
            //            sourceName = aaa.sourceName,
            //            weifa = t.Sum(a => a.newStatus == 3 ? a.newSaleCount : 0),
            //            yifa = t.Sum(a => a.newStatus == 4 ? a.newSaleCount : 0),
            //        };
            //if (Dic["Mindate"] != "")
            //{
            //    if (Dic["Maxdate"] != "")
            //    {
            //        q = q.Where(a => a.createTime >= Convert.ToDateTime(Dic["Mindate"]) && a.createTime <= Convert.ToDateTime(Dic["Maxdate"]));
            //    }
            //    else
            //    {
            //        q = q.Where(a => a.createTime >= Convert.ToDateTime(Dic["Mindate"]));
            //    }
            //}
            //else
            //{
            //    if (Dic["Maxdate"] != "")
            //    {
            //        q = q.Where(a => a.createTime <= Convert.ToDateTime(Dic["Maxdate"]));
            //    }
            //}
            //if (Dic["sendSource"] != "")
            //{
            //    q = q.Where(a => a.sendSource == Dic["sendSource"]);
            //}
            //counts = q.Count().ToString();
            //if (pageIndex == 0)
            //{
            //    dt = LinqToDataTable.LINQToDataTable(q.OrderByDescending(a => a.createTime).Take(pageSize));
            //}
            //else
            //{
            //    dt = LinqToDataTable.LINQToDataTable(q.OrderByDescending(a => a.createTime).Skip((pageIndex - 1) * pageSize).Take(pageSize));
            //}

            return dt;



        }

        /// <summary>
        /// 获取出货报表--今日出货报表
        /// </summary>
        /// <returns></returns>
        public DataTable GetShipmentReportT()
        {
            DataTable dt = new DataTable();
            DateTime Mindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime Maxdate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            //var q = from c in context.apiSendOrder
            //        group c by new { createTime = Convert.ToDateTime(c.createTime.ToString().Substring(0, 10)), c.sendSource } into t
            //        join a in context.productsource on t.Key.sendSource equals a.SourceCode
            //        into aa
            //        from aaa in aa.DefaultIfEmpty()
            //        select new
            //        {
            //            createTime = Convert.ToDateTime(t.Key.createTime.ToString().Substring(0, 10)),
            //            sendSource = t.Key.sendSource,
            //            sourceName = aaa.sourceName,
            //            weifa = t.Sum(a => a.newStatus == 3 ? a.newSaleCount : 0),
            //            yifa = t.Sum(a => a.newStatus == 4 ? a.newSaleCount : 0),
            //        };
            //q = q.Where(a => a.createTime >= Mindate && a.createTime < Maxdate);
            //dt = LinqToDataTable.LINQToDataTable(q.OrderByDescending(a => a.createTime));
            string sql = @"select a.*,b.sourceName from (
            select CONVERT(nvarchar(10),createTime,23) as createTime,sendSource,
            sum(case when newStatus=3 then 1 else 0 end) as weifa,
            SUM(case when newStatus=4 then 1 else 0 end) as yifa
            from apiSendOrder 
            group by CONVERT(nvarchar(10),createTime,23),sendSource) as  a
             left join productsource b on a.sendSource=b.SourceCode where createTime >='" + Mindate + "' and createTime< '" + Maxdate + "'";
            dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;

        }

    }
}
