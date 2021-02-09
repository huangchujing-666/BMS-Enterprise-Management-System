using Maticsoft.DBUtility;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.IO;
using Aliyun.OpenServices.OpenStorageService;

namespace pbxdata.web.Controllers
{
    public class RetProductController : BaseController
    {
        //
        // GET: /RtnProduct/
        const string accessId = "X9foTnzzxHCk6gK7";
        const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
        const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
        string bucketName = "best-bms";
        bll.RetProductbll Retbll = new bll.RetProductbll();
        aliyun.ControlAliyun CAliyun = new aliyun.ControlAliyun();

        public ActionResult TradeInfo()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }

        /// <summary>
        /// 获取查询条件权限
        /// </summary>
        /// <returns></returns>
        public string GetSearchlistTrade()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            //string[] ss = { "OrderId", "SystemId", "Heidui", "ExBalance", "SellPrice", "Express", "ExpressNo", "Price", "OrderTime", "CustomerId", "Shop", "Contactperson", "Telephone", "Phone", "Weixin", "QQNo", "CusAddress" };
            s += "<ul>";
            foreach (string i in ss)
            {
                if (i == "OrderId")
                {
                    s += "<li>订单编号:<input type='text' id='txtSearchOrderId' /></li>";
                }
                //else if (i == "SystemId")
                //{
                //    s += "<li>系统单号:<input type='text' id='txtSearchSystemId' /></li>";
                //}
                //else if (i == "SellPrice")
                //{
                //    s += "<li>成交金额:<input type='text' id='txtSearchSellPrice' /></li>";
                //}
                else if (i == "SaleStates")
                {
                    s += "<li>交易状态:<select id='txtSearchSaleStates' >";
                    s += "<option value=''>请选择</option>";
                    s += "<option value='1'>已付款</option><option value='2'>未付款</option>";
                    s += "<option value='3'>交易成功</option><option value='4'>交易关闭</option></select></li>";
                }
                else if (i == "Evaluate")
                {
                    s += "<li>评价状态:<input type='text' id='txtSearchEvaluate' /></li>";
                }
                else if (i == "ServiceId")
                {
                    s += "<li>客服:<select id='txtSearchServiceId' >" + GetSerivcelist() + "</select></li>";
                }
                else if (i == "OrderTime")
                {
                    s += "<li>下单日期<input type='date' id='txtSearchOrderTime' />-<input type='date' id='txtSearchOrderTime1' /></li>";
                }
                else if (i == "CustomerId")
                {
                    s += "<li>客户ID:<input type='text' id='txtSearchCustomerId' /></li>";
                }
                else if (i == "Shop")
                {
                    s += "<li>店铺:<input type='text' id='txtSearchShop' /></li>";
                }
                else if (i == "Payment")
                {
                    s += "<li>支付平台:<input type='text' id='txtSearchPayment' /></li>";
                }
                else if (i == "Account")
                {
                    s += "<li>支付账号:<input type='text' id='txtSearchAccount' /></li>";
                }
            }

            s += "<li><input type='button' onclick='OnSearch()' value='  查   询  '  /></li>";
            s += "<li><a href='#' onclick='AddClick()' >添    加</a></li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";

