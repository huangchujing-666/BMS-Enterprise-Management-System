/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       sourceorderdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       sourceorderdal
    * 创建时间：       2015-05-04 14:23:07
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class sourceorderdal : dataoperating, isourceorder
    {
        /// <summary>
        /// 根据订单编号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderSendData(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.apiSendOrder where c.orderId == orderId select c;

            DataTable dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 根据订单编号、货号获取（供应商取消的订单，重新分配）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderSendData(string orderId,string scode)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.apiSendOrder where c.orderId == orderId && c.newScode == scode select c;

            DataTable dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 获取源头订单
        /// </summary>
        /// <returns>返回table集合</returns>
        public DataTable getData(string orderId)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.apiSendOrder
                    join o in context.productsource on c.sendSource equals o.SourceCode into g
                    from s in g.DefaultIfEmpty()
                    where c.orderId == orderId
                    select
                        new
                        {
                            createTime = c.createTime,
                            def1 = c.def1,
                            def2 = c.def2,
                            def3 = c.def3,
                            def4 = c.def4,
                            def5 = c.def5,
                            detailsOrderId = c.detailsOrderId,
                            editTime = c.editTime,
                            newColor = c.newColor,
                            newImg = c.newImg,
                            newOrderId = c.newOrderId,
                            newSaleCount = c.newSaleCount,
                            newScode = c.newScode,
                            newSize = c.newSize,
                            newStatus = c.newStatus,
                            orderId = c.orderId,
                            showStatus = c.showStatus,
                            sendSource = s.sourceName
                        };
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }

        /// <summary>
        /// 获取源头订单总数量
        /// </summary>
        /// <returns></returns>
        public int getDataCount()
        {

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            //int i = (from c in context.apiSendOrder select c).Count();
            int i = context.apiSendOrder.Count();
            return i;
        }

        #region 拆单订单单个操作
        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        public string updateShowOrderState(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = (from c in context.apiSendOrder where c.orderId == orderId select c).DefaultIfEmpty();
                foreach (var item in p)
                {
                    item.showStatus = 1;
                }
                //context.apiSendOrder.Attach(p);
                context.SubmitChanges();

                s = "开放成功";
            }
            catch { s = "开放失败"; }
            return s;


        }


        /// <summary>
        /// 订单状态更改为待发货状态(订单当前状态：0为待确认，1为有货，2为缺货，3为发货，4为取消订单，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string updateSendOrderState(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = (from c in context.apiSendOrder where c.orderId == orderId select c).FirstOrDefault();  //供应商订单表
                p.newStatus = 3;
                var p1 = (from c in context.apiOrderDetails where c.orderId == orderId select c).FirstOrDefault();  //子订单表
                p1.detailsStatus = 3;
                var p2 = (from c in context.apiOrder where c.orderId == orderId select c).FirstOrDefault();   //主订单表
                p2.orderStatus = 3;
                context.SubmitChanges();

                s = "更改成功";
            }
            catch { s = "更改失败"; }
            return s;
        }


        /// <summary>
        /// 取消订单(订单当前状态：0为待确认，1为有货，2为缺货，3为发货，4为取消订单，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string cancelOrder(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = (from c in context.apiSendOrder where c.orderId == orderId select c).FirstOrDefault();  //供应商订单表
                if (p!=null)
                {
                    p.newStatus = 12;
                } 
                var p1 = (from c in context.apiOrderDetails where c.orderId == orderId select c).FirstOrDefault();  //子订单表
                if (p1 != null)
                { p1.detailsStatus = 12; }
                var p2 = (from c in context.apiOrder where c.orderId == orderId select c).FirstOrDefault();   //主订单表
                if (p2 != null)
                {
                    p2.orderStatus = 12;
                }
                context.SubmitChanges();
                s = "取消成功";
            }
            catch (Exception ex) { s = "取消失败(" + ex.Message + ")"; }
            return s;
        }

        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="o">实体连接</param>
        /// <returns></returns>
        public string cancelOrder(string orderId, object o)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = (model.pbxdatasourceDataContext)o;
            try
            {
                var p = (from c in context.apiSendOrder where c.orderId == orderId select c).FirstOrDefault();  //供应商订单表
                if (p != null)
                {
                    p.newStatus = 12;
                }
                var p1 = (from c in context.apiOrderDetails where c.orderId == orderId select c).FirstOrDefault();  //子订单表
                if (p1 != null)
                { p1.detailsStatus = 12; }
                var p2 = (from c in context.apiOrder where c.orderId == orderId select c).FirstOrDefault();   //主订单表
                if (p2 != null)
                {
                    p2.orderStatus = 12;
                }
                context.SubmitChanges();
                s = "取消成功";
            }
            catch (Exception ex) { s = "取消失败(" + ex.Message + ")"; }
            return s;
        }

        #endregion


        #region 主订单批量操作
        /// <summary>
        /// 修改发送订单状态(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        public string updateParentShowOrderState(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = from c in context.apiSendOrder where c.orderId == orderId select c;
                foreach (var q in p)
                {
                    if (q.showStatus >= 1)
                    {
                        return "订单已开发,请勿重复操作";
                    }
                    q.showStatus = 1;
                }

                //p.showStatus = 1;
                //context.apiSendOrder.Attach(p);
                context.SubmitChanges();

                s = "开放成功";
            }
            catch { s = "开放失败"; }
            return s;
        }


        /// <summary>
        /// 订单状态更改为待发货状态(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string updateParentSendOrderState(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = from c in context.apiSendOrder where c.orderId == orderId select c;  //供应商订单表
                foreach (var q in p)
                {
                    if (q.newStatus >= 3)
                    {
                        return "订单已确认发货,请勿重复操作";
                    }
                    q.newStatus = 3;
                }

                var p1 = from c in context.apiOrderDetails where c.orderId == orderId select c;  //子订单表
                foreach (var q1 in p1)
                {
                    if (q1.detailsStatus >= 3)
                    {
                        return "订单已确认发货,请勿重复操作";
                    }
                    q1.detailsStatus = 3;
                }

                var p2 = from c in context.apiOrder where c.orderId == orderId select c;   //主订单表
                foreach (var q2 in p2)
                {
                    if (q2.orderStatus >= 3)
                    {
                        return "订单已确认发货,请勿重复操作";
                    }
                    q2.orderStatus = 3;
                }
                context.SubmitChanges();

                s = "更改成功";
            }
            catch { s = "更改失败"; }
            return s;
        }

        /// <summary>
        /// 取消订单(1待确认，2确认，3待发货，4发货，5收货(交易成功)，11退货，12取消)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string cancelParentOrder(string orderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var p = from c in context.apiSendOrder where c.orderId == orderId select c;  //供应商订单表
                foreach (var q in p)
                {
                    if (q.newStatus >= 12)
                    {
                        return "取消已被订单,请勿重复操作";
                    }
                    q.newStatus = 12;
                }

                var p1 = from c in context.apiOrderDetails where c.orderId == orderId select c;  //子订单表
                foreach (var q1 in p1)
                {
                    if (q1.detailsStatus >= 12)
                    {
                        return "取消已被订单,请勿重复操作";
                    }
                    q1.detailsStatus = 12;
                }

                var p2 = from c in context.apiOrder where c.orderId == orderId select c;   //主订单表
                foreach (var q2 in p2)
                {
                    if (q2.orderStatus >= 12)
                    {
                        return "取消已被订单,请勿重复操作";
                    }
                    q2.orderStatus = 12;
                }
                context.SubmitChanges();

                s = "取消成功";
            }
            catch { s = "取消失败"; }
            return s;
        }

        #endregion



        /// <summary>
        /// 获取拆单订单的主订单ID
        /// </summary>
        /// <returns></returns>
        public DataTable getSourceParentOrderId(int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.apiSendOrder
                     group c by new
                     {
                         orderId = c.orderId,
                         showstatus = c.showStatus,
                         newStatus = c.newStatus
                     } into g
                     select new
                     {
                         orderId = g.Key.orderId,
                         showstatus = g.Key.showstatus,
                         newStatus = g.Key.newStatus
                     }).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            dt = LinqToDataTable.LINQToDataTable(p);

            return dt;
        }
        /// <summary>
        /// 获取拆单订单的主订单ID
        /// </summary>
        /// <returns></returns>
        public DataTable getSourceParentOrderId(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();

            var q = from c in context.apiSendOrder
                    join d in context.apiOrderPayDetails on c.orderId equals d.orderId
                    group c by new { orderId = c.orderId, createTime = c.createTime, editTime = c.editTime, showStatus = c.showStatus, sendSource = c.sendSource,payId = d.payId }
                        into g
                        select new
                        {
                            orderId = g.Key.orderId,
                            newStatus = g.Average(c => c.newStatus),
                            createTime = g.Key.createTime,
                            editTime = g.Key.editTime,
                            showStatus = g.Key.showStatus,
                            sendSource = g.Key.sendSource,
                            payId = g.Key.payId
                        };
            if (Dic["orderId"] != "") //订单ID
            {
                q = q.Where(a => a.orderId == Dic["orderId"]);
            }
            if (Dic["newStatus"] != "")  //订单状态
            {
                q = q.Where(a => a.newStatus == Convert.ToInt32(Dic["newStatus"]));
            }
            #region 时间条件查询
            if (Dic["createTime"] != "" && Dic["editTime"] != "") //创建时间
            {
                q = q.Where(a => a.createTime >= DateTime.Parse(Dic["createTime"]) && a.editTime <= DateTime.Parse(Dic["editTime"]));
            }
            else if (Dic["editTime"] != "")//结束时间
            {
                q = q.Where(a => a.editTime <= DateTime.Parse(Dic["editTime"]));
            }
            else if (Dic["createTime"] != "")  //创建时间
            {
                q = q.Where(a => a.createTime >= DateTime.Parse(Dic["createTime"]));
            }
            #endregion

            if (Dic["showStatus"] != "")//开放状态
            {
                q = q.Where(a => a.showStatus == Convert.ToInt32(Dic["showStatus"]));
            }
            if (Dic["sendSource"] != "")//供应商
            {
                q = q.Where(a => a.sendSource == Dic["sendSource"]);
            }
            if (Dic["SJstatus"] != "") //商检报关状态(0失败，1成功，2未上传)
            {
                var sjstatus = Dic["SJstatus"];
                if (sjstatus == "2")
                {
                    List<string> sjlist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.SJstatus == "0" || a.SJstatus == "1");
                    foreach (var i in sj)
                    {
                        sjlist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => !sjlist.Contains(a.payId));
                }
                else
                {
                    List<string> sjlist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.SJstatus == sjstatus);
                    foreach (var i in sj)
                    {
                        sjlist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => sjlist.Contains(a.payId));
                }
            }
            if (Dic["HGstatus"] != "") //海关报关状态(0失败，1成功，2未上传)
            {
                var hgstatus = Dic["HGstatus"];
                if (hgstatus == "2")
                {
                    List<string> hglist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.HGstatus == "0" || a.HGstatus == "1");
                    foreach (var i in sj)
                    {
                        hglist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => !hglist.Contains(a.payId));
                }
                else
                {
                    List<string> hglist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.HGstatus == hgstatus);
                    foreach (var i in sj)
                    {
                        hglist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => hglist.Contains(a.payId));
                }
            }
            if (Dic["BBCstatus"] != "") //联邦报关状态(0失败，1成功，2未上传)
            {
                var bbcstatus = Dic["BBCstatus"];
                if (bbcstatus == "2")
                {
                    List<string> bbclist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.HGstatus == "0" || a.HGstatus == "1");
                    foreach (var i in sj)
                    {
                        bbclist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => !bbclist.Contains(a.payId));
                }
                else
                {
                    List<string> bbclist = new List<string>();
                    var sj = context.orderCustomsResult.Where(a => a.BBCstatus == bbcstatus);
                    foreach (var i in sj)
                    {
                        bbclist.Add(i.SJOrgOrderChildId);
                    }
                    q = q.Where(a => bbclist.Contains(a.payId));
                }

            }
            if (Dic["Paystatus"] != "") //支付报关状态(fail失败，success成功)
            {
                var payStatus = string.Empty;
                if (Dic["Paystatus"]=="1")    //支付成功
                {
                    payStatus = "SUCCESS";
                }
                else if (Dic["Paystatus"]=="0")   //支付失败
                {
                    payStatus = "FAIL";
                }

                if (string.IsNullOrWhiteSpace(payStatus))
                {
                    List<string> paylist = new List<string>();
                    var sj = context.payCustomsResult.Where(a => a.result_code == "SUCCESS" || a.result_code == "FAIL");
                    foreach (var i in sj)
                    {
                        paylist.Add(i.OrderChildId);
                    }
                    q = q.Where(a => !paylist.Contains(a.payId));
                }
                else
                {
                    List<string> paylist = new List<string>();
                    var sj = context.payCustomsResult.Where(a => a.result_code == payStatus);
                    foreach (var i in sj)
                    {
                        paylist.Add(i.OrderChildId);
                    }
                    q = q.Where(a => paylist.Contains(a.payId));
                }                
            }

            counts = q.ToList().Count;
            var p = q.OrderByDescending(c => c.createTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 根据订单编号获取买家信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public model.apiOrder selectBuyer(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.apiOrder where c.orderId == orderId select c).SingleOrDefault();

            return p;
        }

        /// <summary>
        /// 根据子订单更新发货源头
        /// </summary>
        public string UpdatesendSource(List<string> detailsOrderIdlist, List<string> sendSourcelist)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                for (int i = 0; i < detailsOrderIdlist.Count; i++)
                {
                    var q = context.apiSendOrder.Where(a => a.detailsOrderId == detailsOrderIdlist[i]);
                    foreach (var item in q)
                    {
                        item.sendSource = sendSourcelist[i];
                    }
                    context.SubmitChanges();
                }
                return "修改成功！";
            }
            catch (Exception ex)
            {
                return "修改失败！";
            }


        }


        //def1 nvarchar(20), --商检报关状态(0失败，1成功，2未上传)
        //def2 nvarchar(20), --海关报关状态(0失败，1成功，2未上传)
        //def3 nvarchar(20), --联邦报关状态(0失败，1成功，2未上传)
        /// <summary>
        /// 根据子订单更新商检订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateSJStates(string orderChildId, string status)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.apiSendOrder where c.detailsOrderId == orderChildId select c).SingleOrDefault();
            p.def1 = status;
            try
            {
                context.SubmitChanges();
                s = "更新成功";
            }
            catch
            {
                s = "更新失败";
            }
            return s;
        }


        /// <summary>
        /// 根据子订单更新海关订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateHGStates(string orderChildId, string status)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.apiSendOrder where c.detailsOrderId == orderChildId select c).SingleOrDefault();
            p.def2 = status;
            try
            {
                context.SubmitChanges();
                s = "更新成功";
            }
            catch
            {
                s = "更新失败";
            }
            return s;
        }


        /// <summary>
        /// 根据子订单更新联邦订单报备
        /// </summary>
        /// <param name="orderChildId">子订单ID</param>
        /// <param name="status">更改状态</param>
        /// <returns></returns>
        public string UpdateBBCStates(string orderChildId, string status)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.apiSendOrder where c.detailsOrderId == orderChildId select c).SingleOrDefault();
            p.def3 = status;
            try
            {
                context.SubmitChanges();
                s = "更新成功";
            }
            catch
            {
                s = "更新失败";
            }
            return s;
        }

    }
}
