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
    /// api订单活动优惠表
    /// </summary>
    [Serializable]
    public class apiOrderDiscountModel
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
        /// dicountType
        /// </summary>		
        private int _dicounttype;
        /// <summary>
        /// 优惠类型
        /// </summary>
        public int dicountType
        {
            get { return _dicounttype; }
            set { _dicounttype = value; }
        }
        /// <summary>
        /// dicountAmount
        /// </summary>		
        private decimal _dicountamount;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal dicountAmount
        {
            get { return _dicountamount; }
            set { _dicountamount = value; }
        }
        /// <summary>
        /// remark
        /// </summary>		
        private string _remark;
        /// <summary>
        /// 优惠备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
