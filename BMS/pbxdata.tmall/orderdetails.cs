/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       orderdetails
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.tmall
    * 文 件 名：       orderdetails
    * 创建时间：       2015-05-28 15:27:07
    * 作    者：       lcg
    * 说    明：       获取订单数据中其他属性或者参数
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
    public class orderdetails:tmallapp
    {



        #region 获取物流信息（这里指获取用户地址，电话等信息）
        /// <summary>
        /// 获取物流信息数量
        /// </summary>
        /// <returns></returns>
        public int getLogisticsCount(string date1, string date2)
        {
            List<Shipping> list = new List<Shipping>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            LogisticsOrdersDetailGetRequest req = new LogisticsOrdersDetailGetRequest();
            req.StartCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(date1);
            req.EndCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(date2);
            req.Fields = "tid";
            LogisticsOrdersDetailGetResponse response = client.Execute(req, Sessionkey);

            int i = int.Parse(response.TotalResults.ToString());
            return i;
        }


        /// <summary>
        /// 获取所有物流信息(获取买家信息)
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <returns></returns>
        public List<Shipping> getAllLogistics(string datetime1, string datetime2)
        {
            int num = getLogisticsCount(datetime1, datetime2);
            int pageSize = 100;
            int pageIndex = (num % pageSize) != 0 ? (num / pageSize) + 1 : num / pageSize;
            List<Shipping> list = new List<Shipping>();

            for (int i = 1; i <= pageIndex; i++)
            {
                list.AddRange(getLogistics(datetime1, datetime2, i, pageSize));
            }

            return list;
        }
        



        /// <summary>
        /// 获取物流信息(获取买家信息)
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <param name="index">当前第几页</param>
        /// <param name="pagesize">每页读取的数据量</param>
        /// <returns></returns>
        public List<Shipping> getLogistics(string datetime1, string datetime2, int index, int pagesize)
        {
            List<Shipping> list = new List<Shipping>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            LogisticsOrdersDetailGetRequest req = new LogisticsOrdersDetailGetRequest();
            req.Fields = "status,tid,buyer_nick,receiver_name,receiver_mobile,receiver_phone,receiver_location";
            req.StartCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(datetime1);
            req.EndCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(datetime2);
            req.PageNo = index;
            req.PageSize = pagesize;
            
            LogisticsOrdersDetailGetResponse response = client.Execute(req, Sessionkey);

            list.AddRange(response.Shippings);
            
            return list;
        }

        #endregion


        #region 获取物流运单号
        /// <summary>
        /// 获取物流运单号总数量
        /// </summary>
        /// <returns></returns>
        public int getLogisticsNoCount(string date1, string date2)
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            LogisticsOrdersGetRequest req = new LogisticsOrdersGetRequest();
            req.StartCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(date1);
            req.EndCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(date2);
            req.Fields = "tid";
            LogisticsOrdersGetResponse response = client.Execute(req, Sessionkey);

            int i = int.Parse(response.TotalResults.ToString());
            return i;
        }


        /// <summary>
        /// 获取所有物流信息(获取买家信息)
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <returns></returns>
        public List<Shipping> getAllLogisticsNo(string datetime1, string datetime2)
        {
            int num = getLogisticsNoCount(datetime1, datetime2);
            int pageSize = 100;
            int pageIndex = (num % pageSize) != 0 ? (num / pageSize) + 1 : num / pageSize;
            List<Shipping> list = new List<Shipping>();

            for (int i = 1; i <= pageIndex; i++)
            {
                list.AddRange(getLogisticsNo(datetime1, datetime2, i, pageSize));
            }

            return list;
        }




        /// <summary>
        /// 获取物流信息(获取买家信息)
        /// </summary>
        /// <param name="datetime1">开始时间</param>
        /// <param name="datetime2">结束时间</param>
        /// <param name="index">当前第几页</param>
        /// <param name="pagesize">每页读取的数据量</param>
        /// <returns></returns>
        public List<Shipping> getLogisticsNo(string datetime1, string datetime2, int index, int pagesize)
        {
            List<Shipping> list = new List<Shipping>();
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            LogisticsOrdersGetRequest req = new LogisticsOrdersGetRequest();
            req.Fields = "tid,out_sid,company_name";
            req.StartCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(datetime1);
            req.EndCreated = helpcommon.ParmPerportys.GetDateTimeNowParms(datetime2);
            req.PageNo = index;
            req.PageSize = pagesize;

            LogisticsOrdersGetResponse response = client.Execute(req, Sessionkey);

            list.AddRange(response.Shippings);

            return list;
        }


        #endregion


        #region 获取订单备注
        public Trade getRemarkMsg(string orderId)
        {
            List<Trade> list = new List<Trade>();
            //ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            //TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            //req.Fields = "seller_memo,seller_flag,tid";
            //req.Tid = helpcommon.ParmPerportys.GetLongParms(orderId);
            //TradeFullinfoGetResponse response = client.Execute(req, Sessionkey);
            //Trade td = response.Trade;

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            TradeGetRequest req = new TradeGetRequest();
            req.Fields = "seller_memo,tid";
            req.Tid = 1007800652310837L;
            TradeGetResponse response = client.Execute(req, Sessionkey);
            Trade td = response.Trade;

            return td;
        }

        #endregion

    }
}
