/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       paycustomsresultdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       paycustomsresultdal
    * 创建时间：       2015-08-08 14:54:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class paycustomsresultdal : dataoperating,ipaycustomsresult
    {

        /// <summary>
        /// 插入订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        public string addPayCustoms(model.payCustomsResult payResult)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                context.payCustomsResult.InsertOnSubmit(payResult);
                context.SubmitChanges();
                s = "添加成功";
            }
            catch (Exception ex)
            {
                s = "添加失败(" + ex.Message + ")";
            }
            return s;
        }

        /// <summary>
        /// 更新订单支付报关
        /// </summary>
        /// <param name="payResult"></param>
        /// <returns></returns>
        public string updatePayCustoms(model.payCustomsResult payResult)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.payCustomsResult where c.OrderChildId == payResult.OrderChildId select c).SingleOrDefault();
            p.OrderChildId = payResult.OrderChildId;
            p.error = payResult.error; //错误信息
            p.result_code = payResult.result_code;//响应码(处理结果响应码。SUCCESS：成功,FAIL：失败)
            p.trade_no = payResult.trade_no;//支付宝交易号(该交易在支付宝系统中的交易流水号。最长 64 位。)
            p.alipay_declare_no = payResult.alipay_declare_no;//支付宝报关流水号(支付宝报关流水号)

            p.isSuccess = "";//请求是否成功请求是否成功。请求成功不代表业务处理成功。(T代表成功,F代表失败)
            try
            {
                context.SubmitChanges();
                s = "更新支付成功";
            }
            catch (Exception ex)
            {
                s = "更新支付失败(" + ex.Message + ")";
            }
            return s;
        }


        /// <summary>
        /// 是否存在订单支付报关()
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string existPayCustoms(string orderId)
        {
            bool flag = false;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.payCustomsResult where c.OrderChildId == orderId select c).SingleOrDefault();
            if (p != null)
            {
                if (p.OrderChildId != null)
                {
                    flag = true;
                }
            }
            string s = flag == true ? "已存在" : "不存在";
            return s;
        }


        /// <summary>
        /// 根据订单ID获取支付单的报备状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getPayStatus(string orderId)
        {
            string payStatus = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            payStatus = (from c in context.payCustomsResult
                    where
                        c.OrderChildId == orderId
                    select c.result_code).SingleOrDefault();

            return payStatus;
        }
    }
}