            return s;

        }

        /// <summary>
        /// 查询交易信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="lists"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string SearchRtnPro(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                //Dic.Add("SystemId", SearchInfo[1]);
                Dic.Add("SellPrice", SearchInfo[1]);
                Dic.Add("SaleStates", SearchInfo[2]);
                Dic.Add("Evaluate", SearchInfo[3]);
                Dic.Add("ServiceId", SearchInfo[4]);
                Dic.Add("OrderTime", SearchInfo[5]);
                Dic.Add("OrderTime1", SearchInfo[6]);
                Dic.Add("CustomerId", SearchInfo[7]);
                Dic.Add("Shop", SearchInfo[8]);
                Dic.Add("Payment", SearchInfo[9]);
                Dic.Add("Account", SearchInfo[10]);
                string counts;
                dt = Retbll.GetDate(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabRtnPro(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 交易信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string GetTabRtnPro(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "Shop", "SaleStates", "SellPrice", "OrderTime", "CustomerId", "Contactperson", "Telephone", "Phone", "Weixin", "QQNo", "Provinces", "City", "CusAddress", "Payment", "Account", "ServiceId", "ServiceRemark", "Evaluate" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            //string[] ss = { "OrderId", "Heidui", "ExBalance", "SellPrice", "Express", "ExpressNo", "Price", "OrderTime", "CustomerId", "Shop", "Contactperson", "Telephone", "Phone", "Weixin", "QQNo", "CusAddress" };
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "SellPrice")
                        s.Append("成交总金额");
                    if (ssName[z] == "SaleStates")
                        s.Append("交易状态");
                    if (ssName[z] == "Evaluate")
                        s.Append("评价状态");
                    if (ssName[z] == "ServiceId")
                        s.Append("接待客服");
                    if (ssName[z] == "ServiceRemark")
                        s.Append("客服备注");
                    if (ssName[z] == "OrderTime")
                        s.Append("下单日期");
                    if (ssName[z] == "CustomerId")
                        s.Append("客户ID");
                    if (ssName[z] == "Shop")
                        s.Append("店铺/平台");
                    if (ssName[z] == "Contactperson")
                        s.Append("联系人");
                    if (ssName[z] == "Telephone")
                        s.Append("电话");
                    if (ssName[z] == "Phone")
                        s.Append("手机号");
                    if (ssName[z] == "Weixin")
                        s.Append("微信");
                    if (ssName[z] == "QQNo")
                        s.Append("QQ号码");
                    if (ssName[z] == "Provinces")
                        s.Append("省");
                    if (ssName[z] == "City")
                        s.Append("市");
                    if (ssName[z] == "CusAddress")
                        s.Append("收货地址");
                    if (ssName[z] == "Payment")
                        s.Append("支付平台");
                    if (ssName[z] == "Account")
                        s.Append("支付账号");
                    s.Append("</th>");
                }
            }

            s.Append("<th>操作</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j].Contains("Time"))
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "" ? "" : Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        //1.已付款 2.未付款 3.交易关闭 4.交易成功(需要判断)
                        else if (ssName[j] == "SaleStates")
                        {
                            string item = dt.Rows[i][ssName[j]].ToString();
                            string SaleStates = item == "1" ? "已付款" : item == "2" ? "未付款" : GetSalesStatus(dt.Rows[i]["OrderId"].ToString());

                            s.Append(SaleStates);
                        }
                        else if (ssName[j] == "ServiceId")
                        {
                            s.Append(dt.Rows[i]["ServiceName"].ToString());
                        }
                        else
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                        }
                        s.Append("</td>");
                    }
                }
                s.Append("<td>");
                s.Append("<a href='#' onclick='LookProInfo(\"" + dt.Rows[i]["OrderId"].ToString() + "\")'>查看</a>");



                #region 操作 退款、发货、换货、取消退款、取消退货  1.未发货 2.已发货 3.申请退货退款  4.退货退款成功  5.申请退款  6.退款成功  7.申请换货 8.换货成功 9.交易成功
                if (dt.Rows[i]["SaleStates"].ToString() == "1")
                {
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<a href='#' onclick='EditTrade(\"" + dt.Rows[i]["OrderId"].ToString() + "\")'>编辑</a>");
                    }
                    if (dt.Rows[i]["Shop"].ToString().ToLower() != "app")
                    {
                        s.Append("<a href='#' onclick='AddClick1(\"" + dt.Rows[i]["OrderId"].ToString() + "\")'>添加商品</a>");
                    }
                }
                #endregion

                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    //s.Append("<td><a href='#' onclick='DeleteType(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a></td>");
                    s.Append("<a href='#' onclick='DeleteTrade(\"" + dt.Rows[i]["OrderId"].ToString() + "\")'>删除</a>");
                }
                #endregion
                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }

        /// <summary>
        /// 显示交易状态
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public string GetSalesStatus(string OrderId)
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ProductInfo where c.OrderId == OrderId select c.Def1;
            List<string> list = new List<string>();
            list = q.ToList();
            if (list.Contains("1") || list.Contains("2") || list.Contains("3") || list.Contains("5"))
            {
                s = "交易中";
            }
            else if (list.Contains("7"))
            {
                s = "交易成功";
            }
            else
            {
                s = "交易关闭";
            }
            return s;
        }
        /// <summary>
        /// 添加交易信息
        /// </summary>
        /// <returns></returns>
        public string AddTradeInfo()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", list.Split(',')[0]);
            //Dic.Add("SystemId", list.Split(',')[1]);
            Dic.Add("SaleStates", list.Split(',')[1]);
            Dic.Add("Evaluate", list.Split(',')[2]);
            Dic.Add("ServiceId", list.Split(',')[3]);
            Dic.Add("ServiceRemark", list.Split(',')[4]);
            Dic.Add("OrderTime", list.Split(',')[5]);
            Dic.Add("CustomerId", list.Split(',')[6]);
            Dic.Add("Shop", list.Split(',')[7]);
            Dic.Add("Contactperson", list.Split(',')[8]);
            Dic.Add("Telephone", list.Split(',')[9]);
            Dic.Add("Phone", list.Split(',')[10]);
            Dic.Add("Weixin", list.Split(',')[11]);
            Dic.Add("QQNo", list.Split(',')[12]);
            Dic.Add("Provinces", list.Split(',')[13]);
            Dic.Add("City", list.Split(',')[14]);
            Dic.Add("CusAddress", list.Split(',')[15]);
            Dic.Add("Payment", list.Split(',')[16]);
            Dic.Add("Account", list.Split(',')[17]);
            return Retbll.AddTradeInfo(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 获取编辑交易信息
        /// </summary>
        /// <returns></returns>
        public string GetEditInfo()
        {
            StringBuilder s = new StringBuilder();
            string OrderId = Request.Form["OrderId"].ToString();
            DataTable dt = Retbll.GetDate(OrderId);
            string item = dt.Rows[0]["SaleStates"].ToString();
            string SaleStates = item == "1" ? "已付款" : item == "2" ? "未付款" : GetSalesStatus(dt.Rows[0]["OrderId"].ToString());
            s.Append("<ul>");
            if (dt.Rows[0]["Shop"].ToString() == "app")//--通过订单类型判断编辑权限
            {
                s.Append("<li>订单编号:<input type='text' id='txtEditOrderId' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' /></li>");
                //s.Append("<li>系统单号:<input type='text' id='txtEditSystemId' value='" + dt.Rows[0]["SystemId"] + "'  /></li>");
                s.Append("<li>成交总金额:<input type='text' id='txtEditSellPrice' value='" + dt.Rows[0]["SellPrice"] + "' disabled='disabled' /></li>");
                s.Append("<li>交易状态:<input type='text' id='txtEditSaleStates' value='" + SaleStates + "' disabled='diasbled' /></li>");
                s.Append("<li>下单日期:<input type='date' id='txtEditOrderTime' value='" + Convert.ToDateTime(dt.Rows[0]["OrderTime"].ToString()).ToString("yyyy-MM-dd") + "' disabled='disabled' /></li>");
                s.Append("<li>客户ID:<input type='text' id='txtEditCustomerId' value='" + dt.Rows[0]["CustomerId"] + "'  /></li>");
                s.Append("<li>店铺/平台:<input type='text' id='txtEditShop' value='" + dt.Rows[0]["Shop"] + "' disabled='disabled' /></li>");
                s.Append("<li>联系人:<input type='text' id='txtEditContactperson' value='" + dt.Rows[0]["Contactperson"] + "' disabled='disabled' /></li>");
                s.Append("<li>电话:<input type='text' id='txtEditTelephone' value='" + dt.Rows[0]["Telephone"] + "' disabled='disabled'  /></li>");
                s.Append("<li>手机号:<input type='text' id='txtEditPhone' value='" + dt.Rows[0]["Phone"] + "' disabled='disabled'  /></li>");
                s.Append("<li>微信:<input type='text' id='txtEditWeixin' value='" + dt.Rows[0]["Weixin"] + "' /></li>");
                s.Append("<li>QQ:<input type='text' id='txtEditQQNo' value='" + dt.Rows[0]["QQNo"] + "' /></li>");
                s.Append("<li>省:<input type='text' id='txtEditProvinces' value='" + dt.Rows[0]["Provinces"] + "' disabled='disabled'  /></li>");
                s.Append("<li>市:<input type='text' id='txtEditCity' value='" + dt.Rows[0]["City"] + "' disabled='disabled'  /></li>");
                s.Append("<li>收货地址:<input type='text' id='txtEditCusAddress' value='" + dt.Rows[0]["CusAddress"] + "' disabled='disabled'  /></li>");
                s.Append("<li>支付平台:<input type='text' id='txtEditPayment' value='" + dt.Rows[0]["Payment"] + "' disabled='disabled'  /></li>");
                s.Append("<li>支付账号:<input type='text' id='txtEditAccount' value='" + dt.Rows[0]["Account"] + "' disabled='disabled'  /></li>");
                s.Append("<li>客服:<input type='text' id='txtEditServiceId' value='" + dt.Rows[0]["ServiceName"] + "'  disabled='disabled' /></li>");
                s.Append("<li>评价状态:<input type='text' id='txtEditEvaluate' value='" + dt.Rows[0]["Evaluate"] + "' disabled='disabled'  /></li>");
                s.Append("<li class='liRemark'>备注:<textarea id='txtEditServiceRemark'>" + dt.Rows[0]["ServiceRemark"] + "</textarea></li>");
            }
            else
            {
                s.Append("<li>订单编号:<input type='text' id='txtEditOrderId' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' /></li>");
                //s.Append("<li>系统单号:<input type='text' id='txtEditSystemId' value='" + dt.Rows[0]["SystemId"] + "'  /></li>");
                s.Append("<li>成交总金额:<input type='text' id='txtEditSellPrice' value='" + dt.Rows[0]["SellPrice"] + "' disabled='disabled' /></li>");
                s.Append("<li>交易状态:<input type='text' id='txtEditSaleStates' value='" + SaleStates + "' disabled='diasbled' /></li>");
                s.Append("<li>下单日期:<input type='date' id='txtEditOrderTime' value='" + Convert.ToDateTime(dt.Rows[0]["OrderTime"].ToString()).ToString("yyyy-MM-dd") + "'  /></li>");
                s.Append("<li>客户ID:<input type='text' id='txtEditCustomerId' value='" + dt.Rows[0]["CustomerId"] + "'  /></li>");
                s.Append("<li>店铺/平台:<input type='text' id='txtEditShop' value='" + dt.Rows[0]["Shop"] + "'  /></li>");
                s.Append("<li>联系人:<input type='text' id='txtEditContactperson' value='" + dt.Rows[0]["Contactperson"] + "'  /></li>");
                s.Append("<li>电话:<input type='text' id='txtEditTelephone' value='" + dt.Rows[0]["Telephone"] + "'  /></li>");
                s.Append("<li>手机号:<input type='text' id='txtEditPhone' value='" + dt.Rows[0]["Phone"] + "'  /></li>");
                s.Append("<li>微信:<input type='text' id='txtEditWeixin' value='" + dt.Rows[0]["Weixin"] + "'  /></li>");
                s.Append("<li>QQ:<input type='text' id='txtEditQQNo' value='" + dt.Rows[0]["QQNo"] + "'  /></li>");
                s.Append("<li>省:<input type='text' id='txtEditProvinces' value='" + dt.Rows[0]["Provinces"] + "'  /></li>");
                s.Append("<li>市:<input type='text' id='txtEditCity' value='" + dt.Rows[0]["City"] + "'  /></li>");
                s.Append("<li>收货地址:<input type='text' id='txtEditCusAddress' value='" + dt.Rows[0]["CusAddress"] + "'  /></li>");
                s.Append("<li>支付平台:<input type='text' id='txtEditPayment' value='" + dt.Rows[0]["Payment"] + "'  /></li>");
                s.Append("<li>支付账号:<input type='text' id='txtEditAccount' value='" + dt.Rows[0]["Account"] + "'  /></li>");
                s.Append("<li>客服:<input type='text' id='txtEditServiceId' value='" + dt.Rows[0]["ServiceName"] + "'  disabled='disabled'  /></li>");
                s.Append("<li>评价状态:<input type='text' id='txtEditEvaluate' value='" + dt.Rows[0]["Evaluate"] + "'  /></li>");
                s.Append("<li  class='liRemark'>备注:<textarea id='txtEditServiceRemark' >" + dt.Rows[0]["ServiceRemark"] + "</textarea></li>");
            }
            s.Append("</ul>");
            s.Append("<div class='clearfix'></div><div style='margin-top:10px;'><input type='button' value=' 保   存 ' onclick='SaveTradeEdit()' /><input type='button' class='btnClose' value=' 关   闭 ' onclick='Close()'  /></div>");
            return s.ToString();
        }

        /// <summary>
        /// 保存编辑交易信息
        /// </summary>
        /// <returns></returns>
        public string SaveTradeEdit()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", list.Split(',')[0]);
            //Dic.Add("SystemId", list.Split(',')[1]);
            Dic.Add("SaleStates", list.Split(',')[1]);
            Dic.Add("Evaluate", list.Split(',')[2]);
            Dic.Add("ServiceId", list.Split(',')[3]);
            Dic.Add("ServiceRemark", list.Split(',')[4]);
            Dic.Add("OrderTime", list.Split(',')[5]);
            Dic.Add("CustomerId", list.Split(',')[6]);
            Dic.Add("Shop", list.Split(',')[7]);
            Dic.Add("Contactperson", list.Split(',')[8]);
            Dic.Add("Telephone", list.Split(',')[9]);
            Dic.Add("Phone", list.Split(',')[10]);
            Dic.Add("Weixin", list.Split(',')[11]);
            Dic.Add("QQNo", list.Split(',')[12]);
            Dic.Add("Provinces", list.Split(',')[13]);
            Dic.Add("City", list.Split(',')[14]);
            Dic.Add("CusAddress", list.Split(',')[15]);
            Dic.Add("Payment", list.Split(',')[16]);
            Dic.Add("Account", list.Split(',')[17]);
            return Retbll.SaveTradeEdit(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 删除交易信息
        /// </summary>
        /// <returns></returns>
        public string DeleteTrade()
        {
            string OrderId = Request.Form["OrderId"].ToString();
            return Retbll.DeleteTrade(OrderId, userInfo.User.Id);
        }

        /// <summary>
        /// 退货\退款\换货\发货
        /// type 判断类型1.退货\退款 2.发货
        /// </summary>
        /// <returns></returns>
        public string ReturnGoods()
        {
            string type = Request.Form["type"].ToString();
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            if (type == "1")
            {
                Dic.Add("OrderId", list.Split(',')[0]);
                Dic.Add("RetPrice", list.Split(',')[1]);
                Dic.Add("RetDetails", list.Split(',')[2]);
                Dic.Add("RetType", list.Split(',')[3]);
                Dic.Add("RetAccount", list.Split(',')[4]);
                Dic.Add("Scode", list.Split(',')[5]);
                Dic.Add("Reason", list.Split(',')[6]);
            }
            else if (type == "2")
            {
                Dic.Add("OrderId", list.Split(',')[0]);
                Dic.Add("ExPrice", list.Split(',')[1]);
                Dic.Add("SendTime", list.Split(',')[2]);
                Dic.Add("Express", list.Split(',')[3]);
                Dic.Add("ExpressNo", list.Split(',')[4]);
                Dic.Add("YFHKD", list.Split(',')[5]);
                Dic.Add("YFRMB", list.Split(',')[6]);
                Dic.Add("RetRemark", list.Split(',')[7]);
                Dic.Add("SendPerson", list.Split(',')[8]);
                Dic.Add("SendType", list.Split(',')[9]);
                Dic.Add("SendStatus", list.Split(',')[10]);
                Dic.Add("Scode", list.Split(',')[11]);
            }

            return Retbll.ReturnGoods(Dic, type, userInfo.User.Id);
        }

        /// <summary>
        /// 取消状态
        /// </summary>
        /// <returns></returns>
        public string CancelStatus()
        {
            string OrderId = Request.Form["OrderId"].ToString();
            string Scode = Request.Form["Scode"].ToString();
            string type = Request.Form["type"].ToString();
            return Retbll.CancelStatus(OrderId, Scode, type, userInfo.User.Id);
        }
        /// <summary>
        /// 获取查询条件权限
        /// </summary>
        /// <returns></returns>
        public string GetSearchlistProInfo()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "OrderId", "Scode", "Brand", "Color", "TypeNo", "Size", "Def1" };
            s += "<ul>";
            foreach (string i in ss)
            {
                if (i == "OrderId")
                {
                    s += "<li>订单编号:<input type='text' id='txtSearchOrderId1' /></li>";
                }
                else if (i == "Scode")
                {
                    s += "<li>货品编号:<input type='text' id='txtSearchScode' /></li>";
                }
                else if (i == "Brand")
                {
                    s += "<li>品牌:<select id='selSearchBrand'>" + GetBrand("") + "</select></li>";
                }
                else if (i == "Color")
                {
                    s += "<li>颜色:<input type='text' id='txtSearchColor' /></li>";
                }
                else if (i == "TypeNo")
                {
                    s += "<li>类别:<select id='selSearchTypeNo'>" + GetType("") + "</select></li>";
                }
                else if (i == "Size")
                {
                    s += "<li>尺码:<input type='text' id='txtSearchSize' /></li>";
                }
                else if (i == "Def1")
                {
                    s += "<li>发货状态:<select id='selSearchDef1'><option value=''>请选择</option><option value='1'>未发货</option><option value='2'>已发货</option><option value='3'>退货中</option><option value='4'>退货完成</option><option value='5'>退款中</option><option value='6'>退款完成</option><option value='7'>交易成功</option></select></li>";
                }

            }
            s += "<li><input type='button' onclick='OnSearch1()' value='  查   询  ' /></li>";
            //s += "<li><a href='#' onclick='AddClick1()' >添    加</a></li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";
            return s;

        }

        /// <summary>
        /// 商品信息
        /// </summary>
        public string SearchProInfo(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                Dic.Add("Scode", SearchInfo[1]);
                Dic.Add("Brand", SearchInfo[2]);
                Dic.Add("Color", SearchInfo[3]);
                Dic.Add("TypeNo", SearchInfo[4]);
                Dic.Add("Size", SearchInfo[5]);
                Dic.Add("Def1", SearchInfo[6]);
                int counts;
                dt = Retbll.GetDateProInfo(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabProInfo(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 商品信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string GetTabProInfo(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "Def2", "Scode", "Brand", "Color", "TypeNo", "Imagefile", "Size", "Number", "ProDetails", "ProLink", "DeliveryAttri", "LastOrderId", "SellPrice", "Warehouse", "Def1" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "Def2")
                        s.Append("系统单号");
                    if (ssName[z] == "Scode")
                        s.Append("货品编号");
                    if (ssName[z] == "Brand")
                        s.Append("品牌");
                    if (ssName[z] == "Color")
                        s.Append("颜色");
                    if (ssName[z] == "TypeNo")
                        s.Append("类别");
                    if (ssName[z] == "Imagefile")
                        s.Append("商品示图");
                    if (ssName[z] == "Size")
                        s.Append("尺码");
                    if (ssName[z] == "Number")
                        s.Append("数量");
                    if (ssName[z] == "ProDetails")
                        s.Append("商品描述");
                    if (ssName[z] == "ProLink")
                        s.Append("商品链接");
                    if (ssName[z] == "DeliveryAttri")
                        s.Append("发货属性");
                    if (ssName[z] == "LastOrderId")
                        s.Append("上次订单号");
                    if (ssName[z] == "SellPrice")
                        s.Append("成交金额");
                    if (ssName[z] == "Warehouse")
                        s.Append("出货仓");
                    if (ssName[z] == "Def1") s.Append("发货状态");
                    s.Append("</th>");
                }
            }
            s.Append("<th>操作</th>");
            s.Append("</tr>");
            #endregion


            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j] == "Imagefile")
                        {
                            s.Append("<img src=\"" + dt.Rows[i][ssName[j]].ToString() + "\" onerror='errorImg(this)' />");
                        }
                        else if (ssName[j] == "ProLink")
                        {
                            if (dt.Rows[i][ssName[j]].ToString() == "" && dt.Rows[i][ssName[j]].ToString() == null)
                            {
                                s.Append("<a href=\"#\" target='_blank'>无链接</a>");
                            }
                            else
                            {
                                s.Append("<a href=\"" + dt.Rows[i][ssName[j]].ToString() + "\" target='_blank'>链接</a>");
                            }

                        }
                        //1.未发货 2.已发货  3.退货中  4.退货完成  5.退款中  6.退款完成   7.交易成功
                        else if (ssName[j] == "Def1")
                        {
                            string item = dt.Rows[i][ssName[j]].ToString();
                            string Def1 = item == "1" ? "未发货" : item == "2" ? "已发货" : item == "3" ? "退货中" : item == "4" ? "退货完成" : item == "5" ? "退款中" : item == "6" ? "退款完成" : item == "7" ? "交易成功" : "";
                            s.Append(Def1);
                        }
                        else
                        {

                            s.Append(dt.Rows[i][ssName[j]].ToString());

                        }
                        s.Append("</td>");

                    }

                }

                s.Append("<td>");
                #region 编辑
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {

                    //s.Append("<td><a href='#' onclick='EditAttribute(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["TypeNameVen"].ToString() + "\",\"" + dt.Rows[i]["Vencode"].ToString() + "\")'>编辑</a></td>");
                    s.Append("<a href='#' onclick='EditProductInfo(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                }
                #endregion

                #region 操作
                //商品状态 1.未发货 2.已发货  3.退货中  4.退货完成  5.退款中  6.退款完成   7.交易成功
                //操作按钮 1.申请退款 2.申请退货 3.取消退款 4.取消退货 5.发货 6.申请退货(未发货)
                if (dt.Rows[i]["Def1"].ToString() == "1")
                {
                    //操作按钮 2.申请退货 5.发货
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"5\")'>发货</a>");
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"6\")'>申请退货</a>");
                }
                else if (dt.Rows[i]["Def1"].ToString() == "2")
                {
                    //操作按钮 1.申请退款 2.申请退货 
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"1\")'>申请退款</a>");
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"2\")'>申请退货</a>");
                }
                else if (dt.Rows[i]["Def1"].ToString() == "3")
                {
                    //操作按钮 4.取消退货 
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"4\")'>取消退货</a>");
                }
                else if (dt.Rows[i]["Def1"].ToString() == "4")
                {
                }
                else if (dt.Rows[i]["Def1"].ToString() == "5")
                {
                    //操作按钮 4.取消退货 
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"3\")'>取消退款</a>");

                }
                else if (dt.Rows[i]["Def1"].ToString() == "6")
                {
                }
                else if (dt.Rows[i]["Def1"].ToString() == "7")
                {

                    //操作按钮 1.申请退款 2.申请退货 
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"1\")'>申请退款</a>");
                    s.Append("<a href='#' onclick='ReturnGoods(\"" + dt.Rows[i]["OrderId"].ToString() + "\",\"" + dt.Rows[i]["Scode"].ToString() + "\",\"2\")'>申请退货</a>");
                }
                #endregion

                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteProductInfo(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a>");
                }
                #endregion

                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        public string AddProductInfo()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", list.Split(',')[0]);
            Dic.Add("Scode", list.Split(',')[1]);
            Dic.Add("Number", list.Split(',')[2]);
            Dic.Add("ProDetails", list.Split(',')[3]);
            Dic.Add("ProLink", list.Split(',')[4]);
            Dic.Add("DeliveryAttri", list.Split(',')[5]);
            Dic.Add("LastOrderId", list.Split(',')[6]);
            Dic.Add("SellPrice", list.Split(',')[7]);
            Dic.Add("Warehouse", list.Split(',')[8]);
            Dic.Add("SystemId", list.Split(',')[9]);
            return Retbll.AddProductInfo(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <returns></returns>
        public string EditProductInfo()
        {
            StringBuilder s = new StringBuilder();
            string Id = Request.Form["Id"].ToString();
            DataTable dt = Retbll.EditProductInfo(Id);
            s.Append("<ul>");
            s.Append("<li>订单编号:<input type='text' id='txtEditOrderId1' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' title='" + dt.Rows[0]["Id"] + "' /></li>");
            s.Append("<li>系统单号:<input type='text' id='txtEditSystemId' value='" + dt.Rows[0]["Def2"] + "'  /></li>");
            s.Append("<li>货品编号:<input type='text' id='txtEditScode' value='" + dt.Rows[0]["Scode"] + "'  /></li>");
            s.Append("<li>品牌:<input type='text' id='selEditBrand' value='" + dt.Rows[0]["Brand"].ToString() + "' disabled='disabled' /></li>");
            s.Append("<li>颜色:<input type='text' id='txtEditColor' value='" + dt.Rows[0]["Color"] + "'  disabled='disabled' /></li>");
            s.Append("<li>类别:<input type='text' id='selEditTypeNo' value='" + dt.Rows[0]["TypeNo"].ToString() + "' disabled='disabled' /></li>");
            //s.Append("<li>商品示图:<img src=\""+ dt.Rows[0]["Imagefile"] + "\" style='height:30px;width:60px;' /> </li>");
            s.Append("<li>尺码:<input type='text' id='txtEditSize' value='" + dt.Rows[0]["Size"] + "' disabled='disabled'  /></li>");
            s.Append("<li>数量:<input type='text' id='txtEditNumber' value='" + dt.Rows[0]["Number"] + "' /></li>");
            s.Append("<li>商品链接:<input type='text' id='txtEditProLink' value='" + dt.Rows[0]["ProLink"] + "'  /></li>");
            s.Append("<li>发货属性:<input type='text' id='txtEditDeliveryAttri' value='" + dt.Rows[0]["DeliveryAttri"] + "'  /></li>");
            s.Append("<li>上次订单号:<input type='text' id='txtEditLastOrderId' value='" + dt.Rows[0]["LastOrderId"] + "'  /></li>");
            s.Append("<li>成交金额:<input type='text' id='txtEditSellPrice' value='" + dt.Rows[0]["SellPrice"] + "'  /></li>");
            s.Append("<li>出货仓:<input type='text' id='txtEditWarehouse' value='" + dt.Rows[0]["Warehouse"] + "'  /></li>");
            s.Append("<li class='liRemark'>商品描述:<textarea id='txtEditProDetails' >" + dt.Rows[0]["ProDetails"] + "</textarea></li>");
            s.Append("</ul>");
            s.Append("<div class='clearfix'></div><div style='margin-top:10px;'><input type='button' value=' 保   存 ' onclick='SaveProductInfoEdit()' /><input type='button' class='btnClose' value=' 关   闭 ' onclick='Close()'  /></div>");
            return s.ToString();
        }

        /// <summary>
        /// 保存编辑商品信息
        /// </summary>
        /// <returns></returns>
        public string SaveProductInfoEdit()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list.Split(',')[0]);
            Dic.Add("OrderId", list.Split(',')[1]);
            Dic.Add("Scode", list.Split(',')[2]);
            Dic.Add("Number", list.Split(',')[3]);
            Dic.Add("ProDetails", list.Split(',')[4]);
            Dic.Add("ProLink", list.Split(',')[5]);
            Dic.Add("DeliveryAttri", list.Split(',')[6]);
            Dic.Add("LastOrderId", list.Split(',')[7]);
            Dic.Add("SellPrice", list.Split(',')[8]);
            Dic.Add("Warehouse", list.Split(',')[9]);
            Dic.Add("SystemId", list.Split(',')[10]);
            return Retbll.SaveProductInfoEdit(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <returns></returns>
        public string DeleteProductInfo()
        {
            string Id = Request.Form["Id"].ToString();
            return Retbll.DeleteProductInfo(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 退货信息
        /// </summary>
        /// <returns></returns>
        public ActionResult RetProductInfo()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }

        /// <summary>
        /// 获取查询条件权限
        /// </summary>
        /// <returns></returns>
        public string GetSearchlistRetPro()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            s += "<ul>";
            foreach (string i in ss)
            {
                if (i == "OrderId")
                {
                    s += "<li>订单编号:<input type='text' id='txtSearchOrderId' /></li>";
                }
                else if (i == "RetPrice")
                {
                    s += "<li>退款金额:<input type='text' id='txtSearchRetPrice' /></li>";
                }
                //else if (i == "QuaLevel")
                //{
                //    s += "<li>质量等级:<input type='text' id='txtSearchQuaLevel' /></li>";
                //}
                //else if (i == "RetTime")
                //{
                //    s += "<li>退货时间:<input type='date' id='txtSearchRetTime' />-<input type='date' id='txtSearchRetTime1' /></li>";
                //}
                else if (i == "Express")
                {
                    s += "<li>快递公司:<input type='text' id='txtSearchExpress' /></li>";
                }
                else if (i == "ExpressNo")
                {
                    s += "<li>快递单号:<input type='text' id='txtSearchExpressNo' /></li>";
                }
                else if (i == "RetType")
                {
                    s += "<li>退换货类型:<select id='selSearchRetType'><option value=''>请选择</option><option value='1'>退款</option><option value='2'>退货</option><option value='5'>退货(未发货)</option></select></li></li>";
                }
                else if (i == "ServiceId")
                {
                    s += "<li>处理客服:<input type='text' id='txtSearchServiceId' /></li>";
                }
                else if (i == "Receiver")
                {
                    s += "<li>收货人:<input type='text' id='txtSearchReceiver' /></li>";
                }
                else if (i == "RetAccount")
                {
                    s += "<li>退款账号:<input type='text' id='txtSearchRetAccount' /></li>";
                }
                else if (i == "Def1")
                {
                    s += "<li>退货状态:<select id='selSearchDef1'><option value=''>请选择</option><option value='1'>申请退款</option><option value='2'>申请退货</option><option value='3'>退款成功</option><option value='4'>退货成功</option><option value='5'>申请退货(未发货)</option><option value='6'>退货已收</option></select></li>";
                }
                else if (i == "Def3")
                {
                    s += "<li>退货理由:<select id='selSearchDef3'><option value=''>请选择</option><option value='1'>七天无理由</option><option value='2'>有色差</option><option value='3'>其他</option></select></li>";
                }

            }
            s += "<li><input type='button' onclick='OnSearch()' value='  查   询  '  /></li>";
            //s += "<li><a href='#' onclick='AddClick()' >添    加</a></li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";

            return s;

        }

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        public string SearchRtnProInfo(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                Dic.Add("RetPrice", SearchInfo[1]);
                Dic.Add("QuaLevel", SearchInfo[2]);
                Dic.Add("RetTime", SearchInfo[3]);
                Dic.Add("RetTime1", SearchInfo[4]);
                Dic.Add("Express", SearchInfo[5]);
                Dic.Add("ExpressNo", SearchInfo[6]);
                Dic.Add("RetType", SearchInfo[7]);
                Dic.Add("Receiver", SearchInfo[8]);
                Dic.Add("ServiceId", SearchInfo[9]);
                Dic.Add("RetAccount", SearchInfo[10]);
                Dic.Add("Def1", SearchInfo[11]);
                Dic.Add("Def3", SearchInfo[12]);
                int counts;
                dt = Retbll.GetDate1(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabRtnProInfo(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <param name="dt"></param>
        public string GetTabRtnProInfo(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "RetPrice", "Express", "ExpressNo", "RetDetails", "RetType", "Receiver", "ServiceId", "RetAccount", "Def1", "Def2", "Def3" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "Def2")
                        s.Append("货品编号");
                    if (ssName[z] == "RetPrice")
                        s.Append("退款金额");
                    //if (ssName[z] == "QuaLevel")
                    //    s.Append("质量等级");
                    //if (ssName[z] == "RetTime")
                    //    s.Append("收货时间");
                    if (ssName[z] == "Express")
                        s.Append("快递公司");
                    if (ssName[z] == "ExpressNo")
                        s.Append("快递单号");
                    if (ssName[z] == "RetDetails")
                        s.Append("退货说明");
                    if (ssName[z] == "RetType")
                        s.Append("退换货类型");
                    if (ssName[z] == "Receiver")
                        s.Append("收货人");
                    if (ssName[z] == "ServiceId")
                        s.Append("处理客服");
                    if (ssName[z] == "RetAccount")
                        s.Append("退款账号");
                    if (ssName[z] == "Def1")
                        s.Append("退换状态");
                    if (ssName[z] == "Def3")
                        s.Append("退货理由");
                    s.Append("</th>");
                }
            }
            s.Append("<th>操作</th>");
            //if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            //{
            //    s.Append("<th>编辑</th>");
            //};
            //#region 删除
            //if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            //{
            //    s.Append("<th>删除</th>");
            //}
            //#endregion
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    s.Append("<td>");
                    if (ss.Contains(ssName[j]))
                    {
                        if (ssName[j] == "RetType")//退货类型 1.退款 2.退货 5.退货(未发货)
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "1" ? "退款" : dt.Rows[i][ssName[j]].ToString() == "2" ? "退货" : dt.Rows[i][ssName[j]].ToString() == "5" ? "退货(未发货)" : "");
                        }
                        else if (ssName[j] == "Def1")//1.申请退款 2.申请退货3.退款成功 4.退货成功 5.退货已收
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "1" ? "申请退款" : dt.Rows[i][ssName[j]].ToString() == "2" ? "申请退货" : dt.Rows[i][ssName[j]].ToString() == "3" ? "退款成功" : dt.Rows[i][ssName[j]].ToString() == "4" ? "退货成功" : dt.Rows[i][ssName[j]].ToString() == "5" ? "申请退货(未发货)" : dt.Rows[i][ssName[j]].ToString() == "6" ? "退货已收" : "");
                        }
                        else if (ssName[j] == "Def3")//1.七天无理由 2.有色差 3.其他
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "1" ? "七天无理由" : dt.Rows[i][ssName[j]].ToString() == "2" ? "有色差" : dt.Rows[i][ssName[j]].ToString() == "3" ? "其他" : "");
                        }
                        else
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                        }
                        s.Append("</td>");
                        //if (ssName[j].Contains("Time"))
                        //{
                        //    s.Append("<td>");
                        //    s.Append(Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd"));
                        //    s.Append("</td>");
                        //}
                        //else
                        //{
                        //    s.Append("<td>");
                        //    s.Append(dt.Rows[i][ssName[j]].ToString());
                        //    s.Append("</td>");
                        //}
                    }

                }
                s.Append("<td>");

                #region 编辑
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    if (dt.Rows[i]["Def1"].ToString() == "1" || dt.Rows[i]["Def1"].ToString() == "2" || dt.Rows[i]["Def1"].ToString() == "5")
                    {
                        s.Append("<a href='#' onclick='EditRetProInfo(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                    }
                }
                #endregion
                if (dt.Rows[i]["Def1"].ToString() == "1")//更新交易状态 1.申请退款 2.申请退货3.退款成功 4.退货成功 5.申请退货(未发货) 6.退货已收
                {
                    s.Append("<a  href='#' onclick='UpdateSalesState(\"" + dt.Rows[i]["Id"].ToString() + "\")'>确认退款</a>");
                }
                //else if(dt.Rows[i]["Def1"].ToString() == "2"){
                //    s.Append("<a  href='#' onclick='UpdateSalesState(\"" + dt.Rows[i]["OrderId"].ToString() + "---" + dt.Rows[i]["Def2"].ToString() + "\")'>确认退货</a>");
                //}
                else if (dt.Rows[i]["Def1"].ToString() == "5")
                {
                    s.Append("<a  href='#' onclick='UpdateSalesState(\"" + dt.Rows[i]["Id"].ToString() + "\")'>确认退货</a>");
                }
                else if (dt.Rows[i]["Def1"].ToString() == "6")
                {
                    s.Append("<a  href='#' onclick='UpdateSalesState(\"" + dt.Rows[i]["Id"].ToString() + "\")'>确认退货</a>");
                }
                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteRetProInfo(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a></td>");
                }
                #endregion
                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }

        ///// <summary>
        ///// 添加退货信息
        ///// </summary>
        ///// <returns></returns>
        //public string AddRetProInfo()
        //{
        //    string list = Request.Form["list[]"].ToString();
        //    Dictionary<string, string> Dic = new Dictionary<string, string>();
        //    Dic.Add("OrderId", list.Split(',')[0]);
        //    Dic.Add("RetPrice", list.Split(',')[1]);
        //    Dic.Add("QuaLevel", list.Split(',')[2]);
        //    Dic.Add("RetTime", list.Split(',')[3]);
        //    Dic.Add("Express", list.Split(',')[4]);
        //    Dic.Add("ExpressNo", list.Split(',')[5]);
        //    Dic.Add("RetDetails", list.Split(',')[6]);
        //    Dic.Add("RetType", list.Split(',')[7]);
        //    Dic.Add("Receiver", list.Split(',')[8]);
        //    Dic.Add("ServiceId", list.Split(',')[9]);
        //    Dic.Add("RetAccount", list.Split(',')[10]);
        //    return Retbll.AddRetProInfo(Dic, userInfo.User.Id);
        //}

        /// <summary>
        /// 修改退货信息
        /// </summary>
        /// <returns></returns>
        public string EditRetProInfo()
        {
            StringBuilder s = new StringBuilder();
            string Id = Request.Form["Id"].ToString();
            DataTable dt = new DataTable();
            dt = Retbll.GetDate1(Id);
            var item = dt.Rows[0]["RetType"].ToString() == "1" ? "退款" : "退货";
            s.Append("<ul>");
            s.Append("<li>订单编号:<input type='text' id='txtEditOrderId' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' title='" + dt.Rows[0]["Id"] + "' /></li>");
            s.Append("<li>退款金额:<input type='text' id='txtEditRetPrice' value='" + dt.Rows[0]["RetPrice"] + "'  /></li>");
            //s.Append("<li>质量等级:<input type='text' id='txtEditQuaLevel' value='" + dt.Rows[0]["QuaLevel"] + "'  /></li>");
            //s.Append("<li>收货时间:<input type='date' id='txtEditRetTime' value='" + Convert.ToDateTime(dt.Rows[0]["RetTime"].ToString()).ToString("yyyy-MM-dd") + "'  /></li>");
            s.Append("<li>快递公司:<input type='text' id='txtEditExpress' value='" + dt.Rows[0]["Express"] + "'  /></li>");
            s.Append("<li>快递单号:<input type='text' id='txtEditExpressNo' value='" + dt.Rows[0]["ExpressNo"] + "'  /></li>");
            s.Append("<li>退换货类型:<input type='text' id='txtEditRetType' value='" + item + "' disabled='disabled'  /></li>");
            s.Append("<li>收货人:<input type='text' id='txtEditReceiver' value='" + dt.Rows[0]["Receiver"] + "'  /></li>");
            s.Append("<li>处理客服:<input type='text' id='txtEditServiceId' value='" + dt.Rows[0]["ServiceId"] + "'  /></li>");
            s.Append("<li>退款账号:<input type='text' id='txtEditRetAccount' value='" + dt.Rows[0]["RetAccount"] + "'  /></li>");
            s.Append("<li></li>");
            s.Append("<li class='liRemark'>退货说明:<textarea  id='txtEditRetDetails' >" + dt.Rows[0]["RetDetails"] + "</textarea></li>");
            s.Append("</ul>");
            s.Append("<div class='clearfix'></div><div style='margin-top: 10px;'><input type='button' value=' 保   存 ' onclick='SaveEdit()' /><input type='button' class='btnClose' value=' 关   闭 ' onclick='Close()' /></div>");
            return s.ToString();
        }

        /// <summary>
        /// 修改退货信息
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetProInfo()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list.Split(',')[0]);
            Dic.Add("OrderId", list.Split(',')[1]);
            Dic.Add("RetPrice", list.Split(',')[2]);
            //Dic.Add("QuaLevel", list.Split(',')[3]);
            //Dic.Add("RetTime", list.Split(',')[4]);
            Dic.Add("Express", list.Split(',')[3]);
            Dic.Add("ExpressNo", list.Split(',')[4]);
            Dic.Add("RetDetails", list.Split(',')[5]);
            Dic.Add("RetType", list.Split(',')[6]);
            Dic.Add("Receiver", list.Split(',')[7]);
            Dic.Add("ServiceId", list.Split(',')[8]);
            Dic.Add("RetAccount", list.Split(',')[9]);
            return Retbll.SaveEditRetProInfo(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <returns></returns>
        public string DeleteRetProInfo()
        {
            string Id = Request.Form["Id"].ToString();
            return Retbll.DeleteRetProInfo(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 出货记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ShipmentRecord()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }

        /// <summary>
        /// 获取查询条件权限
        /// </summary>
        /// <returns></returns>
        public string GetSearchlistRecord()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            s += "<ul>";
            foreach (string i in ss)
            {
                if (i == "OrderId")
                {
                    s += "<li>订单编号:<input type='text' id='txtSearchOrderId' /></li>";
                }
                else if (i == "ExPrice")
                {
                    s += "<li>换货金额:<input type='text' id='txtSearchExPrice' /></li>";
                }
                //else if (i == "QuaLevel")
                //{
                //    s += "<li>质量等级:<input type='text' id='txtSearchQuaLevel' /></li>";
                //}
                else if (i == "SendTime")
                {
                    s += "<li>发货时间:<input type='date' id='txtSearchSendTime' />-<input type='date' id='txtSearchSendTime1' /></li>";
                }
                else if (i == "Express")
                {
                    s += "<li>快递公司:<input type='text' id='txtSearchExpress' /></li>";
                }
                else if (i == "ExpressNo")
                {
                    s += "<li>快递单号:<input type='text' id='txtSearchExpressNo' /></li>";
                }
                else if (i == "SendPerson")
                {
                    s += "<li>发货人:<input type='text' id='txtSearchSendPerson' /></li>";
                }
                //else if (i == "SendType")
                //{
                //    s += "<li>出货类型:<input type='text' id='txtSearchSendType' /></li>";
                //}
                else if (i == "SendStatus")
                {
                    s += "<li>发货状态:<select id='txtSearchSendStatus'><option value=''>请选择</option><option value='1'>已发货</option><option value='2'>订单关闭</option></select></li>";
                }

            }
            s += "<li><input type='button' onclick='OnSearch()' value='  查   询  '  /></li>";
            //s += "<li><a href='#' onclick='AddClick()' >添    加</a></li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";
            return s;

        }

        /// <summary>
        /// 返回出货记录
        /// </summary>
        /// <returns></returns>
        public string SearchShipmentRecord(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                Dic.Add("ExPrice", SearchInfo[1]);
                //Dic.Add("QuaLevel", SearchInfo[2]);
                Dic.Add("SendTime", SearchInfo[2]);
                Dic.Add("SendTime1", SearchInfo[3]);
                Dic.Add("Express", SearchInfo[4]);
                Dic.Add("ExpressNo", SearchInfo[5]);
                Dic.Add("SendPerson", SearchInfo[6]);
                Dic.Add("SendType", SearchInfo[7]);
                Dic.Add("SendStatus", SearchInfo[8]);
                int counts;
                dt = Retbll.GetDate2(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabShipmentRecord(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 出货记录
        /// </summary>
        /// <param name="dt"></param>
        public string GetTabShipmentRecord(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "ExPrice", "SendTime", "Express", "ExpressNo", "YFHKD", "YFRMB", "RetRemark", "SendPerson", "SendType", "SendStatus", "Def2" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "Def2")
                        s.Append("货品编号");
                    if (ssName[z] == "ExPrice")
                        s.Append("换货金额");
                    //if (ssName[z] == "QuaLevel")
                    //    s.Append("质量等级");
                    if (ssName[z] == "SendTime")
                        s.Append("发货时间");
                    if (ssName[z] == "Express")
                        s.Append("快递公司");
                    if (ssName[z] == "ExpressNo")
                        s.Append("快递单号");
                    if (ssName[z] == "YFHKD")
                        s.Append("运费HKD");
                    if (ssName[z] == "YFRMB")
                        s.Append("运费RMB");
                    if (ssName[z] == "RetRemark")
                        s.Append("退货说明");
                    if (ssName[z] == "SendPerson")
                        s.Append("发货人");
                    if (ssName[z] == "SendType")
                        s.Append("出货类型");
                    if (ssName[z] == "SendStatus")
                        s.Append("发货状态");
                    //if (ssName[z] == "SaleStates")
                    //    s.Append("交易状态");
                    s.Append("</th>");
                }
            }

            s.Append("<th>操作</th>");
            //if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            //{
            //    s.Append("<th>编辑</th>");
            //};
            //#region 删除
            //if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            //{
            //    s.Append("<th>删除</th>");
            //}
            //#endregion
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        if (ssName[j].Contains("Time"))
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "" ? "" : Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd"));
                            s.Append("</td>");
                        }
                        else if (ssName[j] == "SendType")
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "1" ? "新订单" : "换货");
                            s.Append("</td>");
                        }
                        else if (ssName[j] == "SendStatus")//交易状态
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "1" ? "已发货" : dt.Rows[i][ssName[j]].ToString() == "2" ? "申请换货" : "换货成功");
                            s.Append("</td>");
                        }
                        //else if (ssName[j] == "SaleStates")//交易状态
                        //{
                        //    s.Append("<td>");
                        //    s.Append(dt.Rows[i][ssName[j]].ToString() == "2" ? "已发货" : dt.Rows[i][ssName[j]].ToString() == "7" ? "申请换货" : "换货成功");
                        //    s.Append("</td>");
                        //}
                        else
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                            s.Append("</td>");
                        }

                    }
                }

                s.Append("<td>");
                #region 编辑
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='EditRecord(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                }
                #endregion

                //if (dt.Rows[i]["SendStatus"].ToString() == "2")//更新交易状态
                //{
                //    s.Append("<a href='#' onclick='UpdateSalesState(\"" + dt.Rows[i]["OrderId"].ToString() + "---" + dt.Rows[i]["Def2"].ToString() + "\")'>确认换货</a>");
                //}

                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteRecord(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a>");
                }
                #endregion
                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }

        /// <summary>
        /// 添加出货记录
        /// </summary>
        /// <returns></returns>
        public string AddRecord()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", list.Split(',')[0]);
            Dic.Add("ExPrice", list.Split(',')[1]);
            Dic.Add("QuaLevel", list.Split(',')[2]);
            Dic.Add("SendTime", list.Split(',')[3]);
            Dic.Add("Express", list.Split(',')[4]);
            Dic.Add("ExpressNo", list.Split(',')[5]);
            Dic.Add("YFHKD", list.Split(',')[6]);
            Dic.Add("YFRMB", list.Split(',')[7]);
            Dic.Add("RetRemark", list.Split(',')[8]);
            Dic.Add("SendPerson", list.Split(',')[9]);
            Dic.Add("SendType", list.Split(',')[10]);
            Dic.Add("SendStatus", list.Split(',')[11]);

            return Retbll.AddRecord(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 修改出货记录
        /// </summary>
        /// <returns></returns>
        public string EditRecord()
        {
            StringBuilder s = new StringBuilder();
            string Id = Request.Form["Id"].ToString();
            DataTable dt = new DataTable();
            dt = Retbll.GetDate2(Id);
            var item = dt.Rows[0]["SendType"].ToString() == "1" ? "新订单" : "换货";
            s.Append("<ul>");
            s.Append("<li>订单编号:<input type='text' id='txtEditOrderId' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' title='" + dt.Rows[0]["Id"] + "' /></li>");
            s.Append("<li>换货金额:<input type='text' id='txtEditExPrice' value='" + dt.Rows[0]["ExPrice"] + "'  /></li>");
            //s.Append("<li>质量等级:<input type='text' id='txtEditQuaLevel' value='" + dt.Rows[0]["QuaLevel"] + "'  /></li>");
            s.Append("<li>发货时间:<input type='date' id='txtEditSendTime' value='" + Convert.ToDateTime(dt.Rows[0]["SendTime"].ToString()).ToString("yyyy-MM-dd") + "'  /></li>");
            s.Append("<li>快递公司:<input type='text' id='txtEditExpress' value='" + dt.Rows[0]["Express"] + "'  /></li>");
            s.Append("<li>快递单号:<input type='text' id='txtEditExpressNo' value='" + dt.Rows[0]["ExpressNo"] + "'  /></li>");
            s.Append("<li>运费HKD:<input type='text' id='txtEditYFHKD' value='" + dt.Rows[0]["YFHKD"] + "'  /></li>");
            s.Append("<li>运费RMB:<input type='text' id='txtEditYFRMB' value='" + dt.Rows[0]["YFRMB"] + "'  /></li>");
            s.Append("<li>发货人:<input type='text' id='txtEditSendPerson' value='" + dt.Rows[0]["SendPerson"] + "'  /></li>");
            s.Append("<li>出货类型:<input type='text' id='txtEditSendType' value='" + item + "' disabled='disabled'  /></li>");
            s.Append("<li>发货状态:<input type='text' id='txtEditSendStatus' value='" + dt.Rows[0]["SendStatus"] + "'  /></li>");
            s.Append("<li class='liRemark'>发货说明:<textarea  id='txtEditRetRemark' >" + dt.Rows[0]["RetRemark"] + "</textarea></li>");
            s.Append("</ul>");
            s.Append("<div class='clearfix'></div><div style='margin-top: 10px;'><input type='button' value=' 保   存 ' onclick='SaveEdit()' /><input type='button' class='btnClose' value=' 关   闭 ' onclick='Close()' /></div>");
            return s.ToString();
        }

        /// <summary>
        /// 修改出货记录
        /// </summary>
        /// <returns></returns>
        public string SaveEditRecord()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list.Split(',')[0]);
            Dic.Add("OrderId", list.Split(',')[1]);
            Dic.Add("ExPrice", list.Split(',')[2]);
            //Dic.Add("QuaLevel", list.Split(',')[3]);
            Dic.Add("SendTime", list.Split(',')[3]);
            Dic.Add("Express", list.Split(',')[4]);
            Dic.Add("ExpressNo", list.Split(',')[5]);
            Dic.Add("YFHKD", list.Split(',')[6]);
            Dic.Add("YFRMB", list.Split(',')[7]);
            Dic.Add("RetRemark", list.Split(',')[8]);
            Dic.Add("SendPerson", list.Split(',')[9]);
            Dic.Add("SendType", list.Split(',')[10]);
            Dic.Add("SendStatus", list.Split(',')[11]);
            return Retbll.SaveEditRecord(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 删除出货记录
        /// </summary>
        /// <returns></returns>
        public string DeleteRecord()
        {
            string Id = Request.Form["Id"].ToString();
            return Retbll.DeleteRecord(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 退货库存
        /// </summary>
        /// <returns></returns>
        public ActionResult RetBalance()
        {

            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion

                ViewData["myMenuId"] = menuId;
                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }


        /// <summary>
        /// 退货库存获取查询条件权限
        /// </summary>
        /// <returns></returns>
        public string GetSearchlistRetBalance()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            s += "<ul>";
            foreach (string i in ss)
            {
                if (i == "OrderId")
                {
                    s += "<li>订单编号:<input type='text' id='txtSearchOrderId' /></li>";
                }
                else if (i == "Scode")
                {
                    s += "<li>货品编号:<input type='text' id='txtSearchScode' /></li>";
                }
                else if (i == "Brand")
                {
                    s += "<li>品牌:<select id='selSearchBrand'>" + GetBrand("") + "</select></li>";
                }
                else if (i == "Color")
                {
                    s += "<li>颜色:<input type='text' id='txtSearchColor' /></li>";
                }
                else if (i == "TypeNo")
                {
                    s += "<li>类别:<select id='selSearchTypeNo'>" + GetType("") + "</select></li>";
                }
                else if (i == "Size")
                {
                    s += "<li>尺码:<input type='text' id='txtSearchSize' /></li>";
                }
                else if (i == "LastOrderId")
                {
                    s += "<li>上次订单号:<input type='text' id='txtSearchLastOrderId' /></li>";
                }
                else if (i == "RetTime")
                {
                    s += "<li>收货时间:<input type='date' id='txtSearchRetTime' />-<input type='date' id='txtSearchRetTime1' /></li>";
                }
                //else if (i == "Price")
                //{
                //    s += "<li>交易价:<input type='text' id='txtSearchPrice' />-<input type='text' id='txtSearchPrice1' /></li>";
                //}
                //else if (i == "Number")
                //{
                //    s += "<li>数量:<input type='text' id='txtSearchNumber' /></li>";
                //}
                //else if (i == "RetPrice")
                //{
                //    s += "<li>退款金额:<input type='text' id='txtSearchRetPrice' /></li>";
                //}
                //else if (i == "QuaLevel")
                //{
                //    s += "<li>质量等级:<input type='text' id='txtSearchQuaLevel' /></li>";
                //}
                //else if (i == "RetNum")
                //{
                //    s += "<li>退货次序:<input type='text' id='txtSearchRetNum' /></li>";
                //}

                //else if (i == "SellPrice")
                //{
                //    s += "<li>成交金额:<input type='text' id='txtSearchSellPrice' /></li>";
                //}
                //else if (i == "Heidui")
                //{
                //    s += "<li>核对:<input type='text' id='txtSearchHeidui' /></li>";
                //}
                //else if (i == "ExBalance")
                //{
                //    s += "<li>核对转库:<input type='text' id='txtSearchExBalance' /></li>";
                //}

            }
            s += "<li><input type='button' onclick='OnSearch()' value='  查   询  '  /></li>";
            s += "<li><a href='#' onclick='AddClick()' >添    加</a></li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";
            return s;

        }

        /// <summary>
        /// 返回退货库存
        /// </summary>
        /// <returns></returns>
        public string SearchRetBalance(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                Dic.Add("Scode", SearchInfo[1]);
                Dic.Add("Brand", SearchInfo[2]);
                Dic.Add("Color", SearchInfo[3]);
                Dic.Add("TypeNo", SearchInfo[4]);
                Dic.Add("Size", SearchInfo[5]);
                Dic.Add("LastOrderId", SearchInfo[6]);
                Dic.Add("RetTime", SearchInfo[7]);
                Dic.Add("RetTime1", SearchInfo[8]);
                int counts;
                dt = Retbll.GetDate3(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabRetBalance(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 退货库存
        /// </summary>
        /// <param name="dt"></param>
        public string GetTabRetBalance(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "Scode", "Brand", "Color", "TypeNo", "Imagefile", "Size", "Price", "Number", "QuaLevel", "RetTime", "ProDetails", "ProLink", "RetNum", "LastOrderId", "SellPrice", "Heidui", "ExBalance" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "Scode")
                        s.Append("货品编号");
                    if (ssName[z] == "Brand")
                        s.Append("品牌");
                    if (ssName[z] == "Color")
                        s.Append("颜色");
                    if (ssName[z] == "TypeNo")
                        s.Append("类别");
                    if (ssName[z] == "Imagefile")
                        s.Append("商品示图");
                    if (ssName[z] == "Size")
                        s.Append("尺码");
                    if (ssName[z] == "Price")
                        s.Append("交易价");
                    if (ssName[z] == "Number")
                        s.Append("数量");
                    if (ssName[z] == "QuaLevel")
                        s.Append("质量等级");
                    if (ssName[z] == "RetTime")
                        s.Append("收货时间");
                    if (ssName[z] == "ProDetails")
                        s.Append("货品描述");
                    if (ssName[z] == "ProLink")
                        s.Append("商品链接");
                    if (ssName[z] == "RetNum")
                        s.Append("退货次序");
                    if (ssName[z] == "LastOrderId")
                        s.Append("上次订单号");
                    if (ssName[z] == "SellPrice")
                        s.Append("成交金额");
                    if (ssName[z] == "Heidui")
                        s.Append("核对");
                    if (ssName[z] == "ExBalance")
                        s.Append("转库");

                    s.Append("</th>");
                }
            }
            s.Append("<th>操作</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j].Contains("Price"))
                        {
                            s.Append(Convert.ToDecimal(dt.Rows[i][ssName[j]].ToString()).ToString("f2"));
                        }
                        else if (ssName[j].Contains("Time"))
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString() == "" ? "" : Convert.ToDateTime(dt.Rows[i][ssName[j]].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else if (ssName[j].Contains("Imagefile"))
                        {
                            if (dt.Rows[i][ssName[j]].ToString().Contains("best-bms"))
                            {
                                string item = dt.Rows[i][ssName[j]].ToString() + "@10q." + dt.Rows[i][ssName[j]].ToString().Split('.')[dt.Rows[i][ssName[j]].ToString().Split('.').Length - 1];
                                s.Append("<img src=\"" + item + "\"  onerror='errorImg(this)' />");
                            }
                            else
                            {
                                s.Append("<img src=\"" + dt.Rows[i][ssName[j]].ToString() + "\"  onerror='errorImg(this)' />");
                            }

                        }
                        else
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                        }
                        s.Append("</td>");

                    }
                }

                s.Append("<td>");
                #region 编辑
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='EditRetBalance(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                }
                #endregion

                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteRetBalance(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a>");
                }

                s.Append("<a href='#' onclick='UploadPic(\"" + dt.Rows[i]["Id"].ToString() + "\")'>上传图片</a>");
                #endregion
                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }

        /// <summary>
        /// 添加退货库存
        /// </summary>
        /// <returns></returns>
        public string AddRetBalance()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", list.Split(',')[0]);
            Dic.Add("Scode", list.Split(',')[1]);
            Dic.Add("Price", list.Split(',')[2]);
            Dic.Add("Number", list.Split(',')[3]);
            Dic.Add("QuaLevel", list.Split(',')[4]);
            Dic.Add("RetTime", list.Split(',')[5]);
            Dic.Add("ProDetails", list.Split(',')[6]);
            Dic.Add("ProLink", list.Split(',')[7]);
            Dic.Add("RetNum", list.Split(',')[8]);
            Dic.Add("LastOrderId", list.Split(',')[9]);
            Dic.Add("SellPrice", list.Split(',')[10]);
            Dic.Add("Heidui", list.Split(',')[11]);
            Dic.Add("ExBalance", list.Split(',')[12]);

            return Retbll.AddRetBalance(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 修改退货库存
        /// </summary>
        /// <returns></returns>
        public string EditRetBalance()
        {
            StringBuilder s = new StringBuilder();
            string Id = Request.Form["Id"].ToString();
            DataTable dt = new DataTable();
            dt = Retbll.GetDate3(Id);
            s.Append("<ul>");
            s.Append("<li>订单编号:<input type='text' id='txtEditOrderId' value='" + dt.Rows[0]["OrderId"] + "' disabled='disabled' title='" + dt.Rows[0]["Id"] + "' /></li>");
            s.Append("<li>货品编号:<input type='text' id='txtEditScode' value='" + dt.Rows[0]["Scode"] + "'  /></li>");
            s.Append("<li>品牌:<input type='text' value='" + dt.Rows[0]["Brand"] + "'  disabled='disabled' /></li>");
            s.Append("<li>颜色:<input type='text' value='" + dt.Rows[0]["Color"] + "'  disabled='disabled' /></li>");
            s.Append("<li>类别:<input type='text' value='" + dt.Rows[0]["TypeNo"] + "'  disabled='disabled' /></li>");
            s.Append("<li>尺码:<input type='text' value='" + dt.Rows[0]["Size"] + "'  disabled='disabled' /></li>");
            s.Append("<li>交易价:<input type='text' id='txtEditPrice' value='" + Convert.ToDecimal(dt.Rows[0]["Price"].ToString()).ToString("f2") + "'  /></li>");
            s.Append("<li>数量:<input type='text' id='txtEditNumber' value='" + dt.Rows[0]["Number"] + "'  /></li>");
            s.Append("<li>质量等级:<input type='text' id='txtEditQuaLevel' value='" + dt.Rows[0]["QuaLevel"] + "'  /></li>");
            s.Append("<li>收货时间:<input type='date' id='txtEditRetTime' value='" + Convert.ToDateTime(dt.Rows[0]["RetTime"].ToString()).ToString("yyyy-MM-dd") + "'  /></li>");
            s.Append("<li>商品链接:<input type='text' id='txtEditProLink' value='" + dt.Rows[0]["ProLink"] + "'  /></li>");
            s.Append("<li>退货次序:<input type='text' id='txtEditRetNum' value='" + dt.Rows[0]["RetNum"] + "'  /></li>");
            s.Append("<li>上次订单号:<input type='text' id='txtEditLastOrderId' value='" + dt.Rows[0]["LastOrderId"] + "'  /></li>");
            s.Append("<li>成交金额:<input type='text' id='txtEditSellPrice' value='" + Convert.ToDecimal(dt.Rows[0]["SellPrice"].ToString()).ToString("f2") + "'  /></li>");
            s.Append("<li>核对:<input type='text' id='txtEditHeidui' value='" + dt.Rows[0]["Heidui"] + "'  /></li>");
            s.Append("<li>核对转库:<input type='text' id='txtEditExBalance' value='" + dt.Rows[0]["ExBalance"] + "'  /></li>");
            s.Append("<li class='liRemark'>货品描述:<textarea id='txtEditProDetails' >" + dt.Rows[0]["ProDetails"] + "</textarea></li>");
            s.Append("</ul>");
            s.Append("<div class='clearfix'></div><div style='margin-top: 10px;'><input type='button' value=' 保   存 ' onclick='SaveEdit()' /><input type='button' class='btnClose' value=' 关   闭 ' onclick='Close()' /></div>");
            return s.ToString();
        }

        /// <summary>
        /// 修改退货库存
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetBalance()
        {
            string list = Request.Form["list[]"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list.Split(',')[0]);
            Dic.Add("OrderId", list.Split(',')[1]);
            Dic.Add("Scode", list.Split(',')[2]);
            Dic.Add("Price", list.Split(',')[3]);
            Dic.Add("Number", list.Split(',')[4]);
            Dic.Add("QuaLevel", list.Split(',')[5]);
            Dic.Add("RetTime", list.Split(',')[6]);
            Dic.Add("ProDetails", list.Split(',')[7]);
            Dic.Add("ProLink", list.Split(',')[8]);
            Dic.Add("RetNum", list.Split(',')[9]);
            Dic.Add("LastOrderId", list.Split(',')[10]);
            Dic.Add("SellPrice", list.Split(',')[11]);
            Dic.Add("Heidui", list.Split(',')[12]);
            Dic.Add("ExBalance", list.Split(',')[13]);
            return Retbll.SaveEditRetBalance(Dic, userInfo.User.Id);
        }

        /// <summary>
        /// 删除出货记录
        /// </summary>
        /// <returns></returns>
        public string DeleteRetBalance()
        {
            string Id = Request.Form["Id"].ToString();
            return Retbll.DeleteRetBalance(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 上传退货示例图
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public string UploadPic(FormContext from)
        {
            var file = Request.Files["Filedata"];

            bool blean = false;
            string url = "/UploadImage/" + file.FileName;
            string uploadPath = Server.MapPath("~/UploadImage/");
            if (file != null)
            {
                file.SaveAs(uploadPath + file.FileName);
                return url;
            }
            return blean.ToString();
        }
        //public string SavePic()
        //{
        //    string s = string.Empty;
        //    string Id = Request.Form["Id"].ToString();
        //    string Url = Request.Form["url"].ToString();
        //    return s;
        //}
        /// <summary>
        /// 图片存到阿里云
        /// </summary>
        public bool SavePic()
        {
            bool flag = false;
            string rootName = "RetImages";
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            try
            {
                string Id = Request.Form["Id"].ToString();
                string ImgUrl = Request.Form["url"].ToString();
                int n = 1;
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.RetBalance.Where(a => a.Id == Convert.ToInt32(Id));
                string Scode = q.FirstOrDefault().Scode;
                string OrderId = q.FirstOrDefault().OrderId;
                var Imagefile = q.First().Imagefile;
                if (!(Imagefile == null || Imagefile == ""))
                {
                    n = Convert.ToInt32(Imagefile.ToString().Split('_')[Imagefile.ToString().Split('_').Length - 1].Split('.')[0]);
                }
                string Path = ImgUrl.Split('/')[ImgUrl.Split('/').Length - 1].ToString();// 本地文件名
                string item = OrderId + "_" + Scode.Replace("*", "_") + "_" + n + "." + Path.Split('.')[Path.Split('.').Length - 1].ToString();// 云文件名
                client.DeleteObject(bucketName, rootName + "/" + item);//删除已有文件
                n = n + 1;
                item = OrderId + "_" + Scode.Replace("*", "_") + "_" + n + "." + Path.Split('.')[Path.Split('.').Length - 1].ToString();// 生成新云文件名
                PutObject(Path, rootName + "/" + item);//存入新上传图片
                //string Path = ImgUrl.Split('/')[ImgUrl.Split('/').Length - 1].ToString();// 本地文件名
                //string item = OrderId + "_" + Scode.Replace("*", "_")+"." + Path.Split('.')[Path.Split('.').Length - 1].ToString();// 云文件名
                //client.DeleteObject(bucketName, rootName + "/" + item);//删除已有文件
                //PutObject(Path, rootName + "/" + item);//存入新上传图片
                string YunPath = "http://best-bms.pbxluxury.com/" + rootName + "/" + item;// 云文件路径
                foreach (var i in q)
                {
                    i.Imagefile = YunPath;
                }
                context.SubmitChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 图片存到阿里云
        /// </summary>
        /// <param name="filename"></param>//本地图片路径
        /// <param name="objkey"></param>//储存到云名称
        public void PutObject(string filename, string objkey)
        {
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //定义文件流
            var objStream = new System.IO.FileStream(Server.MapPath("~/UploadImage/") + filename, System.IO.FileMode.OpenOrCreate);
            //定义 object 描述
            var objMetadata = new ObjectMetadata();
            //执行 put 请求，并且返回对象的MD5摘要。
            var putResult = client.PutObject(bucketName, objkey.Replace("*", "_"), objStream, objMetadata);
        }
        /// <summary>
        /// 操作记录查询
        /// </summary>
        /// <returns></returns>
        public ActionResult OperationRecord()
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion

                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }

        /// <summary>
        /// 操作记录查询
        /// </summary>
        /// <returns></returns>
        public string SearchOperationRecord(string menuId, string lists, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", SearchInfo[0]);
                Dic.Add("OperaTable", SearchInfo[1]);
                Dic.Add("OperaType", SearchInfo[2]);
                Dic.Add("OperaTime", SearchInfo[3]);
                Dic.Add("OperaTime1", SearchInfo[4]);
                int counts;
                dt = Retbll.GetDate4(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabOperationRecord(dt, menuId) + "-*-" + counts;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 获取操作记录表格
        /// </summary>
        /// <param name="dt"></param>
        public string GetTabOperationRecord(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            string[] ssName = { "OrderId", "OperaTable", "OperaType", "OperaTime", "UserId" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "OrderId", "OperaTable", "OperaType", "OperaTime", "UserId" };
            StringBuilder s = new StringBuilder();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "OperaTable")
                        s.Append("操作表格");
                    if (ssName[z] == "OperaType")
                        s.Append("操作类型");
                    if (ssName[z] == "OperaTime")
                        s.Append("操作时间");
                    if (ssName[z] == "UserId")
                        s.Append("操作人");
                    s.Append("</th>");
                }
            }
            s.Append("<th>操作</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        if (ssName[j].Contains("Time"))
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                            s.Append("</td>");
                        }
                        else
                        {
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                            s.Append("</td>");
                        }

                    }
                }
                s.Append("<td>");
                #region 编辑
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='EditRetBalance(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                }
                #endregion

                #region 删除
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteRetBalance(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a>");
                }
                #endregion
                s.Append("</td>");
                s.Append("</tr>");
            }
            #endregion
            return s.ToString();

        }


        //---公共方法
        /// <summary>
        /// 更新交易状态
        /// </summary>OrderId 存储了OrderId以及Socde 通过-分割
        /// <returns></returns>
        public string UpdateSalesState()
        {
            string Id = Request.Form["Id"].ToString();
            return Retbll.UpdateSalesState(Id, userInfo.User.Id);

        }
        /// <summary>
        /// 类别下拉框
        /// </summary>
        /// <param name="TypeNo"></param>
        /// <returns></returns>
        public string GetType(string TypeNo)
        {
            string s = string.Empty;
            DataTable dt = DbHelperSQL.Query(@"select * from producttype").Tables[0];
            s += "<option value='' >请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (TypeNo == dt.Rows[i]["TypeNo"].ToString())
                {
                    s += "<option value='" + dt.Rows[i]["TypeNo"] + "' selected='selected'>" + dt.Rows[i]["TypeName"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i]["TypeNo"] + "'>" + dt.Rows[i]["TypeName"] + "</option>";
                }

            }
            return s;
        }

        /// <summary>
        /// 品牌下拉框
        /// </summary>
        /// <param name="Brand"></param>
        /// <returns></returns>
        public string GetBrand(string BrandName)
        {
            string s = string.Empty;
            DataTable dt = DbHelperSQL.Query(@"select * from brand").Tables[0];
            s += "<option value='' >请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (BrandName == dt.Rows[i]["BrandName"].ToString())
                {
                    s += "<option value='" + dt.Rows[i]["BrandName"] + "' selected='selected'>" + dt.Rows[i]["BrandName"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i]["BrandName"] + "'>" + dt.Rows[i]["BrandName"] + "</option>";
                }

            }
            return s;
        }

        /// <summary>
        /// 获取操作记录表的操作类型
        /// </summary>
        public string GetOperaRecordlist()
        {
            string s = string.Empty;
            DataTable dt = DbHelperSQL.Query(@"select OperaType from OperationRecord group by OperaType order by LEN(OperaType)").Tables[0];
            if (dt.Rows.Count > 0)
            {
                s += "<option value='' >请选择</option>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += "<option value='" + dt.Rows[i]["OperaType"].ToString() + "'>" + dt.Rows[i]["OperaType"].ToString() + "</option>";
                }
            }
            return s;
        }

        /// <summary>
        /// 获取客户下拉列表
        /// </summary>
        /// <returns></returns>
        public string GetSerivcelist()
        {
            string s = string.Empty;
            DataTable dt = DbHelperSQL.Query(@"select * from users where personaId='5' or  personaId='6'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                s += "<option value='' >请选择</option>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += "<option value='" + dt.Rows[i]["Id"].ToString() + "'>" + dt.Rows[i]["userRealName"].ToString() + "</option>";
                }
            }
            return s;
        }
    }
}
