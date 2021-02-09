
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
    /// api订单详情表
    /// </summary>
    [Serializable]
    public class apiOrderDetailsModel
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
            /// detailsOrderId
            /// </summary>		
            private string _detailsorderid;
            /// <summary>
            /// 子订单编号
            /// </summary>
            public string detailsOrderId
            {
                get { return _detailsorderid; }
                set { _detailsorderid = value; }
            }
            /// <summary>
            /// detailsScode
            /// </summary>		
            private string _detailsscode;
            /// <summary>
            /// 商品编号
            /// </summary>
            public string detailsScode
            {
                get { return _detailsscode; }
                set { _detailsscode = value; }
            }
            /// <summary>
            /// detailsColor
            /// </summary>		
            private string _detailscolor;
            /// <summary>
            /// 颜色
            /// </summary>
            public string detailsColor
            {
                get { return _detailscolor; }
                set { _detailscolor = value; }
            }
            /// <summary>
            /// detailsImg
            /// </summary>		
            private string _detailsimg;
            /// <summary>
            /// 图片
            /// </summary>
            public string detailsImg
            {
                get { return _detailsimg; }
                set { _detailsimg = value; }
            }
            /// <summary>
            /// detailsItemPrice
            /// </summary>		
            private decimal _detailsitemprice;
            /// <summary>
            /// 商品售价
            /// </summary>
            public decimal detailsItemPrice
            {
                get { return _detailsitemprice; }
                set { _detailsitemprice = value; }
            }
            /// <summary>
            /// detailsTaxPrice
            /// </summary>		
            private decimal _detailstaxprice;
            /// <summary>
            /// 税费金额
            /// </summary>
            public decimal detailsTaxPrice
            {
                get { return _detailstaxprice; }
                set { _detailstaxprice = value; }
            }
            /// <summary>
            /// detailsSaleCount
            /// </summary>		
            private int _detailssalecount;
            /// <summary>
            /// 销售数量
            /// </summary>
            public int detailsSaleCount
            {
                get { return _detailssalecount; }
                set { _detailssalecount = value; }
            }
            /// <summary>
            /// detailsDeliveryPrice
            /// </summary>		
            private string _detailsdeliveryprice;
            /// <summary>
            /// 快递费用
            /// </summary>
            public string detailsDeliveryPrice
            {
                get { return _detailsdeliveryprice; }
                set { _detailsdeliveryprice = value; }
            }
            /// <summary>
            /// detailsStatus
            /// </summary>		
            private int _detailsstatus;
            /// <summary>
            ///  订单状态
            /// </summary>
            public int detailsStatus
            {
                get { return _detailsstatus; }
                set { _detailsstatus = value; }
            }
            /// <summary>
            /// detailsTime
            /// </summary>		
            private DateTime _detailstime;
           /// <summary>
            /// 下单时间
           /// </summary>
            public DateTime detailsTime
            {
                get { return _detailstime; }
                set { _detailstime = value; }
            }
            /// <summary>
            /// detailsPayTime
            /// </summary>		
            private DateTime _detailspaytime;
            /// <summary>
            /// 付款时间
            /// </summary>
            public DateTime detailsPayTime
            {
                get { return _detailspaytime; }
                set { _detailspaytime = value; }
            }
            /// <summary>
            /// detailsEditTime
            /// </summary>		
            private DateTime _detailsedittime;
            /// <summary>
            ///   编辑时间
            /// </summary>
            public DateTime detailsEditTime
            {
                get { return _detailsedittime; }
                set { _detailsedittime = value; }
            }
            /// <summary>
            /// detailsSendTime
            /// </summary>		
            private DateTime _detailssendtime;
            /// <summary>
            /// 发送时间
            /// </summary>
            public DateTime detailsSendTime
            {
                get { return _detailssendtime; }
                set { _detailssendtime = value; }
            }
            /// <summary>
            /// detailsSucessTime
            /// </summary>		
            private DateTime _detailssucesstime;
            /// <summary>
            /// 成交时间
            /// </summary>
            public DateTime detailsSucessTime
            {
                get { return _detailssucesstime; }
                set { _detailssucesstime = value; }
            }
            /// <summary>
            /// def1
            /// </summary>		
            private string _def1;
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
