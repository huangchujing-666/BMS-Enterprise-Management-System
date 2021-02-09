/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       shoporderdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       shoporderdal
    * 创建时间：       2015-04-01 09:18:29
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class shoporderdal : dataoperating, ishoporder
    {

        Errorlogdal errordal = new Errorlogdal();
        /// <summary>
        /// 获取店铺订单数据
        /// </summary>
        /// <returns></returns>
        public int getDataCount(string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            int i = 0;
            //int i  = (from c in context.OrderDetails where c.OrderId == c.porder.OrderId select c).Count();
            var p = (from c in context.OrderDetails
                     join pd in context.productstock on c.OrderScode equals pd.Scode into g
                     from mm in g.DefaultIfEmpty()
                     orderby c.OrderSucessTime descending
                     select new
                     {
                         OrderId = c.OrderId,
                         OrderChildenId = c.OrderChildenId,
                         ProductId = c.ProductId,
                         DetailsName = c.DetailsName,
                         OrderScode = c.OrderScode,
                         OrderColor = c.OrderColor,
                         OrderImg = c.OrderImg,
                         DetailsSum = c.DetailsSum,
                         DetailsPrice = c.DetailsPrice,
                         OrderSendTime = c.OrderSendTime,
                         OrderSucessTime = c.OrderSucessTime,

                         ShopName = c.porder.ShopName,
                         OrderPrice = c.porder.OrderPrice,
                         OrderTime = c.porder.OrderTime,
                         OrderPayTime = c.porder.OrderPayTime,
                         OrderEditTime = c.porder.OrderEditTime,
                         //OrderSendTime,
                         //OrderSucessTime,
                         OrderNick = c.porder.OrderNick,
                         PayState = c.porder.PayState,
                         OrderState = c.porder.OrderState,
                         OrderState1 = c.porder.OrderState1,
                         CustomServerId = c.porder.CustomServerId,
                         UserId = c.porder.UserId,

                         Cat=mm.Cat,
                         Cat2=mm.Cat2

                     });
            if (!string.IsNullOrWhiteSpace(orderId))
            {
                p = p.Where(c => c.OrderId == orderId);
            }
            if (!string.IsNullOrWhiteSpace(scode))
            {
                p = p.Where(c => c.OrderScode == scode);
            }
            if (!string.IsNullOrWhiteSpace(brand))
            {
                p = p.Where(c => c.Cat == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                p = p.Where(c => c.Cat2 == type);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                p = p.Where(c => c.OrderState == int.Parse(status));
            }
            //if (!string.IsNullOrWhiteSpace(shopname))
            //{
            //    p = p.Where(c => c.OrderId == shopname);
            //}
            if (!string.IsNullOrWhiteSpace(servicecustom))
            {
                p = p.Where(c => c.CustomServerId == int.Parse(servicecustom));
            }
            if (!string.IsNullOrWhiteSpace(buynick))
            {
                p = p.Where(c => c.OrderNick == buynick);
            }
            //if (!string.IsNullOrWhiteSpace(price1)&&!string.IsNullOrWhiteSpace(price2)) //成本价
            //{
            //    p = p.Where(c => c.OrderPrice >= DateTime.Parse(price1) && c.OrderTime <= DateTime.Parse(price2));
            //}
            //else if (!string.IsNullOrWhiteSpace(price1))
            //{
            //    p = p.Where(c => c.OrderTime >= DateTime.Parse(price1));
            //}
            //else if (!string.IsNullOrWhiteSpace(price2))
            //{
            //    p = p.Where(c => c.OrderTime <= DateTime.Parse(price2));
            //}

            if (!string.IsNullOrWhiteSpace(pprice1) && !string.IsNullOrWhiteSpace(pprice2)) //价格
            {
                p = p.Where(c => c.DetailsPrice >= decimal.Parse(pprice1) && c.DetailsPrice <= decimal.Parse(pprice2));
            }
            else if (!string.IsNullOrWhiteSpace(pprice1))
            {
                p = p.Where(c => c.DetailsPrice >= decimal.Parse(pprice1));
            }
            else if (!string.IsNullOrWhiteSpace(price2))
            {
                p = p.Where(c => c.DetailsPrice <= decimal.Parse(pprice2));
            }

            if (!string.IsNullOrWhiteSpace(ordertime1) && !string.IsNullOrWhiteSpace(ordertime2))
            {
                p = p.Where(c => c.OrderTime >= DateTime.Parse(ordertime1) && c.OrderTime <= DateTime.Parse(ordertime2));
            }
            else if (!string.IsNullOrWhiteSpace(ordertime1))
            {
                p = p.Where(c => c.OrderTime >= DateTime.Parse(ordertime1));
            }
            else if (!string.IsNullOrWhiteSpace(ordertime2))
            {
                p = p.Where(c => c.OrderTime <= DateTime.Parse(ordertime2));
            }

            if (!string.IsNullOrWhiteSpace(paytime1) && !string.IsNullOrWhiteSpace(paytime2))
            {
                p = p.Where(c => c.OrderPayTime >= DateTime.Parse(paytime1) && c.OrderPayTime <= DateTime.Parse(paytime2));
            }
            else if (!string.IsNullOrWhiteSpace(paytime1))
            {
                p = p.Where(c => c.OrderPayTime >= DateTime.Parse(paytime1));
            }
            else if (!string.IsNullOrWhiteSpace(paytime2))
            {
                p = p.Where(c => c.OrderPayTime <= DateTime.Parse(paytime2));
            }

            if (!string.IsNullOrWhiteSpace(sendtime1) && !string.IsNullOrWhiteSpace(sendtime2))
            {
                p = p.Where(c => c.OrderSendTime >= DateTime.Parse(sendtime1) && c.OrderSendTime <= DateTime.Parse(sendtime2));
            }
            else if (!string.IsNullOrWhiteSpace(sendtime1))
            {
                p = p.Where(c => c.OrderSendTime >= DateTime.Parse(sendtime1));
            }
            else if (!string.IsNullOrWhiteSpace(sendtime2))
            {
                p = p.Where(c => c.OrderSendTime <= DateTime.Parse(sendtime2));
            }

            if (!string.IsNullOrWhiteSpace(sucesstime1) && !string.IsNullOrWhiteSpace(sucesstime2))
            {
                p = p.Where(c => c.OrderSucessTime >= DateTime.Parse(sucesstime1) && c.OrderSucessTime <= DateTime.Parse(sucesstime2));
            }
            else if (!string.IsNullOrWhiteSpace(sucesstime1))
            {
                p = p.Where(c => c.OrderSucessTime >= DateTime.Parse(sucesstime1));
            }
            else if (!string.IsNullOrWhiteSpace(sucesstime2))
            {
                p = p.Where(c => c.OrderSucessTime <= DateTime.Parse(sucesstime2));
            }
            i = p.Count();

            return i;
        }


        /// <summary>
        /// 获取店铺订单数据(分页)
        /// </summary>
        /// <returns></returns>
        public DataTable getData(int pageIndex, int pageSize, string orderId, string scode, string brand, string type, string status, string shopname, string servicecustom, string buynick, string price1, string price2, string pprice1, string pprice2, string ordertime1, string ordertime2, string paytime1, string paytime2, string sendtime1, string sendtime2, string sucesstime1, string sucesstime2)
        {
            DataTable dt = new DataTable();
            string mys = string.Empty;
            mys = string.IsNullOrWhiteSpace(mys) ? string.Empty : mys;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();

            var p = (from c in context.OrderDetails
                     join o in context.porder on c.OrderId equals o.OrderId
                     //join u in context.users on o.CustomServerId equals u.Id
                     join pd in context.productstock on c.OrderScode equals pd.Scode into g
                     from mm in g.DefaultIfEmpty()
                     join aso in
                         (from c1 in context.activeShopOrder join c2 in context.activeShop on c1.acId equals c2.acId select new { acScode = c1.asoScode, acName = c2.acName }) on mm.Scode equals aso.acScode into g1
                     from mm1 in g1.DefaultIfEmpty()
                     join os in context.orderComments on c.OrderId equals os.ocOrderId into g2
                     from mm2 in g2.DefaultIfEmpty()
                     join oe in context.OrderExpress on c.OrderId equals oe.OrderId into g3
                     from mm3 in g3.DefaultIfEmpty()
                     orderby c.OrderTime descending, c.OrderId ascending
                     select new
                     {
                         OrderId = o.OrderId,
                         OrderChildenId = c.OrderChildenId,
                         ShopName = o.ShopName,
                         OrderTime = o.OrderTime,
                         OrderPayTime = o.OrderPayTime,
                         OrderSendTime = c.OrderSendTime,
                         OrderSucessTime = c.OrderSucessTime,
                         OrderNick = o.OrderNick,
                         OrderState = o.OrderState,
                         //OrderState1 = o.OrderState1,
                         PayState = o.PayState,
                         CustomServerId = o.CustomServerId,
                         DetailsName = c.DetailsName,
                         OrderScode = c.OrderScode,
                         OrderImg = c.OrderImg,
                         DetailsSum = c.DetailsSum,
                         DetailsPrice = c.DetailsPrice,

                         
                         Cat = mm.Cat,
                         Cat2 = mm.Cat2,
                         Clolor = mm.Clolor,
                         Pricee = mm.Pricee,

                         acName = mm1.acName,

                         ocPostPrice = mm2.ocPostPrice,
                         ocOtherPrice = mm2.ocOtherPrice,
                         ocBanner = mm2.ocBanner,
                         ocRemark = mm2.ocRemark,
                         ocComment = mm2.ocComment,
                         //扣点，利润，出库源头

                         ExpressNo = mm3.ExpressNo,
                         ExpressName = mm3.ExpressName,
                         CustomName = mm3.CustomName,
                         CustomPhone = mm3.CustomPhone,
                         CustomCity1 = mm3.CustomCity1,
                         CustomCity2 = mm3.CustomCity2,
                         CustomCity3 = mm3.CustomCity3,
                         CustomAddress = mm3.CustomAddress
                     });


            if (!string.IsNullOrWhiteSpace(orderId))
            {
                p = p.Where(c => c.OrderId == orderId);
            }
            if (!string.IsNullOrWhiteSpace(scode))
            {
                p = p.Where(c => c.OrderScode == scode);
            }
            if (!string.IsNullOrWhiteSpace(brand))
            {
                p = p.Where(c => c.Cat == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                p = p.Where(c => c.Cat2 == type);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                p = p.Where(c => c.OrderState == int.Parse(status));
            }
            //if (!string.IsNullOrWhiteSpace(shopname))
            //{
            //    p = p.Where(c => c.OrderId == shopname);
            //}
            if (!string.IsNullOrWhiteSpace(servicecustom))
            {
                p = p.Where(c => c.CustomServerId == int.Parse(servicecustom));
            }
            if (!string.IsNullOrWhiteSpace(buynick))
            {
                p = p.Where(c => c.OrderNick == buynick);
            }
            //if (!string.IsNullOrWhiteSpace(price1)&&!string.IsNullOrWhiteSpace(price2)) //成本价
            //{
            //    p = p.Where(c => c.OrderPrice >= DateTime.Parse(price1) && c.OrderTime <= DateTime.Parse(price2));
            //}
            //else if (!string.IsNullOrWhiteSpace(price1))
            //{
            //    p = p.Where(c => c.OrderTime >= DateTime.Parse(price1));
            //}
            //else if (!string.IsNullOrWhiteSpace(price2))
            //{
            //    p = p.Where(c => c.OrderTime <= DateTime.Parse(price2));
            //}

            if (!string.IsNullOrWhiteSpace(pprice1) && !string.IsNullOrWhiteSpace(pprice2)) //价格
            {
                p = p.Where(c => c.DetailsPrice >= decimal.Parse(pprice1) && c.DetailsPrice <= decimal.Parse(pprice2));
            }
            else if (!string.IsNullOrWhiteSpace(pprice1))
            {
                p = p.Where(c => c.DetailsPrice >= decimal.Parse(pprice1));
            }
            else if (!string.IsNullOrWhiteSpace(price2))
            {
                p = p.Where(c => c.DetailsPrice <= decimal.Parse(pprice2));
            }

            if (!string.IsNullOrWhiteSpace(ordertime1) && !string.IsNullOrWhiteSpace(ordertime2))
            {
                p = p.Where(c => c.OrderTime >= DateTime.Parse(ordertime1) && c.OrderTime <= DateTime.Parse(ordertime2));
            }
            else if (!string.IsNullOrWhiteSpace(ordertime1))
            {
                p = p.Where(c => c.OrderTime >= DateTime.Parse(ordertime1));
            }
            else if (!string.IsNullOrWhiteSpace(ordertime2))
            {
                p = p.Where(c => c.OrderTime <= DateTime.Parse(ordertime2));
            }

            if (!string.IsNullOrWhiteSpace(paytime1) && !string.IsNullOrWhiteSpace(paytime2))
            {
                p = p.Where(c => c.OrderPayTime >= DateTime.Parse(paytime1) && c.OrderPayTime <= DateTime.Parse(paytime2));
            }
            else if (!string.IsNullOrWhiteSpace(paytime1))
            {
                p = p.Where(c => c.OrderPayTime >= DateTime.Parse(paytime1));
            }
            else if (!string.IsNullOrWhiteSpace(paytime2))
            {
                p = p.Where(c => c.OrderPayTime <= DateTime.Parse(paytime2));
            }

            if (!string.IsNullOrWhiteSpace(sendtime1) && !string.IsNullOrWhiteSpace(sendtime2))
            {
                p = p.Where(c => c.OrderSendTime >= DateTime.Parse(sendtime1) && c.OrderSendTime <= DateTime.Parse(sendtime2));
            }
            else if (!string.IsNullOrWhiteSpace(sendtime1))
            {
                p = p.Where(c => c.OrderSendTime >= DateTime.Parse(sendtime1));
            }
            else if (!string.IsNullOrWhiteSpace(sendtime2))
            {
                p = p.Where(c => c.OrderSendTime <= DateTime.Parse(sendtime2));
            }

            if (!string.IsNullOrWhiteSpace(sucesstime1) && !string.IsNullOrWhiteSpace(sucesstime2))
            {
                p = p.Where(c => c.OrderSucessTime >= DateTime.Parse(sucesstime1) && c.OrderSucessTime <= DateTime.Parse(sucesstime2));
            }
            else if (!string.IsNullOrWhiteSpace(sucesstime1))
            {
                p = p.Where(c => c.OrderSucessTime >= DateTime.Parse(sucesstime1));
            }
            else if (!string.IsNullOrWhiteSpace(sucesstime2))
            {
                p = p.Where(c => c.OrderSucessTime <= DateTime.Parse(sucesstime2));
            }

            p = p.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 添加店铺天猫订单数据
        /// </summary>
        /// <returns></returns>
        public string addTmallOrder()
        {
            string s = string.Empty;
            //DataTable dt = new DataTable();
            //model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            //try
            //{

            //    try
            //    {
            //        SqlConnection con = new SqlConnection();
            //        SqlTransaction transaction = con.BeginTransaction();
            //        SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.UseInternalTransaction, transaction);
            //        sbc.DestinationTableName = "porder";

            //        try
            //        {
            //            sbc.WriteToServer(dt);
            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            transaction.Rollback();
            //        }


            //    }
            //    catch (Exception ex) { }

            //    context.porder.InsertOnSubmit(mdPorder);
            //    context.SubmitChanges();
            //    s = "添加成功！";
            //}catch(Exception ex){
            //    s = "添加失败！";
            //}
            return s;
        }

        /// <summary>
        /// 修改店铺天猫订单数据(主订单)
        /// </summary>
        /// <returns></returns>
        public string updateTmallOrder(string orderId, decimal orderPrice, DateTime orderTime, DateTime orderPayTime, DateTime orderEditTime, DateTime orderSendTime, DateTime orderSucessTime, int payState, int orderState, int orderState1)
        {
            string s = string.Empty;
            try
            {

                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.porder p = (from c in context.porder where c.OrderId == orderId select c).FirstOrDefault();
                if (p != null)
                {
                    p.OrderTime = orderTime;
                    p.OrderPrice = orderPrice;
                    if (!string.IsNullOrWhiteSpace(orderTime.ToString()))
                        p.OrderTime = orderTime;
                    if (!string.IsNullOrWhiteSpace(orderPayTime.ToString()) && orderPayTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderPayTime = orderPayTime;
                    if (!string.IsNullOrWhiteSpace(orderEditTime.ToString()) && orderEditTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderEditTime = orderEditTime;
                    if (!string.IsNullOrWhiteSpace(orderSendTime.ToString()) && orderSendTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderSendTime = orderSendTime;
                    if (!string.IsNullOrWhiteSpace(orderSucessTime.ToString()) && orderSucessTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderSucessTime = orderSucessTime;
                    p.PayState = payState;
                    p.OrderState = orderState;
                    p.OrderState1 = orderState1;

                    context.SubmitChanges();
                    s = "更新成功！";
                }
            }
            catch (Exception ex)
            {
                s = "更新失败！";
            }
            return s;
        }


        /// <summary>
        /// 修改店铺天猫订单数据(子订单)
        /// </summary>
        /// <returns></returns>
        public string updateChildTmallOrder(string OrderChildenId, decimal DetailsPrice, DateTime orderSendTime, DateTime orderSucessTime)
        {
            string s = string.Empty;
            try
            {

                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.OrderDetails p = (from c in context.OrderDetails where c.OrderId == OrderChildenId select c).FirstOrDefault();
                if (p != null)
                {
                    p.DetailsPrice = DetailsPrice;
                    if (!string.IsNullOrWhiteSpace(orderSendTime.ToString()) && orderSendTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderSendTime = orderSendTime;
                    if (!string.IsNullOrWhiteSpace(orderSucessTime.ToString()) && orderSucessTime.ToString() != "0001-01-01 00:00:00")
                        p.OrderSucessTime = orderSucessTime;

                    context.SubmitChanges();
                    s = "更新成功！";
                }
            }
            catch (Exception ex)
            {
                s = "更新失败！";
            }
            return s;
        }


        /// <summary>
        /// 获取某时间段店铺订单数据(根据时间获取)
        /// </summary>
        /// <returns></returns>
        public DataTable getData(string date1, string date2)
        {
            DataTable dt = new DataTable();

            DateTime d1 = DateTime.Parse(date1 + " 00:00:00");
            DateTime d2 = DateTime.Parse(date2 + " 23:59:59");
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.porder orderby c.OrderTime where c.OrderTime >= d1 && c.OrderTime <= d2 select c;
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }


        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是主订单)
        /// </summary>
        /// <returns>返回时间</returns>
        public string getDataTime()
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.porder orderby c.OrderTime descending select c).FirstOrDefault();
            s = p == null ? "2015-01-09" : DateTime.Parse(p.OrderTime.ToString()).ToString("yyyy-MM-dd");
            return s;
        }


        /// <summary>
        /// 获取最晚时间的一条订单(这里指的是子订单)
        /// </summary>
        /// <returns>返回时间</returns>
        public string getChildDataTime()
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.OrderDetails orderby c.OrderTime descending select c).FirstOrDefault();
            s = p == null ? "2015-01-09" : DateTime.Parse(p.OrderTime.ToString()).ToString("yyyy-MM-dd");
            //s = "2015-04-12";
            return s;
        }


        /// <summary>
        /// 获取某时间段的所有子订单ID
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public string[] getDataChildOrderId(string date1, string date2)
        {
            DateTime d1 = DateTime.Parse(date1 + " 00:00:00");
            DateTime d2 = DateTime.Parse(date2 + " 23:59:59");
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string[] ss = (from c in context.OrderDetails where 
                               (c.OrderSucessTime >= d1 && c.OrderSucessTime <= d2) || 
                               (c.OrderEditTime >= d1 && c.OrderEditTime <= d2) || 
                               (c.OrderTime >= d1 && c.OrderTime <= d2) || 
                               (c.OrderSendTime >= d1 && c.OrderSendTime <= d2) || 
                               (c.OrderPayTime >= d1 && c.OrderPayTime <= d2) 
                           select c.OrderChildenId).ToArray();
            return ss;
        }


        /// <summary>
        /// 获取某时间段的所有物流订单ID
        /// </summary>
        /// <returns></returns>
        public string[] getExpressOrderId()
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string[] ss = (from c in context.OrderExpress select c.OrderId).ToArray();
            return ss;
        }


        /// <summary>
        /// 更新物流订单运单号
        /// </summary>
        /// <returns></returns>
        public string updateExpressNo(string sqlText)
        {
            string s = string.Empty;
            int i = Maticsoft.DBUtility.DbHelperSQL.ExecuteSql(sqlText);
            if (i > 0)
            {
                s = "更新成功";
            }
            else
            {
                s = "更新失败";
            }
            return s;
        }


        /// <summary>
        /// 获取订单备注，备注旗帜信息
        /// </summary>
        /// <returns></returns>
        public string[] getCommentOrderRemarkId()
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.orderComments select c.ocOrderId).ToArray();

            return p;
        }


        /// <summary>
        /// 获取某段时间主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public string[] getOrderId(string date1, string date2)
        {
            string s = string.Empty;
            DateTime d1 = DateTime.Parse(date1);
            DateTime d2 = DateTime.Parse(date2);
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.porder where 
                         (c.OrderSucessTime >= d1 && c.OrderSucessTime <= d2) || 
                         (c.OrderTime >= d1 && c.OrderTime <= d2) || 
                         (c.OrderEditTime >= d1 && c.OrderEditTime <= d2) || 
                         (c.OrderPayTime >= d1 && c.OrderPayTime <= d2) || 
                         (c.OrderSendTime >= d1 && c.OrderSendTime <= d2) 
                     select c.OrderId).ToArray();

            return p;
        }


        /// <summary>
        /// 获取某段时间某个店铺主订单
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        public string[] getShopOrderId(string date1, string date2, string shopName)
        {
            string s = string.Empty;
            DateTime d1 = DateTime.Parse(date1);
            DateTime d2 = DateTime.Parse(date2);
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p1 = from c in context.porder
                     where
                         (c.OrderSucessTime >= d1 && c.OrderSucessTime <= d2) ||
                         (c.OrderTime >= d1 && c.OrderTime <= d2) ||
                         (c.OrderEditTime >= d1 && c.OrderEditTime <= d2) ||
                         (c.OrderPayTime >= d1 && c.OrderPayTime <= d2) ||
                         (c.OrderSendTime >= d1 && c.OrderSendTime <= d2)
                     select c;
            var p = (from c in p1 where c.ShopName == shopName select c.OrderId).ToArray();


            return p;
        }


        /// <summary>
        /// 根据子订单ID获取此商品相关信息
        /// </summary>
        /// <returns></returns>
        public DataTable orderEdit(string orderchildenid)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.OrderDetails
                     join o in context.porder on c.OrderId equals o.OrderId
                     //join u in context.users on o.CustomServerId equals u.Id
                     join pd in context.productstock on c.OrderScode equals pd.Scode into g
                     from mm in g.DefaultIfEmpty()
                     join aso in
                         (from c1 in context.activeShopOrder join c2 in context.activeShop on c1.acId equals c2.acId select new { acScode = c1.asoScode, acName = c2.acName }) on mm.Scode equals aso.acScode into g1
                     from mm1 in g1.DefaultIfEmpty()
                     join os in context.orderComments on c.OrderId equals os.ocOrderId into g2
                     from mm2 in g2.DefaultIfEmpty()
                     join oe in context.OrderExpress on c.OrderId equals oe.OrderId into g3
                     from mm3 in g3.DefaultIfEmpty()
                     //join pt in context.product on c.OrderScode equals pt.Bcode into g4
                     //from mm4 in g4.DefaultIfEmpty()
                     join u in context.users on o.CustomServerId equals u.Id into g5
                     from mm5 in g5.DefaultIfEmpty()
                     orderby c.OrderSucessTime descending, c.OrderId ascending
                     where c.OrderChildenId == orderchildenid
                     select new
                     {
                         OrderId = o.OrderId, //订单编号
                         OrderChildenId = c.OrderChildenId,
                         ShopName = o.ShopName, //店铺名称
                         OrderTime = o.OrderTime,
                         OrderPayTime = o.OrderPayTime,
                         OrderSendTime = c.OrderSendTime,
                         OrderSucessTime = c.OrderSucessTime,
                         OrderNick = o.OrderNick,
                         OrderState = o.OrderState,
                         //OrderState1 = o.OrderState1,
                         PayState = o.PayState,
                         CustomServerId = o.CustomServerId,
                         DetailsName = c.DetailsName,
                         OrderScode = c.OrderScode,
                         OrderImg = c.OrderImg,
                         DetailsSum = c.DetailsSum,
                         DetailsPrice = c.DetailsPrice,

                         Scode=mm.Scode,
                         Cat = mm.Cat,
                         Cat2 = mm.Cat2,
                         Clolor = mm.Clolor,
                         Pricea = mm.Pricea,
                         Priceb = mm.Priceb,
                         Pricec = mm.Pricec,
                         Priced = mm.Priced,
                         Pricee = mm.Pricee,

                         acName = mm1.acName,

                         ocPostPrice = mm2.ocPostPrice,
                         ocOtherPrice = mm2.ocOtherPrice,
                         ocBanner = mm2.ocBanner,
                         ocRemark = mm2.ocRemark,
                         ocComment = mm2.ocComment,
                         //扣点，利润，出库源头

                         ExpressNo = mm3.ExpressNo,
                         ExpressName = mm3.ExpressName,
                         CustomName = mm3.CustomName,
                         CustomPhone = mm3.CustomPhone,
                         CustomCity1 = mm3.CustomCity1,
                         CustomCity2 = mm3.CustomCity2,
                         CustomCity3 = mm3.CustomCity3,
                         CustomAddress = mm3.CustomAddress,

                         CustomerName=mm5.userRealName
                     });
            dt = LinqToDataTable.LINQToDataTable(p);

            return dt;

        }
        
        /// <summary>
        /// 根据子订单ID修改此商品相关信息
        /// </summary>
        /// <returns></returns>
        public string orderUpdate(string orderchildenid, Dictionary<string, string> Dic)
        {
            //Dic["orderXSFS1"].ToString()销售方式------------暂未用到
            //Dic["orderCHCK1"].ToString()出库源头------------暂未用到
            DataTable dt = new DataTable();
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var c = context.OrderDetails.Where(a => a.OrderChildenId == orderchildenid);
                foreach (var a in c)
                {
                    
                    a.OrderScode = Dic["orderScode1"].ToString();//--货号
                }
                //var o = context.porder.Where(a => a.OrderId == Dic["orderid1"]);
                //foreach (var a in o)
                //{
                //    a.CustomServerId = Convert.ToInt32(Dic["orderSSKF1"].ToString());//--所属客服

                //}
                var e = (context.OrderExpress.Where(a => a.OrderId == Dic["orderid1"])).FirstOrDefault();
                e.ExpressName = Dic["CourierCompanies1"].ToString();//--快递公司
                e.ExpressNo = Dic["CourierNo1"].ToString();//--快递编号
                //context.OrderExpress.Attach(e);
                //if (Dic["CourierCompanies1"].ToString() != "")
                //{
                //    
                //}
                //if (Dic["CourierNo1"].ToString() != "")
                //{
                //    
                //}
                

                if (Dic["orderSSKF1"].ToString() != "-1")
                {
                    var o = (context.porder.Where(a => a.OrderId == Dic["orderid1"])).FirstOrDefault();
                    o.CustomServerId = Convert.ToInt32(Dic["orderSSKF1"].ToString());
                }
                //foreach (var a in e)
                //{
                //    a.ExpressName = Dic["CourierCompanies1"].ToString();//--快递公司
                //    a.ExpressNo = Dic["CourierNo1"].ToString();//--快递编号

                //}
                var oc = context.orderComments.Where(a => a.ocOrderId == Dic["orderid1"]);
                foreach (var a in oc)
                {
                    
                    a.ocOtherPrice = Convert.ToDecimal(Dic["orderOtherMoney1"].ToString());//--其他费用
                    a.ocRemark = Dic["orderRemark"].ToString();//--备注
                    a.ocPostPrice = Convert.ToDecimal(Dic["orderPostage1"].ToString());//--邮费

                }
                
                context.SubmitChanges();
                errordal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->shoporderdal->orderUpdate()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "通过子订单号修改信息->" + orderchildenid,
                    UserId =Convert.ToInt32(Dic["UserId"].ToString())
                });
                return "修改成功!";
            }
            catch(Exception ex)
            {
                errordal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->shoporderdal->orderUpdate()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = Convert.ToInt32(Dic["UserId"].ToString())
                });
                return "修改失败!";
            }
            
            
        }

    }
}
