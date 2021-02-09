/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ApiOrderController
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.web.Controllers
    * 文 件 名：       ApiOrderController
    * 创建时间：       2015/05/04 14:22:40
    * 作    者：       lcg
    * 说    明：       源头订单
    * 修改时间：
    * 修 改 人：
*************************************************************************************/

using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using pbxdata.bll;
using System.Transactions;

namespace pbxdata.web.Controllers
{
    public class SourceOrderController : BaseController
    {

        /// <summary>
        /// 获取源头订单
        /// </summary>
        /// <returns></returns>
        public ActionResult getSourceOrder()
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


        ///// <summary>
        ///// 获取订单列表
        ///// </summary>
        //public string GetSearchlist()
        //{
        //    string s = string.Empty;
        //    int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
        //    int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
        //    PublicHelpController ph = new PublicHelpController();
        //    string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
        //    foreach (var item in ss)
        //    {
        //        if (item == "orderId")
        //        {
        //            s += " 订单ID:<input type='text' id='txtorderId' />";
        //            continue;
        //        }
        //        else if (item == "newStatus")
        //        {
        //            s += " 订单状态:<select id='selnewStatus'><option value=''>请选择</option><option value='1'>待确认</option><option value='2'>已确认</option><option value='3'>待发货</option>" +
        //                 "<option value='4'>发货</option><option value='5'>交易成功</option><option value='6'>通关异常</option><option value='7'>通关成功</option>" +
        //                 "<option value='11'>退货</option><option value='12'>取消</option></select>";
        //            continue;
        //        }
        //        else if (item == "createTime")
        //        {
        //            s += " 创建时间:<input type='date' id='txtcreateTime' />";
        //            continue;
        //        }
        //        else if (item == "editTime")
        //        {
        //            s += " 编辑时间:<input type='date' id='txteditTime' />";
        //            continue;
        //        }
        //        else if (item == "showStatus")
        //        {
        //            s += " 开放状态:<select id='selshowStatus'><option value=''>请选择</option><option value='1'>开放</option><option value='0'>未开放</option></select>";
        //            continue;
        //        }
        //        else if (item == "sendSource")
        //        {
        //            s += " 供应商:<select id='selsendSource'>" + GetVencode() + "</select>";
        //            continue;
        //        }

        //    }
        //    if (ph.isFunPermisson(roleId, menuId, funName.ciqOrder))
        //    {
        //        s += "  <br/>商检:<select id='selSJstatus'>" + GetciqOrderlist() + "</select>";
        //        s += "  海关:<select id='selHGstatus'>" + GetciqOrderlist() + "</select>";
        //        s += "  物流:<select id='selBBCstatus'>" + GetciqOrderlist() + "</select>";
        //    }
        //    s += "<input type='button' value=' 查   询 ' onclick='OnSearch()' style='width:80px;margin-left:10px;' />";
        //    return s;
        //}



