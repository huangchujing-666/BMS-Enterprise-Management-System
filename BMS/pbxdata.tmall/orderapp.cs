/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apporder
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.tmall
    * 文 件 名：       apporder
    * 创建时间：       2015-04-01 14:04:20
    * 作    者：       lcg
    * 说    明：       获取订单（主订单，子订单）数据
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace pbxdata.tmall
{
    public class orderapp : tmallapp
    {
        //public orderapp(string appkey, string appsecret) : base(appkey, appsecret) { }


        #region 主订单

        #region 读取订单
        /// <summary>
        /// 返回读取的订单
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <param name="index">当前第几页</param>
        /// <param name="pagesize">每页读取的数据量</param>
        /// <param name="hasNext">是否存在下一页</param>
        /// <returns></returns>
        public List<Trade> getSoldTrade(string datetime1, string datetime2, int index, int pagesize, out bool hasNext)
        {
            List<Trade> list = new List<Trade>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            TradesSoldGetRequest req = new TradesSoldGetRequest();
            req.Fields = "seller_nick, buyer_nick, type, created, tid, status, payment, pay_time, end_time, modified, consign_time";
            req.StartCreated = DateTime.Parse(datetime1);
            req.EndCreated = DateTime.Parse(datetime2);
            req.PageNo = index;
            req.PageSize = pagesize;
            if (Sessionkey == "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799")
            {
                req.Type = "tmall_i18n,step";
            }
            req.UseHasNext = true;
            TradesSoldGetResponse response = client.Execute(req, Sessionkey);
            list.AddRange(response.Trades);
            hasNext = response.HasNext;
            return list;
        }

        /// <summary>
        /// 返回所有订单
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <param name="index">当前第几页</param>
        /// <param name="pagesize">每页读取的数据量</param>
        /// <returns></returns>
        public List<Trade> getSoldTrades(string datetime1, string datetime2, int index = 1, int pagesize = 40)
        {
            List<Trade> list = new List<Trade>();

            DateTime d1 = helpcommon.ParmPerportys.GetDateTimeParms(datetime1);
            DateTime d2 = helpcommon.ParmPerportys.GetDateTimeParms(datetime2);
            int day2 = d2.Day; //结束日期的天
            int months = (d2.Year - d1.Year) * 12 + (d2.Month - d1.Month); //获取两个时间相差月份

            for (int i = 0; i <= months; i++)
            {
                DateTime d3 = d2.AddMonths(-i);
                int days = i == 0 ? d3.Day : System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(d3.Year, d3.Month);
                d3 = i == 0 ? d3 : d3.AddDays(days - day2);
                
                for (int j = 0; j < days; j++)
                {
                    DateTime d5 = d3.AddDays(-j);
                    string d6 = d5.ToString("yyyy-MM-dd") + " 00:00:00";
                    string d7 = d5.ToString("yyyy-MM-dd") + " 23:59:59";
                    bool hasNext = true;
                    index = 1;
                    pagesize = 40;

                    while (hasNext)
                    {
                        list.AddRange(getSoldTrade(d6, d7, index, pagesize, out hasNext));
                        index++;
                    }
                }
            }

            return list;
        }
        #endregion


        #region 更新订单
        /// <summary>
        /// 获取增量订单
        /// </summary>
        /// <returns></returns>
        public List<Trade> getSoldIncrement(string datetime1, string datetime2, int index, int pagesize, out bool hasNext)
        {
            List<Trade> list = new List<Trade>();

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            TradesSoldIncrementGetRequest req = new TradesSoldIncrementGetRequest();
            req.Fields = "seller_nick, buyer_nick, type, created, tid, status, payment, pay_time, end_time, modified, consign_time,orders";
            req.StartModified = DateTime.Parse(datetime1);
            req.EndModified = DateTime.Parse(datetime2);
            req.PageNo = index;
            req.PageSize = pagesize;
            if (Sessionkey == "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799")
            {
                req.Type = "tmall_i18n,step";
            }
            req.UseHasNext = true;
            TradesSoldIncrementGetResponse response = client.Execute(req, Sessionkey);
            list.AddRange(response.Trades);
            hasNext = response.HasNext;

            return list;
        }

        /// <summary>
        /// 获取所有增量订单（三个月以内）
        /// </summary>
        /// <param name="datetime1"></param>
        /// <param name="datetime2"></param>
        /// <param name="index"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<Trade> getSoldIncrements(string datetime1, string datetime2, int index = 1, int pagesize = 40)
        {
            List<Trade> list = new List<Trade>();

            DateTime d1 = helpcommon.ParmPerportys.GetDateTimeParms(datetime1);
            DateTime d2 = helpcommon.ParmPerportys.GetDateTimeParms(datetime2);
            int day2 = d2.Day; //结束日期的天
            int months = (d2.Year - d1.Year) * 12 + (d2.Month - d1.Month); //获取两个时间相差月份

            //for (int i = 0; i < months; i++)
            //{
                //DateTime d3 = d2.AddMonths(-i);
                //int days = i == 0 ? d3.Day : System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(d3.Year, d3.Month);
                //d3 = i == 0 ? d3 : d3.AddDays(days - day2);

            //DateTime d3 = d2;
            //int days = months == 0 ? d2.Day - d1.Day : System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(d3.Year, d3.Month);
            //d3 = d3.AddDays(days - day2);

                for (int j = 0; j < 9; j++)
                {
                    DateTime d5 = d2.AddDays(-j);
                    string d6 = d5.ToString("yyyy-MM-dd") + " 00:00:00";
                    string d7 = d5.ToString("yyyy-MM-dd") + " 23:59:59";
                    bool hasNext = true;
                    index = 1;
                    pagesize = 40;

                    while (hasNext)
                    {
                        list.AddRange(getSoldIncrement(d6, d7, index, pagesize, out hasNext));
                        index++;
                    }
                }
            //}

            return list;
        }

        #endregion

        #endregion


        #region 子订单(完整信息)

        #region 读取子订单

        public List<Trade> getFullInfo(string orderId)
        {
            List<Trade> list = new List<Trade>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            req.Fields = "tid,created,pay_time,modified,orders";
            req.Tid = helpcommon.ParmPerportys.GetLongParms(orderId);
            TradeFullinfoGetResponse response = client.Execute(req, Sessionkey);
            if (response.Trade!=null)
            {
                list.Add(response.Trade);
            }

            return list;
        }


        #endregion

        #region 更新子订单

        #endregion

        #endregion


        #region 子订单(部分信息)

        #region 读取子订单
        /// <summary>
        /// 读取子订单(部分信息)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<Trade> getTradePartial(string orderId)
        {
            List<Trade> list = new List<Trade>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            req.Fields = "tid,created,pay_time,modified,orders";
            req.Tid = helpcommon.ParmPerportys.GetLongParms(orderId);
            TradeFullinfoGetResponse response = client.Execute(req, Sessionkey);
            if (response.Trade != null)
            {
                list.Add(response.Trade);
            }

            return list;
        }


        #endregion

        #endregion


    }   
}
