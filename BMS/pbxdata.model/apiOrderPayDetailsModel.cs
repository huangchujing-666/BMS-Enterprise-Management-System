/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderModel
    * 机器名称：       OC-20140327WJCW
    * 命名空间：       pbxdata.model
    * 文 件 名：       apiorderModel
    * 创建时间：       2015-04-21 11:21:57
    * 作    者：       rwh
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    /// <summary>
    /// api订单支付详情表
    /// </summary>
    [Serializable]
    public class apiOrderPayDetailsModel
    {

        /// <summary>
        /// orderId
        /// </summary>		
        private string _orderid;
        /// <summary>
        /// 主订单编号
        /// </summary>
        public string orderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        /// <summary>
        /// payMentName
        /// </summary>		
        private string _paymentname;
        /// <summary>
        /// 支付方式
        /// </summary>
        public string payMentName
        {
            get { return _paymentname; }
            set { _paymentname = value; }
        }
        /// <summary>
        /// payPlatform
        /// </summary>		
        private string _payplatform;
        /// <summary>
        /// 支付平台
        /// </summary>
        public string payPlatform
        {
            get { return _payplatform; }
            set { _payplatform = value; }
        }
        /// <summary>
        /// payId
        /// </summary>		
        private string _payid;
        /// <summary>
        /// 支付内部流水号
        /// </summary>
        public string payId
        {
            get { return _payid; }
            set { _payid = value; }
        }
        /// <summary>
        /// payOuterId
        /// </summary>		
        private string _payouterid;
        /// <summary>
        /// 支付外部流
        /// </summary>
        public string payOuterId
        {
            get { return _payouterid; }
            set { _payouterid = value; }
        }
        /// <summary>
        /// payPrice
        /// </summary>		
        private string _payprice;
        /// <summary>
        ///  支付金额
        /// </summary>
        public string payPrice
        {
            get { return _payprice; }
            set { _payprice = value; }
        }
        /// <summary>
        /// payTime
        /// </summary>		
        private string _paytime;
        /// <summary>
        /// 支付时间
        /// </summary>
        public string payTime
        {
            get { return _paytime; }
            set { _paytime = value; }
        }
        /// <summary>
        /// sellerAccount
        /// </summary>		
        private string _selleraccount;
        /// <summary>
        /// 卖家支付
        /// </summary>
        public string sellerAccount
        {
            get { return _selleraccount; }
            set { _selleraccount = value; }
        }
        /// <summary>
        /// buyerAccount
        /// </summary>		
        private string _buyeraccount;
        /// <summary>
        /// 买家支付
        /// </summary>
        public string buyerAccount
        {
            get { return _buyeraccount; }
            set { _buyeraccount = value; }
        }

    }
}
