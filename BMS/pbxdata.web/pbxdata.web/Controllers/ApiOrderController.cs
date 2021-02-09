/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ApiOrderController
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.web.Controllers
    * 文 件 名：       ApiOrderController
    * 创建时间：       2015/04/20 11:22:40
    * 作    者：       lcg
    * 说    明：       APP订单(非API订单) 订单状态：1 待确认 2 确认 3 待发货 4 发货 5 收货（交易成功） 11 退单 12 取消订单
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class ApiOrderController : BaseController
    {
        /// <summary>
        /// 获取是否有权限查看API订单
        /// </summary>
        /// <returns></returns>
        public ActionResult getApiOrder()
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
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        public string getData()
        {
            #region 搜索条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合

            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件
            string orderId = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "") ?? string.Empty; //订单ID
            string orderScode = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "") ?? string.Empty; //订单商品货号
            string orderColor = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "") ?? string.Empty; //订单商品颜色
            string orderBuyName = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "") ?? string.Empty; //客户姓名
            string orderAddress = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "") ?? string.Empty; //客户地址
            string orderMobile = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "") ?? string.Empty; //客户手机
            string orderStatus = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "") ?? string.Empty; //订单状态
            string orderPayStatus = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //支付状态

            orderId = orderId == "\'\'" ? string.Empty : orderId;
            orderScode = orderScode == "\'\'" ? string.Empty : orderScode;
            orderColor = orderColor == "\'\'" ? string.Empty : orderColor;
            orderBuyName = orderBuyName == "\'\'" ? string.Empty : orderBuyName;
            orderAddress = orderAddress == "\'\'" ? string.Empty : orderAddress;
            orderMobile = orderMobile == "\'\'" ? string.Empty : orderMobile;
            orderStatus = orderStatus == "\'\'" ? string.Empty : orderStatus;
            orderPayStatus = orderPayStatus == "\'\'" ? string.Empty : orderPayStatus;

            dic.Add("orderId", orderId);
            dic.Add("orderScode", orderScode);
            dic.Add("orderColor", orderColor);
            dic.Add("orderBuyName", orderBuyName);
            dic.Add("orderAddress", orderAddress);
            dic.Add("orderMobile", orderMobile);
            dic.Add("orderStatus", orderStatus);
            dic.Add("orderPayStatus", orderPayStatus);
            #endregion
            //int te = 0;
            bool splitSingle = false; //是否存在分配订单的功能权限
            bool updateName = false; //是否存在编辑订单的功能权限
            bool deleteName = false; //是否存在删除订单的功能权限

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);
            List<model.apiOrderModel> list = new List<model.apiOrderModel>();
            StringBuilder s = new StringBuilder();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            string[] ssName = apiorderBll.getDataName("apiOrder");
            string[] ssName1 = apiorderBll.getDataName("apiOrderPayDetails");
            //DataTable dt = new DataTable();
            //DataTable dt = apiorderBll.getOrderMsg( pageIndex, pageSize);
            int count = 0;
            DataTable dt = apiorderBll.getOrderMsg(dic, pageIndex, pageSize, out count);
            //int count = apiorderBll.getDataCount();

            int pageCount = count / pageSize;
            pageCount = count % pageSize > 0 ? pageCount + 1 : pageCount;

            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName); //获取有权限的字段

            splitSingle = ph.isFunPermisson(roleId, menuId, funName.splitSingle); //是否存在分配订单的功能权限
            updateName = ph.isFunPermisson(roleId, menuId, funName.updateName); //是否存在编辑订单的功能权限
            deleteName = ph.isFunPermisson(roleId, menuId, funName.deleteName); //是否存在删除订单的功能权限

            #region TABLE添加
            if (ph.isFunPermisson(roleId, menuId, funName.addName))
            {
                s.Append("<tr>");
                s.Append("<th colspan='50' class='mytableadd'>");
                s.Append("<div style='padding-top: 2px;'>");
                //s.Append("<input type='button' value='展开' onclick='ShowAllTable(this)'/>");
                //s.Append("<a href='#'  onclick='javascript: showDiv()' >添加</a>");

                s.Append("</div>");
                s.Append("</th>");
                s.Append("<tr>");
            }
            #endregion

            #region TABLE表头
            s.Append("<tr>");
            s.Append("<th><input type='checkbox' id='selectAll' onclick='selectAll()' ></th>");
            s.Append("<th>详情</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {

                    if (ssName[z] == "orderId")
                    {
                        s.Append("<th>"); s.Append("订单编号"); s.Append("</th>");
                    }
                    if (ssName[z] == "realName")
                    { s.Append("<th>"); s.Append("姓名"); s.Append("</th>"); }
                    if (ssName[z] == "provinceId")
                    { s.Append("<th>"); s.Append("省"); s.Append("</th>"); }
                    if (ssName[z] == "cityId")
                    { s.Append("<th>"); s.Append("市"); s.Append("</th>"); }
                    if (ssName[z] == "district")
                    { s.Append("<th>"); s.Append("县"); s.Append("</th>"); }
                    if (ssName[z] == "buyNameAddress")
                    { s.Append("<th>"); s.Append("地址"); s.Append("</th>"); }
                    if (ssName[z] == "postcode")
                    { s.Append("<th>"); s.Append("邮编"); s.Append("</th>"); }
                    if (ssName[z] == "phone")
                    { s.Append("<th>"); s.Append("手机"); s.Append("</th>"); }
                    if (ssName[z] == "orderMsg")
                    { s.Append("<th>"); s.Append("订单备注"); s.Append("</th>"); }
                    if (ssName[z] == "itemPrice")
                    { s.Append("<th>"); s.Append("总金额"); s.Append("</th>"); }
                    if (ssName[z] == "orderStatus")
                    { s.Append("<th>"); s.Append("订单状态"); s.Append("</th>"); }
                    if (ssName[z] == "deliveryPrice")
                    { s.Append("<th>"); s.Append("快递费"); s.Append("</th>"); }
                    if (ssName[z] == "favorablePrice")
                    { s.Append("<th>"); s.Append("优惠额"); s.Append("</th>"); }
                    if (ssName[z] == "taxPrice")
                    { s.Append("<th>"); s.Append("税费"); s.Append("</th>"); }
                    if (ssName[z] == "orderPrice")
                    { s.Append("<th>"); s.Append("订单金额"); s.Append("</th>"); }
                    if (ssName[z] == "paidPrice")
                    { s.Append("<th>"); s.Append("实付金额"); s.Append("</th>"); }
                    if (ssName[z] == "isPay")
                    {
                        s.Append("<th>"); s.Append("支付状态"); s.Append("</th>");
                    }
                    //if (ssName[z] == "payTime")
                    //    s.Append("支付时间");
                    //if (ssName[z] == "payOuterId")
                    //    s.Append("支付流水号");
                    if (ssName[z] == "createTime")
                    { s.Append("<th>"); s.Append("创建时间"); s.Append("</th>"); }
                    if (ssName[z] == "invoiceType")
                    { s.Append("<th>"); s.Append("发票类型"); s.Append("</th>"); }
                    if (ssName[z] == "invoiceTitle")
                    { s.Append("<th>"); s.Append("发票抬头"); s.Append("</th>"); }
                    if (ssName[z] == "Def1")
                    { s.Append("<th>"); s.Append("身份证"); s.Append("</th>"); }
                    if (ssName[z] == "Def2")
                    { s.Append("<th>"); s.Append("自提"); s.Append("</th>"); }
                    //if (ssName[z] == "Def3")
                    //    s.Append("默认3");
                    //if (ssName[z] == "Def4")
                    //    s.Append("默认4");
                    //if (ssName[z] == "Def5")
                    //    s.Append("默认5");


                }
            }

            for (int z = 0; z < ssName1.Length; z++)
            {
                if (ss.Contains(ssName1[z]))
                {

                    if (ssName1[z] == "payMentName")
                    { s.Append("<th>"); s.Append("支付方式"); s.Append("</th>"); }
                    if (ssName1[z] == "payPlatform")
                    { s.Append("<th>"); s.Append("支付平台"); s.Append("</th>"); }
                    if (ssName1[z] == "payId")
                    { s.Append("<th>"); s.Append("内部流水号"); s.Append("</th>"); }
                    if (ssName1[z] == "payOuterId")
                    { s.Append("<th>"); s.Append("外部流水号"); s.Append("</th>"); }
                    if (ssName1[z] == "payPrice")
                    { s.Append("<th>"); s.Append("支付金额"); s.Append("</th>"); }
                    if (ssName1[z] == "payTime")
                    { s.Append("<th>"); s.Append("支付时间"); s.Append("</th>"); }
                    if (ssName1[z] == "sellerAccount")
                    { s.Append("<th>"); s.Append("卖家支付账号"); s.Append("</th>"); }
                    if (ssName1[z] == "buyerAccount")
                    { s.Append("<th>"); s.Append("买家支付账号"); s.Append("</th>"); }

                }
            }

            s.Append("<th>编辑</th><th>删除</th><th>供应商</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count < 1)
            {
                s.Append("<tr><td colspan='50' style='text-align:center;font-size:12px;color:red;'>本次搜索暂无数据!</td></tr>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr class='trSourceOrder' ondblclick=detailstable(this)>");
                s.Append("<td><input type='checkbox' name='ckbTable' ></td>");
                s.Append("<td onclick='showTable(this)'>+</td>");//11111111111111111111111

                for (int j = 0; j < ss.Length; j++)
                {
                    if (ss[j].ToLower() == "id")
                    {
                        s.Append("<td>");
                        s.Append("<label id='lblId'>" + dt.Rows[i][ss[j]].ToString() + "</label>");
                        s.Append("</td>");
                    }
                    else if (ss[j].ToLower() == "ispay")
                    {
                        s.Append("<td>");
                        if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("已支付");
                        }
                        else
                        {
                            s.Append("未支付");
                        }

                        s.Append("</td>");
                    }
                    else if (ss[j].ToLower() == "orderstatus")
                    {
                        s.Append("<td>");
                        if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("待确认");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "2")
                        {
                            s.Append("已确认");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "3")
                        {
                            s.Append("待发货");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "4")
                        {
                            s.Append("已发货");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "5")
                        {
                            s.Append("交易成功");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "6")
                        {
                            s.Append("通关异常");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "7")
                        {
                            s.Append("通关成功");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "11")
                        {
                            s.Append("退货");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "12")
                        {
                            s.Append("取消");
                        }
                        s.Append("</td>");

                    }
                    else if (ss[j].ToLower() == "payprice" || ss[j].ToLower() == "itemprice" || ss[j].ToLower() == "deliveryprice" || ss[j].ToLower() == "favorableprice" || ss[j].ToLower() == "taxprice" || ss[j].ToLower() == "orderprice" || ss[j].ToLower() == "paidprice")
                    {
                        s.Append("<td>");
                        s.Append(helpcommon.ParmPerportys.GetDecimalParms(dt.Rows[i][ss[j]].ToString()).ToString("F2"));
                        s.Append("</td>");
                    }
                    else
                    {
                        s.Append("<td>");
                        s.Append(dt.Rows[i][ss[j]].ToString());
                        s.Append("</td>");
                    }
                }

                #region 编辑
                s.Append("<td>");
                if (updateName)
                {
                    s.Append("<a href='#' onclick='userEdit(" + dt.Rows[i][0].ToString() + ")'>编辑</a>");
                }
                else
                {
                    s.Append("<a href='#'>无编辑权限</a>");
                }
                s.Append("</td>");
                #endregion

                #region 删除
                s.Append("<td>");
                if (deleteName)
                {
                    s.Append("<a href='#' onclick='del(" + dt.Rows[i][0].ToString() + ")'>删除</a>");
                }
                else
                {
                    s.Append("<a href='#'>无删除权限</a>");
                }

                s.Append("</td>");
                #endregion

                #region 分单
                //def2   是否已分配(0未分配，1分配)
                s.Append("<td>");
                if (dt.Rows[i]["isPay"].ToString() == "1")
                {
                    if (splitSingle)
                    {
                        if (dt.Rows[i]["def2"].ToString() != "1")
                        {
                            if (dt.Rows[i]["orderStatus"].ToString() != "1")
                            {
                                s.Append("<a href='#' id='cdLink'>禁止分配</a>");
                            }
                            else
                            {
                                s.Append("<a href='#' id='cdLink' onclick=\"javascript:splitSingle('" + dt.Rows[i]["orderId"].ToString() + "');\">分配供应商</a>"); //分配供应商（开始决定一个订单只能存在一件货，先改成一个订单可以存在多件货）
                            }
                        }
                        else if (dt.Rows[i]["def2"].ToString() == "1")
                        {
                            s.Append("<a href='#' id='cdLink'>分配完成</a>");
                        }
                    }
                    else
                    {
                        s.Append("<a href='#'>无分配权限</a>");
                    }
                }
                else
                {
                    s.Append("<a href='#' id='cdLink'>禁止分配</a>");
                }
                s.Append("</td>");
                #endregion
                s.Append("</tr>");
            }
            #endregion

            #region 分页
            s.Append("-----");
            s.Append(pageCount + "-----" + count);
            #endregion

            apiorderBll = null;

            return s.ToString();
        }


        /// <summary>
        /// 获取订单详情列表
        /// </summary>
        /// <returns></returns>
        public string getOrderDetails()
        {
            StringBuilder s = new StringBuilder();
            string orderId = Request.Form["orderId"].ToString(); //订单ID
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId); //角色
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"].ToString()); //菜单
            try
            {
                bll.apiorderbll apiorderBll = new bll.apiorderbll();
                PublicHelpController ph = new PublicHelpController();
                DataTable dt = apiorderBll.getOrderDetailsMsg(orderId);
                string[] ssName = apiorderBll.getDataName("apiOrderDetails");
                string[] ssNameParent = apiorderBll.getDataName("apiOrder"); //主菜单显示字段
                string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectChildOrder);



                s.Append("<tr>");
                s.Append("<td colspan='" + ssNameParent.Length + 2 + "'>");
                s.Append("<table class='details' cellspacing='0' cellpadding='0'>");

                if (dt != null)
                {
                    #region 表头
                    s.Append("<tr>");
                    for (int z = 0; z < ssName.Length; z++)
                    {
                        if (ss.Contains(ssName[z]))
                        {
                            s.Append("<td>");
                            if (ssName[z] == "detailsOrderId")
                                s.Append("子订单编号");
                            if (ssName[z] == "detailsScode")
                                s.Append("商品货号");
                            if (ssName[z] == "detailsColor")
                                s.Append("颜色");
                            if (ssName[z] == "detailsImg")
                                s.Append("图片");
                            if (ssName[z] == "detailsItemPrice")
                                s.Append("成交价");
                            if (ssName[z] == "detailsTaxPrice")
                                s.Append("税金");
                            if (ssName[z] == "detailsSaleCount")
                                s.Append("数量");
                            if (ssName[z] == "detailsDeliveryPrice")
                                s.Append("快递费");
                            if (ssName[z] == "detailsStatus")
                                s.Append("订单状态");
                            if (ssName[z] == "detailsTime")
                                s.Append("下单时间");
                            if (ssName[z] == "detailsPayTime")
                                s.Append("付款时间");
                            if (ssName[z] == "detailsEditTime")
                                s.Append("编辑时间");
                            if (ssName[z] == "detailsSendTime")
                                s.Append("发货时间");
                            if (ssName[z] == "detailsSucessTime")
                                s.Append("结束时间");
                            if (ssName[z] == "detailsSplit")
                                s.Append("分配状态");
                            if (ssName[z] == "Def1")
                                s.Append("默认1");
                            if (ssName[z] == "Def2")
                                s.Append("默认2");
                            if (ssName[z] == "Def3")
                                s.Append("默认3");
                            if (ssName[z] == "Def4")
                                s.Append("默认4");
                            if (ssName[z] == "Def5")
                                s.Append("默认5");

                            s.Append("</td>");
                        }
                    }

                    //s.Append("<td>分配</td><td>编辑</td><td>删除</td>");
                    s.Append("</tr>");
                    #endregion

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
                        s.Append("<tr>");
                        for (int j = 0; j < ss.Length; j++)
                        {
                            if (ss[j].ToLower() == "id")
                            {
                                s.Append("<td>");
                                s.Append("<label id='lblId'>" + dt.Rows[i][ss[j]].ToString() + "</label>");
                                s.Append("</td>");
                            }
                            else if (ss[j].ToLower().Contains("price"))
                            {
                                s.Append("<td>");
                                s.Append("<label>" + Convert.ToDecimal(dt.Rows[i][ss[j]].ToString() == "" ? "0" : dt.Rows[i][ss[j]]).ToString("f2") + "</label>");
                                s.Append("</td>");
                            }
                            else if (ss[j].ToLower() == "detailsimg")
                            {
                                s.Append("<td>");
                                s.Append("<img src=\"" + dt.Rows[i][ss[j]].ToString() + "\" style='height:60px;' onerror='errorImg(this)'  />");
                                s.Append("</td>");
                            }
                            else
                            {
                                if (ss[j].ToLower() == "detailssplit" && (dt.Rows[i][ss[j]].ToString() == "1" || dt.Rows[i][ss[j]].ToString() == "0")) //订单分配
                                {
                                    if (dt.Rows[i][ss[j]].ToString() == "1")
                                    {
                                        s.Append("<td class='orderSplitStates'>");
                                        s.Append("已分配");
                                        s.Append("</td>");
                                    }
                                    else
                                    {
                                        s.Append("<td>");
                                        s.Append("未分配");
                                        s.Append("</td>");
                                    }
                                }
                                else if (ss[j].ToLower() == "detailsstatus") //订单详情
                                {
                                    #region 订单详情值
                                    s.Append("<td>");
                                    if (dt.Rows[i][ss[j]].ToString() == "1")
                                    {
                                        s.Append("待确认");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "2")
                                    {
                                        s.Append("已确认");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "3")
                                    {
                                        s.Append("待发货");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "4")
                                    {
                                        s.Append("已发货");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "5")
                                    {
                                        s.Append("交易成功");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "6")
                                    {
                                        s.Append("通关异常");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "7")
                                    {
                                        s.Append("通关成功");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "11")
                                    {
                                        s.Append("退货");
                                    }
                                    else if (dt.Rows[i][ss[j]].ToString() == "12")
                                    {
                                        s.Append("取消");
                                    }
                                    s.Append("</td>");
                                    #endregion
                                }
                                else
                                {
                                s.Append("<td>");
                                    s.Append(dt.Rows[i][ss[j]].ToString());
                                    s.Append("</td>");
                                }
                            }
                        }

                        s.Append("</tr>");
                    }
                }
            }
            catch (Exception ex) { return ex.Message; }

            s.Append("</table>");
            s.Append("</td>");
            s.Append("</tr>");

            return s.ToString();
        }


        /// <summary>
        /// 分配供应商
        /// </summary>
        /// <returns></returns>
        public string getSplitSingle()
        {
            //def2   是否已分配(0未分配，1分配)

            string s = string.Empty;
            string splitResult = string.Empty;                                                        //分配订单返回的结果
            string repeatOrderIds = string.Empty;                                                     //重复分配的订单（表示已分配成功的订单）
            //string noOrderIds = string.Empty;                                                         //禁止分配的订单(表示订单状态非待确认状态)
            OrderHelper oh = new OrderHelper();                                                       //订单类
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            List<string> list = new List<string>();                                                   //接收订单编号（过滤空值）
            string orderId = Request.Form["orderId"] ?? string.Empty;                                 //订单ID
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ',');                        //分割订单ID，查看是否多个订单同时分配

            for (int i = 0; i < orderIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(orderIds[i]))
                {
                    list.Add(orderIds[i]);
                }
            }

            #region 过滤已分配完成的订单
            List<model.apiOrder> listApiOrder = apiorderBll.getOrderMsg(orderIds);                //根据主订单获取子订单商品

            for (int m = listApiOrder.Count-1; m >= 0; m--)
            {
                if (listApiOrder[m].def2 == "1" || listApiOrder[m].orderStatus != 1)                //过滤已分配成功的订单（表示已分配成功的订单）和禁止分配的订单(表示订单状态非待确认状态)
                {
                    repeatOrderIds += listApiOrder[m].orderId + ",";   
                    listApiOrder.RemoveAt(m);
                }
            }
            if (listApiOrder.Count <= 0)
            {
                return "订单(" + repeatOrderIds + ")不允许重复分配";
            }
            #endregion

            if (list.Count > 1)
            {
                #region 分配多个订单
                for (int m = 0; m < listApiOrder.Count; m++)
                {
                    DataTable dt = apiorderBll.getOrderDetailsMsg(listApiOrder[m].orderId);           //根据主订单获取子订单商品

                    if (dt != null)
                    {
                        string[] scode = new string[dt.Rows.Count];                                   //一个订单是否有多个商品(获取一个订单中有几个商品)
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            scode[i] = dt.Rows[i]["detailsScode"].ToString();                         //得到子订单中的商品货号
                        }
                        s += oh.getBalance(listApiOrder[m].orderId, scode) + "\r\n";
                    }
                }
                #endregion
            }
            else
            {
                #region 分配单个订单
                orderId = orderId.Trim(',');
                DataTable dt = apiorderBll.getOrderDetailsMsg(orderId);                               //根据主订单获取子订单商品

                if (dt != null)
                {
                    string[] scode = new string[dt.Rows.Count];                                       //一个订单是否有多个商品(获取一个订单中有几个商品)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        scode[i] = dt.Rows[i]["detailsScode"].ToString();                             //得到子订单中的商品货号
                    }
                    s = oh.getBalance(orderId, scode);
                }
                #endregion
            }

            if (!string.IsNullOrWhiteSpace(repeatOrderIds))
            {
                s += "\r\n订单(" + repeatOrderIds + ")不允许重复分配";
            }

            apiorderBll = null;
            return s;
        }


        /// <summary>
        /// 编辑信息
        /// </summary>
        public string EditApiOrder()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"].ToString());
            string OrderId = Request.Form["OrderId"].ToString();
            StringBuilder s = new StringBuilder();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            PublicHelpController ph = new PublicHelpController();
            DataTable dt = apiorderBll.getOrderDetailsMsg(OrderId);
            string[] ssName = apiorderBll.getDataName("apiOrderDetails");
            string[] ssNameParent = apiorderBll.getDataName("apiOrder"); //主菜单显示字段
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectChildOrder);
            if (dt != null)
            {
                #region 表头
                s.Append("<tr>");
                for (int z = 0; z < ssName.Length; z++)
                {
                    if (ss.Contains(ssName[z]))
                    {
                        s.Append("<th>");
                        if (ssName[z] == "detailsOrderId")
                            s.Append("子订单ID");
                        if (ssName[z] == "detailsScode")
                            s.Append("商品货号");
                        if (ssName[z] == "detailsColor")
                            s.Append("颜色");
                        if (ssName[z] == "detailsImg")
                            s.Append("图片");
                        if (ssName[z] == "detailsItemPrice")
                            s.Append("商品售价");
                        if (ssName[z] == "detailsTaxPrice")
                            s.Append("税费金额");
                        if (ssName[z] == "detailsSaleCount")
                            s.Append("销售数量");
                        if (ssName[z] == "detailsDeliveryPrice")
                            s.Append("快递费用");
                        if (ssName[z] == "detailsStatus")
                            s.Append("详情状态");
                        if (ssName[z] == "detailsTime")
                            s.Append("下单时间");
                        if (ssName[z] == "detailsPayTime")
                            s.Append("付款时间");
                        if (ssName[z] == "detailsEditTime")
                            s.Append("编辑时间");
                        if (ssName[z] == "detailsSendTime")
                            s.Append("发货时间");
                        if (ssName[z] == "detailsSucessTime")
                            s.Append("结束时间");
                        if (ssName[z] == "detailsSplit")
                            s.Append("分配状态");
                        if (ssName[z] == "Def1")
                            s.Append("默认1");
                        if (ssName[z] == "Def2")
                            s.Append("默认2");
                        if (ssName[z] == "Def3")
                            s.Append("默认3");
                        if (ssName[z] == "Def4")
                            s.Append("默认4");
                        if (ssName[z] == "Def5")
                            s.Append("默认5");

                        s.Append("</th>");
                    }
                }
                s.Append("</tr>");
                #endregion

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<tr>");
                    for (int j = 0; j < ss.Length; j++)
                    {
                        if (ss[j].ToLower() == "id")
                        {
                            s.Append("<td>");
                            s.Append("<label id='lblId'>" + dt.Rows[i][ss[j]].ToString() + "</label>");
                            s.Append("</td>");
                        }
                        else if (ss[j].ToLower().Contains("price"))
                        {
                            s.Append("<td>");
                            s.Append("<label>" + Convert.ToDecimal(dt.Rows[i][ss[j]]).ToString("f2") + "</label>");
                            s.Append("</td>");
                        }
                        else if (ss[j].ToLower() == "detailsimg")
                        {
                            s.Append("<td>");
                            s.Append("<img src=\"" + dt.Rows[i][ss[j]].ToString() + "\" style='height:60px;' onerror='errorImg(this)'  />");
                            s.Append("</td>");
                        }
                        else
                        {
                            if (ss[j].ToLower() == "detailssplit" && (dt.Rows[i][ss[j]].ToString() == "1" || dt.Rows[i][ss[j]].ToString() == "0"))
                            {
                                if (dt.Rows[i][ss[j]].ToString() == "1")
                                {
                                    s.Append("<td class='orderSplitStates'>");
                                    s.Append("已分配");
                                    s.Append("</td>");
                                }
                                else
                                {
                                    s.Append("<td>");
                                    s.Append("未分配");
                                    s.Append("</td>");
                                }
                            }
                            else
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i][ss[j]].ToString());
                                s.Append("</td>");
                            }
                        }
                    }
                    s.Append("</tr>");
                }
            }
            s.Append("-*-");
            DataTable dt1 = DbHelperSQL.Query(@"select * from apiOrderRemark a left join users b on a.UserId=b.Id where a.OrderId='" + OrderId + "'").Tables[0];
            if (dt1.Rows.Count > 0)
            {
                s.Append("<table class='mytable'>");
                s.Append("<tr><th style='width:125px'>操作时间</th><th>备注内容</th><th style='width:55px'>操作人</th>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<th style='width:35px'>编辑</th>");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<th style='width:35px'>删除</th>");
                }
                s.Append("</tr>");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    s.Append("<tr><td>" + dt1.Rows[i]["Edittime"] + "</td><td class='tdRemark'>" + dt1.Rows[i]["Remark"] + "</td><td>" + dt1.Rows[i]["userRealName"] + "</td>");
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        if (userInfo.User.Id.ToString() == dt1.Rows[i]["UserId"].ToString())
                        {
                            s.Append("<td class='EditRemark'><a href='#' title='" + dt1.Rows[i]["Id"] + "' >编辑</a></td>");
                        }
                        else
                        {
                            s.Append("<td></td>");
                        }

                    }
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='DeleteRemark(\"" + dt1.Rows[i]["Id"] + "\")'>删除</a></td>");
                    }
                    s.Append("</tr>");
                }
                s.Append("</table>");
            }

            return s.ToString();
        }

        /// <summary>
        /// 添加备注
        /// </summary>
        public string UpdateApiOrder()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string OrderId = Request.Form["OrderId"].ToString();
            string Remark = Request.Form["Remark"].ToString();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("OrderId", OrderId);
            Dic.Add("Remark", Remark);
            Dic.Add("Edittime", DateTime.Now.ToString());
            Dic.Add("UserId", roleId.ToString());
            return apiorderBll.UpdateApiOrder(Dic);
        }

        /// <summary>
        /// 删除备注
        /// </summary>
        public string DeleteRemark()
        {
            string Id = Request.QueryString["Id"].ToString();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            return apiorderBll.DeleteRemark(Id);
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        public string EditRemark()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string Id = Request.Form["Id"].ToString();
            string Remark = Request.Form["Remark"].ToString();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", Id);
            Dic.Add("Remark", Remark);
            Dic.Add("Edittime", DateTime.Now.ToString());
            Dic.Add("UserId", roleId.ToString());
            return apiorderBll.EditRemark(Dic);
        }
    }
}
