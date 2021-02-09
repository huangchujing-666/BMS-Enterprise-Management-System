using Com.Alipay;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace pbxdata.web.Controllers
{
    public class ForexRefundController : BaseController
    {
        //
        // GET: /ForexRefund/
        /// <summary>
        /// 支付宝退款
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public string ForexRefund(string OrderId)
        {

            string result = string.Empty;
            DataTable dt = new DataTable();
            string sql = @"select * from apiOrderPayDetails where orderId='" + OrderId + "' ";
            dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Refund no
                string out_return_no = dt.Rows[i]["payOuterId"].ToString();
                //required  外部流水号

                //Old Partner transaction ID
                string out_trade_no = dt.Rows[i]["payId"].ToString();
                //required  内部流水号 短的 在支付国际出现的号码

                //Refund sum
                string return_amount = Convert.ToDecimal(dt.Rows[i]["payPrice"].ToString()).ToString("f2");
                //required

                //Currency
                string currency = "HKD";//要与创建时候的币种一致  本地存的是人民币 需要换算 不然报错
                //Refer to abbreviation of currencies
                //Refund Transaction time
                string gmt_return = DateTime.Now.ToString("yyyyMMddHHmmss");
                //YYYYMMDDHHMMSS Beijing Time
                //Reason for refundament
                string reason = "required";
                //required


                ////////////////////////////////////////////////////////////////////////////////////////////////

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "forex_refund");
                sParaTemp.Add("out_return_no", out_return_no);
                sParaTemp.Add("out_trade_no", out_trade_no);
                sParaTemp.Add("return_amount", return_amount);
                sParaTemp.Add("currency", currency);
                sParaTemp.Add("gmt_return", gmt_return);
                sParaTemp.Add("reason", reason);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);

                //请在这里加上商户的业务逻辑程序代码

                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(sHtmlText);
                    string strXmlResponse = xmlDoc.SelectSingleNode("/alipay").InnerText;
                    //FREPEATED_REFUNDMENT_REQUEST----重复的退款请求
                    //REPEATED_REFUND MENT_REQUEST_TO _ONE_TRADE----一笔交易多次退款
                    //PURCHASE_TRADE_NOT_EXIST----交易不存在
                    //RETURN_AMOUNT_EXCEED----退款金额超限
                    //ILLEGAL_ARGUMENT----参数不正确
                    //CURRENCY_NOT_SAME----币种不一致（与创建交易时的币种不一致）
                    //SYSTEM_EXCEPTIO N----系统异常
                    //REFUNDMENT_VALID_DATE_EXCEED----退款有效期超过退款周期
                    switch (strXmlResponse)
                    {
                        case "T": result = "退款成功!"; break;
                        case "FREPEATED_REFUNDMENT_REQUEST": result = "重复的退款请求!"; break;
                        case "REPEATED_REFUND MENT_REQUEST_TO _ONE_TRADE": result = "一笔交易多次退款!"; break;
                        case "PURCHASE_TRADE_NOT_EXIST": result = "交易不存在!"; break;
                        case "RETURN_AMOUNT_EXCEED": result = "退款金额超限!"; break;
                        case "ILLEGAL_ARGUMENT": result = "参数不正确!"; break;
                        case "CURRENCY_NOT_SAME": result = "币种不一致!"; break;
                        case "SYSTEM_EXCEPTION": result = "系统异常!"; break;
                        case "REFUNDMENT_VALID_DATE_EXCEED": result = "退款有效期超过退款周期!"; break;
                        default: result = "退款失败!"; break;
                    }
                }
                catch (Exception exp)
                {
                    result = sHtmlText;
                }
            }

            return result;
        }
        /// <summary>
        /// 支付宝退款 传入订单号以及金额
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="return_amount"></param>
        /// <returns></returns>
        public string ForexRefund1(string OrderId, string return_amount)
        {

            string result = string.Empty;
            DataTable dt = new DataTable();
            string sql = @"select * from apiOrderPayDetails where orderId='" + OrderId + "' ";
            dt = DbHelperSQL.Query(sql).Tables[0];

            //Refund no
            string out_return_no = dt.Rows[0]["payOuterId"].ToString();
            //required  长的

            //Old Partner transaction ID
            string out_trade_no = dt.Rows[0]["payId"].ToString();
            //required  短的

            //required

            //Currency
            string currency = "HKD";
            //Refer to abbreviation of currencies
            //Refund Transaction time
            string gmt_return = DateTime.Now.ToString("yyyyMMddHHmmss");
            //YYYYMMDDHHMMSS Beijing Time
            //Reason for refundament
            string reason = "";
            //required


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "forex_refund");
            sParaTemp.Add("out_return_no", out_return_no);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("return_amount", return_amount);
            sParaTemp.Add("currency", currency);
            sParaTemp.Add("gmt_return", gmt_return);
            sParaTemp.Add("reason", reason);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp);

            //请在这里加上商户的业务逻辑程序代码

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                string strXmlResponse = xmlDoc.SelectSingleNode("/alipay").InnerText;
                //FREPEATED_REFUNDMENT_REQUEST----重复的退款请求
                //REPEATED_REFUND MENT_REQUEST_TO _ONE_TRADE----一笔交易多次退款
                //PURCHASE_TRADE_NOT_EXIST----交易不存在
                //RETURN_AMOUNT_EXCEED----退款金额超限
                //ILLEGAL_ARGUMENT----参数不正确
                //CURRENCY_NOT_S AME----币种不一致（与创建交易时的币种不一致）
                //SYSTEM_EXCEPTIO N----系统异常
                //REFUNDMENT_VALID_DATE_EXCEED----退款有效期超过退款周期
                switch (strXmlResponse)
                {
                    case "T": result = "退款成功!"; break;
                    case "FREPEATED_REFUNDMENT_REQUEST": result = "重复的退款请求!"; break;
                    case "REPEATED_REFUND MENT_REQUEST_TO _ONE_TRADE": result = "一笔交易多次退款!"; break;
                    case "PURCHASE_TRADE_NOT_EXIST": result = "交易不存在!"; break;
                    case "RETURN_AMOUNT_EXCEED": result = "退款金额超限!"; break;
                    case "ILLEGAL_ARGUMENT": result = "参数不正确!"; break;
                    case "CURRENCY_NOT_SAME": result = "币种不一致!"; break;
                    case "SYSTEM_EXCEPTION": result = "系统异常!"; break;
                    case "REFUNDMENT_VALID_DATE_EXCEED": result = "退款有效期超过退款周期!"; break;
                    default: result = "退款失败!"; break;
                }
            }
            catch (Exception exp)
            {
                result = sHtmlText;
            }


            return result;
        }

        /// <summary>
        /// 取消退款 通过订单号
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public string RefundCancel(string OrderId)
        {
            string result = string.Empty;
            DataTable dt = new DataTable();
            string sql = @"select * from apiOrderPayDetails where orderId='" + OrderId + "' ";
            dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //业务参数赋值；
                string gateway = "https://mapi.alipay.com/gateway.do?";	//'支付接口
                string service = "forex_cancel_refund";
                string partner = Config.Partner;		//partner		合作伙伴ID			保留字段
                string sign_type = "MD5";
                string out_trade_no = dt.Rows[i]["payOuterId"].ToString(); ;   //--payId  内部流水号
                string out_return_no = dt.Rows[i]["payId"].ToString(); ;  //--payOuterId    外部流水号    
                string return_amount = dt.Rows[i]["payPrice"].ToString(); ;  //--payPrice    退款金额
                string currency = "HKD";
                string key = Config.Key;
                string _input_charset = Config.Input_charset.ToLower();
                AliPay ap = new AliPay();
                string aliay_url = ap.CreatUrl(
                    gateway,
                    service,
                    partner,
                    sign_type,
                    out_trade_no,
                    out_return_no,
                    return_amount,
                    currency,
                    key,
                    _input_charset
                    );

                //这种方式是把xml解析再显示的
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(aliay_url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
                System.IO.Stream rssStream = myResponse.GetResponseStream();
                System.Xml.XmlDocument rssDoc = new System.Xml.XmlDocument();
                rssDoc.Load(rssStream);

                string Response = rssDoc.SelectSingleNode("/alipay").InnerText;
                switch (Response)
                {
                    case "T": result = "退款成功!"; break;
                    case "SYSTEM_EXCEPTION": result = "支付宝系统异常!"; break;
                    case "REFUND_RECORD_NOT_EXIST": result = "退款申请流水不存在,或此退款流水已撤销!"; break;
                    case "ILLEGAL_ARGUMENT": result = "参数不正确!"; break;
                    case "REFUND_INFO_NOT_SAME": result = "退款信息不一致(包括外部交易号/退款币种/退款金额)!"; break;
                    case "NOT_ALLOW_CANCEL_REFUND": result = "不允许撤销退款!"; break;
                    default: result = "退款失败!"; break;
                }
            }
            return result;
        }
        /// <summary>
        /// 添加退款操作记录
        /// </summary>
        /// <param name="mm"></param>
        public void OperaRecord(model.OperationRecord mm)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            mm.OperaTime = DateTime.Now;
            context.OperationRecord.InsertOnSubmit(mm);
            context.SubmitChanges();
        }

    }
    #region alipay类文件请不要随意更改

    /// <summary>
    /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
    /// </summary>
    public class AliPay
    {

        public static string GetMD5(string s, string _input_charset)
        {

            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string[] BubbleSort(string[] r)
        {
            /// <summary>
            /// 冒泡排序法
            /// </summary>

            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }

        public string CreatUrl(
           string gateway,
         string service,
         string partner,
        string sign_type,
         string out_trade_no,
        string out_return_no,
        string return_amount,
        string currency,
         string key,
         string _input_charset

            )
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner,
                "out_trade_no=" + out_trade_no,  
                "out_return_no=" + out_return_no, 
                "return_amount=" + return_amount, 
                "currency=" + currency,
                "_input_charset="+_input_charset          

                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }

            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {

                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);


            //返回支付Url；
            return parameter.ToString();

        }



    }
    #endregion
}