        public string GetciqOrderlist()
        {
            //0失败，1成功，2未上传,3上传成功
            string s = string.Empty;
            s += "<option value='-1'>请选择</option>";
            s += "<option value='0'>失败</option>";
            s += "<option value='1'>成功</option>";
            s += "<option value='2'>未上传</option>";
            return s;
        }



        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <returns></returns>
        public string getData()
        {

            #region 搜索条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合

            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件
            string orderId = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "") ?? string.Empty; //订单ID
            string newStatus = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "") ?? string.Empty; //订单状态
            string createTime = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "") ?? string.Empty; //创建时间
            string editTime = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "") ?? string.Empty; //编辑时间
            string showStatus = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "") ?? string.Empty; //开放状态
            string sendSource = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "") ?? string.Empty; //供应商
            string SJstatus = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "") ?? string.Empty; //商检状态
            string HGstatus = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "") ?? string.Empty; //海关状态
            string BBCstatus = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //物流状态
            string Paystatus = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //支付状态
            


            #region 为了处理null问题
            sendSource = string.IsNullOrWhiteSpace(sendSource) == true ? "" : sendSource.Replace("null","") ;
            SJstatus = string.IsNullOrWhiteSpace(SJstatus) == true ? "" : SJstatus.Replace("null", "");
            HGstatus = string.IsNullOrWhiteSpace(HGstatus) == true ? "" : HGstatus.Replace("null", "");
            BBCstatus = string.IsNullOrWhiteSpace(BBCstatus) == true ? "" : BBCstatus.Replace("null", "");
            Paystatus = string.IsNullOrWhiteSpace(Paystatus) == true ? "" : Paystatus.Replace("null", "");
            #endregion


            orderId = orderId == "\'\'" ? string.Empty : orderId;
            newStatus = newStatus == "\'\'" ? string.Empty : newStatus;
            createTime = createTime == "\'\'" ? string.Empty : createTime;
            editTime = editTime == "\'\'" ? string.Empty : editTime;
            showStatus = showStatus == "\'\'" ? string.Empty : showStatus;
            sendSource = sendSource == "\'\'" ? string.Empty : sendSource;
            SJstatus = SJstatus == "\'\'" ? string.Empty : SJstatus;
            HGstatus = HGstatus == "\'\'" ? string.Empty : HGstatus;
            BBCstatus = BBCstatus == "\'\'" ? string.Empty : BBCstatus;
            Paystatus = Paystatus == "\'\'" ? string.Empty : Paystatus;

            dic.Add("orderId", orderId);
            dic.Add("newStatus", newStatus);
            dic.Add("createTime", createTime);
            dic.Add("editTime", editTime);
            dic.Add("showStatus", showStatus);
            dic.Add("sendSource", sendSource);
            dic.Add("SJstatus", SJstatus);
            dic.Add("HGstatus", HGstatus);
            dic.Add("BBCstatus", BBCstatus);
            dic.Add("Paystatus", Paystatus);
            #endregion

            bool checkOrderShow = false; //是否存在审核开放功能权限
            bool sendOrderState = false; //是否存在确认订单状态功能权限
            bool cancelOrder = false; //是否存在取消订单功能权限


            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);
            List<model.apiSendOrder> list = new List<model.apiSendOrder>();
            StringBuilder s = new StringBuilder();
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
            int count;
            //DataTable dt = sourceorderBll.getSourceParentOrderId(pageIndex, pageSize);
            DataTable dt = sourceorderBll.getSourceParentOrderId(dic, pageIndex, pageSize, out count);
            int pageCount = count / pageSize;
            pageCount = count % pageSize > 0 ? pageCount + 1 : pageCount;
            PublicHelpController ph = new PublicHelpController();


            checkOrderShow = ph.isFunPermisson(roleId, menuId, funName.checkOrderShow);
            sendOrderState = ph.isFunPermisson(roleId, menuId, funName.sendOrderState);
            cancelOrder = ph.isFunPermisson(roleId, menuId, funName.cancelOrder);


            #region TABLE添加
            s.Append("<tr>");
            s.Append("<th colspan='50' class='mytableadd'>");
            s.Append("<div style='padding-top: 2px;'>");
            //s.Append("<input type='button' value='展开' onclick='ShowAllTable(this)'/>");
            s.Append("</div>");
            s.Append("</th>");
            s.Append("<tr>");
            #endregion

            #region TABLE表头
            s.Append("<tr>");
            s.Append("<th width='20'><input type='checkbox' id='selectAll' onclick='selectAll()' /></th>");
            s.Append("<th width='20'>详情</th>");
            s.Append("<th>订单编号</th>");
            s.Append("<th>支付编号</th>");
            s.Append("<th>订单状态</th>");
            s.Append("<th>开放</th><th>商检报备</th><th>海关报备</th><th>物流报备</th><th>支付报备</th>");

            s.Append("<th style='width:80px;text-align:center;'>编辑订单</th>");
            s.Append("<th style='width:80px;text-align:center;'>查看买家</th>");
            s.Append("<th>审核</th>");
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
                s.Append("<td align='left'><input type='checkbox' name='checkbox1' value='" + dt.Rows[i]["orderId"].ToString() + "' /></td>");
                s.Append("<td style='width:20px;cursor:pointer;' onclick='getDataSourceOrder(this)'>+</td>");
                s.Append("<td>" + dt.Rows[i]["orderId"].ToString() + "</td>");
                s.Append("<td class='payId'>" + dt.Rows[i]["payId"].ToString() + "</td>");
                string newstatus = dt.Rows[i]["newStatus"].ToString();

                #region 订单状态
                if (newstatus == "1")
                {
                    s.Append("<td>待确认</td>");
                }
                else if (newstatus == "2")
                {
                    s.Append("<td>已确认</td>");
                }
                else if (newstatus == "3")
                {
                    s.Append("<td>待发货</td>");
                }
                else if (newstatus == "4")
                {
                    s.Append("<td >已发货</td>");
                }
                else if (newstatus == "5")
                {
                    s.Append("<td >交易成功</td>");
                }
                else if (newstatus == "6")
                {
                    s.Append("<td >海关异常</td>");
                }
                else if (newstatus == "7")
                {
                    s.Append("<td >海关通关</td>");
                }
                else if (newstatus == "11")
                {
                    s.Append("<td >客户退单</td>");
                }
                else if (newstatus == "12")
                {
                    s.Append("<td >订单取消</td>");
                }
                #endregion

                #region 审核开放(是否开放给供应商查看,0为未开放，1为开放)
                s.Append("<td>");
                if (dt.Rows[i]["showstatus"].ToString() == "0")
                {
                    s.Append("未开放");
                }
                else
                {
                    s.Append("已开放");
                }
                s.Append("</td>");
                #endregion

                #region 报备状态

                #region 三个订单报备状态（商检，海关，联邦）
                bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
                //DataTable dtStatus = ordercustomsresultBll.getOrderReportStatus(dt.Rows[i]["orderId"].ToString());
                DataTable dtStatus = ordercustomsresultBll.getOrderReportStatus(dt.Rows[i]["payId"].ToString()); //以支付单号为订单号报关物流、商检、海关
                string sjStatus = dtStatus.Rows.Count > 0 ? dtStatus.Rows[0]["SJstatus"].ToString() : "2";
                string hgStatus = dtStatus.Rows.Count > 0 ? dtStatus.Rows[0]["HGstatus"].ToString() : "2";
                string bbcStatus = dtStatus.Rows.Count > 0 ? dtStatus.Rows[0]["BBCstatus"].ToString() : "2";
                #endregion

                #region 支付单报备状态
                bll.paycustomsresultbll paycustomsresultBll = new paycustomsresultbll();
                string payStatus = paycustomsresultBll.getPayStatus(dt.Rows[i]["payId"].ToString());
                #endregion

                #region 商检报备(0失败，1成功，2未上传)
                s.Append("<td>");
                if (sjStatus == "0")
                {
                    s.Append("失败");
                }
                else if (sjStatus == "1")
                {
                    s.Append("成功");
                }
                else if (sjStatus == "2")
                {
                    s.Append("未上传");
                }
                s.Append("</td>");
                #endregion

                #region 海关报备(0失败，1成功，2未上传)
                s.Append("<td>");
                if (hgStatus == "0")
                {
                    s.Append("失败");
                }
                else if (hgStatus == "1")
                {
                    s.Append("成功");
                }
                else if (hgStatus == "2")
                {
                    s.Append("未上传");
                }
                s.Append("</td>");
                #endregion

                #region 联邦报备(0失败，1成功，2未上传)
                s.Append("<td>");
                if (bbcStatus == "0")
                {
                    s.Append("失败");
                }
                else if (bbcStatus == "1")
                {
                    s.Append("成功");
                }
                else if (bbcStatus == "2")
                {
                    s.Append("未上传");
                }
                s.Append("</td>");
                #endregion

                #region 支付报备(fail失败，success成功)
                s.Append("<td>");
                if (string.IsNullOrWhiteSpace(payStatus))
                {
                    s.Append("未上传");
                }
                else if (payStatus.ToUpper() == "SUCCESS")
                {
                    s.Append("成功");
                }
                else if (payStatus.ToUpper() == "FAIL")
                {
                    s.Append("失败");
                }
                s.Append("</td>");
                #endregion

                #endregion

                s.Append("<td style='width:80px;text-align:center;'><a href='#' onclick='EditOrder(" + dt.Rows[i]["orderId"].ToString() + ")'>编辑订单</a></td>");
                s.Append("<td style='width:80px;text-align:center;'><a href='#' onclick='showDivEdit();selectBuyer(" + dt.Rows[i][0].ToString() + ")'>查看买家</a></td>");

                s.Append("<td class='tdOpera'>");
                #region 审核开放(是否开放给供应商查看,0为未开放，1为开放)

                if (true)
                {
                    if (dt.Rows[i]["showstatus"].ToString() == "0")
                    {
                        s.Append("<a href='#' onclick='javascript: checkOrder(" + dt.Rows[i]["orderId"].ToString() + ")'>审核开放</a>");
                    }
                }
                
                #endregion

                #region 确认订单状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)

                if (true)
                {
                    if (helpcommon.ParmPerportys.GetNumParms(dt.Rows[i]["newStatus"].ToString()) == 2)
                    {
                        s.Append("<a href='#' onclick='javascript: sendOrder(" + dt.Rows[i]["orderId"].ToString() + ")'>审核通知发货</a>");
                    }
                }

                #endregion

                #region 取消订单(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)

                if (true)
                {
                    if (helpcommon.ParmPerportys.GetNumParms(dt.Rows[i]["newStatus"].ToString()) < 4)
                    {
                        s.Append("<a href='#' onclick='javascript: cancelOrder(" + dt.Rows[i]["orderId"].ToString() + ")'>审核取消订单</a>");
                    }
                }

                #endregion

                s.Append("</td>");

                s.Append("</tr>");
            }


            #region 分页
            s.Append("-----");
            s.Append(pageCount + "-----" + count);
            #endregion
            #endregion

            return s.ToString();
        }


        /// <summary>
        /// 获取订单分配数据
        /// </summary>
        /// <returns></returns>
        public string getDataSourceOrder()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]);
            List<model.apiSendOrder> list = new List<model.apiSendOrder>();
            StringBuilder s = new StringBuilder();
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();



            string[] ssName = sourceorderBll.getDataName("apiSendOrder");
            //DataTable dt = new DataTable();
            DataTable dt = sourceorderBll.getData(orderId);

            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);

            s.Append("<tr>");
            s.Append("<td colspan='99'>");
            s.Append("<table id='details' cellspacing='0' cellpadding='0' style='width:100%;'>");
            #region TABLE表头
            s.Append("<tr>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<td>");
                    if (ssName[z] == "orderId")
                        s.Append("订单ID");
                    if (ssName[z] == "detailsOrderId")
                        s.Append("子订单ID");
                    if (ssName[z] == "newOrderId")
                        s.Append("分配新订单ID");
                    if (ssName[z] == "newScode")
                        s.Append("货号");
                    if (ssName[z] == "newColor")
                        s.Append("颜色");
                    if (ssName[z] == "newSize")
                        s.Append("尺寸");
                    if (ssName[z] == "newImg")
                        s.Append("图片");
                    if (ssName[z] == "newSaleCount")
                        s.Append("数量");
                    if (ssName[z] == "newStatus")
                        s.Append("订单状态");
                    if (ssName[z] == "createTime")
                        s.Append("创建时间");
                    if (ssName[z] == "editTime")
                        s.Append("编辑时间");
                    if (ssName[z] == "showStatus")
                        s.Append("开放状态");
                    if (ssName[z] == "sendSource")
                        s.Append("供应商");
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

            //s.Append("<td>开放</td><td>发货</td><td>取消订单</td><td>海关报备</td><td>编辑</td><td>删除</td>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
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
                    else if (ss[j].ToLower() == "newimg")
                    {
                        s.Append("<td>");
                        s.Append("<img src=\"" + dt.Rows[i][ss[j]].ToString() + "\" style='height:60px;' onerror='errorImg(this)'  />");
                        s.Append("</td>");
                    }
                    else if (ss[j].ToLower() == "newstatus")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("待确认");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "2")
                        {
                            s.Append("<td>");
                            s.Append("已确认");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "3")
                        {
                            s.Append("<td>");
                            s.Append("待发货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "4")
                        {
                            s.Append("<td>");
                            s.Append("已发货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "5")
                        {
                            s.Append("<td>");
                            s.Append("交易成功");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "6")
                        {
                            s.Append("<td>海关异常</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "7")
                        {
                            s.Append("<td>海关通关</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "11")
                        {
                            s.Append("<td>");
                            s.Append("退货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "12")
                        {
                            s.Append("<td>");
                            s.Append("订单取消");
                            s.Append("</td>");
                        }
                        else
                        {
                            s.Append("<td align='left'>待确认</td>");
                        }
                    }
                    else if (ss[j].ToLower() == "def1")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "0")
                        {
                            s.Append("<td>");
                            s.Append("商检报备失败");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("商检报备成功");
                            s.Append("</td>");
                        }
                        else
                        {
                            s.Append("<td>");
                            s.Append("商检未报备");
                            s.Append("</td>");
                        }
                    }
                    else if (ss[j].ToLower() == "def2")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "0")
                        {
                            s.Append("<td>");
                            s.Append("海关报备失败");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("海关报备成功");
                            s.Append("</td>");
                        }
                        else
                        {
                            s.Append("<td>");
                            s.Append("海关未报备");
                            s.Append("</td>");
                        }
                    }
                    else if (ss[j].ToLower() == "def3")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "0")
                        {
                            s.Append("<td>");
                            s.Append("联邦报备失败");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("联邦报备成功");
                            s.Append("</td>");
                        }
                        else
                        {
                            s.Append("<td>");
                            s.Append("联邦未报备");
                            s.Append("</td>");
                        }
                    }
                    else if (ss[j].ToLower() == "showstatus")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "0")
                        {
                            s.Append("<td>");
                            s.Append("未开放");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("已开放");
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
                s.Append("</tr>");
            }
            #endregion
            s.Append("</table>");
            s.Append("</td>");
            s.Append("</tr>");
            sourceorderBll = null;

            return s.ToString();
        }

        #region 开放订单
        /// <summary>
        /// 开放订单(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        /// <returns></returns>
        public string checkOrder()
        {
            string s = string.Empty;
            string orderId = string.Empty; //订单ID
            string[] orderIds; //订单ID集合
            string success = string.Empty; //数据更新成功（开放订单成功）
            string fail = string.Empty; //数据更新失败（开放订单失败）
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();

            orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]); //订单ID
            orderId = orderId.Trim(',');
            orderIds = helpcommon.StrSplit.StrSplitData(orderId,',');

            try
            {
            CreateSocket();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            for (int i = 0; i < orderIds.Length; i++)
            {
                Send("0!" + orderIds[i] + "!app");
                s = sourceorderBll.updateShowOrderState(orderIds[i]);

                if (s.Contains("成功"))
                {
                    success += orderIds[i] + ",";
                }
                else
                {
                    fail += orderIds[i] + ",";
                }

            }
            client.Close();

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = "订单" + success.Trim(',') + "开放成功\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = "订单" + fail.Trim(',') + "开放失败\r\n";
            }
            sourceorderBll = null;

            return success + fail;
        }
        /// <summary>
        /// 将订单状态更改为发送状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <returns></returns>
        public string sendOrder()
        {
            string s = string.Empty;
            string orderId = string.Empty;//订单ID
            string orderResult = string.Empty; //订单确认返回结果
            string success = string.Empty; //数据更新成功
            string fail = string.Empty; //数据更新失败
            string confirmFail = string.Empty; //API确认失败
            string[] orderIds; //订单ID集合

            orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]); //订单ID
            orderId = orderId.Trim(',');
            orderIds = helpcommon.StrSplit.StrSplitData(orderId, ',');

            for (int i = 0; i < orderIds.Length; i++)
            {
                //app调用确认订单(1发货，2确认，3取消)
                orderResult = MD5DAL.AppAPIHelper.ChangeOrderStatus(orderIds[i], 2); //订单确认返回结果
                helpcommon.appOrderMsg msg = helpcommon.ReSerialize.ReserializeMethod(orderResult);

                if (msg != null)
                {
                    if (msg.Code == "0")
                    {
                        bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
                        s = sourceorderBll.updateSendOrderState(orderIds[i]);
                        sourceorderBll = null;

                        if (s.Contains("成功"))
                        {
                            success += orderIds[i] + ",";
                        }
                        else
                        {
                            fail += orderIds[i] + ",";
                        }
                    }
                    else
                    {
                        //s = "确认失败,error:" + msg.Message;
                        confirmFail += "error:(" + orderIds[i] + ")" + msg.Message + ";";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = "订单" + success + "确定成功\r\n";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = "订单" + fail + "确定失败\r\n";
            }

            return success + fail + confirmFail;
        }
        /// <summary>
        /// 取消订单(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        /// <returns></returns>
        public string cancelOrder()
        {
            string s = string.Empty;
            string result = string.Empty;
            string orderResult = string.Empty; //取消订单返回结果
            string success = string.Empty;  //数据更新成功
            string fail = string.Empty;  //数据更新失败
            string confirmFail = string.Empty; //API确认失败
            string saleBanlance = string.Empty; //销售库存
            string cancelOrder = string.Empty; //取消订单返回值
            string orderId = string.Empty; //订单ID
            string[] orderIds; //订单ID集合

            orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]);
            orderId = orderId.Trim(',');
            orderIds = helpcommon.StrSplit.StrSplitData(orderId, ',');


            for (int h = 0; h < orderIds.Length; h++)
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                context.Connection.Open();
                context.CommandTimeout = 1000;
                context.Transaction = context.Connection.BeginTransaction();

                //app调用取消订单(1发货，2确认，3取消)
                orderResult = MD5DAL.AppAPIHelper.ChangeOrderStatus(orderIds[h], 3); //取消订单返回结果
                helpcommon.appOrderMsg msg = helpcommon.ReSerialize.ReserializeMethod(orderResult);
                if (msg != null)
                {
                    if (msg.Code == "0")
                    {
                        bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
                        bll.apiorderbll apiorderBll = new bll.apiorderbll();
                        ProductBll Productbll = new ProductBll();
                        DataTable dt = apiorderBll.getOrderDetailsMsg(orderIds[h]);//根据主订单获取子订单详情
                        if (dt != null)
                        {
                            string[] scode = new string[dt.Rows.Count];  //一个订单是否有多个商品
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                //TransactionScope //分布式事物

                                scode[i] = dt.Rows[i]["detailsScode"].ToString(); //子订单货号
                                cancelOrder = sourceorderBll.cancelOrder(orderIds[h], context); //取消订单(主订单)


                                //DataTable dtLocalBanlace = apiorderBll.getOrderMsg(orderIds[h], scode[i].ToString());//返回此scode货号所在订单中的商品数量。
                                DataTable dtLocalBanlace = apiorderBll.getOrderMsg(orderIds[h], scode[i].ToString(), context);//返回此scode货号所在订单中的商品数量。
                                int localBalance = helpcommon.ParmPerportys.GetNumParms(dtLocalBanlace.Rows[i]["detailsSaleCount"].ToString());
                                saleBanlance = Productbll.minusBanlace(scode[i], localBalance, context);//减掉此次购买商品数量


                                if (cancelOrder.Contains("成功"))
                                {
                                    if (saleBanlance.Contains("成功"))
                                    {
                                        success += orderIds[h] + ",";
                                        context.Transaction.Commit();
                                    }
                                    else
                                    {
                                        fail += orderIds[h] + ",";
                                        confirmFail += "error:(" + orderIds[h] + ")" + saleBanlance + ";";
                                        context.Transaction.Rollback();
                                    }

                                }
                                else
                                {
                                    fail += orderIds[h] + ",";
                                    confirmFail += "error:(" + orderIds[h] + ")" + cancelOrder + ";";
                                    context.Transaction.Rollback();
                                }

                            }
                        }

                        sourceorderBll = null;
                        Productbll = null;
                    }
                    else
                    {
                        confirmFail += "error:(" + orderIds[h] + ")" + msg.Message + ";\r\n";
                    }
                }
            } //context.Transaction.Commit();

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = "订单" + success + "取消成功\r\n";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = "订单" + fail + "取消失败\r\n";
            }
            if (!string.IsNullOrWhiteSpace(s))
            {
                s += "\r\n";
            }


            return success + fail + s + confirmFail;
        }

        #endregion

        /// <summary>
        /// 根据主订单编号查看买家信息
        /// </summary>
        /// <returns></returns>
        public string selectBuyer()
        {
            StringBuilder s = new StringBuilder();
            string orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]);
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
            model.apiOrder MDapiOrder = sourceorderBll.selectBuyer(orderId);

            s.Append("{\"buyMsg\":[");
            //真实名称
            s.Append("{\"realName\"");
            s.Append(":\"");
            s.Append(MDapiOrder.realName);
            s.Append("\"},");
            //省份
            s.Append("{\"provinceId\"");
            s.Append(":\"");
            s.Append(MDapiOrder.provinceId);
            s.Append("\"},");
            //城市
            s.Append("{\"cityId\"");
            s.Append(":\"");
            s.Append(MDapiOrder.cityId);
            s.Append("\"},");
            //县区
            s.Append("{\"district\"");
            s.Append(":\"");
            s.Append(MDapiOrder.district);
            s.Append("\"},");
            //详细地址
            s.Append("{\"buyNameAddress\"");
            s.Append(":\"");
            s.Append(MDapiOrder.buyNameAddress);
            s.Append("\"},");
            //邮编
            s.Append("{\"postcode\"");
            s.Append(":\"");
            s.Append(MDapiOrder.postcode);
            s.Append("\"},");
            //电话
            s.Append("{\"phone\"");
            s.Append(":\"");
            s.Append(MDapiOrder.phone);
            s.Append("\"},");
            //订单备注
            s.Append("{\"orderMsg\"");
            s.Append(":\"");
            s.Append(MDapiOrder.orderMsg);
            s.Append("\"},");
            //订单状态
            s.Append("{\"orderStatus\"");
            s.Append(":\"");
            s.Append(MDapiOrder.orderStatus);
            s.Append("\"},");
            //商品总价
            s.Append("{\"itemPrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.itemPrice);
            s.Append(Convert.ToDecimal(MDapiOrder.itemPrice.ToString() == "" ? "0" : MDapiOrder.itemPrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //快递费用
            s.Append("{\"deliveryPrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.deliveryPrice);
            s.Append(Convert.ToDecimal(MDapiOrder.deliveryPrice.ToString() == "" ? "0" : MDapiOrder.deliveryPrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //优惠费用
            s.Append("{\"favorablePrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.favorablePrice);
            s.Append(Convert.ToDecimal(MDapiOrder.favorablePrice.ToString() == "" ? "0" : MDapiOrder.favorablePrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //税费
            s.Append("{\"taxPrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.taxPrice);
            s.Append(Convert.ToDecimal(MDapiOrder.taxPrice.ToString() == "" ? "0" : MDapiOrder.taxPrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //订单金额（商品总价+快递费用-优惠费用+税费）
            s.Append("{\"orderPrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.orderPrice);
            s.Append(Convert.ToDecimal(MDapiOrder.orderPrice.ToString() == "" ? "0" : MDapiOrder.orderPrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //实际支付金额
            s.Append("{\"paidPrice\"");
            s.Append(":\"");
            //s.Append(MDapiOrder.paidPrice);
            s.Append(Convert.ToDecimal(MDapiOrder.paidPrice.ToString() == "" ? "0" : MDapiOrder.paidPrice.ToString()).ToString("f2"));
            s.Append("\"},");
            //支付状态（0：未支付，1：已支付）
            s.Append("{\"isPay\"");
            s.Append(":\"");
            s.Append(MDapiOrder.isPay);
            s.Append("\"},");
            //支付时间
            s.Append("{\"payTime\"");
            s.Append(":\"");
            s.Append(MDapiOrder.payTime);
            s.Append("\"},");
            //支付流水号
            s.Append("{\"payOuterId\"");
            s.Append(":\"");
            s.Append(MDapiOrder.payOuterId);
            s.Append("\"},");
            //订单创建时间
            s.Append("{\"createTime\"");
            s.Append(":\"");
            s.Append(MDapiOrder.createTime);
            s.Append("\"},");
            //身份证号
            s.Append("{\"def1\"");
            s.Append(":\"");
            s.Append(MDapiOrder.def1);
            s.Append("\"}");

            s.Append("]}");

            return s.ToString();
        }

        /// <summary>
        /// 根据主订单编号获取订单信息
        /// </summary>
        /// <returns></returns>
        public string EditApiOrder()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string orderId = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderId"]);
            List<model.apiSendOrder> list = new List<model.apiSendOrder>();
            StringBuilder s = new StringBuilder();
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
            string[] ssName = sourceorderBll.getDataName("apiSendOrder");
            //DataTable dt = new DataTable();
            DataTable dt = sourceorderBll.getData(orderId);

            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);

            #region TABLE表头
            s.Append("<tr>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "orderId")
                        s.Append("订单ID");
                    if (ssName[z] == "detailsOrderId")
                        s.Append("详情订单ID");
                    if (ssName[z] == "newOrderId")
                        s.Append("拆单新订单ID");
                    if (ssName[z] == "newScode")
                        s.Append("货号");
                    if (ssName[z] == "newColor")
                        s.Append("颜色");
                    if (ssName[z] == "newSize")
                        s.Append("尺寸");
                    if (ssName[z] == "newImg")
                        s.Append("图片");
                    if (ssName[z] == "newSaleCount")
                        s.Append("销售数量");
                    if (ssName[z] == "newStatus")
                        s.Append("订单状态");
                    if (ssName[z] == "createTime")
                        s.Append("创建时间");
                    if (ssName[z] == "editTime")
                        s.Append("编辑时间");
                    if (ssName[z] == "showStatus")
                        s.Append("是否开放");
                    if (ssName[z] == "sendSource")
                        s.Append("发货源头");
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

            #region TABLE内容
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
                    else if (ss[j].ToLower() == "sendsource")
                    {
                        s.Append("<td>");
                        s.Append("<input type='hidden' class='hidnewOrderId' value='" + dt.Rows[i]["detailsOrderId"].ToString() + "' />" + GetVencode1(dt.Rows[i]["newScode"].ToString(), dt.Rows[i]["sendSource"].ToString()) + "");
                        s.Append("</td>");
                    }
                    else if (ss[j].ToLower() == "newimg")
                    {
                        s.Append("<td>");
                        s.Append("<img src=\"" + dt.Rows[i][ss[j]].ToString() + "\" style='height:60px;' onerror='errorImg(this)'  />");
                        s.Append("</td>");
                    }
                    else if (ss[j].ToLower() == "newstatus")
                    {
                        if (dt.Rows[i][ss[j]].ToString() == "0")
                        {
                            s.Append("<td>");
                            s.Append("待确认");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "1")
                        {
                            s.Append("<td>");
                            s.Append("待确认");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "2")
                        {
                            s.Append("<td>");
                            s.Append("确认");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "3")
                        {
                            s.Append("<td>");
                            s.Append("待发货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "4")
                        {
                            s.Append("<td>");
                            s.Append("发货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "5")
                        {
                            s.Append("<td>");
                            s.Append("交易成功");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "11")
                        {
                            s.Append("<td>");
                            s.Append("退货");
                            s.Append("</td>");
                        }
                        else if (dt.Rows[i][ss[j]].ToString() == "12")
                        {
                            s.Append("<td>");
                            s.Append("取消订单");
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
                s.Append("</tr>");
            }
            #endregion
            s.Append("-*-");
            DataTable dt1 = DbHelperSQL.Query(@"select * from apiOrderRemark a left join users b on a.UserId=b.Id where a.OrderId='" + orderId + "'").Tables[0];
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
        /// 根据货号获取供应商 优先级1.供应商等级2.库存3.价格
        /// </summary>
        public string GetVencode1(string Scode, string sendSource)
        {
            string sql = @"select a.* from productsource a left join productstock b on a.SourceCode=b.Vencode where b.Scode='" + Scode + "' and b.Balance>0 order by a.SourceLevel,b.Balance desc,b.Pricee";
            DataTable dt1 = DbHelperSQL.Query(sql).Tables[0];
            string s = string.Empty;
            s += "<select>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i]["SourceCode"].ToString() == sendSource)
                {
                    s += "<option value='" + dt1.Rows[i]["SourceCode"] + "' selected='selected' >" + dt1.Rows[i]["sourceName"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt1.Rows[i]["SourceCode"] + "'>" + dt1.Rows[i]["sourceName"] + "</option>";
                }

            }
            s += "</select>";
            return s;
        }
        /// <summary>
        /// 根据获取供应商
        /// </summary>
        public string GetVencode()
        {
            string sql = @"select * from productsource";
            DataTable dt1 = DbHelperSQL.Query(sql).Tables[0];
            string s = string.Empty;
            s += "<option value='-1'>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i]["SourceCode"] + "'>" + dt1.Rows[i]["sourceName"] + "</option>";
            }
            return s;
        }


        /// <summary>
        /// 添加备注
        /// </summary>
        public string UpdateApiOrder()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string OrderId = Request.Form["OrderId"].ToString();
            string Remark = Request.Form["Remark"].ToString();
            string detailsOrderIds = Request.Form["detailsOrderIds[]"].ToString();
            string sendSources = Request.Form["sendSources[]"].ToString();
            List<string> detailsOrderIdlist = new List<string>();
            List<string> sendSourcelist = new List<string>();
            string rtn = string.Empty;
            string[] detailsOrderId = detailsOrderIds.Split(',');
            string[] sendSource = sendSources.Split(',');
            foreach (string item in detailsOrderId)
            {
                if (item != "")
                    detailsOrderIdlist.Add(item);
            }
            foreach (string item in sendSource)
            {
                if (item != "")
                    sendSourcelist.Add(item);
            }
            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
            rtn = "发货源" + sourceorderBll.UpdatesendSource(detailsOrderIdlist, sendSourcelist);
            if (Remark != "")
            {
                bll.apiorderbll apiorderBll = new bll.apiorderbll();
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("OrderId", OrderId);
                Dic.Add("Remark", Remark);
                Dic.Add("Edittime", DateTime.Now.ToString());
                Dic.Add("UserId", roleId.ToString());
                rtn += "备注" + apiorderBll.UpdateApiOrder(Dic);
            }

            return rtn;
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

        #region 连接服务端，发送消息
        Socket serverSocket;
        SocketError serverSocketError;
        Socket client;

        public void CreateSocket()
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("112.74.74.98"), 9090);
            //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.70"), 9091);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Connect(serverEndPoint);
        }


        public void Send(string str)
        {
            byte[] byteDate = Encoding.UTF8.GetBytes(str);
            serverSocket.BeginSend(byteDate, 0, byteDate.Length, 0, new AsyncCallback(SendCallback), serverSocket);
        }
        private void SendCallback(IAsyncResult ar)
        {
            string s = string.Empty;
            client = (Socket)ar.AsyncState;
            int bytesSent = client.EndSend(ar, out serverSocketError);
            if (serverSocketError != SocketError.Success)
            { }
            //client.Close();
        }
        #endregion


    }
}
