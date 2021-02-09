/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       apiorderdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       apiorderdal
    * 创建时间：       2015-04-20 13:38:05
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class apiorderdal : dataoperating, iapiorder
    {
        /// <summary>
        /// 返回订单详情信息(根据主订单编号查询子订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="hg">海关</param>
        /// <returns></returns>
        public List<model.apiOrderDetails> getOrderDetailsMsg(string orderId, string hg)
        {
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            List<model.apiOrderDetails> list = context.apiOrderDetails.Where(c => c.orderId == orderId).ToList();
            return list;
        }


        /// <summary>
        /// 返回订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public model.apiOrder getOrderMsg(string orderId)
        {
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            model.apiOrder MdApiOrder = context.apiOrder.SingleOrDefault(c => c.orderId == orderId);
            return MdApiOrder;
        }


        /// <summary>
        /// 返回(批量)订单信息(查询主订单表当前订单编号的列（所有数据）)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<model.apiOrder> getOrderMsg(string[] orderIds)
        {
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            List<model.apiOrder> list = context.apiOrder.Where(c => orderIds.Contains(c.orderId)).ToList();
            return list;
        }


        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderMsg(string orderId, string scode)
        {
            IDataParameter[] ipara = new IDataParameter[] { 
                new SqlParameter("orderId",SqlDbType.NVarChar,20),
                new SqlParameter("scode",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = orderId;
            ipara[1].Value = scode;

            DataTable dt = Select(ipara, "selectOrderDetails");
            return dt;
        }
        /// <summary>
        /// 返回订单信息(查询已拆单的数据表)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderMsg(string orderId, string scode,object o)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = (model.pbxdatasourceDataContext)o;
            var p = from c in context.apiOrderDetails where c.orderId==orderId && c.detailsScode == scode select c;
            dt = LinqToDataTable.LINQToDataTable(p);

            return dt;
        }



        /// <summary>
        /// 返回所有API订单
        /// </summary>
        /// <returns></returns>
        public DataTable getOrderMsg(int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = (from c in context.apiOrder select c).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }

        /// <summary>
        /// 返回所有APP订单
        /// </summary>
        /// <returns></returns>
        public DataTable getOrderMsg(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int count)
        {
            DataTable dt = new DataTable();
            
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = (from c in context.apiOrder
                    join o in context.apiOrderPayDetails on c.orderId equals o.orderId into g
                    from s in g.DefaultIfEmpty()
                    select new
                    {
                        orderId = c.orderId,
                        realName = c.realName,
                        provinceId = c.provinceId,
                        cityId = c.cityId,
                        district = c.district,
                        buyNameAddress = c.buyNameAddress,
                        postcode = c.postcode,
                        phone = c.phone,
                        orderMsg = c.orderMsg,
                        orderStatus = c.orderStatus,
                        itemPrice = c.itemPrice,
                        deliveryPrice = c.deliveryPrice,
                        favorablePrice = c.favorablePrice,
                        taxPrice = c.taxPrice,
                        orderPrice = c.orderPrice,
                        paidPrice = c.paidPrice,
                        isPay = c.isPay,
                        createTime = c.createTime,
                        invoiceType = c.invoiceType,
                        invoiceTitle = c.invoiceTitle,
                        def1 = c.def1,
                        def2 = c.def2,
                        payMentName = s.payMentName,
                        payPlatform = s.payPlatform,
                        payId = s.payId,
                        payOuterId = s.payOuterId,
                        payPrice = s.payPrice,
                        payTime = s.payTime,
                        sellerAccount = s.sellerAccount,
                        buyerAccount = s.buyerAccount
                    }).DefaultIfEmpty();

            var dicValue = Dic["orderId"];
            if (Dic["orderId"] != "")
            {
                p = p.Where(a => a.orderId == Dic["orderId"]);
            }
            if (Dic["orderScode"] != "")
            {
                var ScodeOrderId = (from c in context.apiOrderDetails where c.detailsScode == Dic["orderScode"] select c.orderId).ToList();
                p = p.Where(a => ScodeOrderId.Contains(a.orderId));
            }
            if (Dic["orderColor"] != "")
            {
                var ColorOrderId = (from c in context.apiOrderDetails where c.detailsColor == Dic["orderColor"] select c.orderId).ToList();
                p = p.Where(a => ColorOrderId.Contains(a.orderId));
            }
            if (Dic["orderBuyName"] != "")
            {
                p = p.Where(a => a.realName == Dic["orderBuyName"]);
            }
            if (Dic["orderAddress"] != "")
            {
                p = p.Where(a => a.buyNameAddress.Contains(Dic["orderAddress"]));
            }
            if (Dic["orderMobile"] != "")
            {
                p = p.Where(a => a.phone == Dic["orderMobile"]);
            }
            if (Dic["orderStatus"] != "")
            {
                p = p.Where(a => a.orderStatus == Convert.ToInt32(Dic["orderStatus"]));
            }
            if (Dic["orderPayStatus"] != "")
            {
                int orderPayStatus = Convert.ToInt32(Dic["orderPayStatus"]);
                p = p.Where(a => a.isPay == orderPayStatus);
            }
            count = p.Count();
            p = p.OrderByDescending(a => a.createTime);
            p = p.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }

        /// <summary>
        /// 根据主订单获取子订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable getOrderDetailsMsg(string orderId)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = (from c in context.apiOrderDetails where c.orderId == orderId select c).ToList();
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 订单是否已拆单
        /// </summary>
        /// <returns></returns>
        public string getOrderSplit(string detailsOrderId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            model.apiOrderDetails p = (from c in context.apiOrderDetails where c.detailsSplit == decimal.One && c.detailsOrderId == detailsOrderId select c).FirstOrDefault();
            if (p != null)
            {
                s = "订单已分配,请勿重复分配";
            }
            return s;
        }


        /// <summary>
        /// 获取API订单总条数
        /// </summary>
        /// <returns></returns>
        public int getDataCount()
        {
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            int i = context.apiOrderDetails.Count();

            return i;
        }
        /// <summary>
        /// 添加备注
        /// </summary>
        public string UpdateApiOrder(Dictionary<string, string> Dic)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                model.apiOrderRemark tt = new model.apiOrderRemark()
                {
                    OrderId = Dic["OrderId"],
                    Edittime = Dic["Edittime"],
                    Remark = Dic["Remark"],
                    UserId = Convert.ToInt32(Dic["UserId"]),
                };
                context.apiOrderRemark.InsertOnSubmit(tt);
                context.SubmitChanges();
                return "添加成功！";
            }
            catch (Exception ex)
            {
                return "添加失败！";
            }
        }
        /// <summary>
        /// 删除备注
        /// </summary>
        public string DeleteRemark(string Id)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.apiOrderRemark.Where(a => a.Id == Convert.ToInt32(Id));
                context.apiOrderRemark.DeleteOnSubmit(q.First());
                context.SubmitChanges();
                return "删除成功！";
            }
            catch (Exception ex)
            {
                return "删除失败！";
            }

        }
        /// <summary>
        /// 修改备注
        /// </summary>
        public string EditRemark(Dictionary<string, string> Dic)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.apiOrderRemark.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                foreach (var item in q)
                {
                    item.Remark = Dic["Remark"];
                    item.Edittime = Dic["Edittime"];
                }
                context.SubmitChanges();
                return "修改成功！";
            }
            catch (Exception ex)
            {
                return "修改失败！";
            }
           
        }

    }
}
