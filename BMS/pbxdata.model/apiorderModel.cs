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
    /// api订单表
    /// </summary>
    [Serializable]
    public class apiOrderModel
    {
        private apiOrderPayDetailsModel[] _apiOrderPayDetails;
        /// <summary>
        /// api订单支付详情表
        /// </summary>
        public apiOrderPayDetailsModel[] apiOrderPayDetails
        {
            get { return _apiOrderPayDetails; }
            set { _apiOrderPayDetails = value; }
        }

        private apiOrderDiscountModel[] _apiOrderDiscount;
        /// <summary>
        /// api订单详情表
        /// </summary>
        public apiOrderDiscountModel[] apiOrderDiscount
        {
            get { return _apiOrderDiscount; }
            set{_apiOrderDiscount=value;}
        }
        /// <summary>
        /// 子订单
        /// </summary>
        private apiOrderDetailsModel[] _apiOrderDetails;
        /// <summary>
        /// 子订单
        /// </summary>
        public apiOrderDetailsModel[] apiOrderDetails
        {
            get { return _apiOrderDetails; }
            set { _apiOrderDetails = value; }
        }
        /// <summary>
        /// orderId
        /// </summary>		
        private string _orderid;
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        /// <summary>
        /// realName
        /// </summary>		
        private string _realname;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName
        {
            get { return _realname; }
            set { _realname = value; }
        }
        /// <summary>
        /// provinceId
        /// </summary>		
        private string _provinceid;
        /// <summary>
        /// 省ID

        /// </summary>
        public string provinceId
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        /// <summary>
        /// cityId
        /// </summary>		
        private string _cityid;
        /// <summary>
        /// 城市ID

        /// </summary>
        public string cityId
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// district
        /// </summary>		
        private string _district;
        /// <summary>
        /// 区域ID
        /// </summary>
        public string district
        {
            get { return _district; }
            set { _district = value; }
        }
        /// <summary>
        /// buyNameAddress
        /// </summary>		
        private string _buynameaddress;
        /// <summary>
        /// 地址
        /// </summary>
        public string buyNameAddress
        {
            get { return _buynameaddress; }
            set { _buynameaddress = value; }
        }
        /// <summary>
        /// postcode
        /// </summary>		
        private string _postcode;
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }
        /// <summary>
        /// phone
        /// </summary>		
        private string _phone;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// orderMsg
        /// </summary>		
        private string _ordermsg;
        /// <summary>
        /// 订单备注
        /// </summary>
        public string orderMsg
        {
            get { return _ordermsg; }
            set { _ordermsg = value; }
        }
        /// <summary>
        /// orderStatus
        /// </summary>		
        private string _orderstatus;
        /// <summary>
        /// 订单状态
        /// </summary>
        public string orderStatus
        {
            get { return _orderstatus; }
            set { _orderstatus = value; }
        }
        /// <summary>
        /// itemPrice
        /// </summary>		
        private string _itemprice;
        /// <summary>
        /// 商品总价
        /// </summary>
        public string itemPrice
        {
            get { return _itemprice; }
            set { _itemprice = value; }
        }
        /// <summary>
        /// deliveryPrice
        /// </summary>		
        private string _deliveryprice;
        /// <summary>
        /// 快递费用
        /// </summary>
        public string deliveryPrice
        {
            get { return _deliveryprice; }
            set { _deliveryprice = value; }
        }
        /// <summary>
        /// favorablePrice
        /// </summary>		
        private string _favorableprice;
        /// <summary>
        /// 优惠费用
        /// </summary>
        public string favorablePrice
        {
            get { return _favorableprice; }
            set { _favorableprice = value; }
        }
        /// <summary>
        /// taxPrice
        /// </summary>		
        private string _taxprice;
        /// <summary>
        /// 税费
        /// </summary>
        public string taxPrice
        {
            get { return _taxprice; }
            set { _taxprice = value; }
        }
        /// <summary>
        /// orderPrice
        /// </summary>		
        private string _orderprice;
        /// <summary>
        /// 订单金额（商品总价+快递费用-优惠费用+税费）
        /// </summary>
        public string orderPrice
        {
            get { return _orderprice; }
            set { _orderprice = value; }
        }
        /// <summary>
        /// paidPrice
        /// </summary>		
        private string _paidprice;
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string paidPrice
        {
            get { return _paidprice; }
            set { _paidprice = value; }
        }
        /// <summary>
        /// isPay
        /// </summary>		
        private string _ispay;
        /// <summary>
        /// 未支付，1：已支付
        /// </summary>
        public string isPay
        {
            get { return _ispay; }
            set { _ispay = value; }
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
        /// payOuterId
        /// </summary>		
        private string _payouterid;
        /// <summary>
        /// 支付流水号
        /// </summary>
        public string payOuterId
        {
            get { return _payouterid; }
            set { _payouterid = value; }
        }
        /// <summary>
        /// createTime
        /// </summary>		
        private string _createtime;
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string createTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// invoiceType
        /// </summary>		
        private string _invoicetype;
        /// <summary>
        /// 发票类型
        /// </summary>
        public string invoiceType
        {
            get { return _invoicetype; }
            set { _invoicetype = value; }
        }
        /// <summary>
        /// invoiceTitle
        /// </summary>		
        private string _invoicetitle;
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoiceTitle
        {
            get { return _invoicetitle; }
            set { _invoicetitle = value; }
        }
        //private 
        /// <summary>
        /// def1
        /// </summary>		
        private string _def1;
        /// <summary>
        /// 
        /// </summary>
        public string def1
        {
            get { return _def1; }
            set { _def1 = value; }
        }
        /// <summary>
        /// def2
        /// </summary>		
        private string _def2;
        public string def2
        {
            get { return _def2; }
            set { _def2 = value; }
        }
        /// <summary>
        /// def3
        /// </summary>		
        private string _def3;
        public string def3
        {
            get { return _def3; }
            set { _def3 = value; }
        }
        /// <summary>
        /// def4
        /// </summary>		
        private string _def4;
        public string def4
        {
            get { return _def4; }
            set { _def4 = value; }
        }
        /// <summary>
        /// def5
        /// </summary>		
        private string _def5;
        public string def5
        {
            get { return _def5; }
            set { _def5 = value; }
        }

    }
}

